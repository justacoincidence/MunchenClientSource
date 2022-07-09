using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Patching.Patches;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class AntiPortalHandler : ModuleComponent
	{
		private readonly Dictionary<IntPtr, PortalTracker> trackedPortals = new Dictionary<IntPtr, PortalTracker>();

		protected override string moduleName => "Anti Portal Handler";

		internal override void OnUpdate()
		{
			if (!Configuration.GetGeneralConfig().PortalsProtectionsDeleteUnused)
			{
				return;
			}
			for (int i = 0; i < trackedPortals.Count; i++)
			{
				KeyValuePair<IntPtr, PortalTracker> keyValuePair = trackedPortals.ElementAt(i);
				if (!keyValuePair.Value.isMine && Time.realtimeSinceStartup - keyValuePair.Value.lastTimerSet > 2f)
				{
					Networking.Destroy(keyValuePair.Value.portal.gameObject);
					string text = LanguageManager.GetUsedLanguage().PortalDroppedUnused.Replace("{username}", keyValuePair.Value.owner);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text, ConsoleColor.Gray, "OnUpdate", 52);
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text);
					trackedPortals.Remove(keyValuePair.Value.portal.Pointer);
				}
			}
		}

		internal override bool OnPortalEntered(ref PortalInternal portal)
		{
			if (!Configuration.GetGeneralConfig().PortalsProtectionsConfirmation)
			{
				return true;
			}
			string newValue = PlayerWrappers.GetPlayer(portal)?.displayName ?? "World";
			string id = portal.field_Private_ApiWorld_0.id;
			string newValue2 = ((portal.field_Private_String_4 != null) ? InstanceHistoryHandler.GetInstanceID(portal.field_Private_String_4) : LanguageManager.GetUsedLanguage().PortalConfirmationRandomInstance);
			IntPtr portalPointer = portal.Pointer;
			GeneralWrappers.AlertAction(portal.field_Private_ApiWorld_0.name, LanguageManager.GetUsedLanguage().PortalConfirmationInstance.Replace("{instanceId}", newValue2) + "\n" + LanguageManager.GetUsedLanguage().PortalConfirmationDropper.Replace("{username}", newValue), LanguageManager.GetUsedLanguage().PortalConfirmationEnter, delegate
			{
				PortalsPatch.originalPortalEntered(portalPointer);
				GeneralWrappers.ClosePopup();
			}, LanguageManager.GetUsedLanguage().CancelText, delegate
			{
				GeneralWrappers.ClosePopup();
			});
			return false;
		}

		internal override void OnPortalDestroyed(ref PortalInternal portal)
		{
			trackedPortals.Remove(portal.Pointer);
		}

		internal override void OnPortalSetTimer(ref PortalInternal portal, float timer)
		{
			if (!trackedPortals.ContainsKey(portal.Pointer) || trackedPortals[portal.Pointer].isMine)
			{
				return;
			}
			if (Configuration.GetGeneralConfig().PortalsProtectionsDeleteBadlyConfigured)
			{
				if (timer < 0f || timer < trackedPortals[portal.Pointer].lastPortalTimer)
				{
					ConsoleUtils.Info("Debug", $"Portal: {portal.field_Private_ApiWorld_0.name} | {timer} | {trackedPortals[portal.Pointer].lastPortalTimer}", ConsoleColor.Gray, "OnPortalSetTimer", 108);
					Networking.Destroy(portal.gameObject);
					string text = LanguageManager.GetUsedLanguage().PortalDroppedBadlyConfiguredRemoved.Replace("{username}", trackedPortals[portal.Pointer].owner);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text, ConsoleColor.Gray, "OnPortalSetTimer", 115);
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text);
					trackedPortals.Remove(portal.Pointer);
					return;
				}
				if (!trackedPortals[portal.Pointer].checkedWorld && trackedPortals[portal.Pointer].portal.field_Private_ApiWorld_0 != null)
				{
					int num = Mathf.Abs(portal.field_Private_ApiWorld_0.name.Length - Encoding.UTF8.GetByteCount(portal.field_Private_ApiWorld_0.name));
					if (num > portal.field_Private_ApiWorld_0.name.Length / 2)
					{
						ConsoleUtils.Info("Debug", $"Portal: {portal.field_Private_ApiWorld_0.name} | {num} | {portal.field_Private_ApiWorld_0.name.Length} | {Encoding.UTF8.GetByteCount(portal.field_Private_ApiWorld_0.name)}", ConsoleColor.Gray, "OnPortalSetTimer", 129);
						Networking.Destroy(portal.gameObject);
						string text2 = LanguageManager.GetUsedLanguage().PortalDroppedBadlyConfiguredRemoved.Replace("{username}", trackedPortals[portal.Pointer].owner);
						ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text2, ConsoleColor.Gray, "OnPortalSetTimer", 136);
						GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text2);
						trackedPortals.Remove(portal.Pointer);
						return;
					}
					trackedPortals[portal.Pointer].checkedWorld = true;
				}
			}
			trackedPortals[portal.Pointer].lastTimerSet = Time.realtimeSinceStartup;
			trackedPortals[portal.Pointer].lastPortalTimer = timer;
		}

		internal override void OnPortalCreated(ref PortalInternal portal, string worldId, string instanceId, int playerCount)
		{
			PlayerInformation player = PlayerWrappers.GetPlayer(portal);
			if (player == null)
			{
				return;
			}
			trackedPortals.Add(portal.Pointer, new PortalTracker
			{
				portal = portal,
				lastTimerSet = Time.realtimeSinceStartup,
				lastPortalTimer = portal.field_Private_Single_1,
				checkedWorld = false,
				owner = player.displayName,
				isMine = player.isLocalPlayer
			});
			if (player.isLocalPlayer)
			{
				return;
			}
			if (Configuration.GetGeneralConfig().PortalsProtectionsDeleteBadlyConfigured && IsPortalBadlyConfigured(ref portal, worldId, instanceId, playerCount))
			{
				Networking.Destroy(portal.gameObject);
				string text = LanguageManager.GetUsedLanguage().PortalDroppedBadlyConfiguredRemoved.Replace("{username}", player.displayName);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text, ConsoleColor.Gray, "OnPortalCreated", 182);
				GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text);
				trackedPortals.Remove(portal.Pointer);
				return;
			}
			if (APIUser.IsFriendsWith(player.id))
			{
				if (Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteFriends)
				{
					Networking.Destroy(portal.gameObject);
					string text2 = LanguageManager.GetUsedLanguage().PortalDroppedByFriendRemoved.Replace("{username}", player.displayName);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text2, ConsoleColor.Gray, "OnPortalCreated", 198);
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text2);
					trackedPortals.Remove(portal.Pointer);
					return;
				}
			}
			else if (Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteNonFriends)
			{
				Networking.Destroy(portal.gameObject);
				string text3 = LanguageManager.GetUsedLanguage().PortalDroppedByStrangerRemoved.Replace("{username}", player.displayName);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text3, ConsoleColor.Gray, "OnPortalCreated", 214);
				GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text3);
				trackedPortals.Remove(portal.Pointer);
				return;
			}
			float num = Vector3.Distance(player.player.transform.position, portal.transform.position);
			if (!(num > 4f) || !Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlaced)
			{
				return;
			}
			string text4 = LanguageManager.GetUsedLanguage().PortalDroppedToofarRemoved.Replace("{username}", player.displayName).Replace("{distance}", num.ToString());
			if (APIUser.IsFriendsWith(player.id))
			{
				if (!Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlacedAllowFriends)
				{
					Networking.Destroy(portal.gameObject);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text4, ConsoleColor.Gray, "OnPortalCreated", 238);
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text4);
					trackedPortals.Remove(portal.Pointer);
				}
			}
			else
			{
				Networking.Destroy(portal.gameObject);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PortalsMenuName, text4, ConsoleColor.Gray, "OnPortalCreated", 248);
				GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PortalsMenuName, text4);
				trackedPortals.Remove(portal.Pointer);
			}
		}

		internal bool IsPortalBadlyConfigured(ref PortalInternal portal, string worldId, string instanceId, int playerCount)
		{
			if (playerCount < 0 || playerCount > 40)
			{
				ConsoleUtils.Info("Debug", $"1. Portal: {portal.field_Private_ApiWorld_0.name} | {playerCount}", ConsoleColor.Gray, "IsPortalBadlyConfigured", 262);
				return true;
			}
			if (!WorldUtils.IsWorldIdValid(worldId))
			{
				ConsoleUtils.Info("Debug", "2. Portal: " + portal.field_Private_ApiWorld_0.name + " | " + worldId, ConsoleColor.Gray, "IsPortalBadlyConfigured", 271);
				return true;
			}
			if (!WorldUtils.IsInstanceIdValid(instanceId))
			{
				ConsoleUtils.Info("Debug", "3. Portal: " + portal.field_Private_ApiWorld_0.name + " | " + instanceId, ConsoleColor.Gray, "IsPortalBadlyConfigured", 280);
				return true;
			}
			if (instanceId.Length < 4 || portal.field_Private_String_4.Length < 4)
			{
				ConsoleUtils.Info("Debug", $"4. Portal: {portal.field_Private_ApiWorld_0.name} | {instanceId.Length} | {portal.field_Private_String_4.Length}", ConsoleColor.Gray, "IsPortalBadlyConfigured", 289);
				return true;
			}
			if (worldId.Length != Encoding.UTF8.GetByteCount(worldId))
			{
				ConsoleUtils.Info("Debug", $"5. Portal: {portal.field_Private_ApiWorld_0.name} | {worldId.Length} | {Encoding.UTF8.GetByteCount(worldId)}", ConsoleColor.Gray, "IsPortalBadlyConfigured", 299);
				return true;
			}
			if (instanceId.Length != Encoding.UTF8.GetByteCount(instanceId))
			{
				ConsoleUtils.Info("Debug", $"6. Portal: {portal.field_Private_ApiWorld_0.name} | {instanceId.Length} | {Encoding.UTF8.GetByteCount(instanceId)}", ConsoleColor.Gray, "IsPortalBadlyConfigured", 309);
				return true;
			}
			if (portal.field_Private_String_4.Length != Encoding.UTF8.GetByteCount(portal.field_Private_String_4))
			{
				ConsoleUtils.Info("Debug", $"7. Portal: {portal.field_Private_ApiWorld_0.name} | {portal.field_Private_String_4.Length} | {Encoding.UTF8.GetByteCount(portal.field_Private_String_4)}", ConsoleColor.Gray, "IsPortalBadlyConfigured", 319);
				return true;
			}
			return false;
		}
	}
}
