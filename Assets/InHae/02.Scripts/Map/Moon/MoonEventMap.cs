using UnityEngine;

public class MoonEventMap : EventMapBase
{
    private Vector3 _defaultGravity = Physics.gravity;
    private Vector3 _applyGravity;
    
    protected override void MapEventStart()
    {
        base.MapEventStart();
        
        _defaultGravity = Physics.gravity;
        _applyGravity.y = Physics.gravity.y / 6;
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
