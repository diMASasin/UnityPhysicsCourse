using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _addForcePoint;
    [SerializeField] private Vector3 _force;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            _rigidbody.AddForceAtPosition(_force, _addForcePoint.position);      
    }
}
