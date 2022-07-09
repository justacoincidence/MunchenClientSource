using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using MunchenClient.Utils;

namespace MunchenClient.Patching
{
	internal class Patcher
	{
		private static readonly HarmonyLib.Harmony harmonyInstance = new HarmonyLib.Harmony(GeneralUtils.GetClientName());

		private static readonly List<MethodBase> patchedMethods = new List<MethodBase>();

		internal static void PatchMethod(MethodBase targetMethod, HarmonyMethod preMethod, HarmonyMethod postMethod)
		{
			harmonyInstance.Patch(targetMethod, preMethod, postMethod);
			patchedMethods.Add(targetMethod);
		}

		internal static void UnpatchAllMethods()
		{
			for (int i = 0; i < patchedMethods.Count; i++)
			{
				harmonyInstance.Unpatch(patchedMethods[i], HarmonyPatchType.All, harmonyInstance.Id);
			}
			patchedMethods.Clear();
		}
	}
}
