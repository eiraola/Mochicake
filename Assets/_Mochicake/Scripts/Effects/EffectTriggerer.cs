using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerer : MonoBehaviour
{
    [SerializeField]
    private AudioClip _audioClipJump;
    [SerializeField]
    private AudioClip _audioClipAcro;
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private AudioSource _audioSource;

    /// <summary>
    /// Plays effects when the player changes from ground to air
    /// </summary>
    /// <param name="state"></param>
    public void OnGroundedStateChanged(EGroundState state)
    {
        _particleSystem.Play();
        _audioSource.PlayOneShot(_audioClipJump);
    }

    /// <summary>
    /// Plays the effects when the player makes an acrobacy
    /// </summary>
    public void OnAcroDone()
    {
        _audioSource.PlayOneShot(_audioClipAcro);
    }
}
