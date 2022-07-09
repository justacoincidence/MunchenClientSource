using System;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using MunchenClient.Core.Compatibility;
using UnhollowerBaseLib;

namespace MunchenClient.Utils
{
	internal class UnmanagedUtils
	{
		internal enum Protection
		{
			PAGE_NOACCESS = 1,
			PAGE_READONLY = 2,
			PAGE_READWRITE = 4,
			PAGE_WRITECOPY = 8,
			PAGE_EXECUTE = 0x10,
			PAGE_EXECUTE_READ = 0x20,
			PAGE_EXECUTE_READWRITE = 0x40,
			PAGE_EXECUTE_WRITECOPY = 0x80,
			PAGE_GUARD = 0x100,
			PAGE_NOCACHE = 0x200,
			PAGE_WRITECOMBINE = 0x400
		}

		internal struct NativeString
		{
			internal IntPtr Data;

			internal long Capacity;

			internal long Unknown;

			internal long Length;

			internal int Unknown2;
		}

		[StructLayout(LayoutKind.Explicit, Size = 136)]
		internal struct NodeContainer
		{
			[FieldOffset(112)]
			internal unsafe NodeContainer** Subs;

			[FieldOffset(128)]
			internal long DirectSubCount;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, Protection flNewProtect, out Protection lpflOldProtect);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern bool SetProcessAffinityMask(IntPtr hProcess, IntPtr dwProcessAffinityMask);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetCurrentThread();

		[DllImport("kernel32.dll")]
		internal static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

		[DllImport("user32.dll")]
		internal static extern byte MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll")]
		internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetWindow(IntPtr hWnd, int nCmd);

		internal static void VolumeUp()
		{
			keybd_event(175, MapVirtualKey(175u, 0u), 1u, 0u);
			keybd_event(175, MapVirtualKey(175u, 0u), 3u, 0u);
		}

		internal static void VolumeDown()
		{
			keybd_event(174, MapVirtualKey(174u, 0u), 1u, 0u);
			keybd_event(174, MapVirtualKey(174u, 0u), 3u, 0u);
		}

		internal static void MuteOrUnmute()
		{
			keybd_event(173, MapVirtualKey(173u, 0u), 1u, 0u);
			keybd_event(173, MapVirtualKey(173u, 0u), 3u, 0u);
		}

		internal static void Stop()
		{
			keybd_event(178, MapVirtualKey(178u, 0u), 1u, 0u);
			keybd_event(178, MapVirtualKey(178u, 0u), 3u, 0u);
		}

		internal static void Next()
		{
			keybd_event(176, MapVirtualKey(176u, 0u), 1u, 0u);
			keybd_event(176, MapVirtualKey(176u, 0u), 3u, 0u);
		}

		internal static void Previous()
		{
			keybd_event(177, MapVirtualKey(177u, 0u), 1u, 0u);
			keybd_event(177, MapVirtualKey(177u, 0u), 3u, 0u);
		}

		internal static void PlayOrPause()
		{
			keybd_event(179, MapVirtualKey(179u, 0u), 1u, 0u);
			keybd_event(179, MapVirtualKey(179u, 0u), 3u, 0u);
		}

		internal unsafe static void ChangeInstructionAtAddress(IntPtr address, byte instruction)
		{
			VirtualProtect(address, (UIntPtr)4uL, Protection.PAGE_EXECUTE_READWRITE, out var lpflOldProtect);
			byte* ptr = (byte*)address.ToPointer();
			*ptr = instruction;
			VirtualProtect(address, (UIntPtr)4uL, lpflOldProtect, out var _);
		}

		internal unsafe static void PatchEngineOffset<T>(int offset, T patchDelegate, out T delegateField) where T : MulticastDelegate
		{
			delegateField = null;
			if (offset == 0)
			{
				ConsoleUtils.Info("Patcher", "Offset for " + patchDelegate.Method.Name + " not found - abort patching", ConsoleColor.Gray, "PatchEngineOffset", 138);
				return;
			}
			IntPtr ptr = CompatibilityLayer.GetUnityPlayerBaseAddress() + offset;
			MainUtils.antiGCList.Add(patchDelegate);
			MelonUtils.NativeHookAttach((IntPtr)(&ptr), Marshal.GetFunctionPointerForDelegate(patchDelegate));
			delegateField = Marshal.GetDelegateForFunctionPointer<T>(ptr);
		}

		internal unsafe static void PatchICall<T>(string name, out T original, MethodInfo target) where T : MulticastDelegate
		{
			IntPtr intPtr = IL2CPP.il2cpp_resolve_icall(name);
			if (intPtr == IntPtr.Zero)
			{
				ConsoleUtils.Info("ICallPatcher", "ICall " + name + " not found - abort patching", ConsoleColor.Gray, "PatchICall", 156);
				original = null;
				return;
			}
			IntPtr functionPointer = target.MethodHandle.GetFunctionPointer();
			MelonUtils.NativeHookAttach((IntPtr)(&intPtr), functionPointer);
			MainUtils.antiGCList.Add(intPtr);
			MainUtils.antiGCList.Add(functionPointer);
			MainUtils.antiGCList.Add(target);
			original = Marshal.GetDelegateForFunctionPointer<T>(intPtr);
			MainUtils.antiGCList.Add(original);
			ConsoleUtils.Info("ICallPatcher", "ICall " + name + " patched", ConsoleColor.Gray, "PatchICall", 172);
		}

		internal static void UseFunctionInEngine<T>(int offset, out T delegateField) where T : MulticastDelegate
		{
			delegateField = null;
			if (offset != 0)
			{
				IntPtr ptr = CompatibilityLayer.GetUnityPlayerBaseAddress() + offset;
				delegateField = Marshal.GetDelegateForFunctionPointer<T>(ptr);
			}
		}
	}
}
