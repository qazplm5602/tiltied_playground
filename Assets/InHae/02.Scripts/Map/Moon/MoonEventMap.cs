using UnityEngine;

public class MoonEventMap : EventMapBase
{
    [SerializeField] private float _eventGravity = -9.8f;
    private Vector3 _defaultGravity;
    private Vector3 _applyGravity;
    
    protected override void MapEventStart()
    {
        base.MapEventStart();
        _defaultGravity = Physics.gravity;
        _applyGravity.y = _eventGravity;
        Physics.gravity = _applyGravity;
    }

    protected override void MapEventStop()
    {
        base.MapEventStop();
        Physics.gravity = _defaultGravity;
    }
    
    public override void MapClear()
    {
        Physics.gravity = _defaultGravity;
    }
}
