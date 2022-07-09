using System;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Utils;

namespace MunchenClient.Patching.Patches
{
	internal class SteamworksPatch : PatchComponent
	{
		private static bool steamPatched;

		protected override string patchName => "SteamworksPatch";

		internal unsafe override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(SteamworksPatch));
			if (!Configuration.GetGeneralConfig().SpooferSteamID || steamPatched)
			{
				return;
			}
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = UnmanagedUtils.LoadLibrary(MelonUtils.GetGameDataDirectory() + "\\Plugins\\x86_64\\steam_api64.dll");
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Spoofer", "Steam - Loader", e, "OnInitializeOnStart", 33);
			}
			if (intPtr == IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "SteamAPI library returned no valid address - Steam Spoofer won't work", ConsoleColor.Gray, "OnInitializeOnStart", 38);
				return;
			}
			try
			{
				IntPtr procAddress = UnmanagedUtils.GetProcAddress(intPtr, "SteamAPI_Init");
				IntPtr procAddress2 = UnmanagedUtils.GetProcAddress(intPtr, "SteamAPI_RestartAppIfNecessary");
				IntPtr procAddress3 = UnmanagedUtils.GetProcAddress(intPtr, "SteamAPI_GetHSteamUser");
				IntPtr procAddress4 = UnmanagedUtils.GetProcAddress(intPtr, "SteamAPI_RegisterCallback");
				IntPtr procAddress5 = UnmanagedUtils.GetProcAddress(intPtr, "SteamAPI_UnregisterCallback");
				IntPtr procAddress6 = UnmanagedUtils.GetProcAddress(intPtr, "SteamAPI_RunCallbacks");
				IntPtr procAddress7 = UnmanagedUtils.GetProcAddress(intPtr, "SteamAPI_Shutdown");
				MelonUtils.NativeHookAttach((IntPtr)(&procAddress), GetLocalPatch("ShouldCallOriginalSteamFunction").method.MethodHandle.GetFunctionPointer());
				MelonUtils.NativeHookAttach((IntPtr)(&procAddress2), GetLocalPatch("ShouldCallOriginalSteamFunction").method.MethodHandle.GetFunctionPointer());
				MelonUtils.NativeHookAttach((IntPtr)(&procAddress3), GetLocalPatch("ShouldCallOriginalSteamFunction").method.MethodHandle.GetFunctionPointer());
				MelonUtils.NativeHookAttach((IntPtr)(&procAddress4), GetLocalPatch("ShouldCallOriginalSteamFunction").method.MethodHandle.GetFunctionPointer());
				MelonUtils.NativeHookAttach((IntPtr)(&procAddress5), GetLocalPatch("ShouldCallOriginalSteamFunction").method.MethodHandle.GetFunctionPointer());
				MelonUtils.NativeHookAttach((IntPtr)(&procAddress6), GetLocalPatch("ShouldCallOriginalSteamFunction").method.MethodHandle.GetFunctionPointer());
				MelonUtils.NativeHookAttach((IntPtr)(&procAddress7), GetLocalPatch("ShouldCallOriginalSteamFunction").method.MethodHandle.GetFunctionPointer());
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Spoofer", "Steam - Patcher", e2, "OnInitializeOnStart", 63);
			}
			if (GeneralUtils.IsBetaClient())
			{
				ConsoleUtils.Info("Spoofer", "Patching Steam Succeeded", ConsoleColor.Gray, "OnInitializeOnStart", 69);
			}
			steamPatched = true;
		}

		private static bool ShouldCallOriginalSteamFunction()
		{
			return false;
		}

		internal static bool IsSteamPatched()
		{
			return steamPatched;
		}
	}
}
