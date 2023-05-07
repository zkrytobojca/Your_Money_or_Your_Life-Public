using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using Photon.Pun;

namespace StarterAssets
{
	public class PlayerCharacterInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;
		public bool isAbleToLookAround = true;

		[Header("Actions")]
		[SerializeField] private InputActionAsset _inputActions;

		[Header("Photon")]
		[SerializeField] private PhotonView photonView;

		private InputActionMap _playerActionMap;
		private InputAction _moveAction;
		private InputAction _sprintAction;
		private InputAction _lookAction;
		private InputAction _jumpAction;

		private CursorManager _cursorManager;

		private void Awake()
        {
			if (!photonView.IsMine) return;
			_playerActionMap = _inputActions.FindActionMap("Player");
			_moveAction = _playerActionMap.FindAction("Move");
			_sprintAction = _playerActionMap.FindAction("Sprint");
			_lookAction = _playerActionMap.FindAction("Look");
			_jumpAction = _playerActionMap.FindAction("Jump");

			_cursorManager = FindObjectOfType<CursorManager>();
		}

        private void Start()
        {
			if (!photonView.IsMine) return;
			_cursorManager.HideCursor(true);
			_cursorManager.LockCursor(true);
		}

        private void OnEnable()
        {
			if (!photonView.IsMine) return;
			_playerActionMap.Enable();
			_moveAction.performed += context => OnMove(context);
			_moveAction.canceled += context => OnMove(context);
			_sprintAction.performed += context => OnSprint(context);
			_sprintAction.canceled += context => OnSprint(context);
			_lookAction.performed += context => OnLook(context);
			_lookAction.canceled += context => OnLook(context);
			_jumpAction.performed += context => OnJump(context);
			_jumpAction.canceled += context => OnJump(context);
		}

        private void OnDisable()
        {
			if (!photonView.IsMine) return;
			_playerActionMap.Disable();
			_moveAction.performed -= context => OnMove(context);
			_moveAction.canceled -= context => OnMove(context);
			_sprintAction.performed -= context => OnSprint(context);
			_sprintAction.canceled -= context => OnSprint(context);
			_lookAction.performed -= context => OnLook(context);
			_lookAction.canceled -= context => OnLook(context);
			_jumpAction.performed -= context => OnJump(context);
			_jumpAction.canceled -= context => OnJump(context);
		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputAction.CallbackContext context)
		{
			MoveInput(context.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			if(isAbleToLookAround)
            {
				LookInput(context.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			JumpInput(context.performed);
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			SprintInput(context.performed);
		}
#endif

        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
	}
	
}