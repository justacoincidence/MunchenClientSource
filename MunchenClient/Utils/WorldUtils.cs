using System;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Wrappers;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.SDKBase;

namespace MunchenClient.Utils
{
	internal class WorldUtils
	{
		private static GameObject voidClubAnnoyingEntryDoor;

		private static GameObject blindHud;

		private static GameObject flashbangHud;

		private static GameObject basementDoor;

		private static GameObject kitchenDoor;

		private static GameObject kitchenDoorChrome;

		private static GameObject kitchenDoorGlass;

		private static GameObject kitchenDoorCollider;

		private static GameObject annoyingIntro;

		internal static bool isAmongUsGame;

		internal static bool isJustBClub;

		internal static bool isFBTHeaven;

		internal static void GoToWorld(string roomID, List<KeyCode> pressedKeys, Text text)
		{
			GoToWorld(roomID);
		}

		internal static void GoToWorld(string worldId, bool logErrorToHud = true)
		{
			string[] idSplit = worldId.Split(':');
			if (idSplit.Length != 2)
			{
				ConsoleUtils.Info("Player", "Can't go to world (Invalid input)", System.ConsoleColor.Gray, "GoToWorld", 44);
				if (logErrorToHud)
				{
					GeneralWrappers.AlertPopup("Player", "Can't go to world (Invalid input)");
				}
				return;
			}
			if (!IsInstanceIdValid(idSplit[1]))
			{
				ConsoleUtils.Info("Player", "Can't go to world (Invalid instance id)", System.ConsoleColor.Gray, "GoToWorld", 56);
				if (logErrorToHud)
				{
					GeneralWrappers.AlertPopup("Player", "Can't go to world (Invalid instance id)");
				}
				return;
			}
			int num = idSplit[1].IndexOf('~');
			string instanceId2 = ((num != -1) ? idSplit[1].Substring(0, num) : idSplit[1]);
			IsWorldValid(idSplit[0], instanceId2, delegate
			{
				new PortalInternal().Method_Private_Void_String_String_PDM_0(idSplit[0], idSplit[1]);
				ConsoleUtils.Info("Player", "Joining world: " + idSplit[0] + ":" + idSplit[1], System.ConsoleColor.Gray, "GoToWorld", 73);
			}, delegate(string reason)
			{
				ConsoleUtils.Info("Player", "Can't go to world (" + reason + ")", System.ConsoleColor.Gray, "GoToWorld", 90);
				if (logErrorToHud)
				{
					GeneralWrappers.AlertPopup("Player", "Can't go to world (" + reason + ")");
				}
			});
		}

		internal static bool IsInstanceIdValid(string instanceId)
		{
			if (string.IsNullOrEmpty(instanceId))
			{
				return false;
			}
			if (instanceId.Length > 200)
			{
				return false;
			}
			return true;
		}

		internal static bool IsWorldIdValid(string worldId)
		{
			if (string.IsNullOrEmpty(worldId))
			{
				return false;
			}
			if (worldId.Length != 41 || !worldId.StartsWith("wrld_") || worldId[13] != '-' || worldId[18] != '-' || worldId[23] != '-' || worldId[28] != '-')
			{
				return false;
			}
			return true;
		}

		internal static bool IsWorldValid(string worldId, string instanceId, System.Action<ApiWorld, string> onSuccess, System.Action<string> onFailed = null)
		{
			if (!IsInstanceIdValid(instanceId))
			{
				onFailed?.Invoke("InstanceId is invalid");
				return false;
			}
			if (!IsWorldIdValid(worldId))
			{
				onFailed?.Invoke("WorldID is invalid");
				return false;
			}
			API.Fetch<ApiWorld>(worldId, (System.Action<ApiContainer>)delegate(ApiContainer apiContainer)
			{
				onSuccess(apiContainer.Model.Cast<ApiWorld>(), instanceId);
			}, (System.Action<ApiContainer>)delegate(ApiContainer apiContainer)
			{
				onFailed?.Invoke(apiContainer.Error);
			});
			return true;
		}

