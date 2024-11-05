using UnityEngine;
using DG.Tweening;

public class Meteor : MonoBehaviour
{
    [SerializeField] private float _power;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _groundMask;
    
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
        {
            FlyingProcess();
        }
    }

    private void FlyingProcess()
    {
        transform.right = _rigidbody.linearVelocity;

        if (_collider.isTrigger)
            return;

        bool isHit = Physics.Raycast(transform.position, Vector3.down, _distance, _groundMask);

        if (isHit)
            _collider.isTrigger = true;
    }

    public void MeltProcess()
    {
        Vector3 endValue = transform.position;
        endValue.y -= 10;
        transform.DOMove(endValue, 2f).OnComplete(() => Destroy(gameObject));
    }

    public void Init(Vector3 targetPos)
    {
        Vector3 dir = Vector3.Lerp(transform.position, targetPos, 0.3f);
        dir.y += Physics.gravity.y * -1 + _power;
        
        //_rigidbody.AddForce(dir - transform.position, ForceMode.Impulse);
        //_rigidbody.AddTorque(_rigidbody.linearVelocity, ForceMode.Impulse);
        
        _rigidbody.linearVelocity = (dir - transform.position);
        _rigidbody.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Floor"))
        {
            _isStruck = true;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.isKinematic = true; 
            _collider.isTrigger = false;
            transform.parent = other.transform;
        }
    }
}
