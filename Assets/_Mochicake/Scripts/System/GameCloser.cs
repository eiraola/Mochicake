using UnityEngine;
using UnityEngine.InputSystem;

public class GameCloser : MonoBehaviour
{
    [SerializeField]
    private InputAction _closeInput;

    private void OnEnable()
    {
        _closeInput.Enable();
        _closeInput.performed += CloseGame;
    }

    private void CloseGame(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDisable()
    {
        _closeInput.Disable();
        _closeInput.performed -= CloseGame;
    }
}
