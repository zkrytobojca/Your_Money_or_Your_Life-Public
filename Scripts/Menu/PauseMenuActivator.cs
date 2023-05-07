using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuActivator : MonoBehaviour
{
    [Header("Pause Menu Game Object")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Actions")]
    [SerializeField] private InputActionAsset _inputActions;

    private bool isPaused = false;

    private InputActionMap _basicActionMap;
    private InputAction _pauseAction;

    private CursorManager _cursorManager;

    private void Awake()
    {
        _basicActionMap = _inputActions.FindActionMap("Basic");
        _pauseAction = _basicActionMap.FindAction("Pause");

        _cursorManager = FindObjectOfType<CursorManager>();
    }

    private void OnEnable()
    {
        _basicActionMap.Enable();
        _pauseAction.performed += OnPause;
    }

    private void OnDisable()
    {
        _basicActionMap.Disable();
        _pauseAction.performed -= OnPause;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(_basicActionMap == null) _inputActions.FindActionMap("Basic");
        if(_pauseAction == null) _basicActionMap.FindAction("Pause");
        if (_cursorManager == null) FindObjectOfType<CursorManager>();

        Pause();
    }

    public void Pause()
    {
        isPaused = !isPaused;
        DisableAllChildren();
        pauseMenu.SetActive(isPaused);
        _cursorManager.LockCursor(!isPaused);
        _cursorManager.HideCursor(!isPaused);
        DisableActionMaps(isPaused);
    }

    private void DisableActionMaps(bool doDisable)
    {
        // TODO
        if (doDisable)
        {
            _inputActions.Disable();
            _basicActionMap.Enable();
        }
        else
        {
            _inputActions.Enable();
        }
    }

    private void DisableAllChildren()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
