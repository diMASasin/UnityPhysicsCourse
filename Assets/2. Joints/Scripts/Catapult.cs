using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private float _acceptableAngleDelta = 1f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _spawnPoint;
    
    [Header("Idle Joint Spring")]
    [SerializeField] private float _idleSpring = 1000;
    [SerializeField] private float _idleDamper = 1000;
    [SerializeField] private float _idleTargetPosition = 0;
    
    [Header("Launch Joint Spring")]
    [SerializeField] private float _launchSpring = 1000;
    [SerializeField] private float _launchDamper = 0;
    [SerializeField] private float _launchTargetPosition = 45;
    
    private JointSpring _launchJointSpring;
    private JointSpring _idleJointSpring;

    private bool _canReload;
    private bool _canLaunch;

    private void Awake()
    {
         _idleJointSpring = new JointSpring() 
             { spring = _idleSpring, damper = _idleDamper, targetPosition = _idleTargetPosition };
         
         _launchJointSpring = new JointSpring() 
             { spring = _launchSpring, damper = _launchDamper, targetPosition = _launchTargetPosition };
    }

    private void Start() => StartCoroutine(WaitForReloading());

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Launch();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private IEnumerator WaitForReloading()
    {
        do
        {
            yield return null;
        } while (Mathf.Abs(_hingeJoint.angle - _idleJointSpring.targetPosition) > _acceptableAngleDelta);

        Instantiate(_projectilePrefab, _spawnPoint.position, Quaternion.identity);
        
        _canLaunch = true;
    }
    
    private IEnumerator WaitForLaunchingEnd()
    {
        do
        {
            yield return null;
        } while (Mathf.Abs(_hingeJoint.angle - _launchJointSpring.targetPosition) > _acceptableAngleDelta);

        _canReload = true;
    }

    private void Launch()
    {
        if(_canLaunch == false)
            return;
        
        _hingeJoint.spring = _launchJointSpring;
        _canLaunch = false;
        StartCoroutine(WaitForLaunchingEnd());
    }

    private void Reload()
    {
        if(_canReload == false)
            return;
        
        _hingeJoint.spring = _idleJointSpring;
        _canReload = false;
        StartCoroutine(WaitForReloading());
    }
}