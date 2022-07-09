using ExitGames.Client.Photon;
using Il2CppSystem;
using MunchenClient.Utils;
using Photon.Realtime;
using UnityEngine;

namespace MunchenClient.ModuleSystem
{
	internal class ModuleComponent
	{
		protected virtual string moduleName => "Undefined Module";

		internal string GetModuleName()
		{
			return moduleName;
		}

		internal virtual void OnApplicationStart()
		{
		}

		internal virtual void OnApplicationQuit()
		{
		}

		internal virtual void OnUIManagerLoaded()
		{
		}

		internal virtual void OnUpdate()
		{
		}

		internal virtual void OnLateUpdate()
		{
		}

		internal virtual void OnFixedUpdate()
		{
		}

		internal virtual void OnPlayerJoin(PlayerInformation playerInfo)
		{
		}

		internal virtual void OnPlayerLeft(PlayerInformation playerInfo)
		{
		}

		internal virtual void OnLevelWasInitialized(int levelIndex)
		{
		}

		internal virtual void OnLevelWasLoaded(int levelIndex)
		{
		}

		internal virtual void OnLevelWasUnloaded(int levelIndex)
		{
		}

		internal virtual void OnRoomJoined()
		{
		}

		internal virtual void OnRoomLeft()
		{
		}

		internal virtual void OnRoomMasterChanged(PlayerInformation newMaster)
		{
		}

		internal virtual void OnAvatarLoaded(string playerId, string playerName, ref GameObject avatar)
		{
		}

		internal virtual bool OnPortalEntered(ref PortalInternal portal)
		{
			return true;
		}

		internal virtual void OnPortalCreated(ref PortalInternal portal, string worldId, string instanceId, int playerCount)
		{
		}

		internal virtual void OnPortalDestroyed(ref PortalInternal portal)
		{
		}

		internal virtual void OnPortalSetTimer(ref PortalInternal portal, float timer)
		{
		}

		internal virtual bool OnEventReceived(ref EventData eventData)
		{
			return true;
		}

		internal virtual bool OnEventSent(byte eventCode, ref Il2CppSystem.Object eventData, ref RaiseEventOptions raiseEventOptions)
		{
			return true;
		}
	}
}
