using System.Collections.Generic;
using UnityEngine;

public class GroundTiltied : MonoBehaviour, IGroundCompo
{
    [SerializeField] private float _rotRimit;
    [SerializeField] private float _rotWeight = 1f;
    [SerializeField] private float _rotTime;
    [SerializeField] private Transform _endPoint;
    
    private Ground _ground;
    private float _currentRotZ;
    private Rigidbody _rigidbody;
    
    //Debug 용도로 직렬화
    [SerializeField] private float _leftMassSum;
    [SerializeField] private float _rightMassSum;

    public void Initialize(Ground ground)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ground = ground;
    }
    
    private void FixedUpdate()
    {
        CalculateRot();
    }

    //레드 블루 중량 계산
    private void CalculateRot()
    {
        _leftMassSum = 0;
        _rightMassSum = 0;

        List<MassHaveObj> onGroundObj = _ground.GetMassObjs();     
        for (int i = 0; i < onGroundObj.Count; i++)
        {
            
            Vector3 objPos = onGroundObj[i].transform.position;
            Vector3 groundPos = transform.position;

            bool isLeft = objPos.x < groundPos.x;

            if (isLeft)
                _leftMassSum += CalculateMass(onGroundObj[i].GetMass(), objPos, true);
            else
                _rightMassSum += CalculateMass(onGroundObj[i].GetMass(), objPos, false);
        }

        ApplyRotate();
    }

    // 차이에 따른 
    private void ApplyRotate()
    {
        float rotValue = Mathf.Abs(_leftMassSum - _rightMassSum) * _rotWeight;
        _rotTime = Mathf.Clamp(rotValue * 0.5f, 1, 5);
        
        // 지속되는 버전 ex) 레드 무게 2 -> 2의 힘으로 레드쪽으로 계속 기울어짐
        // if (_leftMassSum > _rightMassSum)
        //     _currentRotZ += rotValue;
        // else
        //     _currentRotZ -= rotValue;
        
        //  다른 버전 ex) 레드 무게 2 -> 레드 쪽으로 2만큼 기울어진 후 정지
        if (_leftMassSum < _rightMassSum)
            rotValue *= -1;

        _currentRotZ = Mathf.Lerp(_currentRotZ, rotValue, Time.fixedDeltaTime * _rotTime);
        _currentRotZ = Mathf.Clamp(_currentRotZ, -_rotRimit, _rotRimit);
        _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, 
            Quaternion.Euler(new Vector3(0, 0, _currentRotZ)),
            Time.fixedDeltaTime * _rotTime));
    }

    private float CalculateMass(float mass, Vector3 objPos, bool isLeft)
    {
        float calculateMass;
        float lerpPos;
            
        Vector3 groundPos = transform.position;
        
        //무게 중심 계산 (가운데 있을 경우 무게 = 0, 가운데는 무게/2, 끝 쪽에 있으면 무게)
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

    public void ResetTilt()
    {
        _leftMassSum = 0;
        _rightMassSum = 0;
        _currentRotZ = 0;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
