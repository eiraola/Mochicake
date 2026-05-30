using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locomotion : MonoBehaviour
{

    [SerializeField]
    private float _linearVelocity = 0f;
    [SerializeField]
    private LayerMask _ignoredLayers;
    [SerializeField]
    private Vector3 _frontRaycastPosition;
    [SerializeField]
    private Vector3 _backRaycastPosition;
    [SerializeField]
    private float _rotationSpeed = 1.0f;
    [SerializeField]
    private float _speedLoss = 1.0f;
    [Range(0.0f, 0.5f)]
    [SerializeField]
    private float _frictionFactor = 1.0f;
    [SerializeField]
    private InputAction _Accelerate;
    private Vector3 _lastFrontHitPoint;
    private Vector3 _lastBackHitPoint;

    private void Start()
    {
        _Accelerate.Enable();
        _Accelerate.performed += Accelerate;
    }

    private void Accelerate(InputAction.CallbackContext context)
    {
        _linearVelocity += Mathf.Sign(_linearVelocity) * 2;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        RotateToGround();
        bool isGrounded = StickToGround();
        _linearVelocity -=  (Vector3.Dot(transform.forward, Vector3.up) * _speedLoss * Time.deltaTime);
        _linearVelocity = Mathf.MoveTowards(
            _linearVelocity,
            0f,
            _frictionFactor * Time.deltaTime

        );
        transform.position += transform.forward * _linearVelocity * Time.deltaTime;
    }

    private bool StickToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.2f, -transform.up, out hit, 1f, ~_ignoredLayers))
        {
            transform.position = hit.point;
            if (hit.transform.CompareTag("Air"))
            {
                return false;
            }
            return true;
        }

        return false;
    }

    private void RotateToGround()
    {
        RaycastHit backHit;
        RaycastHit frontHit;
        if (Physics.Raycast(transform.TransformPoint(_frontRaycastPosition) + transform.up * 0.2f, -transform.up, out frontHit, 1f, ~_ignoredLayers)) {
            _lastFrontHitPoint = frontHit.normal;
        }

        if (Physics.Raycast(transform.TransformPoint(_backRaycastPosition) + transform.up * 0.2f, -transform.up, out backHit, 1f, ~_ignoredLayers))
        {
            _lastFrontHitPoint = backHit.normal;
        }
         if(_lastFrontHitPoint == null || _lastBackHitPoint == null)
        {
            return;
        }

        Vector3 averageNormal = (_lastFrontHitPoint + _lastBackHitPoint).normalized;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, averageNormal);
        transform.rotation = Quaternion.LookRotation(forward, averageNormal);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_frontRaycastPosition + transform.position, 0.02f);
        Gizmos.DrawWireSphere(_backRaycastPosition +transform.position, 0.02f);
    }
}
