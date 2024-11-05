using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public List<MassHaveObj> _onGroundObj = new List<MassHaveObj>();
    public Transform _fallAbleArea;
    
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

        _fallAbleArea = transform.Find("FallAreaTrm");
    }
}
