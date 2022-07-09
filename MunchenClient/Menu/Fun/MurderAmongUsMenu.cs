using System.Collections;
using Il2CppSystem;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;
using VRC.Networking;
using VRC.SDKBase;

namespace MunchenClient.Menu.Fun
{
	internal class MurderAmongUsMenu : QuickMenuNestedMenu
	{
		private static bool isFindingKiller = false;

		internal static int currentSkippableSyncKills = 0;

		private static readonly Il2CppSystem.Object[] syncKillParameters = new Il2CppSystem.Object[1] { (String)"SyncKill" };

		private static readonly Il2CppSystem.Object[] syncStartParameters = new Il2CppSystem.Object[1] { (String)"SyncStartGame" };

		private static readonly Il2CppSystem.Object[] syncVictoryBystanderParameters = new Il2CppSystem.Object[1] { (String)"SyncVictoryB" };

		private static readonly Il2CppSystem.Object[] syncVictoryMurdererParameters = new Il2CppSystem.Object[1] { (String)"SyncVictoryM" };

		private static readonly Il2CppSystem.Object[] syncAssignMurdererParameters = new Il2CppSystem.Object[1] { (String)"SyncAssignM" };

		private static readonly Il2CppSystem.Object[] syncAssignBystanderParameters = new Il2CppSystem.Object[1] { (String)"SyncAssignB" };

		private static readonly Il2CppSystem.Object[] syncAssignDetectiveParameters = new Il2CppSystem.Object[1] { (String)"SyncAssignD" };

