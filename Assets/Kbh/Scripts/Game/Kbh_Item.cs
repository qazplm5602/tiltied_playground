using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Kbh_Item : MonoBehaviour
{
   private bool _IsActive = false;
   private ItemSO _itemInfo;
   private TagHandle _playerTag;

   [SerializeField] private Collider _collider;
   [SerializeField] private Renderer _renderer;

   [SerializeField] private UnityEvent OnInit;
   [SerializeField] private UnityEvent OnDestroy;

   private void Awake()
   {
      _collider = GetComponent<Collider>();
      _renderer = transform.Find("Visual").GetComponent<Renderer>();

      _playerTag = TagHandle.GetExistingTag("Player");
   }

   public void Update()
   {
      transform.Rotate(0, 50 * Time.deltaTime, 0);
   }

   public void Initialize(ItemSO itemSO)
   {
      _itemInfo = itemSO;
      _IsActive = true;
      _collider.enabled = true;
      _renderer.material.mainTexture = _itemInfo.texture;

      OnInit?.Invoke();
   }

   public ItemSO GetItemInfo() => _itemInfo;

   private void OnTriggerEnter(Collider other)
   {
      if (_IsActive && other.CompareTag(_playerTag))
      {
         _collider.enabled = false;
         _renderer.enabled = false;
         _IsActive = false;
      }

      OnDestroy?.Invoke();
   }

}
