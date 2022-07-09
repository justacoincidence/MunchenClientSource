using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu;
using MunchenClient.Wrappers;
using RealisticEyeMovements;
using RootMotion.FinalIK;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Management;
using VRC.SDKBase;
using VRC.UI;
using VRCSDK2;

namespace MunchenClient.Utils
{
	internal class PlayerUtils
	{
		internal static readonly Color vrchatTeamColor = new Color(22f / 85f, 49f / 51f, 0.9372549f);

		internal static readonly Color veteranUserColor = new Color(1f, 0.4117647f, 0.7058824f);

		internal static readonly System.Collections.Generic.Dictionary<string, PlayerInformation> playerCachingList = new System.Collections.Generic.Dictionary<string, PlayerInformation>();

		internal static readonly System.Collections.Generic.Dictionary<string, CustomRankInfo> playerCustomRank = new System.Collections.Generic.Dictionary<string, CustomRankInfo>();

		internal static readonly System.Collections.Generic.Dictionary<string, Color> playerColorCache = new System.Collections.Generic.Dictionary<string, Color>();

		internal static readonly System.Collections.Generic.Dictionary<string, GameObject> playerCloneList = new System.Collections.Generic.Dictionary<string, GameObject>();

		internal static PlayerInformation localPlayerInfo = null;

		private static VRC_AnimationController playerAnimController;

		private static VRCVrIkController playerIkController;

		private static float capsuleHiderOffsetReset = 0f;

		private static bool refreshingPlayerColorCache = false;

		internal static void ReloadAllAvatars()
		{
			foreach (System.Collections.Generic.KeyValuePair<string, PlayerInformation> playerCaching in playerCachingList)
			{
				ReloadAvatar(playerCaching.Value.apiUser);
			}
		}

		internal static void ReloadAvatarByID(string avatarId)
		{
			foreach (System.Collections.Generic.KeyValuePair<string, PlayerInformation> playerCaching in playerCachingList)
			{
				if (playerCaching.Value.vrcPlayer.field_Private_VRCAvatarManager_0.field_Private_ApiAvatar_0 != null && playerCaching.Value.vrcPlayer.field_Private_VRCAvatarManager_0.field_Private_ApiAvatar_0.id == avatarId)
				{
					ReloadAvatar(playerCaching.Value.apiUser);
				}
			}
		}

		internal static void ReloadAvatar(string displayName)
		{
			if (playerCachingList.ContainsKey(displayName))
			{
				ReloadAvatar(playerCachingList[displayName].apiUser);
			}
		}

		internal static void ReloadAvatar(PlayerRankStatus rank)
		{
			foreach (System.Collections.Generic.KeyValuePair<string, PlayerInformation> playerCaching in playerCachingList)
			{
				if (GeneralUtils.GetUserRank(playerCaching.Value, ignoreLocal: false) == rank)
				{
					ReloadAvatar(playerCaching.Value.apiUser);
				}
			}
		}

		internal static void ReloadAvatar(APIUser apiUser)
		{
			GeneralWrappers.reloadPlayerAvatarFunction.Invoke(null, apiUser);
		}

		internal static void RefreshAllPlayerColorCache()
		{
			if (!refreshingPlayerColorCache && GeneralUtils.isConnectedToInstance)
			{
				refreshingPlayerColorCache = true;
				MelonCoroutines.Start(RefreshAllPlayerColorCacheEnumerator());
			}
		}

		private static IEnumerator RefreshAllPlayerColorCacheEnumerator()
		{
			foreach (System.Collections.Generic.KeyValuePair<string, PlayerInformation> playerInfo in playerCachingList.ToList())
			{
				playerColorCache[playerInfo.Value.displayName] = VRCPlayer.Method_Public_Static_Color_APIUser_0(playerInfo.Value.apiUser);
				yield return new WaitForEndOfFrame();
			}
			refreshingPlayerColorCache = false;
		}

