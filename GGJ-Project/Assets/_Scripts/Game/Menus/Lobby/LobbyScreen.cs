using System.Collections;
using System.Collections.Generic;
using Overgrown.GameServices.GameLobby;
using Overgrown.GameServices.Matchmaking;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Overgrown
{
	namespace Menus
	{

		public class LobbyScreen : MonoBehaviour
		{
			private List<LobbyGameButton> lobbyList = new List<LobbyGameButton>();

			[Header("Lobby Creation Modal")]
			[SerializeField] private TMP_InputField lobbyNameInputField;
			[SerializeField] private TMP_Dropdown lobbyPlayerCountDropdown;

			[Header("Object References")]
			[SerializeField] private Transform lobbyListContainer;
			[SerializeField] private GameObject lobbyButtonPrefab;

			#region Unity Functions
			private void Start()
			{
				PopulateLobbyList();
			}
			#endregion

			#region Menu Functions
			public async void PopulateLobbyList()
			{
				List<Lobby> queriedLobbies = await LobbyServiceInterface.Instance.QueryFilteredLobbies();

#if UNITY_EDITOR
				Debug.Log($"Queried {queriedLobbies.Count} lobbies from the lobby service.");
#endif

				lobbyList.Clear();
				foreach (Transform item in lobbyListContainer)
				{
					Destroy(item.gameObject);
				}

				foreach (Lobby lobby in queriedLobbies)
				{
					GameObject lobbyButton = Instantiate(lobbyButtonPrefab, lobbyListContainer);
					LobbyGameButton lobbyButtonScript = lobbyButton.GetComponent<LobbyGameButton>();
					lobbyButtonScript.UpdateLobbyData(lobby);
					lobbyList.Add(lobbyButtonScript);
				}
			}
			#endregion

			#region Button Functions
			public async void OnQuickJoinLobbyButton()
			{
				await Matchmaker.Instance.QuickJoinMatch();
			}

			public async void OnCreateLobbyButton()
			{
				int parsedPlayerCount = int.Parse(lobbyPlayerCountDropdown.options[lobbyPlayerCountDropdown.value].text);

				Matchmaker.Instance.CreateMatch(lobbyNameInputField.text, parsedPlayerCount);
			}
			#endregion
		}
	}
}