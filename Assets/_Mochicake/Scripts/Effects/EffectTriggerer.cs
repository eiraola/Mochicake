using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerer : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;


    public void OnGroundedStateChanged(EGroundState state)
    {
        _particleSystem.Play();
    }
}
