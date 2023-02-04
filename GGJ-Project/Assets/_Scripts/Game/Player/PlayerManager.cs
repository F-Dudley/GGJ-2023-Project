using System.Collections.Generic;
using Cinemachine;
using Overgrown.Utils;
using UnityEngine;

namespace Overgrown
{
	namespace GameCore
	{
		namespace Player
		{
			public class PlayerManager : Singleton<PlayerManager>
			{
				[SerializeField] private CinemachineTargetGroup targetGroup = null;

				#region Unity Functions
				private new void Awake()
				{
					base.Awake();

					targetGroup = GetComponent<CinemachineTargetGroup>();
				}
				#endregion

				public void AddPlayerToTargetGroup(Transform playerTransform)
				{
					targetGroup.AddMember(playerTransform, 1, 0);
				}
			}
		}
	}
}