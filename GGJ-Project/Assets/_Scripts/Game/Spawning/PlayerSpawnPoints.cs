using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Overgrown
{
	namespace GameCore
	{
		namespace Spawning
		{
			public class PlayerSpawnPoints : MonoBehaviour
			{

				static PlayerSpawnPoints s_Instance;
				public static PlayerSpawnPoints Instance
				{
					get
					{
						if (s_Instance == null)
						{
							s_Instance = FindObjectOfType<PlayerSpawnPoints>();
						}

						return s_Instance;
					}
				}

				[Header("General Spawning")]
				[SerializeField] private List<Transform> spawnPoints = new List<Transform>();

				#region Unity Functions
				private new void Awake()
				{

				}

				private void OnDestroy()
				{

				}
				#endregion

				public Transform GetSpawnPoint() => spawnPoints[Random.Range(0, spawnPoints.Count)];

				public List<Transform> GetSpawnPoints()
				{
					if (spawnPoints.Count == 0)
					{
						Debug.LogError("No spawn points found in Container!");

						return null;
					}

					return spawnPoints;
				}
			}
		}
	}
}