		internal static void DropPortal(string worldId, string instanceId, int playerCount = 0, float portalTime = 30f)
		{
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			Vector3 position = localPlayerInformation.vrcPlayer.transform.position + localPlayerInformation.vrcPlayer.transform.forward * 2f;
			Quaternion rotation = localPlayerInformation.vrcPlayer.transform.rotation;
			DropPortal(worldId, instanceId, playerCount, portalTime, position, rotation);
		}

		internal static void DropPortal(string worldId, string instanceId, int playerCount, float portalTime, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", position, rotation);
			Networking.RPC(RPC.Destination.AllBufferOne, gameObject, "ConfigurePortal", new Il2CppSystem.Object[3]
			{
				(Il2CppSystem.String)worldId,
				(Il2CppSystem.String)instanceId,
				new Il2CppSystem.Int32
				{
					m_value = playerCount
				}.BoxIl2CppObject()
			});
			gameObject.GetComponent<PortalInternal>().field_Private_Single_1 = 30f - portalTime;
		}

		internal static ApiWorld GetCurrentWorld()
		{
			return RoomManager.field_Internal_Static_ApiWorld_0;
		}

		internal static ApiWorldInstance GetCurrentInstance()
		{
			return RoomManager.field_Internal_Static_ApiWorldInstance_0;
		}

		internal static void CacheAnnoyingGameObjects()
		{
			voidClubAnnoyingEntryDoor = GameObject.Find("Cyber_Entrance/Cyberpunk_street0.9/Building_lift.006");
			blindHud = GameObject.Find("Game Logic/Player HUD/Blind HUD Anim");
			flashbangHud = GameObject.Find("Game Logic/Player HUD/Flashbang HUD Anim");
			basementDoor = GameObject.Find(" - Props/Props (Static) - Hallways - First Floor/door-private");
			kitchenDoor = GameObject.Find("great_pug/kitchen_door");
			kitchenDoorChrome = GameObject.Find("great_pug/kitchen_door_chrome");
			kitchenDoorGlass = GameObject.Find("great_pug/Cube_022  (GLASS)");
			kitchenDoorCollider = GameObject.Find("great_pug/door-frame_004");
			annoyingIntro = GameObject.Find("Lobby/Entrance Corridor/Cancer Spawn");
		}

		internal static void FixVoidClubAnnoyingEntryDoor(bool enableFix)
		{
			if (voidClubAnnoyingEntryDoor != null)
			{
				voidClubAnnoyingEntryDoor.SetActive(!enableFix);
			}
		}

		internal static void FixMurderAntiKillscreen(bool enableFix)
		{
			if (blindHud != null)
			{
				blindHud.transform.localScale = (enableFix ? new Vector3(0f, 0f, 0f) : new Vector3(1f, 1f, 1f));
			}
			if (flashbangHud != null)
			{
				flashbangHud.transform.localScale = (enableFix ? new Vector3(0f, 0f, 0f) : new Vector3(1f, 1f, 1f));
			}
		}

		internal static void FixUnnecessaryDoorsInTheGreatPug(bool enableFix)
		{
			if (basementDoor != null)
			{
				basementDoor.SetActive(!enableFix);
			}
			if (kitchenDoor != null)
			{
				kitchenDoor.SetActive(!enableFix);
			}
			if (kitchenDoorChrome != null)
			{
				kitchenDoorChrome.SetActive(!enableFix);
			}
			if (kitchenDoorGlass != null)
			{
				kitchenDoorGlass.SetActive(!enableFix);
			}
			if (kitchenDoorCollider != null)
			{
				kitchenDoorCollider.GetComponent<BoxCollider>().enabled = !enableFix;
			}
		}

		internal static void FixAnnoyingIntroInJustBClub(bool enableFix)
		{
			if (annoyingIntro != null)
			{
				annoyingIntro.SetActive(!enableFix);
			}
		}
	}
}
