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

				[SerializeField] private Vector3 moveDir = new();

				[Header("Pick Up")]

				[Header("Dashing")]

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

				private void Update()
				{
					moveDir = Vector3.ClampMagnitude(moveDir, 5f);
					controller.Move(moveDir * Time.deltaTime);

					if (!controller.isGrounded) ApplyGravityToPlayer();
				}

				private void FixedUpdate()
				{

				}
				#endregion

				#region Movement
				private void ApplyGravityToPlayer()
				{

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
					moveDir += new Vector3(moveInput.x, 0, moveInput.y);
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
			}
		}
	}
}