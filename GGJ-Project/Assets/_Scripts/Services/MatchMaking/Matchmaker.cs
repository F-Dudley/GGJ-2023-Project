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
		namespace Matchmaking
		{
			public class Matchmaker : Singleton<Matchmaker>
			{
				[Header("Match Settings")]
				[SerializeField] private bool isHost = false;
				[SerializeField] private string joinCode = null;

				[Header("Relay References")]
				//[SerializeField] private Allocation allocation = null;
				[SerializeField] private UnityTransport clientTransportLayer = null;

				#region Unity Functions
				private void Start()
				{
					clientTransportLayer = NetworkManager.Singleton.GetComponent<UnityTransport>();
				}
				#endregion

				#region Relay Lifecycle
				private async void CreateRelay(int maxPlayers)
				{
					try
					{
						Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

						joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
						isHost = true;

						RelayServerData relayData = new(allocation, "dtls");

						clientTransportLayer.SetRelayServerData(relayData);

						NetworkManager.Singleton.StartHost();
					}
					catch (RelayServiceException e)
					{
						Debug.LogError(e);
						throw e;
					}
				}

				private async void JoinRelay(string joinCode)
				{
					try
					{
						JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

						RelayServerData relayData = new(joinAllocation, "dtls");

						clientTransportLayer.SetRelayServerData(relayData);

						NetworkManager.Singleton.StartClient();
					}
					catch (RelayServiceException e)
					{
						Debug.LogError(e);
						throw e;
					}
				}

				private async void LeaveRelay()
				{
					isHost = false;
				}
				#endregion

				#region Relay Events

				#endregion
			}
		}
	}
}