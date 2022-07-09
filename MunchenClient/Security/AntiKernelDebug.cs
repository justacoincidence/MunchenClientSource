using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MunchenClient.Utils;

namespace MunchenClient.Security
{
	internal class AntiKernelDebug
	{
		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		private static extern NtStatus NtQueryInformationProcess([In] IntPtr ProcessHandle, [In] PROCESSINFOCLASS ProcessInformationClass, out IntPtr ProcessInformation, [In] int ProcessInformationLength, [Optional] out int ReturnLength);

		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		private static extern NtStatus NtClose([In] IntPtr Handle);

		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		private static extern NtStatus NtRemoveProcessDebug(IntPtr ProcessHandle, IntPtr DebugObjectHandle);

		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		private static extern NtStatus NtSetInformationDebugObject([In] IntPtr DebugObjectHandle, [In] DebugObjectInformationClass DebugObjectInformationClass, [In] IntPtr DebugObjectInformation, [In] int DebugObjectInformationLength, [Optional] out int ReturnLength);

		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		private static extern NtStatus NtQuerySystemInformation([In] SYSTEM_INFORMATION_CLASS SystemInformationClass, IntPtr SystemInformation, [In] int SystemInformationLength, [Optional] out int ReturnLength);

		public static bool PerformChecks(out string failedCheck)
		{
			if (CheckDebugPort())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (CheckDebugPort)", ConsoleColor.Gray, "PerformChecks", 33);
				failedCheck = "CheckDebugPort";
				return true;
			}
			if (CheckKernelDebugInformation())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (CheckKernelDebugInformation)", ConsoleColor.Gray, "PerformChecks", 43);
				failedCheck = "CheckKernelDebugInformation";
				return true;
			}
			if (DetachFromDebuggerProcess())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (DetachFromDebuggerProcess)", ConsoleColor.Gray, "PerformChecks", 53);
				failedCheck = "DetachFromDebuggerProcess";
				return true;
			}
			failedCheck = string.Empty;
			return false;
		}

		private static bool CheckDebugPort()
		{
			IntPtr ProcessInformation = new IntPtr(0);
			if (NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugPort, out ProcessInformation, Marshal.SizeOf(ProcessInformation), out var _) == NtStatus.Success && ProcessInformation != new IntPtr(0))
			{
				ConsoleUtils.Info("AntiLeak", $"DebugPort: {ProcessInformation}", ConsoleColor.Gray, "CheckDebugPort", 75);
				return true;
			}
			return false;
		}

		private unsafe static bool DetachFromDebuggerProcess()
		{
			uint structure = 0u;
			if (NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugObjectHandle, out var ProcessInformation, IntPtr.Size, out var ReturnLength) != 0)
			{
				return false;
			}
			if (NtSetInformationDebugObject(ProcessInformation, DebugObjectInformationClass.DebugObjectFlags, new IntPtr(&structure), Marshal.SizeOf(structure), out ReturnLength) != 0)
			{
				return false;
			}
			if (NtRemoveProcessDebug(Process.GetCurrentProcess().Handle, ProcessInformation) != 0)
			{
				return false;
			}
			if (NtClose(ProcessInformation) != 0)
			{
				return false;
			}
			return true;
		}

		private unsafe static bool CheckKernelDebugInformation()
		{
			SYSTEM_KERNEL_DEBUGGER_INFORMATION structure = default(SYSTEM_KERNEL_DEBUGGER_INFORMATION);
			if (NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemKernelDebuggerInformation, new IntPtr(&structure), Marshal.SizeOf(structure), out var _) == NtStatus.Success && structure.KernelDebuggerEnabled && !structure.KernelDebuggerNotPresent)
			{
				return true;
			}
			return false;
		}
	}
}
