using System;
using System.Collections.Generic;
using MunchenClient.Utils;

namespace MunchenClient.Security
{
	internal class ProcessBlacklist
	{
		private static readonly List<string> blacklistedWindowNames = new List<string>();

		internal static bool InitializeBlacklistChecker()
		{
			try
			{
				blacklistedWindowNames.Clear();
				blacklistedWindowNames.Add("ollydbg");
				blacklistedWindowNames.Add("ida");
				blacklistedWindowNames.Add("disassembly");
				blacklistedWindowNames.Add("scylla");
				blacklistedWindowNames.Add("debug");
				blacklistedWindowNames.Add("[cpu");
				blacklistedWindowNames.Add("immunity");
				blacklistedWindowNames.Add("windbg");
				blacklistedWindowNames.Add("x32dbg");
				blacklistedWindowNames.Add("x64dbg");
				blacklistedWindowNames.Add("import reconstructor");
				blacklistedWindowNames.Add("megadumper");
				blacklistedWindowNames.Add("megadumper 1.0 by codecracker / snd");
				blacklistedWindowNames.Add("wireshark");
			}
			catch (Exception)
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (CheckBlacklistedProcesses Init)", ConsoleColor.Gray, "InitializeBlacklistChecker", 37);
				return false;
			}
			return true;
		}

		internal static bool CheckBlacklistedProcesses(out string foundProcess)
		{
			foundProcess = string.Empty;
			return false;
		}
	}
}
