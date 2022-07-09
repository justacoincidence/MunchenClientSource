using System.Collections.Generic;
using MunchenClient.Utils;
using VRC;
using VRC.Core;
using VRC.DataModel.Core;

namespace MunchenClient.Wrappers
{
	internal static class PlayerWrappers
	{
		internal static VRCPlayer GetCurrentPlayer()
		{
			return VRCPlayer.field_Internal_Static_VRCPlayer_0;
		}

		internal static bool IsLocalPlayer(Player player)
		{
			return player.prop_APIUser_0.id == APIUser.CurrentUser.id;
		}

		internal static Player GetPlayerByName(string username)
		{
			PlayerManager playerManager = GeneralWrappers.GetPlayerManager();
			if (playerManager == null)
			{
				return null;
			}
			Player[] array = playerManager.field_Private_List_1_Player_0.ToArray();
			foreach (Player player in array)
			{
				if (player != null)
				{
					APIUser aPIUser = player.prop_APIUser_0;
					if (aPIUser != null && aPIUser.displayName == username)
					{
						return player;
					}
				}
			}
			return null;
		}

		internal static PlayerInformation GetSelectedPlayer()
		{
			return GetPlayerInformationByID(GeneralWrappers.GetSelectedUserManager().field_Private_IUser_0.Cast<DataModel<APIUser>>().field_Protected_TYPE_0.id);
		}

		internal static PlayerInformation GetPlayer(PortalInternal portal)
		{
			if (portal == null)
			{
				return null;
			}
			return GetPlayerInformationByInstagatorID(portal.field_Private_Int32_0);
		}

		internal static PlayerInformation GetPlayerInformationByAvatarID(string id)
		{
			PlayerInformation result = null;
			foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
			{
				if (playerCaching.Value.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2 != null && playerCaching.Value.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id == id)
				{
					if (playerCaching.Value.IsFriends() || playerCaching.Value.isLocalPlayer)
					{
						return playerCaching.Value;
					}
					result = playerCaching.Value;
				}
			}
			return result;
		}

		internal static PlayerInformation GetPlayerInformationByName(string displayName)
		{
			if (displayName == APIUser.CurrentUser.displayName)
			{
				return GetLocalPlayerInformation();
			}
			if (PlayerUtils.playerCachingList.ContainsKey(displayName))
			{
				return PlayerUtils.playerCachingList[displayName];
			}
			return null;
		}

		internal static PlayerInformation GetPlayerInformationByID(string id)
		{
			if (id == APIUser.CurrentUser?.id)
			{
				return GetLocalPlayerInformation();
			}
			if (PlayerUtils.playerCachingList.Count == 0)
			{
				return null;
			}
			foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
			{
				if (playerCaching.Value.id == id)
				{
					return playerCaching.Value;
				}
			}
			return null;
		}

		internal static PlayerInformation GetPlayerInformationByInstagatorID(int index)
		{
			foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
			{
				if (playerCaching.Value.networkBehaviour.prop_Int32_0 == index)
				{
					return playerCaching.Value;
				}
			}
			return null;
		}

		internal static PlayerInformation GetPlayerInformation(Player player)
		{
			string text = string.Empty;
			if (player != null)
			{
				if (player.prop_APIUser_0 != null)
				{
					text = player.prop_APIUser_0.displayName;
				}
				else if (player.prop_VRCPlayerApi_0 != null)
				{
					text = player.prop_VRCPlayerApi_0.displayName;
				}
			}
			if (text == string.Empty)
			{
				return null;
			}
			if (APIUser.CurrentUser != null && APIUser.CurrentUser.displayName == text)
			{
				return GetLocalPlayerInformation();
			}
			if (PlayerUtils.playerCachingList.ContainsKey(text))
			{
				return PlayerUtils.playerCachingList[text];
			}
			return null;
		}

		internal static PlayerInformation GetInstanceCreatorPlayerInformation()
		{
			if (WorldUtils.GetCurrentInstance() == null)
			{
				return null;
			}
			foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
			{
				if (playerCaching.Value.id == WorldUtils.GetCurrentInstance().ownerId)
				{
					return playerCaching.Value;
				}
			}
			return null;
		}

		internal static PlayerInformation GetInstanceHostPlayerInformation()
		{
			foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
			{
				if (playerCaching.Value.isInstanceMaster)
				{
					return playerCaching.Value;
				}
			}
			return null;
		}

		internal static PlayerInformation GetLocalPlayerInformation()
		{
			if (PlayerUtils.localPlayerInfo == null)
			{
				if (APIUser.CurrentUser != null && PlayerUtils.playerCachingList.ContainsKey(APIUser.CurrentUser.displayName))
				{
					PlayerUtils.localPlayerInfo = PlayerUtils.playerCachingList[APIUser.CurrentUser.displayName];
					return PlayerUtils.playerCachingList[APIUser.CurrentUser.displayName];
				}
				return null;
			}
			return PlayerUtils.localPlayerInfo;
		}
	}
}
