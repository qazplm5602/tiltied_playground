using System.Collections;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private float _power;
    private bool _isStruck;
    private Rigidbody _rigidbody;
    private Collider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!_isStruck)
            transform.right = _rigidbody.linearVelocity;
    }

    public void Init(Vector3 targetPos)
    {
        Vector3 dir = Vector3.Lerp(transform.position, targetPos, 0.3f);
        dir.y += Physics.gravity.y * -1 + _power;
        //_rigidbody.AddForce(dir - transform.position, ForceMode.Impulse);
        _rigidbody.linearVelocity = (dir - transform.position);
        _rigidbody.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Floor"))
        {
            StartCoroutine(StruckRoutine());
            transform.parent = other.transform;
        }
    }

    private IEnumerator StruckRoutine()
    {
        yield return new WaitForSeconds(0.025f);
        _isStruck = true;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.isKinematic = true; 
        _collider.isTrigger = false;
    }
}
