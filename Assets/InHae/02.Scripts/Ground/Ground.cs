using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public List<MassHaveObj> onGroundObj = new List<MassHaveObj>();
    [HideInInspector] public Transform minFallPoint;
    [HideInInspector] public Transform maxFallPoint;
    
    private Dictionary<Type, IGroundCompo> _groundCompos;

    private void Awake()
    {
        _groundCompos = new Dictionary<Type, IGroundCompo>();
        GetComponentsInChildren<IGroundCompo>()
            .ToList()
            .ForEach(x => _groundCompos.Add(x.GetType(), x));

        foreach (var groundCompo in _groundCompos.Values)
        {
            groundCompo.Initialize(this);
        }

    }

    private void Start()
    {
        minFallPoint = transform.Find("FallAreaTrm").Find("MinPoint");
        maxFallPoint = transform.Find("FallAreaTrm").Find("MaxPoint");
    }
}
