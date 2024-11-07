using System;
using UnityEngine;

public class MassHaveObj : MonoBehaviour
{
    [SerializeField] private bool _isMove;
    [SerializeField] private float _mass;
    [SerializeField] private float _speed;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _massHaveLayer;

    private Rigidbody _rigidbody;
    private RaycastHit _lastHit;

    private float _defaultMass;

    private Ground _ground;
    private MassHaveObj _lastUnderObj;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _defaultMass = _mass;
    }

    private void Update()
    {
        AddWeight();
        TiltiedApply();
        
        if(!_isMove)
            return;
        
        float x = Input.GetAxisRaw("Horizontal");
        _rigidbody.linearVelocity = new Vector3(x * _speed, _rigidbody.linearVelocity.y, 0);
    }

    private void OnDestroy()
    {
        if (_ground != null)
            _ground.onGroundObj.Remove(this);
    }

    // 실험용 기울이기
    private void TiltiedApply()
    {
        bool isHit = Physics.Raycast(transform.position, Vector3.down,
            out RaycastHit hit,_rayDistance, _groundLayer);

        if (isHit)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = hit.transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(rot);
        }
    }

    // 오브젝트 위에 올라갔을 경우 아래 오브젝트에 자신의 무게를 더 해줌
    private void AddWeight()
    {
        bool isHit = Physics.Raycast(transform.position, Vector3.down,
            out RaycastHit hit,_rayDistance, _massHaveLayer);
        
        if (isHit)
        {
            if (_lastUnderObj is null)
            {
                _lastUnderObj = hit.collider.GetComponent<MassHaveObj>();
                _lastHit = hit;
                _lastUnderObj.SetMass(GetMass() + _lastUnderObj._defaultMass);
            }
            
            // 오브젝트에서 오브젝트로 건너 갔을 경우 건너간 오브젝트로 변경
            if(_lastHit.collider != hit.collider)
                _lastUnderObj = hit.collider.GetComponent<MassHaveObj>();

        }
        else
        {
            // 오브젝트에서 내려오면 그 오브젝트의 기본 무게로 변경
            if (_lastUnderObj is not null)
            {
                _lastUnderObj.SetMass(_lastUnderObj._defaultMass);
                _lastUnderObj = null;
            }
        }
    }

    public void SetGround(Ground ground) => _ground = ground;
    public float GetMass() => _mass;
    public void SetMass(float mass) => _mass = mass;
    public void SetDefaultMass(float mass) => _defaultMass = mass;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * _rayDistance);
    }
}
