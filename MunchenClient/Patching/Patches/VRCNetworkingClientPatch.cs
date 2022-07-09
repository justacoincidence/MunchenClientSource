using System;
using ExitGames.Client.Photon;
using MunchenClient.Core;
using MunchenClient.ModuleSystem;
using MunchenClient.Utils;
using VRC.Core;

namespace MunchenClient.Patching.Patches
{
	internal class VRCNetworkingClientPatch : PatchComponent
	{
		protected override string patchName => "VRCNetworkingClientPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VRCNetworkingClientPatch));
			PatchMethod(typeof(VRCNetworkingClient).GetMethod("OnEvent"), GetLocalPatch("VRCNetworkingClientReceiveEventPatch"), null);
		}

		private static bool VRCNetworkingClientReceiveEventPatch(ref EventData __0)
		{
			if (!Enum.IsDefined(typeof(EventCodes), __0.Code))
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, $"Unknown Event Found: {__0.Code}", ConsoleColor.Yellow, "VRCNetworkingClientReceiveEventPatch", 26);
			}
			return ModuleManager.OnEventReceived(ref __0);
		}
	}
}
