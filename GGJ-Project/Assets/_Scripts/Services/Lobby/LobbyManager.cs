using System.Collections.Generic;
using System.Threading.Tasks;
using Overgrown.Utils;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Overgrown
{
	namespace GameServices
	{
		namespace Lobby
		{
			public class LobbyManager : Singleton<LobbyManager>
			{

				[Header("Main Lobby References")]
				[SerializeField] private static Unity.Services.Lobbies.Models.Lobby currentLobby;

				[Header("Lobby Settings")]
				public static readonly int MaxPlayers = 4;

				private static readonly int MaxQueryLobbiesCount = 20;

				#region Unity Functions

				#endregion

				#region Lobby Main Functions
				public async Task<List<Unity.Services.Lobbies.Models.Lobby>> QueryFilteredLobbies()
				{
					QueryLobbiesOptions queryOptions = new QueryLobbiesOptions
					{
						Count = MaxQueryLobbiesCount,
						Filters = new List<QueryFilter>
						{
							new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
							new(QueryFilter.FieldOptions.IsLocked, "0", QueryFilter.OpOptions.EQ),
						},
						Order = new List<QueryOrder>
						{
							new(false, QueryOrder.FieldOptions.AvailableSlots)
						}
					};

					var queriedLobbies = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);

					return queriedLobbies.Results;
				}

				public async void CreateLobby(string lobbyName, int chosenPlayerCount)
				{
					try
					{
						if (chosenPlayerCount > MaxPlayers) chosenPlayerCount = MaxPlayers;

						currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, chosenPlayerCount);

					}
					catch (LobbyServiceException e)
					{
						Debug.LogError(e);

						throw e;
					}
				}

				public async void QuickJoinLobby()
				{
					QuickJoinLobbyOptions joinOptions = new QuickJoinLobbyOptions
					{
						Filter = new List<QueryFilter> {
							new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.NE),
							new(QueryFilter.FieldOptions.IsLocked, "0", QueryFilter.OpOptions.EQ),
						},
					};

					await LobbyService.Instance.QuickJoinLobbyAsync(joinOptions);
				}

				public async Task JoinLobby(string lobbyId)
				{
					currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
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
				public async void LockLobby()
				{
					try
					{
						await LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
						{
							IsLocked = true
						});
					}
					catch (LobbyServiceException e)
					{
						Debug.LogError(e);

						throw e;
					}
				}
				#endregion
			}
		}
	}
}