using System;
using MunchenClient.Utils;

namespace MunchenClient.Security
{
	internal class AntiBreakpoint
	{
		internal unsafe static bool InitializeAntiBreakpoint()
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			for (int i = 0; i < GeneralUtils.GetMainProcess().Modules.Count; i++)
			{
				string text = GeneralUtils.GetMainProcess().Modules[i].FileName.Substring(GeneralUtils.GetMainProcess().Modules[i].FileName.LastIndexOf('\\') + 1);
				if (text == "KERNEL32.DLL")
				{
					intPtr = GeneralUtils.GetMainProcess().Modules[i].BaseAddress;
				}
				else if (text == "ntdll.dll")
				{
					intPtr2 = GeneralUtils.GetMainProcess().Modules[i].BaseAddress;
				}
			}
			if (intPtr == IntPtr.Zero || intPtr2 == IntPtr.Zero)
			{
				return false;
			}
			IntPtr moduleHandle = UnmanagedUtils.GetModuleHandle(null);
			IntPtr procAddress = UnmanagedUtils.GetProcAddress(intPtr, "ExitProcess");
			IntPtr procAddress2 = UnmanagedUtils.GetProcAddress(intPtr2, "DbgUiRemoteBreakin");
			IntPtr procAddress3 = UnmanagedUtils.GetProcAddress(intPtr2, "DbgBreakPoint");
			if (moduleHandle == IntPtr.Zero || procAddress == IntPtr.Zero || procAddress2 == IntPtr.Zero || procAddress3 == IntPtr.Zero)
			{
				return false;
			}
			try
			{
				UnmanagedUtils.VirtualProtect(procAddress2, (UIntPtr)6uL, UnmanagedUtils.Protection.PAGE_EXECUTE_READWRITE, out var lpflOldProtect);
				UnmanagedUtils.ChangeInstructionAtAddress(procAddress2, 104);
				IntPtr* ptr = (IntPtr*)(void*)IntPtr.Add(procAddress2, 1);
				*ptr = procAddress;
				UnmanagedUtils.ChangeInstructionAtAddress(IntPtr.Add(procAddress2, 5), 195);
				UnmanagedUtils.VirtualProtect(procAddress2, (UIntPtr)6uL, lpflOldProtect, out lpflOldProtect);
			}
			catch (Exception)
			{
				return false;
			}
			try
			{
				UnmanagedUtils.VirtualProtect(procAddress3, (UIntPtr)6uL, UnmanagedUtils.Protection.PAGE_EXECUTE_READWRITE, out var lpflOldProtect2);
				UnmanagedUtils.ChangeInstructionAtAddress(procAddress3, 104);
				IntPtr* ptr2 = (IntPtr*)(void*)IntPtr.Add(procAddress3, 1);
				*ptr2 = procAddress;
				UnmanagedUtils.ChangeInstructionAtAddress(IntPtr.Add(procAddress3, 5), 195);
				UnmanagedUtils.VirtualProtect(procAddress3, (UIntPtr)6uL, lpflOldProtect2, out lpflOldProtect2);
			}
			catch (Exception)
			{
				return false;
			}
			try
			{
				IntPtr* ptr3 = (IntPtr*)(void*)moduleHandle;
				ptr3 = null;
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}
}
