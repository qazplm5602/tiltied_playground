using UnityEngine;

public class MassHaveObj : MonoBehaviour
{
    [SerializeField] private bool _isMove;
    [SerializeField] private float _mass;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _rigidbody;

    private Ground _ground;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        TiltiedApply();
        if(!_isMove)
            return;
        
        float x = Input.GetAxisRaw("Horizontal");
        _rigidbody.linearVelocity= new Vector3(x * _speed, _rigidbody.linearVelocity.y, 0);
    }

    private void OnDestroy()
    {
        if (_ground != null)
            _ground.onGroundObj.Remove(this);
    }

    private void TiltiedApply()
    {
        bool isHit = Physics.Raycast(transform.position, Vector3.down,
            out RaycastHit hit,3f, _groundLayer);

        if (isHit)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = hit.transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(rot);
        }
    }

    public void SetGround(Ground ground) => _ground = ground;
    public float GetMass() => _mass;
}
