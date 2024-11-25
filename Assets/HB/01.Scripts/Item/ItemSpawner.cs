using UnityEngine;

namespace HB {
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private Item[] _items;
        [SerializeField] private Transform spawnAreaMin;
        [SerializeField] private Transform spawnAreaMax;
        [SerializeField] private float _minRandomTime = 1f;
        [SerializeField] private float _maxRandomTime = 5f;
        [SerializeField] private int itemLimit = 2;

        private float _timer = 0.0f;
        private float _nextSpawnTime;
        private int _spawnedItemCount = 0;

        private void Start()
        {
            SetNextSpawnTime();
        }

        private void Update()
        {
            if (_spawnedItemCount < itemLimit)
            {
                _timer += Time.deltaTime;

                if (_timer >= _nextSpawnTime)
                {
                    SpawnItem();
                    SetNextSpawnTime();
                }
            }
        }

        private void SpawnItem()
        {
            int randomIndex = Random.Range(0, _items.Length);

            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x),
                Random.Range(spawnAreaMin.position.y, spawnAreaMax.position.y)
            );

            Item item = Instantiate(_items[randomIndex], spawnPosition, Quaternion.identity);

            _spawnedItemCount++;
            item.OnDestroyEvent += HandleItemDestroyed;
        }

        private void HandleItemDestroyed()
        {
            _spawnedItemCount--;
        }

        private void SetNextSpawnTime()
        {
            _nextSpawnTime = Random.Range(_minRandomTime, _maxRandomTime);
            _timer = 0.0f;
        }
    }
}
