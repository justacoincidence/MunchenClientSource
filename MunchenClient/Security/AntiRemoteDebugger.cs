using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MunchenClient.Utils;

namespace MunchenClient.Security
{
	internal class AntiRemoteDebugger
	{
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsDebuggerPresent();

		public static bool PerformChecks(out string failedCheck)
		{
			if (CheckDebuggerManagedPresent())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (CheckDebuggerManagedPresent)", ConsoleColor.Gray, "PerformChecks", 26);
				failedCheck = "CheckDebuggerManagedPresent";
				return true;
			}
			if (CheckDebuggerUnmanagedPresent())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (CheckDebuggerUnmanagedPresent)", ConsoleColor.Gray, "PerformChecks", 36);
				failedCheck = "CheckDebuggerUnmanagedPresent";
				return true;
			}
			if (CheckRemoteDebugger())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (CheckRemoteDebugger)", ConsoleColor.Gray, "PerformChecks", 46);
				failedCheck = "CheckRemoteDebugger";
				return true;
			}
			if (CheckRemoteLogger())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (CheckRemoteLogger)", ConsoleColor.Gray, "PerformChecks", 56);
				failedCheck = "CheckRemoteLogger";
				return true;
			}
			failedCheck = string.Empty;
			return false;
		}

		private static bool CheckDebuggerManagedPresent()
		{
			if (Debugger.IsAttached)
			{
				return true;
			}
			return false;
		}

		private static bool CheckDebuggerUnmanagedPresent()
		{
			if (IsDebuggerPresent())
			{
				return true;
			}
			return false;
		}

		private static bool CheckRemoteDebugger()
		{
			bool isDebuggerPresent = false;
			if (CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent) && isDebuggerPresent)
			{
				return true;
			}
			return false;
		}

		private static bool CheckRemoteLogger()
		{
			if (Debugger.IsLogging())
			{
				return true;
			}
			return false;
		}
	}
}
