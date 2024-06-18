using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _strafeSpeed = 3;
    [SerializeField] private float _horizontalTurnSenitivity;
    [SerializeField] private float _verticalTurnSenitivity;
    [SerializeField] private float _verticalMinAngle = -89;
    [SerializeField] private float _verticalMaxAngle = 89;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _gravityScale = 1;

    [Header("Debug")] [SerializeField, ReadOnly]
    private Vector3 _velocity;

    private float _cameraAngle;
    private Transform _cameraTransform;
    private Vector3 _verticalVelocity;
    private Vector3 _horizontalVelocity;

    private readonly IPlayerInput _playerInput = new PlayerInput();

    private void Awake() => _cameraTransform = _camera.transform;

    private void Update()
    {
        if (_characterController == null)
            return;

        CalculateHorizontalVelocity();
        CalculateVerticalVelocity();
        _velocity = _horizontalVelocity + _verticalVelocity;
        
        HandleRotationInput();

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void CalculateHorizontalVelocity()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        _horizontalVelocity = right * (_speed * _playerInput.GetHorizontalAxis());
        _horizontalVelocity += forward * (_strafeSpeed * _playerInput.GetVerticalAxis());
        _horizontalVelocity.y = 0;
    }

    private void CalculateVerticalVelocity()
    {
        if (_characterController.isGrounded)
            _verticalVelocity = _playerInput.GetJumpKeyDown() == true ? Vector3.up * _jumpForce : Vector3.down;
        else
            _verticalVelocity += _gravityScale * Time.deltaTime * Physics.gravity;
    }

    private void HandleRotationInput()
    {
        _cameraAngle -= _playerInput.GetMouseY() * _verticalTurnSenitivity;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _verticalMinAngle, _verticalMaxAngle);
        _cameraTransform.localEulerAngles = Vector3.right * _cameraAngle;

        transform.Rotate(Vector3.up * (_horizontalTurnSenitivity * _playerInput.GetMouseX()));
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit != null && hit.rigidbody != null)
            hit.rigidbody.velocity = Vector3.up * 100;
    }

    private void OnDrawGizmosSelected() =>
        Gizmos.DrawWireCube(transform.position, new Vector3(1, _characterController.height, 1));
}