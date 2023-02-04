using System.Collections;
using System.Collections.Generic;
using Overgrown.GameServices.Matchmaking;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

using UnityEngine.UI;

namespace Overgrown
{
	namespace Menus
	{
		public class LobbyGameButton : MonoBehaviour
		{
			[SerializeField] private Lobby lobbyData;

			[Header("Button References")]
			[SerializeField] private TextMeshProUGUI lobbyNameText;
			[SerializeField] private TextMeshProUGUI playerAmountText;
			[SerializeField] private Toggle isPrivateCheckbox;

			public async void OnLobbySelect()
			{
				await Matchmaker.Instance.JoinMatch(lobbyData.Id);
			}

			public void UpdateLobbyData(Lobby lobbyData)
			{
				this.lobbyData = lobbyData;
				UpdateButtonFields();
			}

			public void UpdateButtonFields()
			{
				if (lobbyNameText != null) lobbyNameText.text = lobbyData.Name;
				if (playerAmountText != null) playerAmountText.text = $"{lobbyData.Players.Count}/{lobbyData.MaxPlayers}";
				if (isPrivateCheckbox != null) isPrivateCheckbox.isOn = lobbyData.IsPrivate;
			}
		}
	}
}