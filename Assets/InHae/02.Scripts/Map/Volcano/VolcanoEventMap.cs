using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VolcanoEventMap : EventMapBase
{
    [SerializeField] private Meteor _meteor;
    
    [Header("Particle Setting")]
    [SerializeField] private ParticleSystem _explodeEffect;
    
    [Header("Meteor Setting")]
    [SerializeField] private Transform _meteorStartPos;
    [SerializeField] private int _minMeteorCount;
    [SerializeField] private int _maxMeteorCount;
    [SerializeField] private float _intervalTime;
    
    private List<Meteor> _meteorList = new List<Meteor>();

    private Coroutine _fallRoutine;
    protected override void MapEventStart()
    {
        base.MapEventStart();
        _fallRoutine = StartCoroutine(StartFallMeteor());
    }

    protected override void MapEventStop()
    {
        base.MapEventStop();
        foreach (Meteor meteor in _meteorList)
        {
            meteor.MeltProcess();
        }
        
        _meteorList.Clear();
    }

    public override void MapClear()
    {
        if (_fallRoutine != null)
            StopCoroutine(_fallRoutine);
        
        foreach (Meteor meteor in _meteorList)
        {
            Destroy(meteor.gameObject);
        }
        
        _meteorList.Clear();
    }

    private IEnumerator StartFallMeteor()
    {
        int randMeteorCount = Random.Range(_minMeteorCount, _maxMeteorCount + 1);

        Vector3 minPoint = _ground.minFallPoint.position;
        Vector3 maxPoint = _ground.maxFallPoint.position;

        for (int i = 0; i < randMeteorCount; i++)
        {
            Vector3 targetPos = minPoint;
            
            float x = Random.Range(minPoint.x, maxPoint.x);
            float z = Random.Range(minPoint.z, maxPoint.z);

            targetPos.x = x;
            targetPos.z = z;

            _explodeEffect.Play();
            
            Meteor meteor = Instantiate(_meteor, _meteorStartPos.position, Quaternion.identity);
            _meteorList.Add(meteor);
            meteor.Init(targetPos);
            
            yield return new WaitForSeconds(_intervalTime);
        }
    }
}
