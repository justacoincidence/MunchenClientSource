using System;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC;

namespace MunchenClient.Misc
{
	public class MunchenLovenseBoneSystem : MonoBehaviour
	{
		internal Player owner;

		internal LovenseBoneType boneInformation;

		internal Vector3 lastCalculatedPosition = Vector3.zero;

		public MunchenLovenseBoneSystem(IntPtr ptr)
			: base(ptr)
		{
			owner = Player.prop_Player_0;
		}

		public void OnTriggerEnter(Collider collision)
		{
			if (LovenseConnectAPI.IsLovenseConnected() && boneInformation.lovenseBoneType == LovenseType.Feel && PlayerWrappers.IsLocalPlayer(owner))
			{
				MunchenLovenseBoneSystem component = collision.GetComponent<MunchenLovenseBoneSystem>();
				if (component != null && component.owner != owner)
				{
					LovenseHandler.RegisterCollidingBone(this, component);
				}
			}
		}

		public void OnTriggerExit(Collider collision)
		{
			if (LovenseConnectAPI.IsLovenseConnected() && boneInformation.lovenseBoneType == LovenseType.Feel && PlayerWrappers.IsLocalPlayer(owner))
			{
				MunchenLovenseBoneSystem component = collision.GetComponent<MunchenLovenseBoneSystem>();
				if (component != null && component.owner != owner)
				{
					LovenseHandler.UnregisterCollidingBone(this, component);
				}
			}
		}
	}
}
