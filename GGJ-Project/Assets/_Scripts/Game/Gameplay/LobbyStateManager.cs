using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyStateManager : NetworkBehaviour
{
	[Header("Lobby State")]
	public bool IsLobbyReady { get; private set; }

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
	}

	public override void OnNetworkDespawn()
	{
		base.OnNetworkDespawn();
	}

}