		internal MurderAmongUsMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().MurderAmongUsMenuName, LanguageManager.GetUsedLanguage().MurderAmongUsDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow4 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AntiBystanderKillscreen, Configuration.GetGeneralConfig().MurderAntiKillscreen, delegate
			{
				Configuration.GetGeneralConfig().MurderAntiKillscreen = true;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixMurderAntiKillscreen(enableFix: true);
			}, LanguageManager.GetUsedLanguage().AntiBystanderKillscreenDescription, delegate
			{
				Configuration.GetGeneralConfig().MurderAntiKillscreen = false;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixMurderAntiKillscreen(enableFix: false);
			}, LanguageManager.GetUsedLanguage().AntiBystanderKillscreenDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().MurderForceWeaponPickupable, Configuration.GetGeneralConfig().MurderForceWeaponPickupable, delegate
			{
				Configuration.GetGeneralConfig().MurderForceWeaponPickupable = true;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixMurderAntiKillscreen(enableFix: true);
			}, LanguageManager.GetUsedLanguage().MurderForceWeaponPickupableDescription, delegate
			{
				Configuration.GetGeneralConfig().MurderForceWeaponPickupable = false;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixMurderAntiKillscreen(enableFix: false);
			}, LanguageManager.GetUsedLanguage().MurderForceWeaponPickupableDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AnnounceKiller, Configuration.GetGeneralConfig().MurderAnnounceKiller, delegate
			{
				Configuration.GetGeneralConfig().MurderAnnounceKiller = true;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixMurderAntiKillscreen(enableFix: true);
			}, LanguageManager.GetUsedLanguage().AnnounceKillerDescription, delegate
			{
				Configuration.GetGeneralConfig().MurderAnnounceKiller = false;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixMurderAntiKillscreen(enableFix: false);
			}, LanguageManager.GetUsedLanguage().AnnounceKillerDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AnnounceKiller, delegate
			{
				FindKiller(0f);
			}, LanguageManager.GetUsedLanguage().AnnounceKillerDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().TeleportShotgun, delegate
			{
				VRC_Pickup[] array8 = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int num2 = 0; num2 < array8.Length; num2++)
				{
					if (array8[num2].gameObject.name.ToLower().Contains("shotgun"))
					{
						Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array8[num2].gameObject);
						array8[num2].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
						array8[num2].pickupable = true;
						array8[num2].DisallowTheft = false;
						break;
					}
				}
			}, LanguageManager.GetUsedLanguage().TeleportShotgunDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().TeleportPistol, delegate
			{
				VRC_Pickup[] array7 = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int num = 0; num < array7.Length; num++)
				{
					if (array7[num].gameObject.name.ToLower().Contains("luger"))
					{
						Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array7[num].gameObject);
						array7[num].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
						array7[num].pickupable = true;
						array7[num].DisallowTheft = false;
						break;
					}
				}
			}, LanguageManager.GetUsedLanguage().TeleportPistolDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().TeleportKnife, delegate
			{
				VRC_Pickup[] array6 = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int n = 0; n < array6.Length; n++)
				{
					if (array6[n].gameObject.name.ToLower().Contains("knife"))
					{
						Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array6[n].gameObject);
						array6[n].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
						array6[n].pickupable = true;
						array6[n].DisallowTheft = false;
						break;
					}
				}
			}, LanguageManager.GetUsedLanguage().TeleportKnifeDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().TeleportRevolver, delegate
			{
				VRC_Pickup[] array5 = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int m = 0; m < array5.Length; m++)
				{
					if (array5[m].gameObject.name.ToLower().Contains("revolver"))
					{
						Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array5[m].gameObject);
						array5[m].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
						array5[m].pickupable = true;
						array5[m].DisallowTheft = false;
						break;
					}
				}
			}, LanguageManager.GetUsedLanguage().TeleportRevolverDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().TeleportGrenade, delegate
			{
				VRC_Pickup[] array4 = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int l = 0; l < array4.Length; l++)
				{
					if (array4[l].gameObject.name.ToLower().Contains("frag"))
					{
						Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array4[l].gameObject);
						array4[l].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
						array4[l].pickupable = true;
						array4[l].DisallowTheft = false;
						break;
					}
				}
			}, LanguageManager.GetUsedLanguage().TeleportGrenadeDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().TeleportBearTrap, delegate
			{
				VRC_Pickup[] array3 = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int k = 0; k < array3.Length; k++)
				{
					if (array3[k].gameObject.name.ToLower().Contains("bear"))
					{
						Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array3[k].gameObject);
						array3[k].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
						array3[k].pickupable = true;
						array3[k].DisallowTheft = false;
						break;
					}
				}
			}, LanguageManager.GetUsedLanguage().TeleportBearTrapDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().TeleportAllWeapons, delegate
			{
				VRC_Pickup[] array2 = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].gameObject.name.ToLower().Contains("shotgun") || array2[j].gameObject.name.ToLower().Contains("luger") || array2[j].gameObject.name.ToLower().Contains("knife") || array2[j].gameObject.name.ToLower().Contains("revolver") || array2[j].gameObject.name.ToLower().Contains("frag") || array2[j].gameObject.name.ToLower().Contains("bear"))
					{
						Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array2[j].gameObject);
						array2[j].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
						array2[j].pickupable = true;
						array2[j].DisallowTheft = false;
					}
				}
			}, LanguageManager.GetUsedLanguage().TeleportAllWeaponsDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().MakeAllWeaponsPickupable, delegate
			{
				VRC_Pickup[] array = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].gameObject.name.ToLower().Contains("shotgun") || array[i].gameObject.name.ToLower().Contains("luger") || array[i].gameObject.name.ToLower().Contains("knife") || array[i].gameObject.name.ToLower().Contains("revolver") || array[i].gameObject.name.ToLower().Contains("frag") || array[i].gameObject.name.ToLower().Contains("bear"))
					{
						array[i].pickupable = true;
						array[i].DisallowTheft = false;
					}
				}
			}, LanguageManager.GetUsedLanguage().MakeAllWeaponsPickupableDescription);
			new QuickMenuSingleButton(parentRow4, LanguageManager.GetUsedLanguage().MassMurder, delegate
			{
				currentSkippableSyncKills = 0;
				foreach (UdonSync item in Resources.FindObjectsOfTypeAll<UdonSync>())
				{
					if (item.gameObject.name.Contains("Player Node"))
					{
						currentSkippableSyncKills++;
						Networking.RPC(RPC.Destination.All, item.gameObject, "UdonSyncRunProgramAsRPC", syncKillParameters);
					}
				}
			}, LanguageManager.GetUsedLanguage().MassMurderDescription);
			new QuickMenuSingleButton(parentRow4, LanguageManager.GetUsedLanguage().ForceStartGame, delegate
			{
				UdonSync udonSync = null;
				foreach (UdonSync item2 in Resources.FindObjectsOfTypeAll<UdonSync>())
				{
					if (item2.gameObject.name == "Game Logic")
					{
						udonSync = item2;
					}
					else if (item2.gameObject.name.Contains("Player Node"))
					{
						Networking.RPC(RPC.Destination.All, item2.gameObject, "UdonSyncRunProgramAsRPC", GeneralUtils.fastRandom.NextBool() ? syncAssignBystanderParameters : (GeneralUtils.fastRandom.NextBool() ? syncAssignDetectiveParameters : syncAssignMurdererParameters));
					}
				}
				Networking.RPC(RPC.Destination.All, udonSync.gameObject, "UdonSyncRunProgramAsRPC", syncStartParameters);
			}, LanguageManager.GetUsedLanguage().ForceStartGameDescription);
			new QuickMenuSingleButton(parentRow4, LanguageManager.GetUsedLanguage().ForceEndGame, delegate
			{
				foreach (UdonSync item3 in Resources.FindObjectsOfTypeAll<UdonSync>())
				{
					if (item3.gameObject.name == "Game Logic")
					{
						Networking.RPC(RPC.Destination.All, item3.gameObject, "UdonSyncRunProgramAsRPC", GeneralUtils.fastRandom.NextBool() ? syncVictoryBystanderParameters : syncVictoryMurdererParameters);
						break;
					}
				}
			}, LanguageManager.GetUsedLanguage().ForceEndGameDescription);
		}

		internal static void FindKiller(float waitTime)
		{
			if (!isFindingKiller)
			{
				isFindingKiller = true;
				MelonCoroutines.Start(FindKillerEnumerator(waitTime, WorldUtils.isAmongUsGame));
			}
		}

		private static IEnumerator FindKillerEnumerator(float waitTime, bool isAmongUs)
		{
			yield return new WaitForSeconds(waitTime);
			GameObject murdererObj = GameObject.Find("Game Logic/Game Canvas/Postgame/Murderer Name");
			if (murdererObj != null)
			{
				string playerName = murdererObj.GetComponent<Text>().text;
				if (playerName != PlayerWrappers.GetLocalPlayerInformation().displayName)
				{
					GeneralUtils.InformHudText(isAmongUs ? "AmongUs" : "Murder", (isAmongUs ? "Imposter" : "Murder") + " is " + playerName, logToConsole: true);
				}
			}
			isFindingKiller = false;
		}
	}
}
