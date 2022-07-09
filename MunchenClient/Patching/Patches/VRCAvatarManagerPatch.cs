using System;
using System.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem;
using MunchenClient.Utils;
using UnityEngine;
using VRC.Core;

namespace MunchenClient.Patching.Patches
{
	internal class VRCAvatarManagerPatch : PatchComponent
	{
		protected override string patchName => "VRCAvatarManagerPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VRCAvatarManagerPatch));
			PatchMethod(typeof(VRCAvatarManager).GetMethod("Method_Private_Boolean_ApiAvatar_GameObject_0"), GetLocalPatch("OnAvatarInitializedPatch"), null);
		}

		private static bool OnAvatarInitializedPatch(VRCAvatarManager __instance, ApiAvatar __0, ref GameObject __1)
		{
			if (__1 == null)
			{
				return true;
			}
			PerformanceProfiler.StartProfiling("OnAvatarInstantiated");
			if (Configuration.GetGeneralConfig().AntiAvatarIntroMusic)
			{
				List<AudioSource> list = MiscUtils.FindAllComponentsInGameObject<AudioSource>(__1);
				foreach (AudioSource item in list)
				{
					if ((item.isPlaying || item.playOnAwake) && item.enabled && item.gameObject.activeInHierarchy)
					{
						item.playOnAwake = false;
						item.Stop();
					}
				}
				List<ParticleSystem> list2 = MiscUtils.FindAllComponentsInGameObject<ParticleSystem>(__1);
				foreach (ParticleSystem item2 in list2)
				{
					if ((item2.isPlaying || item2.playOnAwake) && item2.gameObject.activeInHierarchy)
					{
						item2.playOnAwake = false;
						item2.Stop();
					}
				}
			}
			try
			{
				ModuleManager.OnAvatarLoaded(__instance.field_Private_VRCPlayer_0.prop_Player_0.prop_APIUser_0.id, __instance.field_Private_VRCPlayer_0.prop_Player_0.prop_APIUser_0.displayName, ref __1);
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("AvatarProcessor", "Module Processor", e, "OnAvatarInitializedPatch", 75);
			}
			Configuration.GetAntiCrashConfig().BlacklistedAvatars.Remove(__0.id);
			Configuration.SaveAntiCrashConfig();
			PerformanceProfiler.EndProfiling("OnAvatarInstantiated");
			if (GeneralUtils.IsBetaClient() && PerformanceProfiler.GetProfiling("OnAvatarInstantiated") > 100f)
			{
				ConsoleUtils.Info("AvatarProcessor", LanguageManager.GetUsedLanguage().AntiCrashProcessedAvatar.Replace("{processingtime}", string.Format("{0:F2}", PerformanceProfiler.GetProfiling("OnAvatarInstantiated"))), ConsoleColor.Gray, "OnAvatarInitializedPatch", 86);
			}
			return true;
		}
	}
}
