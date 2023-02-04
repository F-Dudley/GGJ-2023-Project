using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Overgrown
{
	namespace GameCore
	{
		namespace Input
		{
			[Serializable]
			[CreateAssetMenu(fileName = "InputReader", menuName = "Overgrown/Input/InputReader", order = 0)]
			public class InputReader : ScriptableObject, PlayerInput.IPlayerActions, PlayerInput.IMenuActions
			{
				[Header("General References")]
				[SerializeField] private PlayerInput playerInput = null;

				// [Header("Player Actions")]
				public event Action<Vector2> MoveEvent;

				public event Action PickUpEvent;
				public event Action ThrowEvent;
				public event Action DashEvent;

				// [Header("Menu Actions")]
				public event Action PauseEvent;

				#region Unity Functions
				private void OnEnable()
				{
					if (playerInput == null)
					{
						playerInput = new();

						playerInput.Player.Enable();

						playerInput.Player.SetCallbacks(this);
						playerInput.Menu.SetCallbacks(this);
					}
				}

				private void OnDisable()
				{

				}
				#endregion

				public void SetGameplay()
				{
					playerInput.Player.Enable();
					playerInput.Menu.Disable();
				}

				public void SetMenu()
				{
					playerInput.Player.Disable();
					playerInput.Menu.Enable();
				}

				#region Player Actions
				public void OnMovement(InputAction.CallbackContext context)
				{
					MoveEvent?.Invoke(context.ReadValue<Vector2>());
				}

				public void OnPickUp(InputAction.CallbackContext context)
				{
					if (context.phase == InputActionPhase.Started)
					{
						PickUpEvent?.Invoke();
					}
				}

				public void OnThrow(InputAction.CallbackContext context)
				{
					if (context.phase == InputActionPhase.Started)
					{
						ThrowEvent?.Invoke();
					}
				}

				public void OnDash(InputAction.CallbackContext context)
				{
					if (context.phase == InputActionPhase.Started)
					{
						DashEvent?.Invoke();
					}
				}
				#endregion

				#region Menu Actions

				#endregion
			}
		}
	}
}