using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VolcanoEventMap : EventMapBase
{
    [SerializeField] private Meteor _meteor;
    [SerializeField] private Transform _meteorStartPos;
    
    [SerializeField] private int _minMeteorCount;
    [SerializeField] private int _maxMeteorCount;
    [SerializeField] private float _intervalTime;

    private List<Meteor> _meteorList = new List<Meteor>();
    protected override void MapEventStart()
    {
        base.MapEventStart();
        StartCoroutine(StartFallMeteor());
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

    private IEnumerator StartFallMeteor()
    {
        int randMeteorCount = Random.Range(_minMeteorCount, _maxMeteorCount + 1);

        Vector3 minPoint = _ground._fallAbleArea.Find("MinPoint").position;
        Vector3 maxPoint = _ground._fallAbleArea.Find("MaxPoint").position;

        for (int i = 0; i < randMeteorCount; i++)
        {
            Vector3 targetPos = minPoint;
            
            float x = Random.Range(minPoint.x, maxPoint.x);
            float z = Random.Range(minPoint.z, maxPoint.z);

            targetPos.x = x;
            targetPos.z = z;

            Meteor meteor = Instantiate(_meteor, _meteorStartPos.position, Quaternion.identity);
            meteor.Init(targetPos);
            _meteorList.Add(meteor);
            yield return new WaitForSeconds(_intervalTime);
        }
    }
}
