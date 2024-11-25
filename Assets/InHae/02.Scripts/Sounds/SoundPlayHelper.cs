using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundPlayHelper : MonoBehaviour
{
    private Dictionary<Type, BaseSoundHelper> _soundHelpers;

    private void Awake()
    {
        _soundHelpers = new Dictionary<Type, BaseSoundHelper>();

        GetComponentsInChildren<BaseSoundHelper>().ToList()
            .ForEach(x => _soundHelpers.Add(x.GetType(), x));
    }

    public T GetHelper<T>() where T : BaseSoundHelper
    {
        if (_soundHelpers.TryGetValue(typeof(T), out BaseSoundHelper soundPlay))
        {
            return soundPlay as T;
        }

        return null;
    }
}
