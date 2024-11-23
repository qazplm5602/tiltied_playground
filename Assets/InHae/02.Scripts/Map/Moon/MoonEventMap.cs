using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoonEventMap : EventMapBase
{
    [SerializeField] private int _stoneCount;
    [SerializeField] private Transform _stoneParent;
    [SerializeField] private float _minStoneLoopTime;
    [SerializeField] private float _maxStoneLoopTime;
    
    private Vector3 _defaultGravity = Physics.gravity;
    private Vector3 _applyGravity;

    private List<MoonStone> _moonStones;

    protected override void Awake()
    {
        base.Awake();
        
        _moonStones = new List<MoonStone>();
        _moonStones = _stoneParent.GetComponentsInChildren<MoonStone>().ToList();
        _moonStones.ForEach(x => x.gameObject.SetActive(false));
    }

    protected override void MapEventStart()
    {
        base.MapEventStart();
        
        _defaultGravity = Physics.gravity;
        _applyGravity.y = Physics.gravity.y / 6;
        Physics.gravity = _applyGravity;
        ManagerManager.GetManager<BallGoalSimulateManager>().SetGravity(250);

        for (int i = 0; i < _stoneCount; i++)
        {
            int randIDx = Random.Range(0, _moonStones.Count - i);
            int lastIdx = _moonStones.Count - i - 1;
            (_moonStones[randIDx], _moonStones[lastIdx]) = (_moonStones[lastIdx], _moonStones[randIDx]);
        }

        for (int i = _moonStones.Count - _stoneCount; i < _moonStones.Count; i++)
        {
            _moonStones[i].GravityOn(Random.Range(_minStoneLoopTime, _maxStoneLoopTime));
        }
    }

    protected override void MapEventStop()
    {
        base.MapEventStop();
        MapOnClear();
    }
    
    public override void MapOnClear()
    {
        Physics.gravity = _defaultGravity;
        ManagerManager.GetManager<BallGoalSimulateManager>().SetGravity(1550);
        
        for (int i = _moonStones.Count - _stoneCount; i < _moonStones.Count; i++)
        {
            _moonStones[i].GravityOff();
        }
    }
}
