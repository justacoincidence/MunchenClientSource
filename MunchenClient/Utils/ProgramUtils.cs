using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace MunchenClient.Utils
{
	internal class ProgramUtils
	{
		private const string keyBase = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

		internal static string GetInstallPath(string programName)
		{
			string result = string.Empty;
			RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			RegistryKey key = registryKey.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
			if (key != null)
			{
				foreach (RegistryKey item in from keyName in key.GetSubKeyNames()
					select key.OpenSubKey(keyName))
				{
					if (item.GetValue("DisplayName") is string text && text.ToLower().Contains(programName))
					{
						result = item.GetValue("InstallLocation").ToString();
						break;
					}
				}
				key.Close();
			}
			return result;
		}

		internal static string GetProgramPath(string programName)
		{
			string installPath = GetInstallPath(programName);
			if (string.IsNullOrEmpty(installPath))
			{
				return string.Empty;
			}
			string[] files = Directory.GetFiles(installPath);
			foreach (string text in files)
			{
				if (!text.Contains("unins") && !text.Contains("000") && text.EndsWith(".exe"))
				{
					return text;
				}
			}
			return string.Empty;
		}

		internal static bool IsProcessRunning(string processName)
		{
			Process[] processes = Process.GetProcesses();
			Process[] array = processes;
			foreach (Process process in array)
			{
				if (process.ProcessName.ToLower().Contains(processName))
				{
					return true;
				}
			}
			return false;
		}
	}
}
