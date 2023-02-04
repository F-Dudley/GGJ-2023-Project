using System.Collections;
using System.Collections.Generic;
using Overgrown.GameCore.Input;
using Overgrown.GameCore.Player;
using UnityEngine;

namespace Overgrown
{
	namespace GameCore
	{
		namespace Player
		{
			public class PlayerController : MonoBehaviour
			{
				[Header("Movement")]
				[SerializeField] private float moveForce = 5.0f;
				[SerializeField] private float dashForce = 3.0f;

				[SerializeField] private float maxSpeed = 5f;

				[SerializeField] private Vector3 moveVelocity = Vector3.zero;

				[Header("Pick Up")]

				[Header("Dashing")]

				[Header("Ground Check")]
				[SerializeField] private bool isGrounded = false;
				[SerializeField] private float groundCheckDistance = 0.5f;
				[SerializeField] private LayerMask groundLayers;

				[Header("Player References")]
				[SerializeField] private Transform playerBody = null;

				[SerializeField] private CharacterController controller = null;
				[SerializeField] private InputReader input = null;

				#region Unity Functions
				private void Awake()
				{
					controller = GetComponent<CharacterController>();

				}

				private void Start()
				{
					BindEventHandlers();

					//PlayerManager.Instance.AddPlayerToTargetGroup(transform);
				}

				private void OnDestroy()
				{
					UnBindEventHandlers();
				}

				private void FixedUpdate()
				{
					isGrounded = GroundCheck();

					Movement();
					BodyRotation();

					ApplyGravityToPlayer();
				}

				private void OnDrawGizmos()
				{
					Gizmos.DrawRay(transform.position + (Vector3.up * 0.05f), Vector3.down * groundCheckDistance);
				}
				#endregion

				#region Movement
				private void Movement()
				{
					moveVelocity = Vector3.ClampMagnitude(moveVelocity, maxSpeed);

					controller.Move(moveVelocity * moveForce * Time.deltaTime);
				}

				private void BodyRotation()
				{
					//playerBody.rotation = Quaternion.Slerp(playerBody.rotation, Quaternion.LookRotation(moveVelocity, Vector3.forward), 0.25f);
					//playerBody.eulerAngles = new Vector3(0, playerBody.eulerAngles.y, 0);

					if (moveVelocity.x != 0 || moveVelocity.z != 0)
					{
						playerBody.rotation = Quaternion.Slerp(playerBody.rotation, Quaternion.LookRotation(moveVelocity, Vector3.up), 0.2f);
						playerBody.rotation = Quaternion.Euler(0, playerBody.rotation.eulerAngles.y, 0);
					}
				}

				private bool GroundCheck()
				{
					return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore);
				}

				private void ApplyGravityToPlayer()
				{
					if (!isGrounded)
					{
						moveVelocity.y -= PlayerConstants.GRAVITY_STRENGTH * Time.deltaTime;
					}
					else
					{
						moveVelocity.y = -1;
					}
				}
				#endregion

				#region InputHandlers
				private void BindEventHandlers()
				{
					input.MoveEvent += HandleMovement;
					input.PickUpEvent += HandlePickUp;
					input.ThrowEvent += HandleThrow;
					input.DashEvent += HandleDash;
				}

				private void UnBindEventHandlers()
				{
					input.MoveEvent -= HandleMovement;
					input.PickUpEvent -= HandlePickUp;
					input.ThrowEvent -= HandleThrow;
					input.DashEvent -= HandleDash;
				}

				private void HandleMovement(Vector2 moveInput)
				{
					moveVelocity = (GetCameraForward() * moveInput.y) + (GetCameraRight() * moveInput.x);
				}

				private void HandlePickUp()
				{

				}

				private void HandleThrow()
				{

				}

				private void HandleDash()
				{

				}
				#endregion

				#region Utils
				private Vector3 GetCameraForward()
				{
					Vector3 forward = Camera.main.transform.forward;
					forward.y = 0;

					return forward.normalized;
				}

				private Vector3 GetCameraRight()
				{
					Vector3 right = Camera.main.transform.right;
					right.y = 0;

					return right;
				}
				#endregion
			}
		}
	}
}