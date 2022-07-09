using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using AntiShaderCrashAPI;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Core.Compatibility;
using MunchenClient.Utils;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class AntiShaderCrashHandler : ModuleComponent
	{
		private bool hasBeenInitialized = false;

		protected override string moduleName => "Anti Shader Crash Handler";

		internal override void OnApplicationStart()
		{
			int loadPlugin = CompatibilityLayer.GetUnityPlayerOffsets().loadPlugin;
			if (loadPlugin == 0)
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, "Shader AntiCrash is limited due to engine incompatibility", ConsoleColor.Gray, "OnApplicationStart", 25);
				return;
			}
			string path = MelonUtils.GetGameDataDirectory() + "/Plugins/x86_64/MünchenClient.AntiShaderCrashModule.dll";
			try
			{
				using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MünchenClient.AntiShaderCrashModule.dll");
				using FileStream destination = File.Open(path, FileMode.Create, FileAccess.Write);
				stream.CopyTo(destination);
			}
			catch (IOException)
			{
			}
			IntPtr ptr = IntPtr.Add(CompatibilityLayer.GetUnityPlayerBaseAddress(), loadPlugin);
			IntPtr intPtr = Marshal.StringToHGlobalAnsi("MünchenClient.AntiShaderCrashModule");
			CompatibilityLayer.FindAndLoadUnityPlugin delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer<CompatibilityLayer.FindAndLoadUnityPlugin>(ptr);
			delegateForFunctionPointer(intPtr, out var loadedModule);
			if (loadedModule == IntPtr.Zero)
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, "Failed to find engine offset - Anti Shader Crash won't work", ConsoleColor.Gray, "OnApplicationStart", 48);
				return;
			}
			AntiShaderCrashProxy.Init(loadedModule);
			Marshal.FreeHGlobal(intPtr);
			hasBeenInitialized = true;
			ApplyAntiShaderConfig(state: false);
		}

		internal override void OnRoomLeft()
		{
			if (hasBeenInitialized && Configuration.GetAntiCrashConfig().AntiShaderCrash)
			{
				ApplyAntiShaderConfig(state: false);
			}
		}

		internal override void OnRoomJoined()
		{
			if (hasBeenInitialized && Configuration.GetAntiCrashConfig().AntiShaderCrash)
			{
				ApplyAntiShaderConfig(state: true);
			}
		}

		internal static void ApplyAntiShaderConfig(bool state)
		{
			AntiShaderCrashProxy.SetLoopLimit(state ? Configuration.GetAntiCrashConfig().MaxShaderLoopLimit : 512);
			AntiShaderCrashProxy.SetGeometryLimit(state ? Configuration.GetAntiCrashConfig().MaxShaderGeometryLimit : 100);
			AntiShaderCrashProxy.SetMaxTesselationPower(state ? Configuration.GetAntiCrashConfig().MaxShaderTesselationPower : 10f);
			AntiShaderCrashProxy.SetFilteringState(state, state, state);
		}
	}
}
