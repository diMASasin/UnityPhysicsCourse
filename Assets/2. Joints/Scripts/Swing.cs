using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _addForcePoint;
    [SerializeField] private Vector3 _force;
    
    private bool _isSwingKeyDown;

    private void Update()
    {
        _isSwingKeyDown = Input.GetKeyDown(KeyCode.W);
        
        if(_isSwingKeyDown)
            _rigidbody.AddForceAtPosition(_force, _addForcePoint.position);
    }
}
