using System;
using MunchenClient.Utils;

namespace MunchenClient.Patching.Patches
{
	internal class VRCAPIPatch : PatchComponent
	{
		internal static bool sendGetRequestCallOriginal;

		protected override string patchName => "VRCAPIPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VRCAPIPatch));
		}

		private static bool SendGetRequestPatch(string __0)
		{
			ConsoleUtils.Info("SendGetRequestPatch", __0, ConsoleColor.Gray, "SendGetRequestPatch", 37);
			return true;
		}

		private static bool SendPostRequestPatch(string __0)
		{
			ConsoleUtils.Info("SendPostRequestPatch", __0, ConsoleColor.Gray, "SendPostRequestPatch", 44);
			return true;
		}

		private static bool SendPostFormRequestPatch(string __0)
		{
			ConsoleUtils.Info("SendPostFormRequestPatch", __0, ConsoleColor.Gray, "SendPostFormRequestPatch", 51);
			return true;
		}

		private static bool SendPutRequestPatch(string __0)
		{
			ConsoleUtils.Info("SendPutRequestPatch", __0, ConsoleColor.Gray, "SendPutRequestPatch", 58);
			return true;
		}

		private static bool SendDeleteRequestPatch(string __0)
		{
			ConsoleUtils.Info("SendDeleteRequestPatch", __0, ConsoleColor.Gray, "SendDeleteRequestPatch", 65);
			return true;
		}

		private static bool SendRequestPatch(string __0)
		{
			ConsoleUtils.Info("SendRequestPatch", __0, ConsoleColor.Gray, "SendRequestPatch", 72);
			return true;
		}

		private static bool SendRequestInternalPatch(string __0)
		{
			ConsoleUtils.Info("SendRequestInternalPatch", __0, ConsoleColor.Gray, "SendRequestInternalPatch", 79);
			return true;
		}
	}
}
