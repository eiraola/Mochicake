using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private InputAction _mouseDownInputAction;
    [SerializeField]
    private InputAction _mouseUpInputAction;
    [SerializeField]
    private UnityEvent _onLeftSwipe = new UnityEvent();
    [SerializeField]
    private UnityEvent _onRightSwipe = new UnityEvent();
    [SerializeField]
    private UnityEvent _onUpSwipe = new UnityEvent();
    [SerializeField]
    private UnityEvent _onDownSwipe = new UnityEvent();
    private Vector2 _mouseInitPosition = Vector2.zero;
    private void OnEnable()
    {
        _mouseDownInputAction.Enable();
        _mouseUpInputAction.Enable();
        _mouseDownInputAction.performed += OnMouseDownInput;
        _mouseUpInputAction.performed += OnMouseUpInput;
    }

    private void OnDisable()
    {
        _mouseDownInputAction.Disable();
        _mouseUpInputAction.Disable();
        _mouseDownInputAction.performed -= OnMouseDownInput;
        _mouseUpInputAction.performed -= OnMouseUpInput;
    }

    private void OnMouseDownInput(InputAction.CallbackContext context)
    {
        _mouseInitPosition = Mouse.current.position.ReadValue();
    }

    private void OnMouseUpInput(InputAction.CallbackContext context)
    {
        Vector2 mouseUpPosition = Mouse.current.position.ReadValue();
        Vector2 delta = mouseUpPosition - _mouseInitPosition;

        if (delta.magnitude < 50f) 
            return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                _onRightSwipe?.Invoke();
            else
                _onLeftSwipe?.Invoke();
        }
        else
        {
            if (delta.y > 0)
                _onUpSwipe?.Invoke();
            else
                _onDownSwipe?.Invoke();
        }
    }


}
