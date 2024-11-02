using UnityEngine;

public class GroundTiltied : MonoBehaviour, IGroundCompo
{
    [SerializeField] private float _rotRimit;
    [SerializeField] private float _rotTime = 1f;
    [SerializeField] private Transform _endPoint;
    
    private Ground _ground;
    private float _currentRotZ;

    public void Initialize(Ground ground)
    {
        _ground = ground;
    }
    
    private void FixedUpdate()
    {
        CalculateRot();
    }

    private void CalculateRot()
    {
        float leftMassSum = 0;
        float rightMassSum = 0;
        
        foreach (MassHaveObj obj in _ground._onGroundObj)
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
        float calculateMass;
        float lerpPos;
            
        Vector3 groundPos = transform.position;
        
        if (!isLeft)
        {
            lerpPos = Mathf.InverseLerp(groundPos.x, groundPos.x + _endPoint.position.x, objPos.x);
            calculateMass = Mathf.Lerp(0, mass, lerpPos);
        }
        else
        {
            lerpPos = Mathf.InverseLerp(groundPos.x, groundPos.x - _endPoint.position.x, objPos.x);
            calculateMass = Mathf.Lerp(0, mass, lerpPos);
        }
        
        return calculateMass;
    }
}
