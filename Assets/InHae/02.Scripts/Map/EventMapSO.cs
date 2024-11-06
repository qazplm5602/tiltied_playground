using UnityEngine;

[CreateAssetMenu(fileName = "EventMapSO", menuName = "Scriptable Objects/EventMapSO")]
public class EventMapSO : ScriptableObject
{
    public EventMapEnum mapType;
    public GameObject mapPrefab;
    public Material skyBox;
    public int minEventTime;
    public int maxEventTime;
    public int eventDurationTime;
}
