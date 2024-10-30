using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Vector3 _searchSize;
    [SerializeField] private LayerMask _objLayer;
    [SerializeField] private float _rotRimit;
    [SerializeField] private float _rotTime = 1f;
    [SerializeField] private Transform _endPoint;
    
    public List<MassHaveObj> _onGroundObj = new List<MassHaveObj>();
    public Collider[] _colliders;
    
    private float _currentRotZ;
    
    private void FixedUpdate()
    {
        //OnFieldObjSearch();
        CalculateRot();
    }

    private void OnFieldObjSearch()
    {
        int size = Physics.OverlapBoxNonAlloc(transform.position, _searchSize * 0.5f, _colliders,
            transform.rotation, _objLayer);

        if (size > 0)
        {
            _onGroundObj = _colliders.ToList().ConvertAll(x =>
            {
                if (x.TryGetComponent(out MassHaveObj massHaveObj))
                    return massHaveObj;
                
                return default;
            });
        }
    }

    private void CalculateRot()
    {
        float leftMassSum = 0;
        float rightMassSum = 0;
        
        foreach (MassHaveObj obj in _onGroundObj)
        {
            Vector3 objPos = obj.transform.position;
            Vector3 groundPos = transform.position;

            bool isLeft = objPos.x < groundPos.x;

            if (isLeft)
                leftMassSum += CalculateMass(obj.GetMass(), objPos, true);
            else
                rightMassSum += CalculateMass(obj.GetMass(), objPos, false);
        }

        ApplyRotate(leftMassSum, rightMassSum);
    }

    private void ApplyRotate(float leftMassSum, float rightMassSum)
    {
        float rotValue = Mathf.Abs(leftMassSum - rightMassSum);
        
        if (leftMassSum > rightMassSum)
            _currentRotZ += rotValue;
        else
            _currentRotZ -= rotValue;

        _currentRotZ = Mathf.Clamp(_currentRotZ, -_rotRimit, _rotRimit);
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.Euler(new Vector3(0, 0, _currentRotZ)),
            Time.deltaTime * _rotTime);
    }

    private float CalculateMass(float mass, Vector3 objPos, bool isLeft)
    {
        float calculMass = mass;
        float lerpPos;
            
        Vector3 groundPos = transform.position;
        
        if (!isLeft)
        {
            lerpPos = Mathf.InverseLerp(groundPos.x, groundPos.x + _endPoint.position.x, objPos.x);
            calculMass = Mathf.Lerp(0, mass, lerpPos);
        }
        else
        {
            lerpPos = Mathf.InverseLerp(groundPos.x, groundPos.x - _endPoint.position.x, objPos.x);
            calculMass = Mathf.Lerp(0, mass, lerpPos);
        }
        
        return calculMass;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.TryGetComponent(out MassHaveObj obj))
            _onGroundObj.Add(obj);
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(other.collider.TryGetComponent(out MassHaveObj obj))
            _onGroundObj.Remove(obj);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _searchSize);
    }
}
