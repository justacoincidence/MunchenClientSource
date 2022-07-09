using System;
using System.Linq;
using System.Reflection;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Core.Compatibility;
using MunchenClient.ModuleSystem;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;

namespace MunchenClient.Patching.Patches
{
	internal class NetworkManagerPatch : PatchComponent
	{
		private static bool isConnectedToInstance = true;

		protected override string patchName => "NetworkManagerPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(NetworkManagerPatch));
			PatchMethod((from mb in typeof(NetworkManager).GetMethods()
				where mb.Name.StartsWith("Method_Public_Void_Player_") && CheckMethod(mb, "OnPlayerJoined")
				select mb).First(), null, GetLocalPatch("OnPlayerJoinPatch"));
			PatchMethod((from mb in typeof(NetworkManager).GetMethods()
				where mb.Name.StartsWith("Method_Public_Void_Player_") && CheckMethod(mb, "OnPlayerLeft")
				select mb).Last(), null, GetLocalPatch("OnPlayerLeavePatch"));
			PatchMethod(typeof(NetworkManager).GetMethod("OnJoinedRoom"), null, GetLocalPatch("OnJoinedRoomPatch"));
			PatchMethod(typeof(NetworkManager).GetMethod("OnLeftRoom"), null, GetLocalPatch("OnLeftRoomPatch"));
			PatchMethod(typeof(NetworkManager).GetMethod("OnMasterClientSwitched"), null, GetLocalPatch("OnMasterClientSwitchedPatch"));
		}

		internal static void OnUIManagerInit()
		{
			foreach (VRC.Player item in UnityEngine.Object.FindObjectsOfType<VRC.Player>())
			{
				VRC.Player __ = item;
				OnPlayerJoinPatch(ref __);
			}
		}

		private static void OnJoinedRoomPatch()
		{
			isConnectedToInstance = true;
			ModuleManager.OnRoomJoined();
		}

		private static void OnLeftRoomPatch()
		{
			isConnectedToInstance = false;
			AssetManagementPatch.OnRoomLeft();
			ModuleManager.OnRoomLeft();
		}

		private static void OnMasterClientSwitchedPatch(ref Photon.Realtime.Player __0)
		{
			if (__0 != null)
			{
				PlayerInformation playerInformation = PlayerWrappers.GetPlayerInformation(__0.field_Public_Player_0);
				if (playerInformation != null)
				{
					ModuleManager.OnRoomMasterChanged(playerInformation);
				}
			}
		}

