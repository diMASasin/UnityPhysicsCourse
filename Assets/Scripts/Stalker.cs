using Unity.Collections;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Transform _target;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _castMaxDistance = 1.1f;
    [SerializeField] private int _targetDistance = 5;
    [SerializeField] private float _gravityModifier = 1;

    [Header("Debug")] 
    [ReadOnly, SerializeField] private float _angle;
    [ReadOnly, SerializeField] private float _distance;
    [ReadOnly, SerializeField] private bool _isGrounded;
    [ReadOnly, SerializeField] private Vector3 _direction;
    [ReadOnly, SerializeField] private Vector3 _velocity;

    private float _sphereCastRadius;
    private RaycastHit _hitInfo;

    private void Awake() => _sphereCastRadius = _collider.radius + 0.1f;

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded(out _hitInfo);
        CalculateGroundAngle(_hitInfo);
        CalculateDirection(_hitInfo);
        LookAtWalkDirection();
        CalculateVelocity();
        
        _rigidbody.velocity = _velocity;
    }

    private bool IsGrounded(out RaycastHit hitInfo)
    {
        Vector3 origin = (transform.position + Vector3.up * _collider.height / 4f + transform.forward * 0.1f);
        return Physics.SphereCast(origin, _sphereCastRadius, Vector3.down, out hitInfo, _castMaxDistance, _layerMask);
    }

    private void CalculateGroundAngle(RaycastHit hitInfo)
    {
        Vector3 downPosition = transform.position - _collider.height / 2 * Vector3.up;
        _angle = Vector3.SignedAngle(downPosition, hitInfo.point, Vector3.right);
    }

    private void CalculateDirection(RaycastHit hitInfo)
    {
        _direction = Vector3.ProjectOnPlane(_target.transform.position - transform.position, hitInfo.normal);
        _distance = Vector3.Distance(transform.position, _target.transform.position);
        _direction.Normalize();
    }

    private void LookAtWalkDirection() => 
        transform.forward = Vector3.ProjectOnPlane(_direction, Vector3.up).normalized;

    private void CalculateVelocity()
    {
        _velocity = _distance <= _targetDistance ? Vector3.zero : _direction * _speed;
        
        if (_isGrounded == false)
            _direction = Physics.gravity * _gravityModifier;
    }

    private void OnDrawGizmos()
    {
        if (_collider == null)
            return;
        
        if (_isGrounded == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_hitInfo.point + Vector3.up * _collider.height / 4, _sphereCastRadius);
        }
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_hitInfo.point, _hitInfo.normal);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _velocity);
    }
}