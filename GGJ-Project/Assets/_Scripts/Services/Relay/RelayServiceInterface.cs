using System.Collections.Generic;
using System.Threading.Tasks;
using Overgrown.Utils;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Overgrown
{
	namespace GameServices
	{
		namespace GameRelay
		{
			public class RelayServiceInterface : Singleton<RelayServiceInterface>
			{
				[Header("Netcode References")]
				[SerializeField] private UnityTransport transportLayer = null;

				#region Unity Functions
				private void Start()
				{
					transportLayer = NetworkManager.Singleton.GetComponent<UnityTransport>();
				}
				#endregion

				#region Relay Lifecycle

				/// <summary>
				/// Creates a Relay Allocation and Join Code.
				/// </summary>
				/// <param name="playerCount">How Many Players are to be Connected. (Sanitised)</param>
				/// <returns>The Allocations Join Code</returns>
				public async Task<string> CreateRelay(int playerCount)
				{
					try
					{
						Allocation allocation = await RelayService.Instance.CreateAllocationAsync(playerCount);
						string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

						UpdateTransportLayer(allocation);
						NetworkManager.Singleton.StartHost();

						return joinCode;
					}
					catch (RelayServiceException e)
					{
						Debug.LogError(e);

						throw e;
					}
				}

				/// <summary>
				/// Joins a Relay Allocation from JoinCode.
				/// </summary>
				/// <param name="joinCode">The Join Code for the Allocation</param>
				/// <returns></returns>
				public async Task JoinRelay(string joinCode)
				{
					try
					{
						JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

						UpdateTransportLayer(joinAllocation);
						NetworkManager.Singleton.StartClient();

						return;
					}
					catch (RelayServiceException e)
					{
						Debug.LogError(e);

						throw e;
					}
				}

				public async Task LeaveRelay()
				{
					try
					{
						return;
					}
					catch (RelayServiceException e)
					{
						Debug.LogError(e);

						throw e;
					}
				}

				#endregion

				private void UpdateTransportLayer(Allocation allocation)
				{
					RelayServerData newServerData = new(allocation, "dtls");

					transportLayer.SetRelayServerData(newServerData);
				}

				private void UpdateTransportLayer(JoinAllocation joinAllocation)
				{
					RelayServerData newServerData = new(joinAllocation, "dtls");

					transportLayer.SetRelayServerData(newServerData);
				}
			}
		}
	}
}