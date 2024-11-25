
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class ItemSpawner : MonoBehaviour
{
   [SerializeField] private ItemSO[] _itemSOs;
   [SerializeField] private Vector2 _itemSpawnRange;
   [SerializeField] private IEnumerator _currentSpawnRoutine;
   [SerializeField] private Vector2 _itemSpawnDelayTime = new(4f, 10f);
   [SerializeField] private Vector2Int _itemSpawnIteration = new(1, 2);
   [Space]
   [SerializeField] private Kbh_Item _itemPrefab;

   [SerializeField] private List<Kbh_Item> _itemList = new();
   [SerializeField] private int maxSpawnCount = 3;

   private float RandomPlusmn => (Random.value - 0.5f) * 2;

   private void Awake()
   {
      StartSpawn();
   }

   public void StartSpawn()
   {
      StopSpawn();

      _currentSpawnRoutine = SpawnRoutine();
      StartCoroutine(_currentSpawnRoutine);
   }

   public void StopSpawn()
   {
      if (_currentSpawnRoutine is not null)
      {
         StopCoroutine(_currentSpawnRoutine);
         _currentSpawnRoutine = null;
      }
   }

   public void ClearSpawn()
   {
      _itemList.ForEach(x => Destroy(x));
      _itemList.Clear();
   }

   private IEnumerator SpawnRoutine()
   {
      while(true)
      {
         float delayTime = Random.Range(_itemSpawnDelayTime.x, _itemSpawnDelayTime.y);
         int spawnCnt = Random.Range(_itemSpawnIteration.x, _itemSpawnIteration.y);

         yield return new WaitForSeconds(delayTime);

         for(int i = 0; i<spawnCnt; ++i)
            SpawnAnyRandomItem();
      }
   }

   private bool CheckItemSpawn() {
      return _itemList.Where(v => v != null).Count() < maxSpawnCount;
   }

   private void SpawnAnyRandomItem()
   {
      if (!CheckItemSpawn()) return;

      Vector2 randomPos = new(_itemSpawnRange.x / 2 * RandomPlusmn, _itemSpawnRange.y / 2 * RandomPlusmn);
      Vector3 worldPos
        = new(randomPos.x , transform.position.y, randomPos.y);

      int itemIdx = Random.Range(0, _itemSOs.Length);
      ItemSO itemInfo = _itemSOs[itemIdx];


      Kbh_Item itemObj = Instantiate(_itemPrefab, worldPos, Quaternion.identity, transform);
      _itemList.Add(itemObj);
      itemObj.Initialize(itemInfo);
      itemObj.OnDestroy.AddListener(HandleEatItem);
   }

   private void HandleEatItem(Kbh_Item item) {
      _itemList.Remove(item);
      Destroy(item.gameObject);
   }

#if UNITY_EDITOR
   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.green;
      Vector3 fixedRange
         = new (_itemSpawnRange.x, 0, _itemSpawnRange.y);
      Gizmos.DrawCube(transform.position, fixedRange);
      Gizmos.color = Color.white;
   }
#endif


}
