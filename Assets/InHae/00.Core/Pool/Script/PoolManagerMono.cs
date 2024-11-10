using System;
using UnityEngine;

public class PoolManagerMono : MonoBehaviour
{
    [SerializeField] private PoolManagerSO _poolManagerSo;

    private void Awake()
    {
        _poolManagerSo.InitializePool(transform);
    }
}
