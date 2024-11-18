using System;
using DG.Tweening;
using UnityEngine;

public class MassHaveObj : MonoBehaviour
{
    [SerializeField] private float _mass;
    
    [Header("BoxCast Setting")]
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _massHaveLayer;
    
    [Header("OverlapBox Setting")]
    [SerializeField] private Transform _castTrm;
    [SerializeField] private Vector3 _castSize;
    [SerializeField] private LayerMask _groundLayer;
    
    private RaycastHit _lastHit;
    private Collider[] _groundColliders = new Collider[1];

    private float _defaultMass;

    private Ground _ground;
    private MassHaveObj _lastUnderObj;

    private Action<float> _massChangeEvent;

    private void Awake()
    {
        _defaultMass = _mass;
        _massChangeEvent += UpdateMass;
    }

    private void Update()
    {
        AddWeight();
        GroundCheck();
    }

    private void OnDestroy()
    {
        if (_ground != null)
            _ground.RemoveMassObj(this);
    }

    // 오브젝트 위에 올라갔을 경우 아래 오브젝트에 자신의 무게를 더 해줌
    private void AddWeight()
    {
        bool isHit = Physics.BoxCast(transform.position, _castSize * 0.5f, Vector3.down, 
            out RaycastHit hit, Quaternion.identity, _rayDistance,_massHaveLayer);
        
        if (isHit)
        {
            // 처음 오브젝트 위에 올라갔거나 오브젝트를 건너간 경우
            if (_lastUnderObj is null || _lastHit.collider != hit.collider)
            {
                if (_lastUnderObj is not null)
                {
                    _lastUnderObj.SetMass(_lastUnderObj.GetMass() - GetMass());
                }
                
                _lastHit = hit;
                _lastUnderObj = hit.collider.GetComponent<MassHaveObj>();
                _lastUnderObj.SetMass(GetMass() + _lastUnderObj._defaultMass);
            }
        }
        else
        {
            // 오브젝트에서 내려오면 그 오브젝트의 기본 무게로 변경
            if (_lastUnderObj is not null)
            {
                _lastUnderObj.SetMass(_lastUnderObj.GetMass() - GetMass());
                _lastUnderObj = null;
                _lastHit = default;
            }
        }
    }

    private void UpdateMass(float mass)
    {
        if (_lastUnderObj is not null)
        {
            _lastUnderObj.SetMass(mass + _lastUnderObj._defaultMass);
        }
    }

    private void GroundCheck()
    {
        int cnt = Physics.OverlapBoxNonAlloc(_castTrm.position, _castSize * 0.5f, _groundColliders,
            Quaternion.identity, _groundLayer);
        
        if (cnt > 0)
        {
            if(_ground is not null)
                return;
            
            _ground = _groundColliders[0].GetComponentInParent<Ground>();
            _ground.AddMassObj(this);
        }
        else
        {
            if(_ground is null)
                return;
            
            _ground.RemoveMassObj(this);
            _ground = null;
        }
    }

    public float GetMass() => _mass;

    public void SetMass(float mass)
    {
        _mass = mass;
        _massChangeEvent?.Invoke(_mass);
    }

    public void MassLerp(float time, float targetMass) => DOTween.To(() => _mass, 
        currentMass => _mass = currentMass, targetMass, time);
    public void SetDefaultMass(float mass) => _defaultMass = mass;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube (_castTrm.position, _castSize);

        Gizmos.color = Color.green;
        Gizmos.DrawRay (transform.position, Vector3.down * _rayDistance);
        Gizmos.DrawWireCube (transform.position + Vector3.down * _rayDistance, _castSize );
    }
}
