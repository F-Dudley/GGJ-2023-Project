using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

#if UNITY_EDITOR
using ParrelSync;
#endif

namespace Overgrown
{
	namespace GameServices
	{
		public static class Authentication
		{
			public static string PlayerId { get; private set; }

			public static async Task Login()
			{
				// Check if the user is already logged in.
				if (AuthenticationService.Instance.IsSignedIn) return;

				// Login the user.
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

				PlayerId = AuthenticationService.Instance.PlayerId;

#if UNITY_EDITOR
				Debug.Log($"Player Logged In. Player ID: {PlayerId}");
#endif
			}

			public static void Logout()
			{
				// Check if the user is already logged out.
				if (!AuthenticationService.Instance.IsSignedIn) return;

#if UNITY_EDITOR
				Debug.Log($"Player Logged Out. Player ID: {PlayerId}");
#endif
				// Logout the user.
				AuthenticationService.Instance.SignOut();
			}
		}
	}
}