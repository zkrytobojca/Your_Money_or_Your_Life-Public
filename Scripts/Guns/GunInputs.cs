using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunInputs : MonoBehaviour
{
	[Header("Gun Input Values")]
	public bool shoot;
	public bool scope;
	public bool reload;
	public Vector2 switchWeapon;

	[Header("Actions")]
	[SerializeField] private InputActionAsset _inputActions;

	[Header("Photon")]
	[SerializeField] private PhotonView photonView;

	private InputActionMap _gunActionMap;
	private InputAction _shootAction;
	private InputAction _scopeAction;
	private InputAction _reloadAction;
	private InputAction _switchWeaponAction;

	private void Awake()
	{
		if (!photonView.IsMine) return;
		_gunActionMap = _inputActions.FindActionMap("Gun");
		_shootAction = _gunActionMap.FindAction("Shoot");
		_scopeAction = _gunActionMap.FindAction("Scope");
		_reloadAction = _gunActionMap.FindAction("Reload");
		_switchWeaponAction = _gunActionMap.FindAction("Switch");
	}

	private void OnEnable()
	{
		if (!photonView.IsMine) return;
		_gunActionMap.Enable();
		_shootAction.performed += context => OnShoot(context);
		_shootAction.canceled += context => OnShoot(context);
		_scopeAction.performed += context => OnScope(context);
		_scopeAction.canceled += context => OnScope(context);
		_reloadAction.performed += context => OnReload(context);
		_reloadAction.canceled += context => OnReload(context);
		_switchWeaponAction.performed += context => OnSwitchWeapon(context);
		_switchWeaponAction.canceled += context => OnSwitchWeapon(context);
	}

	private void OnDisable()
	{
		if (!photonView.IsMine) return;
		_gunActionMap.Disable();
		_shootAction.performed -= context => OnShoot(context);
		_shootAction.canceled -= context => OnShoot(context);
		_scopeAction.performed -= context => OnScope(context);
		_scopeAction.canceled -= context => OnScope(context);
		_reloadAction.performed -= context => OnReload(context);
		_reloadAction.canceled -= context => OnReload(context);
		_switchWeaponAction.performed -= context => OnSwitchWeapon(context);
		_switchWeaponAction.canceled -= context => OnSwitchWeapon(context);
	}

	public void OnShoot(InputAction.CallbackContext context)
	{
		ShootInput(context.performed);
	}

	public void OnScope(InputAction.CallbackContext context)
	{
		ScopeInput(context.performed);
	}

	public void OnReload(InputAction.CallbackContext context)
	{
		ReloadInput(context.performed);
	}

	public void OnSwitchWeapon(InputAction.CallbackContext context)
	{
		SwitchWeaponInput(context.ReadValue<Vector2>());
	}

	public void ShootInput(bool newShootInput)
    {
		shoot = newShootInput;
    }

	public void ScopeInput(bool newScopeInput)
	{
		scope = newScopeInput;
	}

	public void ReloadInput(bool newReloadInput)
	{
		reload = newReloadInput;
	}

	public void SwitchWeaponInput(Vector2 newSwitchWeapon)
	{
		switchWeapon = newSwitchWeapon;
	}
}
