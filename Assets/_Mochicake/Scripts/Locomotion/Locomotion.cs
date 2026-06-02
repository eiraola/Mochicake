using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Locomotion : MonoBehaviour
{
    [SerializeField]
    private float _maxVelocity = 10f;
    [SerializeField]
    private float _impulseStrength = 10f;
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
    private UnityEvent _onAccelerate = new UnityEvent();
    private float _linearVelocity = 0f;
    private bool _isGrounded = true;
    private RaycastHit _backHit;
    private RaycastHit _frontHit;

    /// <summary>
    /// Increases the player speed clamping it to a max value
    /// </summary>
    public void Accelerate()
    {
        if (!_isGrounded)
        {
            return;
        }

        _linearVelocity += Mathf.Sign(_linearVelocity) * _impulseStrength;
        _linearVelocity = Mathf.Clamp(_linearVelocity, - _maxVelocity, _maxVelocity);
        _onAccelerate?.Invoke();

    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        RotateToGround();
        _isGrounded = StickToGround();
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
            if (hit.transform.CompareTag(Constants.AIR_TAG))
            {
                return false;
            }
            return true;
        }

        return false;
    }

    private void RotateToGround()
    {
        if(!Physics.Raycast(transform.TransformPoint(_frontRaycastPosition) + transform.up * 0.2f, -transform.up, out _frontHit, 1f, ~_ignoredLayers))
        {
            return;
        }

        if(!Physics.Raycast(transform.TransformPoint(_backRaycastPosition) + transform.up * 0.2f, -transform.up, out _backHit, 1f, ~_ignoredLayers))
        {
            return;
        }
        
        Vector3 averageNormal = (_frontHit.normal + _backHit.normal).normalized;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, averageNormal);
        transform.rotation = Quaternion.LookRotation(forward, averageNormal);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_frontRaycastPosition + transform.position, 0.02f);
        Gizmos.DrawWireSphere(_backRaycastPosition +transform.position, 0.02f);
    }
}
