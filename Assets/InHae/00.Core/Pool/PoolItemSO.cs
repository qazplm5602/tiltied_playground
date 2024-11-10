using UnityEngine;

[CreateAssetMenu(fileName = "PoolItemSO", menuName = "Scriptable Objects/PoolItemSO")]
public class PoolItemSO : ScriptableObject
{
    public PoolType poolType;
    public GameObject prefab;
    public int initCount;
}
