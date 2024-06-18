using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Rigidbody _rigidbody;

    private void OnDrawGizmos()
    {
        if(_collider == null)
            return;

        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * _collider.height / 2 + Vector3.forward * 0.1f;
        Vector3 direction = -transform.up;
        
        // if (_rigidbody.SweepTest(transform.forward, out RaycastHit hitInfo, 10f))
        //     Gizmos.DrawWireSphere(hitInfo.point + Vector3.up * 0.5f, 0.5f);

        if (Physics.SphereCast(origin, _collider.radius, direction, out RaycastHit hitInfo2, 2f))
        {
            float radius = 0.25f;
            Gizmos.DrawWireSphere(hitInfo2.point + Vector3.up * radius, radius);
        }

        Gizmos.DrawRay(hitInfo2.point, hitInfo2.normal);
        Gizmos.DrawRay(transform.position, transform.forward * 2);
        Gizmos.DrawRay(transform.position, (transform.forward + hitInfo2.normal) * 3);
    }
}