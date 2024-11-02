using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EventMapEnum
{
    Volcano,
    Moon,
}

public abstract class EventMapBase : MonoBehaviour
{
    private float _currentDeltaTime;
    private int _randomEventTime;

    protected EventMapSO _mapSo;
    protected bool _isEventing;
    protected bool _isEventEnd;

    private void Update()
    {
        if (_isEventing)
        {
            
        }
        
        _currentDeltaTime += Time.deltaTime;
        if (_currentDeltaTime >= _randomEventTime)
        {
            MapEventStart();
        }
    }

    public void MapInit(EventMapSO so)
    {
        _mapSo = so;
        _randomEventTime = Random.Range(_mapSo.minEventTime, _mapSo.maxEventTime);
    }
    
    public abstract void MapEventStart();
    public abstract void MapEventStop();
}
