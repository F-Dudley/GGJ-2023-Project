using System.Collections.Generic;
using Cinemachine;
using Overgrown.GameCore.Spawning;
using Overgrown.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overgrown
{
	namespace GameCore
	{
		namespace Player
		{
			public class PlayerManager : NetworkSingleton<PlayerManager>
			{
				[Header("Player References")]
				[SerializeField] private GameObject playerPrefab = null;
				public bool PlayerSpawnDone { get; private set; }

				[Header("Game References")]
				private List<Transform> cachedSpawnPoints = null;

				#region Unity Functions
				private new void Awake()
				{
					base.Awake();
				}

				public void Start()
				{
					BindEvents();
				}

				public void OnDestroy()
				{
					UnBindEvents();
				}
				#endregion

				private void BindEvents()
				{
					SceneManager.sceneLoaded += OnLoadSceneEvent;
					NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadSceneEventCompleted;

					SceneManager.sceneLoaded += OnLoadSceneEvent;
				}

				private void UnBindEvents()
				{
					//NetworkManager.Singleton.SceneManager.OnLoad -= OnLoadSceneEvent;
					NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadSceneEventCompleted;
				}

				private void OnLoadSceneEvent(Scene scene, LoadSceneMode loadSceneMode)
				{
					Debug.Log("SCENE LOADED CODE");
				}

				private void OnLoadSceneEventCompleted(ulong clientName, string sceneName, LoadSceneMode loadSceneMode)
				{
					Debug.LogError("OnLoadSceneEventCompleted: " + sceneName + " " + loadSceneMode);
					cachedSpawnPoints = PlayerSpawnPoints.Instance.GetSpawnPoints();

					if (!PlayerSpawnDone && loadSceneMode == LoadSceneMode.Single)
					{
						PlayerSpawnDone = true;

						foreach (var connectedClient in NetworkManager.Singleton.ConnectedClients)
						{
							Debug.Log("Spawning player for client: " + connectedClient.Key);
							NetworkObject playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(connectedClient.Key);
						}
					}
				}

				private void SpawnPlayer(ulong clientId)
				{
					int spawnPointIndex = Random.Range(0, cachedSpawnPoints.Count);

					Transform spawnPoint = cachedSpawnPoints[spawnPointIndex];
					cachedSpawnPoints.RemoveAt(spawnPointIndex);

					Transform instantiatedPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation).transform;

					NetworkObject networkObject = instantiatedPlayer.GetComponent<NetworkObject>();
					networkObject.SpawnWithOwnership(clientId, true);
				}
			}
		}
	}
}