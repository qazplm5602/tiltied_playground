using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolManagerSO", menuName = "Scriptable Objects/PoolManagerSO")]
public class PoolManagerSO : ScriptableObject
{
    public List<PoolItemSO> poolItems;
    private Dictionary<PoolType, Pool> _pools;
    [SerializeField] private Transform _rootTrm; //디버그용도로 직렬화했음.

    public void InitializePool(Transform root)
    {
        _rootTrm = root;
        _pools = new Dictionary<PoolType, Pool>();

        foreach(PoolItemSO item in poolItems)
        {
            IPoolable poolable = item.prefab.GetComponent<IPoolable>();
            Debug.Assert(poolable != null, $"PoolItem does not have IPoolable {item.prefab.name}");

            Pool pool = new Pool(poolable, _rootTrm, item.initCount);
            _pools.Add(item.poolType, pool);
        }
    }

    public IPoolable Pop(PoolType type)
    {
        if(_pools.TryGetValue(type, out Pool pool))
        {
            return pool.Pop();
        }
        return null;
    }

    public void Push(IPoolable item)
    {
        if(_pools.TryGetValue(item.Type, out Pool pool))
        {
            pool.Push(item);
        }
    }
}
