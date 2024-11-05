using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventMapManager : MonoBehaviour
{
    [SerializeField] private List<EventMapSO> _mapSos;
    
    private Dictionary<EventMapEnum, EventMapSO> _mapSoDictionary;
    private Dictionary<EventMapEnum, GameObject> _mapObjDictionary;

    private EventMapBase _currentMap;
    
    private void Awake()
    {
        _mapSoDictionary = new Dictionary<EventMapEnum, EventMapSO>();
        _mapObjDictionary = new Dictionary<EventMapEnum, GameObject>();
        _mapSos.ForEach(x => _mapSoDictionary.Add(x.mapType, x));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            MapInit(EventMapEnum.Volcano);
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

        _mapObjDictionary[type].SetActive(true);
        _currentMap = _mapObjDictionary[type].GetComponent<EventMapBase>();
        _currentMap.MapInit(_mapSoDictionary[type]);
    }
}
