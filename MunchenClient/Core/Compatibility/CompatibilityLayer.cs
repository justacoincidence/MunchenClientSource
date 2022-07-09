using System;
using System.Runtime.InteropServices;
using MelonLoader;
using MunchenClient.Utils;

namespace MunchenClient.Core.Compatibility
{
	internal class CompatibilityLayer
	{
		[UnmanagedFunctionPointer(CallingConvention.FastCall)]
		internal delegate void FindAndLoadUnityPlugin(IntPtr name, out IntPtr loadedModule);

		internal const int targetGameVersion = 1194;

		internal const int targetGameVersionAlt = 1194;

		private static bool isRunningOculus = true;

		private static bool emmVRCDetected = false;

		private static bool iktweaksDetected = false;

		private static bool fbtSaverDetected = false;

		private static bool notoriousDetected = false;

		private static bool karmaDetected = false;

		internal static void CheckCompatiblity()
		{
			ConsoleUtils.Info("CompatibilityLayer", "Checking Compatibility", ConsoleColor.Gray, "CheckCompatiblity", 25);
			try
			{
				EngineCompatibilityChecker.PrepareUnityModules();
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("CompatibilityLayer", "Unity Module", e, "CheckCompatiblity", 33);
			}
			try
			{
				OculusCompatibilityChecker.CheckOculusCompatibility();
				isRunningOculus = false;
				ConsoleUtils.Info("CompatibilityLayer", "VRChat Version: Steam", ConsoleColor.Gray, "CheckCompatiblity", 43);
			}
			catch (Exception)
			{
				isRunningOculus = true;
				ConsoleUtils.Info("CompatibilityLayer", "VRChat Version: Oculus", ConsoleColor.Gray, "CheckCompatiblity", 49);
			}
			CheckPluginCompatibility();
			CheckModsCompatibility();
		}

		private static void CheckPluginCompatibility()
		{
			for (int i = 0; i < MelonHandler.Plugins.Count; i++)
			{
				string text = MelonHandler.Plugins[i].Info.Name.ToLower().Trim();
				if (string.IsNullOrEmpty(text) || text.StartsWith("notorious"))
				{
					notoriousDetected = true;
					ConsoleUtils.Info("CompatibilityLayer", "Notorious detected", ConsoleColor.Gray, "CheckPluginCompatibility", 67);
				}
			}
		}

		private static void CheckModsCompatibility()
		{
			for (int i = 0; i < MelonHandler.Mods.Count; i++)
			{
				string text = MelonHandler.Mods[i].Info.Name.ToLower().Trim();
				if (text.StartsWith("emm"))
				{
					emmVRCDetected = true;
					ConsoleUtils.Info("CompatibilityLayer", "emmVRC detected", ConsoleColor.Gray, "CheckModsCompatibility", 83);
				}
				else if (text.StartsWith("iktweaks"))
				{
					iktweaksDetected = true;
					ConsoleUtils.Info("CompatibilityLayer", "IKTweaks detected", ConsoleColor.Gray, "CheckModsCompatibility", 89);
				}
				else if (text.StartsWith("fbt") && text.Contains("saver"))
				{
					fbtSaverDetected = true;
					ConsoleUtils.Info("CompatibilityLayer", "FBTSaver detected", ConsoleColor.Gray, "CheckModsCompatibility", 95);
				}
				else if (text.StartsWith("karma"))
				{
					karmaDetected = true;
					ConsoleUtils.Info("CompatibilityLayer", "Karma detected", ConsoleColor.Gray, "CheckModsCompatibility", 101);
				}
				else if (string.IsNullOrEmpty(text) || text.StartsWith("notorious"))
				{
					notoriousDetected = true;
					ConsoleUtils.Info("CompatibilityLayer", "Notorious detected", ConsoleColor.Gray, "CheckModsCompatibility", 107);
				}
			}
		}

		internal static bool IsEmmInstalled()
		{
			return emmVRCDetected;
		}

		internal static bool IsIKTweaksInstalled()
		{
			return iktweaksDetected;
		}

		internal static bool IsFBTSaverInstalled()
		{
			return fbtSaverDetected;
		}

		internal static bool IsNotoriousInstalled()
		{
			return notoriousDetected;
		}

		internal static bool IsKarmaInstalled()
		{
			return karmaDetected;
		}

		internal static bool IsRunningOculus()
		{
			return isRunningOculus;
		}

		internal static int GetCurrentGameVersion()
		{
			return MonoBehaviourPublicStInStBoSiGaStSiBoInUnique.prop_MonoBehaviourPublicStInStBoSiGaStSiBoInUnique_0.field_Public_Int32_0;
		}

		internal static IntPtr GetUnityPlayerBaseAddress()
		{
			if (EngineCompatibilityChecker.unityPlayerModule == null)
			{
				return IntPtr.Zero;
			}
			return EngineCompatibilityChecker.unityPlayerModule.BaseAddress;
		}

		internal static UnityEngineOffsets GetUnityPlayerOffsets()
		{
			if (!EngineCompatibilityChecker.engineOffsets.ContainsKey(EngineCompatibilityChecker.unityPlayerHash))
			{
				return EngineCompatibilityChecker.engineOffsets["Unknown"];
			}
			return EngineCompatibilityChecker.engineOffsets[EngineCompatibilityChecker.unityPlayerHash];
		}
	}
}
