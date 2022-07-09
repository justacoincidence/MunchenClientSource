using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Menu.Fun;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC;
using VRC.Networking;
using VRC.SDKBase;
using VRC.Udon;

namespace MunchenClient.Patching.Patches
{
	internal class UdonPatch : PatchComponent
	{
		internal static readonly Dictionary<string, WorldTriggerFilter> worldFilters = new Dictionary<string, WorldTriggerFilter>
		{
			{
				"wrld_4cf554b4-430c-4f8f-b53e-1f294eed230b",
				new WorldTriggerFilter
				{
					worldType = WorldTriggerType.NotNetworked
				}
			},
			{
				"wrld_d29bb0d0-d268-42dc-8365-926f9d485505",
				new WorldTriggerFilter
				{
					worldType = WorldTriggerType.NotNetworked
				}
			},
			{
				"wrld_791ebf58-54ce-4d3a-a0a0-39f10e1b20b2",
				new WorldTriggerFilter
				{
					worldType = WorldTriggerType.PartiallyNetworked,
					objectType = new Dictionary<string, ObjectTriggerType> { 
					{
						"SomeFuckingTeleportLmao",
						ObjectTriggerType.LocalOnly
					} }
				}
			}
		};

		protected override string patchName => "UdonPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(UdonPatch));
			PatchMethod(typeof(UdonSync).GetMethod("UdonSyncRunProgramAsRPC"), GetLocalPatch("PreOnUdonRPCReceivedPatch"), GetLocalPatch("PostOnUdonRPCReceivedPatch"));
			PatchMethod((from m in typeof(UdonBehaviour).GetMethods()
				where m.Name.Equals("RunProgram") && m.GetParameters()[0].ParameterType == typeof(string)
				select m).First(), GetLocalPatch("OnUdonRunProgramStringPatch"), null);
			PatchMethod((from m in typeof(UdonBehaviour).GetMethods()
				where m.Name.Equals("RunProgram") && m.GetParameters()[0].ParameterType == typeof(uint)
				select m).First(), GetLocalPatch("OnUdonRunProgramUintPatch"), null);
		}

		private static bool PreOnUdonRPCReceivedPatch(UdonSync __instance, string __0, Player __1)
		{
			if (!IsUdonOkayToRun(__instance.gameObject, __1))
			{
				return false;
			}
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			if (Configuration.GetGeneralConfig().BlockAllUdonEvents)
			{
				if (localPlayerInformation == null)
				{
					return false;
				}
				if (__1 != localPlayerInformation.player)
				{
					return false;
				}
			}
			if (__1 != null && Configuration.GetGeneralConfig().BlockedPlayerEvents.ContainsKey(__1.field_Private_APIUser_0.id) && Configuration.GetGeneralConfig().BlockedPlayerEvents[__1.field_Private_APIUser_0.id].BlockedUdon)
			{
				return false;
			}
			string text = __0.ToLower();
			if (localPlayerInformation != null && __1 != null && text.Contains("kill") && __1 == localPlayerInformation.player && MurderAmongUsMenu.currentSkippableSyncKills > 0)
			{
				MurderAmongUsMenu.currentSkippableSyncKills--;
				return false;
			}
			if ((text.Contains("kill") || text.Contains("penalty") || text.Contains("votedout") || text.Contains("freezeme")) && Configuration.GetGeneralConfig().UdonGodmode)
			{
				if (localPlayerInformation == null)
				{
					return false;
				}
				if (__1.prop_APIUser_0.displayName != localPlayerInformation.displayName)
				{
					return false;
				}
			}
			if (text.Contains("synclock") && Configuration.GetGeneralConfig().UdonGodmode)
			{
				if (localPlayerInformation == null || !(__1 != null))
				{
					return false;
				}
				if (__1.prop_APIUser_0.displayName != localPlayerInformation.displayName)
				{
					return false;
				}
			}
			return true;
		}

		private static void PostOnUdonRPCReceivedPatch(string __0)
		{
			string text = __0.ToLower();
			if (text.Contains("fire") && Configuration.GetGeneralConfig().MurderForceWeaponPickupable)
			{
				VRC_Pickup[] array = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						array[i].pickupable = true;
						array[i].DisallowTheft = false;
					}
				}
			}
			if (text.Contains("startgame") && Configuration.GetGeneralConfig().MurderAnnounceKiller)
			{
				MurderAmongUsMenu.FindKiller(6f);
			}
		}

		private static bool OnUdonRunProgramStringPatch(UdonBehaviour __instance, string __0)
		{
			if (WorldUtils.isFBTHeaven && Configuration.GetGeneralConfig().FBTHeavenDoorBypass && !__0.StartsWith("_") && __instance.name.StartsWith("Room"))
			{
				return false;
			}
			return true;
		}

		private static bool OnUdonRunProgramUintPatch(uint __0)
		{
			if (WorldUtils.isJustBClub)
			{
				if (Configuration.GetGeneralConfig().JustBClubDoorBypass && __0 == 3500)
				{
					return false;
				}
				if (Configuration.GetGeneralConfig().JustBClubVIPBypass && __0 == 6000)
				{
					PlayerWrappers.GetLocalPlayerInformation().apiUser.displayName = "Blue-Kun";
					MelonCoroutines.Start(RestoreJustBClubVIPBypass());
				}
			}
			return true;
		}

		private static IEnumerator RestoreJustBClubVIPBypass()
		{
			yield return new WaitForEndOfFrame();
			PlayerInformation localPlayer = PlayerWrappers.GetLocalPlayerInformation();
			localPlayer.apiUser.displayName = localPlayer.displayName;
		}

		internal static bool IsUdonOkayToRun(GameObject udonObject, Player sender)
		{
			if (!Configuration.GetGeneralConfig().AntiWorldTriggers)
			{
				return true;
			}
			string key = WorldUtils.GetCurrentWorld()?.id;
			if (worldFilters.ContainsKey(key))
			{
				switch (worldFilters[key].worldType)
				{
				case WorldTriggerType.NotNetworked:
					return false;
				case WorldTriggerType.MasterOnlyNetworked:
					if (!sender.prop_VRCPlayerApi_0.isMaster)
					{
						return false;
					}
					break;
				case WorldTriggerType.PartiallyNetworked:
					if (!worldFilters[key].objectType.ContainsKey(udonObject.name))
					{
						break;
					}
					switch (worldFilters[key].objectType[udonObject.name])
					{
					case ObjectTriggerType.LocalOnly:
						return false;
					case ObjectTriggerType.MasterOnly:
						if (!sender.prop_VRCPlayerApi_0.isMaster)
						{
							return false;
						}
						break;
					}
					return false;
				}
			}
			return true;
		}
	}
}
