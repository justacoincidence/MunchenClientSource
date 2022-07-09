using MunchenClient.Config;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using UnityEngine;
using VRC.Core;

namespace MunchenClient.Patching.Patches
{
	internal class VRCPlayerPatch : PatchComponent
	{
		protected override string patchName => "VRCPlayerPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VRCPlayerPatch));
			if (!ApplicationBotHandler.IsBot())
			{
				PatchMethod(typeof(VRCPlayer).GetMethod("Method_Public_Static_String_APIUser_0"), GetLocalPatch("GetFriendlyDetailedNameForSocialRankPatch"), null);
				//PatchMethod(typeof(VRCPlayer).GetMethod("Method_Public_Static_String_APIUser_1"), GetLocalPatch("GetFriendlyDetailedNameForSocialRankPatch"), null);
				PatchMethod(typeof(VRCPlayer).GetMethod("Method_Public_Static_Color_APIUser_0"), GetLocalPatch("GetColorForSocialRankPatch"), null);
			}
			else
			{
				PatchMethod(typeof(VRCPlayer).GetMethod("Method_Public_Virtual_Final_New_Void_Single_Single_0"), GetLocalPatch("UnknownUpdatePatch"), null);
			}
		}

		private static bool UnknownUpdatePatch()
		{
			return false;
		}

		private static bool GetFriendlyDetailedNameForSocialRankPatch(APIUser __0, ref string __result)
		{
			if (__0 == null)
			{
				return true;
			}
			if (Configuration.GetGeneralConfig().RanksCustomRanks && PlayerUtils.playerCustomRank.ContainsKey(__0.id) && PlayerUtils.playerCustomRank[__0.id].customRankEnabled)
			{
				__result = PlayerUtils.playerCustomRank[__0.id].customRank;
				return false;
			}
			return true;
		}

		private static bool GetColorForSocialRankPatch(APIUser __0, ref Color __result)
		{
			if (__0 == null)
			{
				return true;
			}
			if (Configuration.GetGeneralConfig().RanksCustomRanks && PlayerUtils.playerCustomRank.ContainsKey(__0.id) && PlayerUtils.playerCustomRank[__0.id].customRankColorEnabled)
			{
				__result = PlayerUtils.playerCustomRank[__0.id].customRankColor;
				return false;
			}
			return true;
		}
	}
}
