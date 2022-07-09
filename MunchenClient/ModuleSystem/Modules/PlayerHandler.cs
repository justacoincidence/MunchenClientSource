using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu;
using MunchenClient.Menu.Others;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using VRC.SDKBase;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class PlayerHandler : ModuleComponent
	{
		private static bool originalSpeedsFetched = false;

		private static float originalWalkSpeed = 0f;

		private static float originalRunSpeed = 0f;

		private static float originalJumpPower = -1f;

		private static float originalStrafeSpeed = 0f;

		private static Vector3 originalGravity = Physics.gravity;

		private static int currentPlayerNameplateUpdatingIndex = 0;

		private static readonly System.Collections.Generic.Dictionary<int, float> playerInvisibleCheck = new System.Collections.Generic.Dictionary<int, float>();

		private static HighlightsFXStandalone playerFriendsESP = null;

		private static HighlightsFXStandalone playerStrangersESP = null;

		private bool didMidJump = false;

		private VRCInput jumpInput = null;

		private bool didJump = false;

		private float timeSinceLastJump = 0f;

		private static readonly int playerLayerMask = -527361;

		private static readonly Vector3 playerDownscale = new Vector3(0.1f, 0.1f, 0.1f);

		protected override string moduleName => "Player Handler";

		internal override void OnUIManagerLoaded()
		{
			jumpInput = VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump");
			playerFriendsESP = GeneralWrappers.GetPlayerCamera().gameObject.AddComponent<HighlightsFXStandalone>();
			playerFriendsESP.blurIterations = 3;
			playerFriendsESP.blurSize = 2f;
			playerStrangersESP = GeneralWrappers.GetPlayerCamera().gameObject.AddComponent<HighlightsFXStandalone>();
			playerStrangersESP.blurIterations = 3;
			playerStrangersESP.blurSize = 2f;
			RefreshFriendsWallhackColors();
			RefreshStrangersWallhackColors();
		}

		internal static void RefreshFriendsWallhackColors()
		{
			if (Configuration.GetGeneralConfig().PlayerWallhackRGBFriends)
			{
				Color.RGBToHSV(playerFriendsESP.highlightColor, out var H, out var S, out var V);
				playerFriendsESP.highlightColor = Color.HSVToRGB(H + Time.deltaTime * 0.1f, S, V);
			}
			else
			{
				playerFriendsESP.highlightColor = Configuration.GetGeneralConfig().PlayerWallhackFriendsColor.GetColor();
			}
		}

		internal static void RefreshStrangersWallhackColors()
		{
			if (Configuration.GetGeneralConfig().PlayerWallhackRGBStrangers)
			{
				Color.RGBToHSV(playerStrangersESP.highlightColor, out var H, out var S, out var V);
				playerStrangersESP.highlightColor = Color.HSVToRGB(H + Time.deltaTime * 0.1f, S, V);
			}
			else
			{
				playerStrangersESP.highlightColor = Configuration.GetGeneralConfig().PlayerWallhackStrangersColor.GetColor();
			}
		}

		internal static void ResetPlayerESPStates()
		{
			playerFriendsESP.field_Protected_HashSet_1_Renderer_0.Clear();
			playerStrangersESP.field_Protected_HashSet_1_Renderer_0.Clear();
		}

		internal static void EnableFriendESPState(Renderer renderer)
		{
			playerFriendsESP.field_Protected_HashSet_1_Renderer_0.Add(renderer);
		}

		internal static void EnableStrangerESPState(Renderer renderer)
		{
			playerStrangersESP.field_Protected_HashSet_1_Renderer_0.Add(renderer);
		}

		internal static void DisableESPForPlayer(Renderer renderer)
		{
			playerFriendsESP.field_Protected_HashSet_1_Renderer_0.Remove(renderer);
			playerStrangersESP.field_Protected_HashSet_1_Renderer_0.Remove(renderer);
		}

		internal override void OnUpdate()
		{
			if (GeneralUtils.isConnectedToInstance)
			{
				if (Configuration.GetGeneralConfig().InvisibleDetection)
				{
					CheckForInvisiblePlayers();
				}
				if (Configuration.GetGeneralConfig().PlayerWallhackRGBFriends)
				{
					RefreshFriendsWallhackColors();
				}
				if (Configuration.GetGeneralConfig().PlayerWallhackRGBStrangers)
				{
					RefreshStrangersWallhackColors();
				}
			}
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			if (!GeneralUtils.isConnectedToInstance || localPlayerInformation == null)
			{
				return;
			}
			if (!originalSpeedsFetched)
			{
				originalWalkSpeed = localPlayerInformation.input.field_Public_Single_1;
				originalRunSpeed = localPlayerInformation.input.field_Public_Single_2;
				originalStrafeSpeed = localPlayerInformation.input.field_Public_Single_0;
				originalGravity = Physics.gravity;
				originalSpeedsFetched = true;
			}
			if (!GeneralUtils.isQuickMenuOpen)
			{
				if (Configuration.GetGeneralConfig().FlightDoubleJump)
				{
					if (jumpInput.prop_Boolean_1)
					{
						if (!didJump)
						{
							if (timeSinceLastJump - Time.realtimeSinceStartup > 0f)
							{
								GeneralUtils.ToggleFlight(!GeneralUtils.flight);
								MovementMenu.flightButton.SetToggleState(GeneralUtils.flight);
								ActionWheelMenu.flightButton.SetButtonText(GeneralUtils.flight ? "Flight: <color=green>On" : "Flight: <color=red>Off");
							}
							didJump = true;
							timeSinceLastJump = Time.realtimeSinceStartup + 0.25f;
						}
					}
					else
					{
						didJump = false;
					}
				}
				if (!GeneralUtils.flight)
				{
					if (Configuration.GetGeneralConfig().Jetpack && jumpInput.prop_Boolean_1)
					{
						Vector3 velocity = localPlayerInformation.playerApi.GetVelocity();
						velocity.y = localPlayerInformation.input.field_Public_Single_3;
						localPlayerInformation.playerApi.SetVelocity(velocity);
					}
					if (Configuration.GetGeneralConfig().AutoBhop && localPlayerInformation.IsGrounded() && jumpInput.prop_Boolean_1)
					{
						Vector3 velocity2 = localPlayerInformation.playerApi.GetVelocity();
						velocity2.y = localPlayerInformation.input.field_Public_Single_3;
						localPlayerInformation.playerApi.SetVelocity(velocity2);
					}
					if (Configuration.GetGeneralConfig().InfiniteJump && !localPlayerInformation.IsGrounded())
					{
						if (jumpInput.prop_Boolean_1)
						{
							if (!didMidJump)
							{
								Vector3 velocity3 = localPlayerInformation.playerApi.GetVelocity();
								velocity3.y = localPlayerInformation.input.field_Public_Single_3;
								localPlayerInformation.playerApi.SetVelocity(velocity3);
								didMidJump = true;
							}
						}
						else
						{
							didMidJump = false;
						}
					}
				}
			}
			if (MicrophoneMenu.microphoneMenuInitialized)
			{
				MicrophoneMenu.currentGainIndicator.SetMainText($"{USpeaker.field_Internal_Static_Single_1 * 100f:##0}%");
			}
			if (MovementMenu.movementMenuInitialized)
			{
				MovementMenu.currentWalkModifierIndicator.SetMainText(localPlayerInformation.input.field_Public_Single_1.ToString());
				MovementMenu.currentRunModifierIndicator.SetMainText(localPlayerInformation.input.field_Public_Single_2.ToString());
				MovementMenu.currentJumpModifierIndicator.SetMainText(localPlayerInformation.input.field_Public_Single_3.ToString());
				MovementMenu.currentGravityModifierIndicator.SetMainText(Physics.gravity.y.ToString());
				MovementMenu.currentFlightSpeedIndicator.SetMainText(Configuration.GetGeneralConfig().FlightSpeed.ToString());
			}
		}

		internal override void OnRoomLeft()
		{
			playerInvisibleCheck.Clear();
			originalSpeedsFetched = false;
		}

		private void CheckForInvisiblePlayers()
		{
			if (PhotonNetwork.prop_Room_0 == null)
			{
				return;
			}
			Il2CppSystem.Collections.Generic.Dictionary<int, Player>.Enumerator enumerator = PhotonNetwork.prop_Room_0.prop_Dictionary_2_Int32_Player_0.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Il2CppSystem.Collections.Generic.KeyValuePair<int, Player> current = enumerator.Current;
				if (current.Value.field_Public_Player_0 != null)
				{
					continue;
				}
				if (!playerInvisibleCheck.ContainsKey(current.Value.field_Private_Int32_0))
				{
					playerInvisibleCheck.Add(current.Value.field_Private_Int32_0, 10f);
				}
				else if (playerInvisibleCheck[current.Value.field_Private_Int32_0] > 0f)
				{
					playerInvisibleCheck[current.Value.field_Private_Int32_0] -= Time.deltaTime;
				}
				else if (current.Value.field_Private_Hashtable_0.ContainsKey("user"))
				{
					Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> dictionary = current.Value.field_Private_Hashtable_0["user"].Cast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
					string text = new Il2CppSystem.String(dictionary["displayName"].Pointer);
					if (!GeneralUtils.CheckNotification(text, "joinedInvisible"))
					{
						GeneralUtils.ToggleNotification(text, "joinedInvisible", state: true);
						string text2 = LanguageManager.GetUsedLanguage().ModerationInvisibleDetected.Replace("{username}", text);
						ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text2, System.ConsoleColor.Gray, "CheckForInvisiblePlayers", 279);
						GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text2);
					}
				}
			}
		}

		internal override void OnLateUpdate()
		{
			UpdateNextPlayer();
		}

		private static void UpdateNextPlayer()
		{
			if (!GeneralUtils.isConnectedToInstance || PlayerUtils.playerCachingList.Count == 0)
			{
				return;
			}
			currentPlayerNameplateUpdatingIndex++;
			if (currentPlayerNameplateUpdatingIndex > PlayerUtils.playerCachingList.Count - 1)
			{
				currentPlayerNameplateUpdatingIndex = 0;
			}
			PlayerInformation value = PlayerUtils.playerCachingList.ElementAt(currentPlayerNameplateUpdatingIndex).Value;
			if (value != null)
			{
				bool flag = UpdatePlayerVisibility(value);
				UpdatePlayerNameplate(value, flag);
				if (!flag)
				{
					UpdatePlayerScale(value);
				}
			}
		}

		private static void ScalePlayerAccordingly(PlayerInformation playerInfo)
		{
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			Vector3 localScale = ((localPlayerInformation == null) ? playerDownscale : (localPlayerInformation.GetAvatar().transform.localScale / 2f));
			playerInfo.GetAvatar().transform.localScale = localScale;
		}

		private static void UpdatePlayerScale(PlayerInformation playerInfo)
		{
			if (!Configuration.GetGeneralConfig().HiddenAvatarScaler)
			{
				return;
			}
			switch (GeneralUtils.GetUserRank(playerInfo, ignoreLocal: false))
			{
			case PlayerRankStatus.Visitor:
				if (PlayerPrefs.GetInt("CustomTrustLevel_Untrusted_CanUseCustomAvatar") == 0)
				{
					ScalePlayerAccordingly(playerInfo);
				}
				break;
			case PlayerRankStatus.NewUser:
				if (PlayerPrefs.GetInt("CustomTrustLevel_BasicTrustLevel1_CanUseCustomAvatar") == 0)
				{
					ScalePlayerAccordingly(playerInfo);
				}
				break;
			case PlayerRankStatus.User:
				if (PlayerPrefs.GetInt("CustomTrustLevel_IntermediateTrustLevel1_CanUseCustomAvatar") == 0)
				{
					ScalePlayerAccordingly(playerInfo);
				}
				break;
			case PlayerRankStatus.Known:
				if (PlayerPrefs.GetInt("CustomTrustLevel_IntermediateTrustLevel2_CanUseCustomAvatar") == 0)
				{
					ScalePlayerAccordingly(playerInfo);
				}
				break;
			case PlayerRankStatus.Trusted:
				if (PlayerPrefs.GetInt("CustomTrustLevel_AdvancedTrustLevel1_CanUseCustomAvatar") == 0)
				{
					ScalePlayerAccordingly(playerInfo);
				}
				break;
			case PlayerRankStatus.Friend:
				if (PlayerPrefs.GetInt("CustomTrustLevel_Friend_CanUseCustomAvatar") == 0)
				{
					ScalePlayerAccordingly(playerInfo);
				}
				break;
			case (PlayerRankStatus)7:
			case (PlayerRankStatus)8:
				break;
			}
		}

		private static void SetAvatarVisibility(PlayerInformation playerInfo, bool state)
		{
			if (!(playerInfo.GetAvatar() == null) && playerInfo.GetAvatar().active != state)
			{
				playerInfo.GetAvatar().SetActive(state);
			}
		}

		private static bool UpdatePlayerVisibility(PlayerInformation playerInfo)
		{
			if (!Configuration.GetGeneralConfig().AdvancedAvatarHider)
			{
				return false;
			}
			if (playerInfo.isLocalPlayer)
			{
				return false;
			}
			if (playerInfo.GetAvatar() == null)
			{
				return false;
			}
			float num = Vector3.Distance(playerInfo.GetAvatar().transform.position, CameraFeaturesHandler.GetActiveCamera().transform.position);
			if (num > 25f)
			{
				SetAvatarVisibility(playerInfo, state: false);
				return true;
			}
			if (num > 2f)
			{
				Vector3 direction = CameraFeaturesHandler.GetActiveCamera().transform.TransformDirection(Vector3.forward);
				RaycastHit hitInfo;
				bool flag = !Physics.Raycast(CameraFeaturesHandler.GetActiveCamera().transform.position, direction, out hitInfo, 5f, playerLayerMask);
				if (!flag)
				{
					VRC_MirrorReflection component = hitInfo.transform.GetComponent<VRC_MirrorReflection>();
					flag = component == null;
				}
				if (flag)
				{
					Vector3 from = playerInfo.GetAvatar().transform.position + playerInfo.GetAvatar().transform.up - CameraFeaturesHandler.GetActiveCamera().transform.position;
					float num2 = Vector3.Angle(from, CameraFeaturesHandler.GetActiveCamera().transform.forward);
					if (num2 < -90f || num2 > 90f)
					{
						SetAvatarVisibility(playerInfo, state: false);
						return true;
					}
				}
			}
			Vector3 end = CameraFeaturesHandler.GetActiveCamera().transform.position + -CameraFeaturesHandler.GetActiveCamera().transform.right / 4f;
			RaycastHit hitInfo2;
			bool flag2 = Physics.Linecast(playerInfo.GetAvatar().transform.position + Vector3.up, end, out hitInfo2, playerLayerMask);
			if (flag2)
			{
				flag2 = hitInfo2.transform.name.Contains("mirror");
			}
			Vector3 end2 = CameraFeaturesHandler.GetActiveCamera().transform.position + CameraFeaturesHandler.GetActiveCamera().transform.right / 4f;
			RaycastHit hitInfo3;
			bool flag3 = Physics.Linecast(playerInfo.GetAvatar().transform.position + Vector3.up, end2, out hitInfo3, playerLayerMask);
			if (flag3)
			{
				flag3 = hitInfo3.transform.name.Contains("mirror");
			}
			Vector3 end3 = CameraFeaturesHandler.GetActiveCamera().transform.position + CameraFeaturesHandler.GetActiveCamera().transform.up / 4f;
			RaycastHit hitInfo4;
			bool flag4 = Physics.Linecast(playerInfo.GetAvatar().transform.position + Vector3.up, end3, out hitInfo4, playerLayerMask);
			if (flag4)
			{
				flag4 = hitInfo4.transform.name.Contains("mirror");
			}
			if (flag2 && flag3 && flag4)
			{
				SetAvatarVisibility(playerInfo, state: false);
				return true;
			}
			SetAvatarVisibility(playerInfo, state: true);
			return false;
		}

		private static void UpdatePlayerNameplate(PlayerInformation playerInfo, bool isOffscreen)
		{
			if (Configuration.GetGeneralConfig().SerializationDetection && PlayerEventsHandler.IsModerationEventsEnabled())
			{
				float num = 3f * (1f + Time.deltaTime * 55f);
				if (playerInfo.lastNetworkedVoicePacket > playerInfo.lastNetworkedUpdateTime + num && !GeneralUtils.CheckNotification(playerInfo.displayName, "serializing"))
				{
					GeneralUtils.ToggleNotification(playerInfo.displayName, "serializing", state: true);
					string text = LanguageManager.GetUsedLanguage().ModerationSerializationDetected.Replace("{username}", playerInfo.displayName);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text, System.ConsoleColor.Gray, "UpdatePlayerNameplate", 525);
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text);
				}
			}
			if (Configuration.GetGeneralConfig().NameplateRankColor)
			{
				Color color = PlayerUtils.playerColorCache[playerInfo.displayName];
				Color color2 = new Color(color.r, color.g, color.b, 0.5f);
				playerInfo.vrcPlayer.field_Public_PlayerNameplate_0.field_Public_TextMeshProUGUI_0.color = color;
				playerInfo.vrcPlayer.field_Public_PlayerNameplate_0.field_Public_TextMeshProUGUI_2.color = color;
				playerInfo.nameplateBackground.color = color2;
				playerInfo.nameplateIconBackground.color = color2;
			}
			if (playerInfo.isLocalPlayer)
			{
				return;
			}
			int field_Private_Int32_ = playerInfo.vrcPlayer.prop_PlayerNet_0.field_Private_Int32_0;
			float num2 = Time.realtimeSinceStartup - playerInfo.lastNetworkedUpdateTime;
			int ping = playerInfo.GetPing();
			float num3 = 0.5f + 0.011f * (float)PlayerUtils.playerCachingList.Count + Mathf.Min(MathUtils.Clamp(ping, 0, 1000), 500f) / 1000f;
			if (num2 > num3 && playerInfo.lagBarrier < 5)
			{
				playerInfo.lagBarrier++;
			}
			if (playerInfo.lastNetworkedUpdatePacketNumber != field_Private_Int32_)
			{
				playerInfo.lastNetworkedUpdatePacketNumber = field_Private_Int32_;
				playerInfo.lastNetworkedUpdateTime = Time.realtimeSinceStartup;
				playerInfo.lagBarrier--;
			}
			if (isOffscreen || !Configuration.GetGeneralConfig().NameplateMoreInfo)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (Configuration.GetGeneralConfig().ShowActorID)
			{
				stringBuilder.Append($"ActorID: <color=red>{playerInfo.actorId}<color=white> | ");
			}
			if (playerInfo.isVRChatStaff)
			{
				stringBuilder.Append("<color=white>[<color=#00FFFF>VRChat Staff<color=white>] | ");
			}
			else if (Configuration.GetGeneralConfig().DetectClientUser && PlayerUtils.IsClientUser(playerInfo))
			{
				stringBuilder.Append("<color=white>[<color=red>Client User<color=white>] | ");
			}
			if (playerInfo.blockedLocalPlayer)
			{
				stringBuilder.Append("<color=white>[<color=red>Blocked<color=white>] | ");
			}
			if (Configuration.GetGeneralConfig().ShowInstanceOwner && playerInfo.isInstanceMaster)
			{
				stringBuilder.Append("<color=white>[<color=blue>Host<color=white>] | ");
			}
			if (!playerInfo.isQuestUser)
			{
				if (playerInfo.isVRUser)
				{
					stringBuilder.Append("<color=white>[<color=green>VR<color=white>] | ");
				}
				else
				{
					stringBuilder.Append("<color=white>[<color=green>PC<color=white>] | ");
				}
			}
			else
			{
				stringBuilder.Append("<color=white>[<color=green>Quest<color=white>] | ");
			}
			if (Configuration.GetGeneralConfig().ShowAvatarStatus && playerInfo.vrcPlayer.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0 != null)
			{
				if (playerInfo.vrcPlayer.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.releaseStatus == "private")
				{
					stringBuilder.Append("<color=white>[<color=red>Private<color=white>] | ");
				}
				else
				{
					stringBuilder.Append("<color=white>[<color=green>Public<color=white>] | ");
				}
			}
			if (Configuration.GetGeneralConfig().DetectLagOrCrash)
			{
				if (num2 > 10f)
				{
					stringBuilder.Append("<color=white>[<color=red>Crashed<color=white>] | ");
				}
				else if (playerInfo.lagBarrier > 0)
				{
					stringBuilder.Append("<color=white>[<color=yellow>Lagging<color=white>] | ");
				}
				else
				{
					stringBuilder.Append("<color=white>[<color=green>Stable<color=white>] | ");
				}
				playerInfo.lagBarrier = MelonUtils.Clamp(playerInfo.lagBarrier, 0, 5);
			}
			if (ping > 200)
			{
				stringBuilder.Append($"Ping: <color=red>{ping}<color=white> | ");
			}
			else if (ping > 100)
			{
				stringBuilder.Append($"Ping: <color=yellow>{ping}<color=white> | ");
			}
			else
			{
				stringBuilder.Append($"Ping: <color=green>{ping}<color=white> | ");
			}
			int fPS = playerInfo.GetFPS();
			if (fPS < 25)
			{
				stringBuilder.Append($"FPS: <color=red>{fPS}<color=white>");
			}
			else if (fPS < 50)
			{
				stringBuilder.Append($"FPS: <color=yellow>{fPS}<color=white>");
			}
			else
			{
				stringBuilder.Append($"FPS: <color=green>{fPS}<color=white>");
			}
			playerInfo.customNameplateText.text = stringBuilder.ToString();
		}

		internal static void IncrementPlayerVolume(float volume)
		{
			USpeaker.field_Internal_Static_Single_1 += volume;
		}

		internal static void DecrementPlayerVolume(float volume)
		{
			USpeaker.field_Internal_Static_Single_1 -= volume;
		}

		internal static void SetPlayerVolume(float volume)
		{
			USpeaker.field_Internal_Static_Single_1 = volume;
		}

		internal static float GetPlayerVolume()
		{
			return USpeaker.field_Internal_Static_Single_1;
		}

		internal static void IncreasePlayerSpeed(PlayerInformation player, float speed)
		{
			player.input.field_Public_Single_1 += speed;
			player.input.field_Public_Single_2 += speed * 2f;
			player.input.field_Public_Single_0 += speed;
		}

		internal static void DecreasePlayerSpeed(PlayerInformation player, float speed)
		{
			player.input.field_Public_Single_1 -= speed;
			player.input.field_Public_Single_2 -= speed * 2f;
			player.input.field_Public_Single_0 -= speed;
		}

		internal static void IncreasePlayerJumpPower(PlayerInformation player, float speed)
		{
			if (originalJumpPower == -1f)
			{
				originalJumpPower = player.input.field_Public_Single_3;
			}
			player.input.field_Public_Single_3 += speed;
		}

		internal static void DecreasePlayerJumpPower(PlayerInformation player, float speed)
		{
			if (originalJumpPower == -1f)
			{
				originalJumpPower = player.input.field_Public_Single_3;
			}
			player.input.field_Public_Single_3 -= speed;
		}

		internal static void ResetPlayerSpeed(PlayerInformation player)
		{
			player.input.field_Public_Single_1 = originalWalkSpeed;
			player.input.field_Public_Single_2 = originalRunSpeed;
			player.input.field_Public_Single_0 = originalStrafeSpeed;
		}

		internal static void ResetGravity()
		{
			Physics.gravity = originalGravity;
		}

		internal static void ResetJumpPower(PlayerInformation player)
		{
			if (originalJumpPower == -1f)
			{
				originalJumpPower = player.input.field_Public_Single_3;
			}
			player.input.field_Public_Single_3 = originalJumpPower;
		}

		internal static void IncreasePlayerFlightSpeed(float speed)
		{
			Configuration.GetGeneralConfig().FlightSpeed += speed;
			Configuration.SaveGeneralConfig();
		}

		internal static void DecreasePlayerFlightSpeed(float speed)
		{
			Configuration.GetGeneralConfig().FlightSpeed -= speed;
			Configuration.SaveGeneralConfig();
		}
	}
}