		internal static void ChangeCapsuleState(bool state)
		{
			if (!(PlayerWrappers.GetCurrentPlayer() == null))
			{
				if (playerAnimController == null)
				{
					playerAnimController = PlayerWrappers.GetCurrentPlayer().GetComponentInChildren<VRC_AnimationController>();
				}
				if (playerIkController == null)
				{
					playerIkController = PlayerWrappers.GetCurrentPlayer().GetComponentInChildren<VRCVrIkController>();
				}
				float num = ((VRC.SDKBase.VRC_SceneDescriptor._instance.RespawnHeightY < 0f) ? (0f - VRC.SDKBase.VRC_SceneDescriptor._instance.RespawnHeightY) : VRC.SDKBase.VRC_SceneDescriptor._instance.RespawnHeightY);
				float num2 = ((!(PlayerWrappers.GetCurrentPlayer().transform.position.y < 0f)) ? (num - 1f) : (num - PlayerWrappers.GetCurrentPlayer().transform.position.y - 1f));
				float field_Private_Single_ = PlayerWrappers.GetCurrentPlayer().prop_PlayerAudioManager_0.field_Private_Single_1;
				if (num2 > field_Private_Single_ - 2.5f)
				{
					num2 = field_Private_Single_ - 2.5f;
				}
				if (num2 > 10f)
				{
					num2 = 10f;
				}
				capsuleHiderOffsetReset = num2;
				PlayerWrappers.GetCurrentPlayer().transform.position += new Vector3(0f, state ? (0f - num2) : capsuleHiderOffsetReset, 0f);
				playerAnimController.field_Private_Boolean_0 = !state;
				MelonCoroutines.Start(ToggleIKControllerEnumerator(state));
				if (state)
				{
					VRCVrCamera.field_Private_Static_VRCVrCamera_0.transform.parent.localPosition += new Vector3(0f, num2 / VRCVrCamera.field_Private_Static_VRCVrCamera_0.transform.parent.parent.transform.localScale.y, 0f);
					MovementMenu.flightButton.SetToggleState(state: true);
					GeneralUtils.ToggleFlight(state: true);
				}
				else
				{
					MovementMenu.flightButton.SetToggleState(state: false);
					GeneralUtils.ToggleFlight(state: false);
					VRCVrCamera.field_Private_Static_VRCVrCamera_0.transform.parent.localPosition = Vector3.zero;
					ReloadAvatar(PlayerRankStatus.Local);
				}
			}
		}

		private static IEnumerator ToggleIKControllerEnumerator(bool state)
		{
			yield return new WaitForSeconds(2f);
			playerIkController.field_Private_Boolean_0 = !state;
		}

		internal static bool IsPlayerAvatarHidden(string id)
		{
			if (!ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0.ContainsKey(id))
			{
				return false;
			}
			Il2CppSystem.Collections.Generic.List<ApiPlayerModeration>.Enumerator enumerator = ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0[id].GetEnumerator();
			while (enumerator.MoveNext())
			{
				ApiPlayerModeration current = enumerator.Current;
				if (current.moderationType == ApiPlayerModeration.ModerationType.HideAvatar)
				{
					return true;
				}
			}
			return false;
		}

		internal static bool IsPlayerAvatarShown(string id)
		{
			if (!ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0.ContainsKey(id))
			{
				return false;
			}
			Il2CppSystem.Collections.Generic.List<ApiPlayerModeration>.Enumerator enumerator = ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0[id].GetEnumerator();
			while (enumerator.MoveNext())
			{
				ApiPlayerModeration current = enumerator.Current;
				if (current.moderationType == ApiPlayerModeration.ModerationType.ShowAvatar)
				{
					return true;
				}
			}
			return false;
		}

		internal static bool IsClientUser(PlayerInformation player)
		{
			if (player.isClientUser)
			{
				return true;
			}
			if (player.lastNetworkedUpdatePacketNumber <= 1)
			{
				return false;
			}
			if (player.GetPing() < 10)
			{
				ConsoleUtils.Info("Detector", player.displayName + " is a client user (2)", System.ConsoleColor.Gray, "IsClientUser", 369);
				player.isClientUser = true;
				return true;
			}
			if (player.GetFPS() < 1)
			{
				ConsoleUtils.Info("Detector", player.displayName + " is a client user (3)", System.ConsoleColor.Gray, "IsClientUser", 378);
				player.isClientUser = true;
				return true;
			}
			if (player.isQuestUser)
			{
				if (player.GetFPS() > 120 || !player.isVRUser)
				{
					ConsoleUtils.Info("Detector", player.displayName + " is a client user (4)", System.ConsoleColor.Gray, "IsClientUser", 390);
					player.isClientUser = true;
					return true;
				}
			}
			else if (player.GetFPS() > 144 || player.GetPing() > 3000)
			{
				ConsoleUtils.Info("Detector", player.displayName + " is a client user (5)", System.ConsoleColor.Gray, "IsClientUser", 402);
				player.isClientUser = true;
				return true;
			}
			if (player.detectedFirstGround)
			{
				if (player.GetVelocity().y == 0f && !player.IsGrounded())
				{
					player.airstuckDetections++;
					if (player.airstuckDetections >= 5)
					{
						ConsoleUtils.Info("Detector", player.displayName + " is a client user (6)", System.ConsoleColor.Gray, "IsClientUser", 417);
						player.isClientUser = true;
						return true;
					}
				}
				else if (player.airstuckDetections > 0)
				{
					player.airstuckDetections--;
				}
			}
			else if (player.IsGrounded())
			{
				player.detectedFirstGround = true;
			}
			return false;
		}

