using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventMapManager : MonoSingleton<EventMapManager>
{
    [SerializeField] private List<EventMapSO> _mapSos;
    [SerializeField] private EventMapEnum _eventMapEnum;
    [SerializeField] private SoundSO _shortWhitle;
    [SerializeField] private SoundSO _gameEndWhitle;
    private Dictionary<EventMapEnum, EventMapSO> _mapSoDictionary;
    private Dictionary<EventMapEnum, GameObject> _mapObjDictionary;

    private EventMapBase _currentMap;

    protected override void Awake()
    {
        base.Awake();
        _mapSoDictionary = new Dictionary<EventMapEnum, EventMapSO>();
        _mapObjDictionary = new Dictionary<EventMapEnum, GameObject>();
        _mapSos.ForEach(x => _mapSoDictionary.Add(x.mapType, x));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            MapInit(_eventMapEnum);
        }
        
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            SoundManager.Instance.PlaySFX(transform.position, _shortWhitle);
        }
        
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SoundManager.Instance.PlaySFX(transform.position, _gameEndWhitle);
        }
#endif
    }
    
    public void MapInit(EventMapEnum type)
    {
        if (!_mapObjDictionary.ContainsKey(type))
        {
            GameObject mapObj = Instantiate(_mapSoDictionary[type].mapPrefab, transform.position,
                Quaternion.identity, transform);
            _mapObjDictionary.Add(type, mapObj);
        }

        if (_currentMap != null)
        {
           _currentMap.MapClear(); 
            _currentMap.gameObject.SetActive(false);
        }
        
        _mapObjDictionary[type].SetActive(true);
        _currentMap = _mapObjDictionary[type].GetComponent<EventMapBase>();
        _currentMap.MapInit(_mapSoDictionary[type]);
    }
}
