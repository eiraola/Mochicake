using System.Collections;
using UnityEngine;

public class Acrobatics : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _spinDuration = 0.5f;
    [SerializeField]
    private Transform _targetTransform;
    private bool _canDoAcrobatics = false;
    private Coroutine _spinCoroutine;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Swaps the availability of doing Acrobatics.
    /// Air = true
    /// Ground = false
    /// </summary>
    /// <param name="state"></param>
    public void SwapAcrobaticsState(EGroundState state)
    {
        if (state == EGroundState.Air)
        {
            _canDoAcrobatics = true;
            return;
        }
        _canDoAcrobatics = false;
        PlayIdleAnimation();
    }

    public void PlayTrickOne()
    {
        PlayAnimation(Constants.TRICK_ONE_ANIM);
    }

    public void PlayTrickTwo()
    {
        PlayAnimation(Constants.TRICK_TWO_ANIM);
    }

    public void PlayTrickThree()
    {
        PlayAnimation(Constants.TRICK_THREE_ANIM);
    }

    public void PlayTrickFour()
    {
        PlayAnimation(Constants.TRICK_FOUR_ANIM);
    }

    /// <summary>
    /// Plays an animation only if the player is in the air
    /// </summary>
    /// <param name="animName"></param>
    public void PlayAnimation(string animName)
    {
       if (!_canDoAcrobatics)
       {
           return;
       }

        if (!_animator)
        {
            return;
        }

        _animator.SetTrigger(animName);
        LaunchSpinCoroutine();
    }


    private void PlayIdleAnimation()
    {
        if (!_animator)
        {
            return;
        }

        _animator.SetTrigger(Constants.IDLE_ANIM);
    }

    private void LaunchSpinCoroutine()
    {
        if (_spinCoroutine != null)
        {
            StopCoroutine(_spinCoroutine);
        }

        _spinCoroutine = StartCoroutine(Spin360());
    }


    private IEnumerator Spin360()
    {
        float elapsedTime = 0f;

        Quaternion initialLocalRotation = Quaternion.identity;
        float t = 0f;
        float easedT = 0f;
        float angle = 0f;
        while (elapsedTime < _spinDuration)
        {
            elapsedTime += Time.deltaTime;

            t = Mathf.Clamp01(elapsedTime / _spinDuration);
            easedT = -(Mathf.Cos(Mathf.PI * t) - 1f) * 0.5f;
            angle = Mathf.Lerp(0f, 360f, easedT);

            _targetTransform.localRotation =
                initialLocalRotation * Quaternion.Euler(0f, angle, 0f);

            yield return null;
        }

        _targetTransform.localRotation = initialLocalRotation;
        _spinCoroutine = null;
    }


}
