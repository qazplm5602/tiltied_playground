using System;
using DG.Tweening;
using UnityEngine;

public class MassHaveObj : MonoBehaviour
{
    [SerializeField] private float _mass;
    [SerializeField] private LayerMask _massHaveLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _castTrm;
    [SerializeField] private Vector3 _castSize;
    
    private Collider _lastCollider;
    private Collider[] _groundColliders = new Collider[1];
    private Collider[] _massObjColliders = new Collider[2];

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
        int cnt = Physics.OverlapBoxNonAlloc(_castTrm.position, _castSize * 0.5f,
            _massObjColliders, Quaternion.identity, _massHaveLayer);
        
        if (cnt > 1)
        {
            Collider selfIgnoreCol = FindIgnoreSelfCollider(_massObjColliders);

            // 처음 오브젝트 위에 올라갔거나 오브젝트를 건너간 경우
            if (_lastUnderObj is null || _lastCollider != selfIgnoreCol)
            {
                if (_lastUnderObj is not null)
                {
                    _lastUnderObj.SetMass(_lastUnderObj.GetMass() - GetMass());
                }
                
                _lastCollider = selfIgnoreCol;
                _lastUnderObj = selfIgnoreCol.GetComponent<MassHaveObj>();
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
            }
        }
    }

    private Collider FindIgnoreSelfCollider(Collider[] colliders)
    {
        Collider selfIgnoreCol = colliders[0];

        foreach (Collider col in colliders)
        {
            if (col.transform != transform)
            {
                selfIgnoreCol = col;
                break;
            }
        }

        return selfIgnoreCol;
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
        Gizmos.DrawWireCube(_castTrm.position, _castSize);
    }
}
