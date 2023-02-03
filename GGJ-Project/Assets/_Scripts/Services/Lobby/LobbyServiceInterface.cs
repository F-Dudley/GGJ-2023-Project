using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Overgrown.Utils;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Overgrown
{
	namespace GameServices
	{
		namespace GameLobby
		{
			/// <summary>
			/// Class for interacting with the Unity lobby service.
			/// </summary>
			public class LobbyServiceInterface : Singleton<LobbyServiceInterface>
			{

				[Header("Main Lobby References")]
				[SerializeField] private Lobby currentLobby;

				private CancellationTokenSource heartbeatSource, lobbyRefreshSource;

				[Header("Query Settings")]
				private const int maxQueryLobbiesCount = 20;

				private readonly List<QueryFilter> queryFilters;
				private readonly List<QueryOrder> queryOrders;

				[Header("Heartbeat Settings")]
				[SerializeField] private const int heartbeatInterval = 20;

				[Header("Refresh Settings")]
				[SerializeField] private const int lobbyRefreshRate = 2;

				public LobbyServiceInterface()
				{
					queryFilters = new List<QueryFilter>
					{
						new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
						new(QueryFilter.FieldOptions.IsLocked, "0", QueryFilter.OpOptions.EQ),
					};
					queryOrders = new List<QueryOrder>
					{
						new(asc: false, field: QueryOrder.FieldOptions.AvailableSlots)
					};
				}

				#region Lobby Main Functions
				public async Task<List<Lobby>> QueryFilteredLobbies()
				{
					QueryLobbiesOptions queryOptions = new QueryLobbiesOptions
					{
						Count = maxQueryLobbiesCount,
						Filters = queryFilters,
						Order = queryOrders,
					};

					var queriedLobbies = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);

					return queriedLobbies.Results;
				}

				public async Task CreateLobby(string lobbyName, int chosenPlayerCount, string relayKey)
				{
					try
					{
						CreateLobbyOptions options = new CreateLobbyOptions
						{
							Data = new Dictionary<string, DataObject>
							{
								{ LobbyServiceKeys.JOIN_KEY, new DataObject(DataObject.VisibilityOptions.Member, "") },
							},
						};

						currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, chosenPlayerCount, options);

						LobbyHeartbeat();
						LobbyRefresh();
					}
					catch (LobbyServiceException e)
					{
						Debug.LogError(e);

						throw e;
					}
				}

				public async Task QuickJoinLobby()
				{
					QuickJoinLobbyOptions joinOptions = new QuickJoinLobbyOptions
					{
						Filter = queryFilters,
					};

					currentLobby = await LobbyService.Instance.QuickJoinLobbyAsync(joinOptions);

					LobbyRefresh();
				}

				public async Task JoinLobby(string lobbyId)
				{
					currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

					LobbyRefresh();
				}

				public async Task LeaveLobby()
				{
					if (currentLobby == null) return;

					try
					{
						if (currentLobby.HostId == Authentication.PlayerId)
						{
							await LobbyService.Instance.DeleteLobbyAsync(currentLobby.Id);
						}
						else
						{
							await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, Authentication.PlayerId);
						}

						currentLobby = null;
					}
					catch (System.Exception e)
					{
						Debug.LogError(e);
						throw e;
					}
				}
				#endregion

				#region Lobby Runtime Functions
				public string GetJoinCode()
				{
					if (currentLobby != null) return currentLobby.Data[LobbyServiceKeys.JOIN_KEY].Value;
					else return null;
				}

				public async void ToggleLobbyLock(bool isLocked)
				{
					try
					{
						await LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
						{
							IsLocked = isLocked
						});
					}
					catch (LobbyServiceException e)
					{
						Debug.LogError(e);

						throw e;
					}
				}

				private async void LobbyHeartbeat()
				{
					heartbeatSource = new CancellationTokenSource();

					while (!heartbeatSource.IsCancellationRequested && currentLobby != null)
					{
						await Lobbies.Instance.SendHeartbeatPingAsync(currentLobby.Id);
						await Task.Delay(heartbeatInterval * 1000);
					}
				}

				private async void LobbyRefresh()
				{
					lobbyRefreshSource = new CancellationTokenSource();
					await Task.Delay(lobbyRefreshRate * 1000);

					while (!lobbyRefreshSource.IsCancellationRequested && currentLobby != null)
					{
						currentLobby = await Lobbies.Instance.GetLobbyAsync(currentLobby.Id);
						await Task.Delay(lobbyRefreshRate * 1000);
					}
				}
				#endregion
			}
		}
	}
}