		private static void OnPlayerJoinPatch(ref VRC.Player __0)
		{
			if (!isConnectedToInstance || __0 == null || PlayerWrappers.GetPlayerInformation(__0) != null)
			{
				return;
			}
			bool flag = __0.prop_APIUser_0.id == APIUser.CurrentUser.id;
			GameObject gameObject = null;
			ImageThreeSlice nameplateBackground = null;
			Image nameplateIconBackground = null;
			GameObject gameObject2 = null;
			RectTransform rectTransform = null;
			TextMeshProUGUI textMeshProUGUI = null;
			try
			{
				gameObject = __0.prop_VRCPlayer_0.transform.Find("Player Nameplate/Canvas").gameObject;
				nameplateBackground = gameObject.transform.Find("Nameplate/Contents/Main/Background").GetComponent<ImageThreeSlice>();
				nameplateIconBackground = gameObject.transform.Find("Nameplate/Contents/Icon/Background").GetComponent<Image>();
				if (!flag)
				{
					Transform transform = gameObject.transform.Find("Nameplate/Contents");
					GameObject gameObject3 = transform.transform.Find(CompatibilityLayer.IsKarmaInstalled() ? "Tags" : "Quick Stats").gameObject;
					gameObject2 = UnityEngine.Object.Instantiate(gameObject3, transform);
					rectTransform = gameObject2.GetComponent<RectTransform>();
					rectTransform.localPosition = MiscUtils.GetNameplateOffset(GeneralUtils.isQuickMenuOpen);
					foreach (RectTransform componentsInChild in gameObject2.GetComponentsInChildren<RectTransform>())
					{
						if (componentsInChild.name != "Trust Text")
						{
							componentsInChild.gameObject.SetActive(value: false);
							continue;
						}
						textMeshProUGUI = componentsInChild.GetComponent<TextMeshProUGUI>();
						textMeshProUGUI.text = "MünchenClient Nameplate";
					}
					if (Configuration.GetGeneralConfig().NameplateMoreInfo)
					{
						gameObject2.SetActive(value: true);
					}
					else
					{
						gameObject2.SetActive(value: false);
					}
				}
				if (Configuration.GetGeneralConfig().NameplateWallhack && CameraFeaturesHandler.GetCameraSetup() == 0)
				{
					gameObject.layer = 19;
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("PatchManager", "OnPlayerJoinPatch - Nameplate", e, "OnPlayerJoinPatch", 146);
			}
			PlayerInformation playerInformation;
			try
			{
				playerInformation = new PlayerInformation
				{
					actorId = ((VRCNetworkBehaviour)__0.prop_VRCPlayer_0).prop_Int32_0,
					actorIdData = ((VRCNetworkBehaviour)__0.prop_VRCPlayer_0).prop_Int32_0 * PhotonNetwork.field_Public_Static_Int32_0 + 1,
					actorIdDataOther = ((VRCNetworkBehaviour)__0.prop_VRCPlayer_0).prop_Int32_0 * PhotonNetwork.field_Public_Static_Int32_0 + 3,
					id = __0.prop_APIUser_0.id,
					displayName = __0.prop_APIUser_0.displayName,
					isLocalPlayer = flag,
					isInstanceMaster = __0.prop_VRCPlayerApi_0.isMaster,
					isVRChatStaff = false,
					isVRUser = __0.prop_VRCPlayerApi_0.IsUserInVR(),
					isQuestUser = (__0.prop_APIUser_0.last_platform != "standalonewindows"),
					isClientUser = false,
					blockedLocalPlayer = false,
					rankStatus = PlayerRankStatus.Unknown,
					player = __0,
					playerApi = __0.prop_VRCPlayerApi_0,
					vrcPlayer = __0.prop_VRCPlayer_0,
					apiUser = __0.prop_APIUser_0,
					networkBehaviour = __0.prop_VRCPlayer_0,
					uSpeaker = __0.prop_VRCPlayer_0.prop_USpeaker_0,
					input = __0.prop_VRCPlayer_0.GetComponent<GamelikeInputController>(),
					airstuckDetections = 0,
					lastNetworkedUpdatePacketNumber = __0.prop_PlayerNet_0.field_Private_Int32_0,
					lastNetworkedUpdateTime = Time.realtimeSinceStartup,
					lastNetworkedVoicePacket = 0f,
					lagBarrier = 0,
					nameplateCanvas = gameObject,
					nameplateBackground = nameplateBackground,
					nameplateIconBackground = nameplateIconBackground,
					customNameplateObject = gameObject2,
					customNameplateTransform = rectTransform,
					customNameplateText = textMeshProUGUI
				};
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("PatchManager", "OnPlayerJoinPatch - Pre Process", e2, "OnPlayerJoinPatch", 195);
				return;
			}
			try
			{
				if (!PlayerUtils.playerColorCache.ContainsKey(__0.prop_APIUser_0.displayName))
				{
					PlayerUtils.playerColorCache.Add(__0.prop_APIUser_0.displayName, VRCPlayer.Method_Public_Static_Color_APIUser_0(__0.prop_APIUser_0));
				}
				else
				{
					PlayerUtils.playerColorCache[__0.prop_APIUser_0.displayName] = VRCPlayer.Method_Public_Static_Color_APIUser_0(__0.prop_APIUser_0);
				}
				PlayerUtils.playerCachingList.Add(playerInformation.displayName, playerInformation);
			}
			catch (Exception e3)
			{
				ConsoleUtils.Exception("PatchManager", "OnPlayerJoinPatch - Post Process", e3, "OnPlayerJoinPatch", 215);
				return;
			}
			ModuleManager.OnPlayerJoin(playerInformation);
			try
			{
				if (__0.prop_APIUser_0.tags.Contains("admin_moderator") || __0.prop_APIUser_0.developerType == APIUser.DeveloperType.Internal || __0.prop_APIUser_0.developerType == APIUser.DeveloperType.Moderator)
				{
					playerInformation.isVRChatStaff = true;
					if (Configuration.GetModerationsConfig().LogModerationsWarnAboutModerators)
					{
						string text = "<color=red>WARNING: <color=white>VRChat Staff joined: <color=purple>" + playerInformation.displayName;
						GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text);
						ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text, ConsoleColor.Red, "OnPlayerJoinPatch", 235);
					}
				}
			}
			catch (Exception e4)
			{
				ConsoleUtils.Exception("PatchManager", "OnPlayerJoinPatch - Staffcheck", e4, "OnPlayerJoinPatch", 241);
			}
		}

		private static void OnPlayerLeavePatch(ref VRC.Player __0)
		{
			if (__0 == null)
			{
				return;
			}
			PlayerInformation playerInformation = PlayerWrappers.GetPlayerInformation(__0);
			if (playerInformation != null && PlayerUtils.playerCachingList.ContainsKey(playerInformation.displayName))
			{
				if (GeneralUtils.notificationTracker.ContainsKey(__0.prop_APIUser_0.displayName))
				{
					GeneralUtils.notificationTracker.Remove(__0.prop_APIUser_0.displayName);
				}
				ModuleManager.OnPlayerLeft(playerInformation);
				PlayerUtils.playerCachingList.Remove(playerInformation.displayName);
			}
		}
	}
}
