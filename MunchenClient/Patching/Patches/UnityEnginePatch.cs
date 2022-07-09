using System;
using System.Linq;
using Il2CppSystem;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using SharpNeatLib.Maths;
using UnhollowerBaseLib;
using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal class UnityEnginePatch : PatchComponent
	{
		private static Il2CppSystem.Object spooferHWID;

		private static Il2CppSystem.Object spooferHWIDOriginal;

		private static Il2CppSystem.Object spooferDeviceModel;

		private static Il2CppSystem.Object spooferDeviceModelOriginal;

		private static Il2CppSystem.Object spooferDeviceName;

		private static Il2CppSystem.Object spooferDeviceNameOriginal;

		private static Il2CppSystem.Object spooferGraphicsDeviceName;

		private static Il2CppSystem.Object spooferGraphicsDeviceNameOriginal;

		private static Il2CppSystem.Object spooferGraphicsDeviceVersion;

		private static Il2CppSystem.Object spooferGraphicsDeviceVersionOriginal;

		private static Il2CppSystem.Object spooferGraphicsDeviceID;

		private static Il2CppSystem.Object spooferGraphicsDeviceIDOriginal;

		private static Il2CppSystem.Object spooferProcessorType;

		private static Il2CppSystem.Object spooferProcessorTypeOriginal;

		private static Il2CppSystem.Object spooferOperatingSystem;

		private static Il2CppSystem.Object spooferOperatingSystemOriginal;

		protected override string patchName => "UnityEnginePatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(UnityEnginePatch));
			FastRandom fastRandom = (ApplicationBotHandler.IsBot() ? ApplicationBotHandler.botFastRandom : GeneralUtils.fastRandom);
			PatchMethod(typeof(Time).GetProperty("smoothDeltaTime").GetGetMethod(), GetLocalPatch("GetSmoothDeltaTimePatch"), null);
			PatchHWIDFunction(fastRandom);
			PatchDeviceModelFunction(fastRandom);
			PatchDeviceNameFunction(fastRandom);
			PatchProcessorTypeFunction(fastRandom);
			PatchOperatingSystemFunction(fastRandom);
			if (!ApplicationBotHandler.IsBot())
			{
				PatchGraphicsDeviceNameFunction(fastRandom);
				PatchGraphicsDeviceVersionFunction(fastRandom);
				PatchGraphicsDeviceIDFunction(fastRandom);
			}
			else
			{
				PatchMethod(typeof(SystemInfo).GetProperty("systemMemorySize").GetGetMethod(), GetLocalPatch("GetSystemMemorySizePatch"), null);
			}
		}

		private static bool GetSmoothDeltaTimePatch(ref float __result)
		{
			if (!Configuration.GetGeneralConfig().SpooferFPS)
			{
				return true;
			}
			float num2;
			if (Configuration.GetGeneralConfig().SpooferRealisticMode)
			{
				Configuration.GetGeneralConfig().SpooferFPS = false;
				int num = (int)(1f / Time.smoothDeltaTime) / 3;
				Configuration.GetGeneralConfig().SpooferFPS = true;
				num2 = ((Configuration.GetGeneralConfig().SpooferFPSCustom != -1) ? (1f / (float)(Configuration.GetGeneralConfig().SpooferFPSCustom + num)) : 0.01f);
			}
			else
			{
				num2 = ((Configuration.GetGeneralConfig().SpooferFPSCustom != -1) ? (1f / (float)Configuration.GetGeneralConfig().SpooferFPSCustom) : 0.01f);
			}
			__result = num2;
			return false;
		}

		internal static void GenerateHardwareIdentifier(FastRandom fastRandom)
		{
			string deviceUniqueIdentifier = SystemInfo.GetDeviceUniqueIdentifier();
			bool flag = Configuration.GetGeneralConfig().SpooferHWID;
			Configuration.GetGeneralConfig().SpooferHWID = false;
			byte[] array = new byte[SystemInfo.deviceUniqueIdentifier.Length / 2];
			fastRandom.NextBytes(array);
			Configuration.GetGeneralConfig().SpooferGeneratedHWID = string.Join(string.Empty, array.Select((byte it) => it.ToString("x2")));
			Configuration.GetGeneralConfig().SpooferHWID = flag;
			Configuration.SaveGeneralConfig();
			spooferHWID = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedHWID));
			ConsoleUtils.Info("Spoofer", "HWID for spoofing succesfully Generated (" + Configuration.GetGeneralConfig().SpooferGeneratedHWID + ")", System.ConsoleColor.Green, "GenerateHardwareIdentifier", 109);
		}

		internal unsafe void PatchHWIDFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceUniqueIdentifier");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("HWIDSpoofer", "Patching 'HWID' failed", System.ConsoleColor.Red, "PatchHWIDFunction", 118);
				return;
			}
			string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			if (string.IsNullOrEmpty(Configuration.GetGeneralConfig().SpooferGeneratedHWID) || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				GenerateHardwareIdentifier(fastRandom);
			}
			spooferHWIDOriginal = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(deviceUniqueIdentifier));
			spooferHWID = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedHWID));
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetHardwareIdentifierPatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferHWID)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'HWID' Succeeded (" + SystemInfo.deviceUniqueIdentifier + ")", System.ConsoleColor.Green, "PatchHWIDFunction", 136);
			}
		}

		internal unsafe void PatchDeviceModelFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceModel");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'DeviceModel' failed", System.ConsoleColor.Red, "PatchDeviceModelFunction", 146);
				return;
			}
			string deviceModel = SystemInfo.deviceModel;
			if (string.IsNullOrEmpty(Configuration.GetGeneralConfig().SpooferGeneratedDeviceModel) || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				Configuration.GetGeneralConfig().SpooferGeneratedDeviceModel = MiscUtils.motherboardList[fastRandom.Next(0, MiscUtils.motherboardList.Count)];
			}
			spooferDeviceModelOriginal = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(deviceModel));
			spooferDeviceModel = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedDeviceModel));
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetDeviceModelPatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferPeripheral)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'DeviceModel' Succeeded (" + SystemInfo.deviceModel + ")", System.ConsoleColor.Green, "PatchDeviceModelFunction", 164);
			}
		}

		internal unsafe void PatchDeviceNameFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceName");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'DeviceName' failed", System.ConsoleColor.Red, "PatchDeviceNameFunction", 174);
				return;
			}
			string deviceName = SystemInfo.deviceName;
			if (string.IsNullOrEmpty(Configuration.GetGeneralConfig().SpooferGeneratedDeviceName) || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				Configuration.GetGeneralConfig().SpooferGeneratedDeviceName = "DESKTOP-" + GeneralUtils.RandomString(fastRandom, 7).ToUpper();
			}
			spooferDeviceNameOriginal = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(deviceName));
			spooferDeviceName = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedDeviceName));
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetDeviceNamePatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferPeripheral)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'DeviceName' Succeeded (" + SystemInfo.deviceName + ")", System.ConsoleColor.Green, "PatchDeviceNameFunction", 192);
			}
		}

		internal unsafe void PatchGraphicsDeviceNameFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetGraphicsDeviceName");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'GraphicsDeviceName' failed", System.ConsoleColor.Red, "PatchGraphicsDeviceNameFunction", 202);
				return;
			}
			string graphicsDeviceName = SystemInfo.graphicsDeviceName;
			if (string.IsNullOrEmpty(Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceName) || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceName = MiscUtils.graphicsCardList[fastRandom.Next(0, MiscUtils.graphicsCardList.Count)];
			}
			spooferGraphicsDeviceNameOriginal = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(graphicsDeviceName));
			spooferGraphicsDeviceName = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceName));
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetGraphicsDeviceNamePatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferPeripheral)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'GraphicsDeviceName' Succeeded (" + SystemInfo.graphicsDeviceName + ")", System.ConsoleColor.Green, "PatchGraphicsDeviceNameFunction", 220);
			}
		}

		internal unsafe void PatchGraphicsDeviceVersionFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetGraphicsDeviceVersion");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'GraphicsDeviceVersion' failed", System.ConsoleColor.Red, "PatchGraphicsDeviceVersionFunction", 230);
				return;
			}
			string graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
			if (string.IsNullOrEmpty(Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceVersion) || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceVersion = MiscUtils.graphicsCardVersionList[fastRandom.Next(0, MiscUtils.graphicsCardVersionList.Count)];
			}
			spooferGraphicsDeviceVersionOriginal = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(graphicsDeviceVersion));
			spooferGraphicsDeviceVersion = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceVersion));
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetGraphicsDeviceVersionPatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferPeripheral)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'GraphicsDeviceVersion' Succeeded (" + SystemInfo.graphicsDeviceVersion + ")", System.ConsoleColor.Green, "PatchGraphicsDeviceVersionFunction", 248);
			}
		}

		internal unsafe void PatchGraphicsDeviceIDFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetGraphicsDeviceID");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'GraphicsDeviceID' failed", System.ConsoleColor.Red, "PatchGraphicsDeviceIDFunction", 258);
				return;
			}
			int graphicsDeviceID = SystemInfo.graphicsDeviceID;
			if (Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceIdentifier < 1000 || Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceIdentifier > 9999 || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceIdentifier = fastRandom.Next(1000, 10000);
			}
			Il2CppSystem.Int32 @int = default(Il2CppSystem.Int32);
			@int.m_value = graphicsDeviceID;
			spooferGraphicsDeviceIDOriginal = @int.BoxIl2CppObject();
			@int = default(Il2CppSystem.Int32);
			@int.m_value = Configuration.GetGeneralConfig().SpooferGeneratedGraphicsDeviceIdentifier;
			spooferGraphicsDeviceID = @int.BoxIl2CppObject();
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetGraphicsDeviceIDPatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferPeripheral)
			{
				ConsoleUtils.Info("Spoofer", $"Patching 'GraphicsDeviceID' Succeeded ({SystemInfo.graphicsDeviceID})", System.ConsoleColor.Green, "PatchGraphicsDeviceIDFunction", 276);
			}
		}

		internal unsafe void PatchProcessorTypeFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetProcessorType");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'ProcessorType' failed", System.ConsoleColor.Red, "PatchProcessorTypeFunction", 286);
				return;
			}
			string processorType = SystemInfo.processorType;
			if (string.IsNullOrEmpty(Configuration.GetGeneralConfig().SpooferGeneratedProcessorType) || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				Configuration.GetGeneralConfig().SpooferGeneratedProcessorType = MiscUtils.cpuList[fastRandom.Next(0, MiscUtils.cpuList.Count)];
			}
			spooferProcessorTypeOriginal = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(processorType.ToString()));
			spooferProcessorType = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedProcessorType.ToString()));
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetProcessorTypePatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferPeripheral)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'ProcessorType' Succeeded (" + SystemInfo.processorType + ")", System.ConsoleColor.Green, "PatchProcessorTypeFunction", 304);
			}
		}

		internal unsafe void PatchOperatingSystemFunction(FastRandom fastRandom)
		{
			System.IntPtr intPtr = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetOperatingSystem");
			if (intPtr == System.IntPtr.Zero)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'OperatingSystem' failed", System.ConsoleColor.Red, "PatchOperatingSystemFunction", 314);
				return;
			}
			string operatingSystem = SystemInfo.operatingSystem;
			if (string.IsNullOrEmpty(Configuration.GetGeneralConfig().SpooferGeneratedOperatingSystem) || Configuration.GetGeneralConfig().SpooferAutoRegenerate)
			{
				Configuration.GetGeneralConfig().SpooferGeneratedOperatingSystem = MiscUtils.operatingSystemList[fastRandom.Next(0, MiscUtils.operatingSystemList.Count)];
			}
			spooferOperatingSystemOriginal = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(operatingSystem.ToString()));
			spooferOperatingSystem = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Configuration.GetGeneralConfig().SpooferGeneratedOperatingSystem.ToString()));
			MelonUtils.NativeHookAttach((System.IntPtr)(&intPtr), GetLocalPatch("GetOpertingSystemPatch").method.MethodHandle.GetFunctionPointer());
			if (GeneralUtils.IsBetaClient() && Configuration.GetGeneralConfig().SpooferPeripheral)
			{
				ConsoleUtils.Info("Spoofer", "Patching 'OperatingSystem' Succeeded (" + SystemInfo.operatingSystem + ")", System.ConsoleColor.Green, "PatchOperatingSystemFunction", 332);
			}
		}

		private static System.IntPtr GetHardwareIdentifierPatch()
		{
			return Configuration.GetGeneralConfig().SpooferHWID ? spooferHWID.Pointer : spooferHWIDOriginal.Pointer;
		}

		private static System.IntPtr GetDeviceModelPatch()
		{
			return Configuration.GetGeneralConfig().SpooferPeripheral ? spooferDeviceModel.Pointer : spooferDeviceModelOriginal.Pointer;
		}

		private static System.IntPtr GetDeviceNamePatch()
		{
			return Configuration.GetGeneralConfig().SpooferPeripheral ? spooferDeviceName.Pointer : spooferDeviceNameOriginal.Pointer;
		}

		private static System.IntPtr GetGraphicsDeviceNamePatch()
		{
			return Configuration.GetGeneralConfig().SpooferPeripheral ? spooferGraphicsDeviceName.Pointer : spooferGraphicsDeviceNameOriginal.Pointer;
		}

		private static System.IntPtr GetGraphicsDeviceVersionPatch()
		{
			return Configuration.GetGeneralConfig().SpooferPeripheral ? spooferGraphicsDeviceVersion.Pointer : spooferGraphicsDeviceVersionOriginal.Pointer;
		}

		private static System.IntPtr GetGraphicsDeviceIDPatch()
		{
			return Configuration.GetGeneralConfig().SpooferPeripheral ? spooferGraphicsDeviceID.Pointer : spooferGraphicsDeviceIDOriginal.Pointer;
		}

		private static System.IntPtr GetProcessorTypePatch()
		{
			return Configuration.GetGeneralConfig().SpooferPeripheral ? spooferProcessorType.Pointer : spooferProcessorTypeOriginal.Pointer;
		}

		private static System.IntPtr GetOpertingSystemPatch()
		{
			return Configuration.GetGeneralConfig().SpooferPeripheral ? spooferOperatingSystem.Pointer : spooferOperatingSystemOriginal.Pointer;
		}

		private static bool GetSystemMemorySizePatch(ref int __result)
		{
			__result = 512;
			return false;
		}
	}
}