		internal static void ToggleBlockOnPlayer(APIUser user)
		{
			PageUserInfo pageUserInfo = GeneralWrappers.GetPageUserInfo();
			if (user.id == APIUser.CurrentUser.id)
			{
				ConsoleUtils.Info("PlayerUtils", "Can't block localplayer", System.ConsoleColor.Gray, "ToggleBlockOnPlayer", 448);
				return;
			}
			pageUserInfo.field_Private_APIUser_0 = new APIUser
			{
				id = user.id
			};
			pageUserInfo.ToggleBlock();
		}

		internal static void IsAvatarValid(string avatarId, System.Action<ApiAvatar> onSuccess, System.Action<string> onFailed = null)
		{
			if (string.IsNullOrEmpty(avatarId))
			{
				onFailed?.Invoke("Empty Avatar Id");
				return;
			}
			if (avatarId.Length != 41 || !avatarId.StartsWith("avtr_") || avatarId[13] != '-' || avatarId[18] != '-' || avatarId[23] != '-' || avatarId[28] != '-')
			{
				onFailed?.Invoke("Invalid Avatar Id");
				return;
			}
			ApiAvatar apiAvatar = new ApiAvatar();
			apiAvatar.id = avatarId;
			((ApiModel)apiAvatar).Get((Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)delegate(ApiContainer x)
			{
				onSuccess(x.Model.Cast<ApiAvatar>());
			}, (Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)delegate(ApiContainer x)
			{
				onFailed?.Invoke(x.Error);
			}, (Il2CppSystem.Collections.Generic.Dictionary<string, BestHTTP.JSON.Json.Token>)null, false);
		}

		internal static void ChangePlayerAvatar(string avatarId, bool logErrorOnHud)
		{
			IsAvatarValid(avatarId, delegate(ApiAvatar avatar)
			{
				GeneralWrappers.GetPageAvatar().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 = avatar;
				GeneralWrappers.GetPageAvatar().ChangeToSelectedAvatar();
			}, delegate(string error)
			{
				ConsoleUtils.Info("Player", "Failed to switch to avatar: (" + error + ")", System.ConsoleColor.Gray, "ChangePlayerAvatar", 500);
				if (logErrorOnHud)
				{
					GeneralWrappers.AlertPopup("Player", "Failed to switch to avatar: (" + error + ")");
				}
			});
		}

		internal static void ClearAllClones()
		{
			foreach (System.Collections.Generic.KeyValuePair<string, GameObject> playerClone in playerCloneList)
			{
				UnityEngine.Object.DestroyImmediate(playerClone.Value);
			}
			playerCloneList.Clear();
		}

		internal static void ClearClone(PlayerInformation player)
		{
			if (player != null && playerCloneList.ContainsKey(player.displayName))
			{
				UnityEngine.Object.DestroyImmediate(playerCloneList[player.displayName]);
				playerCloneList.Remove(player.displayName);
			}
		}

