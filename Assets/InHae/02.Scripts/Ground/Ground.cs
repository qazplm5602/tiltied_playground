using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private List<MassHaveObj> _onGroundObj = new List<MassHaveObj>();
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

    public void ResetGround()
    {
        GetCompo<GroundTiltied>().ResetTilt();
    }
    
    public void AddMassObj(MassHaveObj obj)
    {
        if(!_onGroundObj.Contains(obj))
            _onGroundObj.Add(obj);
    }
    
    public void RemoveMassObj(MassHaveObj obj)
    {
        if(_onGroundObj.Contains(obj))
            _onGroundObj.Remove(obj);
    }

    public List<MassHaveObj> GetMassObjs() => _onGroundObj;
    
    public T GetCompo<T>() where T : class
    {
        if(_groundCompos.TryGetValue(typeof(T), out IGroundCompo compo))
        {
            return compo as T;
        }
        return default;
    }
}
