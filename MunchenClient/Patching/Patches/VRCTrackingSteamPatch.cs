using System;
using System.Linq;
using System.Reflection;
using MunchenClient.Config;
using MunchenClient.Core.Compatibility;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal class VRCTrackingSteamPatch : PatchComponent
	{
		protected override string patchName => "VRCTrackingSteamPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VRCTrackingSteamPatch));
			if (!Configuration.GetGeneralConfig().DisableFBTSavingCompletely)
			{
				return;
			}
			if (!CompatibilityLayer.IsFBTSaverInstalled() || !CompatibilityLayer.IsIKTweaksInstalled())
			{
				ConsoleUtils.Info("CompatibilityLayer", "FBTSaver or IKTweaks installed - Disabling calibration caching", ConsoleColor.Gray, "OnInitializeOnStart", 26);
				return;
			}
			MethodInfo[] methods = typeof(VRCTrackingSteam).GetMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				switch (methods[i].GetParameters().Length)
				{
				case 1:
					if (methods[i].GetParameters().First().ParameterType == typeof(string) && methods[i].ReturnType == typeof(bool) && methods[i].GetRuntimeBaseDefinition() == methods[i])
					{
						PatchMethod(methods[i], GetLocalPatch("IsAvatarCalibratedPatch"), null);
					}
					break;
				case 3:
					if (methods[i].GetParameters().First().ParameterType == typeof(Animator) && methods[i].ReturnType == typeof(void) && methods[i].GetRuntimeBaseDefinition() == methods[i])
					{
						PatchMethod(methods[i], null, GetLocalPatch("OnAvatarCalibrationPerformedPatch"));
					}
					break;
				}
			}
		}

		private static void OnAvatarCalibrationPerformedPatch(ref VRCTrackingSteam __instance)
		{
			if (Configuration.GetAvatarCalibrationsConfig().SaveCalibrations)
			{
				Configuration.GetAvatarCalibrationsConfig().SavedCalibrations[PlayerWrappers.GetCurrentPlayer().prop_VRCAvatarManager_0.prop_ApiAvatar_2.id] = new CachedCalibration
				{
					LeftFootPosition = __instance.field_Public_Transform_10.localPosition,
					LeftFootRotation = __instance.field_Public_Transform_10.localRotation,
					RightFootPosition = __instance.field_Public_Transform_11.localPosition,
					RightFootRotation = __instance.field_Public_Transform_11.localRotation,
					HipPosition = __instance.field_Public_Transform_12.localPosition,
					HipRotation = __instance.field_Public_Transform_12.localRotation
				};
				Configuration.SaveAvatarCalibrationsConfig();
			}
		}

		private static bool IsAvatarCalibratedPatch(ref VRCTrackingSteam __instance, ref bool __result, string __0)
		{
			if (!Configuration.GetAvatarCalibrationsConfig().SaveCalibrations)
			{
				return true;
			}
			if (__instance.field_Private_String_0 == null)
			{
				ConsoleUtils.Info("Calibration Caching", "No avatar found in VRCTrackingSteam", ConsoleColor.Gray, "IsAvatarCalibratedPatch", 89);
				return true;
			}
			if (Configuration.GetAvatarCalibrationsConfig().SavedCalibrations.ContainsKey(__0))
			{
				CachedCalibration cachedCalibration = Configuration.GetAvatarCalibrationsConfig().SavedCalibrations[__0];
				__instance.field_Public_Transform_10.localPosition = cachedCalibration.LeftFootPosition;
				__instance.field_Public_Transform_10.localRotation = cachedCalibration.LeftFootRotation;
				__instance.field_Public_Transform_11.localPosition = cachedCalibration.RightFootPosition;
				__instance.field_Public_Transform_11.localRotation = cachedCalibration.RightFootRotation;
				__instance.field_Public_Transform_12.localPosition = cachedCalibration.HipPosition;
				__instance.field_Public_Transform_12.localRotation = cachedCalibration.HipRotation;
				__result = true;
				ConsoleUtils.Info("Calibration Caching", "Found previous saved calibration", ConsoleColor.Gray, "IsAvatarCalibratedPatch", 111);
				return false;
			}
			ConsoleUtils.Info("Calibration Caching", "No previous saved calibration found", ConsoleColor.Gray, "IsAvatarCalibratedPatch", 118);
			return true;
		}
	}
}
