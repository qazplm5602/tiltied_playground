using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private float _rimitRot;
    private List<MassHaveObj> _onGroundObj = new List<MassHaveObj>();

    private float _currentRotZ;
    
    private void FixedUpdate()
    {
        CalculateRot();
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

        _currentRotZ = Mathf.Clamp(_currentRotZ, -_rimitRot, _rimitRot);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _currentRotZ));
    }

    private float CalculateMass(float mass, Vector3 objPos, bool isLeft)
    {
        float calculMass = mass;
        float lerpPos;
            
        Vector3 groundPos = transform.position;
        
        if (!isLeft)
        {
            lerpPos = Mathf.InverseLerp(groundPos.x, groundPos.x + transform.localScale.x * 0.5f, objPos.x);
            calculMass = Mathf.Lerp(0, mass, lerpPos);
        }
        else
        {
            lerpPos = Mathf.InverseLerp(groundPos.x, groundPos.x - transform.localScale.x * 0.5f, objPos.x);
            calculMass = Mathf.Lerp(0, mass, lerpPos);
        }
        
        return calculMass;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.TryGetComponent(out MassHaveObj massHaveObj))
            _onGroundObj.Add(massHaveObj);
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.collider.TryGetComponent(out MassHaveObj massHaveObj))
            _onGroundObj.Remove(massHaveObj);
    }
}
