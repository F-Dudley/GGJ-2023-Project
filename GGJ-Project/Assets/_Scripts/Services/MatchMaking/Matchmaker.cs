using System.Collections.Generic;
using System.Threading.Tasks;
using Overgrown.GameServices.GameLobby;
using Overgrown.GameServices.GameRelay;
using Overgrown.Utils;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
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
				[Header("Lobby")]

				[Header("Relay")]

				[Header("Creation Settings")]
				[SerializeField] private const int maxPlayerCount = 4;

				[Header("Interface References")]
				[SerializeField] private RelayServiceInterface relayServiceInterface = null;
				[SerializeField] private LobbyServiceInterface lobbyServiceInterface = null;

				#region Unity Functions
				private void Start()
				{
					relayServiceInterface = RelayServiceInterface.Instance;
					lobbyServiceInterface = LobbyServiceInterface.Instance;
				}
				#endregion

				#region Game Lifecycle

				/// <summary>
				/// Creates a Match for the Chosen Player Amount, Allocating a Lobby and Relay.
				/// </summary>
				/// <param name="lobbyName">The Chosen Lobby Name</param>
				/// <param name="playerAmount">The Chosen Player Amount</param>
				/// <returns></returns>
				public async void CreateMatch(string lobbyName, int playerAmount)
				{
					LimitPlayerAmount(ref playerAmount);

					string joinCode = await relayServiceInterface.CreateRelay(playerAmount);

					await lobbyServiceInterface.CreateLobby(lobbyName, playerAmount, joinCode);

					NetworkManager.Singleton.SceneManager.LoadScene("GameLobby", UnityEngine.SceneManagement.LoadSceneMode.Single);
				}

				public async void QuickJoinMatch()
				{
					await lobbyServiceInterface.QuickJoinLobby();
					await relayServiceInterface.JoinRelay(lobbyServiceInterface.GetJoinCode());
				}

				public async void JoinMatch(string lobbyName)
				{
					await lobbyServiceInterface.JoinLobby(lobbyName);
					await relayServiceInterface.JoinRelay(lobbyServiceInterface.GetJoinCode());
				}

				public async void LeaveMatch()
				{
					await lobbyServiceInterface.LeaveLobby();
					await relayServiceInterface.LeaveRelay();

					NetworkManager.Singleton.Shutdown();
				}

				#endregion

				/// <summary>
				/// Sanitises the Player Amount, so it cannot go out of max bounds.
				/// </summary>
				private void LimitPlayerAmount(ref int playerAmount)
				{
					if (playerAmount > maxPlayerCount) playerAmount = maxPlayerCount;
				}
			}
		}
	}
}