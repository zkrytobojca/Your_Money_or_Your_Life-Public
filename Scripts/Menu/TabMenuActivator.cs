using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TabMenuActivator : MonoBehaviour
{
    [Header("Tab Menu Game Object")]
    [SerializeField] private GameObject tabMenu;

    [Header("Actions")]
    [SerializeField] private InputActionAsset _inputActions;

    private bool isActive = false;

    private InputActionMap _basicActionMap;
    private InputAction _tabAction;

    private void OnEnable()
    {
        Initialize();

        _basicActionMap.Enable();
        _tabAction.performed += OnTab;
        _tabAction.canceled += OnTab;
    }

    private void OnDisable()
    {
        _basicActionMap.Disable();
        _tabAction.performed -= OnTab;
        _tabAction.canceled -= OnTab;
    }

    private void Initialize()
    {
        if (tabMenu == null) tabMenu = GetComponentInChildren<TabMenu>().gameObject;

        _basicActionMap = _inputActions.FindActionMap("Basic");
        _tabAction = _basicActionMap.FindAction("Tab");
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (tabMenu == null) tabMenu = GetComponentInChildren<TabMenu>().gameObject;
        if (_basicActionMap == null) _inputActions.FindActionMap("Basic");
        if (_tabAction == null) _basicActionMap.FindAction("Tab");

        ShowRanking();
    }

    public void ShowRanking()
    {
        isActive = !isActive;
        tabMenu.SetActive(isActive);
    }
}
