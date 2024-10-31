using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MassHaveObj : MonoBehaviour
{
    [SerializeField] private bool _isMove;
    [SerializeField] private float _mass;
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!_isMove)
            return;
        
        float x = Input.GetAxisRaw("Horizontal");
        _rigidbody.linearVelocity= new Vector3(x * _speed, _rigidbody.linearVelocity.y, 0);
    }

    public float GetMass() => _mass;
}
