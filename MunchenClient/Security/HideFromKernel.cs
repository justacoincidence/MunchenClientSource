using System;
using System.Runtime.InteropServices;
using MunchenClient.Utils;

namespace MunchenClient.Security
{
	internal class HideFromKernel
	{
		[DllImport("ntdll.dll")]
		private static extern NtStatus NtSetInformationThread(IntPtr ThreadHandle, ThreadInformationClass ThreadInformationClass, IntPtr ThreadInformation, int ThreadInformationLength);

		[DllImport("kernel32.dll")]
		private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		[DllImport("kernel32.dll")]
		private static extern uint SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll")]
		private static extern int ResumeThread(IntPtr hThread);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		internal static bool HideProcessThreadFromKernel(out string threadStatus)
		{
			int currentThreadId = AppDomain.GetCurrentThreadId();
			IntPtr intPtr = OpenThread(ThreadAccess.SET_INFORMATION, bInheritHandle: false, (uint)currentThreadId);
			if (intPtr == IntPtr.Zero)
			{
				ConsoleUtils.Info("AntiLeak", $"Thread pointer is zero - ThreadID: {currentThreadId}", ConsoleColor.Gray, "HideProcessThreadFromKernel", 35);
				CloseHandle(intPtr);
				threadStatus = "Zero pointer";
				return false;
			}
			if (HideFromDebugger(intPtr))
			{
				ConsoleUtils.Info("AntiLeak", $"ThreadID: {currentThreadId} hidden from debugger", ConsoleColor.Gray, "HideProcessThreadFromKernel", 47);
				CloseHandle(intPtr);
				threadStatus = string.Empty;
				return true;
			}
			ConsoleUtils.Info("AntiLeak", $"ThreadID: {currentThreadId} failed hiding from debugger", ConsoleColor.Gray, "HideProcessThreadFromKernel", 58);
			CloseHandle(intPtr);
			threadStatus = "Failed";
			return false;
		}

		private static bool HideFromDebugger(IntPtr Handle)
		{
			if (NtSetInformationThread(Handle, ThreadInformationClass.ThreadHideFromDebugger, IntPtr.Zero, 0) == NtStatus.Success)
			{
				return true;
			}
			return false;
		}
	}
}
