using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EGroundState
{
    Ground,
    Air
}
public class GroundedChecker : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<EGroundState> _onStateChanged = new UnityEvent<EGroundState>();
    private EGroundState _currentState = EGroundState.Ground;
    RaycastHit _hit;

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void CheckGround()
    {

        if (Physics.Raycast(transform.position + transform.up * 0.3f, -transform.up, out _hit, 1.0f))
        {
           if(_hit.transform.CompareTag(Constants.GROUND_TAG) && _currentState != EGroundState.Ground)
            {
                _currentState = EGroundState.Ground;
                _onStateChanged?.Invoke(_currentState);
                return;
            }

           if(_hit.transform.CompareTag(Constants.AIR_TAG) && _currentState != EGroundState.Air){

                _currentState = EGroundState.Air;
                _onStateChanged?.Invoke(_currentState);
            }
        }
    }
}