		internal static void GenerateAvatarClone(PlayerInformation player)
		{
			if (player == null)
			{
				ConsoleUtils.Info("PlayerUtils", "Can't generate avatar clone (Couldn't find player)", System.ConsoleColor.Gray, "GenerateAvatarClone", 538);
				return;
			}
			if (player.GetAvatar() == null)
			{
				ConsoleUtils.Info("PlayerUtils", "Can't generate avatar clone (Couldn't find avatar)", System.ConsoleColor.Gray, "GenerateAvatarClone", 545);
				return;
			}
			ClearClone(player);
			GameObject gameObject = UnityEngine.Object.Instantiate(player.GetAvatar());
			gameObject.name = "Local Avatar Clone";
			gameObject.transform.position = player.GetAvatar().transform.position;
			gameObject.transform.rotation = player.GetAvatar().transform.rotation;
			foreach (Transform componentsInChild in gameObject.GetComponentsInChildren<Transform>(includeInactive: true))
			{
				componentsInChild.gameObject.layer = 0;
				if ((double)componentsInChild.localScale.x < 0.001 && (double)componentsInChild.localScale.y < 0.001 && (double)componentsInChild.localScale.z < 0.001)
				{
					componentsInChild.localScale = new Vector3(1f, 1f, 1f);
				}
			}
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<Animator>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<VRCSDK2.VRC_AvatarDescriptor>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<PipelineManager>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<MonoBehaviourPrivateObUnique>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<DynamicBoneController>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<VRIK>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<FullBodyBipedIK>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<LimbIK>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<EyeAndHeadAnimator>());
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<LookTargetController>());
			playerCloneList.Add(player.displayName, gameObject);
		}

		internal static void OnPlayerBlockStateChanged(PlayerInformation playerInfo, bool isBlocked)
		{
			if (isBlocked)
			{
				if (!GeneralUtils.CheckNotification(playerInfo.displayName, "blockedLocalPlayer"))
				{
					GeneralUtils.ToggleNotification(playerInfo.displayName, "blockedLocalPlayer", state: true);
					playerInfo.blockedLocalPlayer = true;
					string text = LanguageManager.GetUsedLanguage().ModerationBlockDetected.Replace("{username}", playerInfo.displayName);
					if (Configuration.GetModerationsConfig().LogModerationsBlockLog)
					{
						ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text, System.ConsoleColor.Gray, "OnPlayerBlockStateChanged", 598);
					}
					if (Configuration.GetModerationsConfig().LogModerationsBlockHUD)
					{
						GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text);
					}
				}
			}
			else if (GeneralUtils.CheckNotification(playerInfo.displayName, "blockedLocalPlayer"))
			{
				GeneralUtils.ToggleNotification(playerInfo.displayName, "blockedLocalPlayer", state: false);
				playerInfo.blockedLocalPlayer = false;
				string text2 = LanguageManager.GetUsedLanguage().ModerationUnblockDetected.Replace("{username}", playerInfo.displayName);
				if (Configuration.GetModerationsConfig().LogModerationsBlockLog)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text2, System.ConsoleColor.Gray, "OnPlayerBlockStateChanged", 619);
				}
				if (Configuration.GetModerationsConfig().LogModerationsBlockHUD)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text2);
				}
			}
		}

		internal static void OnPlayerMuteStateChanged(PlayerInformation playerInfo, bool isMuted)
		{
			if (isMuted)
			{
				if (!GeneralUtils.CheckNotification(playerInfo.displayName, "mutedLocalPlayer"))
				{
					GeneralUtils.ToggleNotification(playerInfo.displayName, "mutedLocalPlayer", state: true);
					string text = LanguageManager.GetUsedLanguage().ModerationMuteDetected.Replace("{username}", playerInfo.displayName);
					if (Configuration.GetModerationsConfig().LogModerationsMuteLog)
					{
						ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text, System.ConsoleColor.Gray, "OnPlayerMuteStateChanged", 641);
					}
					if (Configuration.GetModerationsConfig().LogModerationsMuteHUD)
					{
						GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text);
					}
				}
			}
			else if (GeneralUtils.CheckNotification(playerInfo.displayName, "mutedLocalPlayer"))
			{
				GeneralUtils.ToggleNotification(playerInfo.displayName, "mutedLocalPlayer", state: false);
				string text2 = LanguageManager.GetUsedLanguage().ModerationUnmuteDetected.Replace("{username}", playerInfo.displayName);
				if (Configuration.GetModerationsConfig().LogModerationsMuteLog)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text2, System.ConsoleColor.Gray, "OnPlayerMuteStateChanged", 661);
				}
				if (Configuration.GetModerationsConfig().LogModerationsMuteHUD)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text2);
				}
			}
		}
	}
}
