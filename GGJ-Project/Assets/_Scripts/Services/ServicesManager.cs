using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Overgrown.Utils;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using ParrelSync;
#endif

namespace Overgrown
{
	namespace GameServices
	{
		public class ServicesManager : Singleton<ServicesManager>
		{
			#region Unity Functions

			private new async void Awake()
			{
				base.Awake();

				if (UnityServices.State != ServicesInitializationState.Uninitialized) return;

				InitializationOptions serviceInitOptions = new();

#if UNITY_EDITOR
				// Specify that Editor Clients are differnt in ParrelSync.
				if (ClonesManager.IsClone()) serviceInitOptions.SetProfile(ClonesManager.GetArgument());
				else serviceInitOptions.SetProfile("Primary");
#endif

				// Initialize the Unity Services.
				await UnityServices.InitializeAsync(serviceInitOptions);
				await Authentication.Login();

				SceneManager.LoadScene("Home", LoadSceneMode.Single);
			}

			private void OnDestroy()
			{
				Authentication.Logout();
			}

#if UNITY_EDITOR
			#region Unity Debugging
			public void OnGui()
			{

			}
			#endregion
#endif
			#endregion
		}
	}
}