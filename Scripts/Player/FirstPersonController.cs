using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using TMPro;
using Photon.Pun;
using System;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Stamina))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerCharacterInputs))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;
		public GameObject fpsCam;

		[Header("Views & Models")]
		public GameObject TP_Model;
		public GameObject FP_Model;

		[Header("Sound")]
		public float walkSoundFrequency = 1f;
		public float sprintSoundFrequency = 0.5f;

		[Header("Photon")]
		public PhotonView photonView;
		public TMP_Text nameBox;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlendZ;
		private float _animationBlendX;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeedZ;
		private int _animIDSpeedX;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;

		private Animator _animator;
		private CharacterController _controller;
		private PlayerCharacterInputs _input;
		private Stamina _stamina;

		private const float _threshold = 0.01f;

		private bool _hasAnimator;

		public static event Action<Vector3> move;
		private float _moveSignalCooldown = 0f;

        private void OnEnable()
        {
			OptionsMenu.updateMouseSensitivity += UpdateRotationSpeed;
		}

        private void OnDisable()
        {
			OptionsMenu.updateMouseSensitivity -= UpdateRotationSpeed;
		}

        private void Awake()
		{
			if (photonView.IsMine)
			{
				SetLayerWithChildren(gameObject, 9);
				UpdateRotationSpeed();
			}
			else
            {
				fpsCam.SetActive(false);
            }
		}

		private void Start()
		{
			nameBox.text = photonView.Owner.NickName;
			if(photonView.IsMine)
            {
				_hasAnimator = TryGetComponent(out _animator);
				_controller = GetComponent<CharacterController>();
				_input = GetComponent<PlayerCharacterInputs>();
				_stamina = GetComponent<Stamina>();

				AssignAnimationIDs();

				// reset our timeouts on start
				_jumpTimeoutDelta = JumpTimeout;
				_fallTimeoutDelta = FallTimeout;
			}
		}

		private void Update()
		{
			if (photonView.IsMine)
			{
				_hasAnimator = TryGetComponent(out _animator);

				JumpAndGravity();
				GroundedCheck();
				Move();

				if(_moveSignalCooldown > 0f && Grounded)
                {
					_moveSignalCooldown -= Time.deltaTime;
					if (_moveSignalCooldown < 0f) _moveSignalCooldown = 0f;
				}
			}
		}

		private void LateUpdate()
		{
			if (photonView.IsMine)
			{
				CameraRotation();
			}
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeedZ = Animator.StringToHash("SpeedZ");
			_animIDSpeedX = Animator.StringToHash("SpeedX");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * Time.deltaTime;
				_rotationVelocity = _input.look.x * RotationSpeed * Time.deltaTime;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = MoveSpeed;

			if (_input.sprint && _stamina.getCurrentStamina() > 0f) targetSpeed = SprintSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			if(_input.move.y > 0)
            {
				_animationBlendZ = Mathf.Lerp(_animationBlendZ, targetSpeed, Time.deltaTime * SpeedChangeRate);
			}
			else if(_input.move.y == 0)
            {
				_animationBlendZ = Mathf.Lerp(_animationBlendZ, 0, Time.deltaTime * SpeedChangeRate);
			}
			else
            {
				_animationBlendZ = Mathf.Lerp(_animationBlendZ, -targetSpeed, Time.deltaTime * SpeedChangeRate);
			}

			if (_input.move.x > 0)
			{
				_animationBlendX = Mathf.Lerp(_animationBlendX, targetSpeed, Time.deltaTime * SpeedChangeRate);
			}
			else if (_input.move.x == 0)
			{
				_animationBlendX = Mathf.Lerp(_animationBlendX, 0, Time.deltaTime * SpeedChangeRate);
			}
			else
			{
				_animationBlendX = Mathf.Lerp(_animationBlendX, -targetSpeed, Time.deltaTime * SpeedChangeRate);
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
				if(_moveSignalCooldown == 0f)
                {
					move?.Invoke(transform.position);
					if (_input.sprint && _stamina.getCurrentStamina() > 0f) _moveSignalCooldown = sprintSoundFrequency;
					else _moveSignalCooldown = walkSoundFrequency;
				}
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeedZ, _animationBlendZ);
				_animator.SetFloat(_animIDSpeedX, _animationBlendX);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		private static void SetLayerWithChildren(GameObject go, int layerNumber)
		{
			foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
			{
				trans.gameObject.layer = layerNumber;
			}
		}

		private void UpdateRotationSpeed()
        {
			if (PlayerPrefs.HasKey("mouse_sensitivity")) RotationSpeed = PlayerPrefs.GetFloat("mouse_sensitivity");
			else RotationSpeed = 0.5f;
		}
	}
}