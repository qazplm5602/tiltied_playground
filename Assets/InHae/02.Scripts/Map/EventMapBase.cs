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
    [SerializeField] protected EventMapSO _mapSo;
    
    private float _currentDeltaTime;
    private int _randomEventTime;

    protected bool _isEventing;
    protected bool _isEventEnd;

    protected Ground _ground;

    protected virtual void Awake()
    {
        _ground = GetComponentInChildren<Ground>();
        MapInit();
    }

    private void Update()
    {
        if (_isEventing)
        {
            MapEventEndCheck();
            return;
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
        SettingInit();
        _randomEventTime = Random.Range(_mapSo.minEventTime, _mapSo.maxEventTime + 1);
    }

    private void MapInit()
    {
        SettingInit();
        _randomEventTime = Random.Range(_mapSo.minEventTime, _mapSo.maxEventTime + 1);
    }

    public abstract void MapClear();

    private void MapEventEndCheck()
    {
        _currentDeltaTime += Time.deltaTime;
        if (_currentDeltaTime >= _mapSo.eventDurationTime)
        {
            _isEventEnd = true;
        }
            
        if (_isEventEnd)
        {
            MapEventStop();
        }
    }

    private void SettingInit()
    {
        _isEventing = false;
        _isEventEnd = false;
        _currentDeltaTime = 0;
    }

    protected virtual void MapEventStart()
    {
        _isEventing = true;
        _currentDeltaTime = 0;
    }

    protected virtual void MapEventStop()
    {
        SettingInit();
        _randomEventTime = Random.Range(_mapSo.minEventTime, _mapSo.maxEventTime + 1);
    }
}
