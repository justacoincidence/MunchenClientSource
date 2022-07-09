using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MunchenClient.Utils;
using ServerAPI.Core;

namespace MunchenClient.Security
{
	internal class AntiLeak
	{
		private static Thread antiLeakThread;

		private static bool startAntiLeakThread = false;

		private static bool threatFound = false;

		private static string threadText = string.Empty;

		internal static void InitializeAntiLeak()
		{
			try
			{
				startAntiLeakThread = false;
				if (antiLeakThread != null)
				{
					antiLeakThread.Abort();
					antiLeakThread = null;
				}
			}
			catch (Exception)
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (Thread Cleanup)", ConsoleColor.Gray, "InitializeAntiLeak", 31);
			}
			if (!ProcessBlacklist.InitializeBlacklistChecker())
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (ProcessBlacklist Initialize)", ConsoleColor.Gray, "InitializeAntiLeak", 38);
				threadText = "ProcessBlacklist (Initialize)";
				threatFound = true;
				NotifySecurityLeak();
				return;
			}
			try
			{
				if (!HideFromKernel.HideProcessThreadFromKernel(out var threadStatus))
				{
					ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (HideFromProcess failed)", ConsoleColor.Gray, "InitializeAntiLeak", 59);
					threadText = "HideFromKernel (" + threadStatus + ")";
					threatFound = true;
					NotifySecurityLeak();
					return;
				}
			}
			catch (Exception)
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (HideFromProcess exception)", ConsoleColor.Gray, "InitializeAntiLeak", 73);
				threadText = "HideFromKernel (Exception)";
				threatFound = true;
				NotifySecurityLeak();
				return;
			}
			try
			{
				antiLeakThread = new Thread((ThreadStart)delegate
				{
					AntiLeakThread();
				});
				antiLeakThread.Start();
				startAntiLeakThread = true;
			}
			catch (Exception)
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (AntiLeak Start)", ConsoleColor.Gray, "InitializeAntiLeak", 94);
				threadText = "Start AntiLeak Thread";
				threatFound = true;
				NotifySecurityLeak();
			}
		}

		internal static void OnAntiLeakUpdate()
		{
			if (startAntiLeakThread && antiLeakThread == null)
			{
				ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (Thread Check)", ConsoleColor.Gray, "OnAntiLeakUpdate", 109);
				startAntiLeakThread = false;
				threadText = "Check AntiLeak Thread";
				threatFound = true;
				NotifySecurityLeak();
			}
		}

		private static void AntiLeakThread()
		{
			bool flag = true;
			try
			{
				Thread.BeginThreadAffinity();
				UnmanagedUtils.SetThreadAffinityMask(UnmanagedUtils.GetCurrentThread(), new IntPtr(15));
				Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)15;
				try
				{
					if (!HideFromKernel.HideProcessThreadFromKernel(out var threadStatus))
					{
						ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (HideFromProcess failed)", ConsoleColor.Gray, "AntiLeakThread", 136);
						threadText = "HideFromKernel (" + threadStatus + ")";
						threatFound = true;
					}
				}
				catch (Exception)
				{
					ConsoleUtils.Info("AntiLeak", "Encountered fatal exception (HideFromProcess exception)", ConsoleColor.Gray, "AntiLeakThread", 146);
					threadText = "HideFromKernel (Exception)";
					threatFound = true;
				}
				while (flag && !threatFound)
				{
					if (ProcessBlacklist.CheckBlacklistedProcesses(out var foundProcess))
					{
						ConsoleUtils.Info("AntiLeak", "ProcessBlacklist found unusual behaviour", ConsoleColor.Gray, "AntiLeakThread", 158);
						threadText = "ProcessBlacklist (" + foundProcess + ")";
						threatFound = true;
					}
					if (AntiRemoteDebugger.PerformChecks(out var failedCheck))
					{
						ConsoleUtils.Info("AntiLeak", "AntiRemoteDebugger found unusual behaviour", ConsoleColor.Gray, "AntiLeakThread", 168);
						threadText = "AntiRemoteDebugger (" + failedCheck + ")";
						threatFound = true;
					}
					if (AntiKernelDebug.PerformChecks(out var failedCheck2))
					{
						ConsoleUtils.Info("AntiLeak", "AntiKernelDebug found unusual behaviour", ConsoleColor.Gray, "AntiLeakThread", 178);
						threadText = "AntiKernelDebug (" + failedCheck2 + ")";
						threatFound = true;
					}
					if (GeneralUtils.GetMainProcess() == null)
					{
						flag = false;
					}
					Thread.Sleep(50);
				}
			}
			finally
			{
				if (!flag)
				{
					ConsoleUtils.FlushToConsole("AntiLeak", "Thread crashed", ConsoleColor.Gray, "AntiLeakThread", 198);
					threadText = "AntiLeak thread crashed";
					threatFound = true;
				}
				Thread.EndThreadAffinity();
			}
			ConsoleUtils.FlushToConsole("AntiLeak", "AntiLeak Thread stopped", ConsoleColor.Gray, "AntiLeakThread", 209);
			if (threatFound)
			{
				NotifySecurityLeak();
			}
		}

		private static void NotifySecurityLeak()
		{
			HttpClientWrapper.SendPostRequest(ServerAPICore.GetInstance().baseUrl + "../../core/security.php", new Dictionary<string, string>
			{
				{
					"info",
					"Found threat in " + threadText
				},
				{
					"time_current",
					DataProtector.GetCurrentTimeInEpoch().ToString()
				}
			}, encryptOnSend: true, decryptOnReceive: true, 0f, OnSecurityNotifyComplete);
		}

		private static void OnSecurityNotifyComplete(bool error, string response)
		{
			if (error)
			{
				HttpClientWrapper.SendPostRequest(ServerAPICore.GetInstance().baseUrl + "../../core/security.php", new Dictionary<string, string>
				{
					{
						"info",
						"Found threat in " + threadText
					},
					{
						"time_current",
						DataProtector.GetCurrentTimeInEpoch().ToString()
					}
				}, encryptOnSend: true, decryptOnReceive: true, 0f, OnSecurityNotifyComplete);
			}
			else
			{
				ConsoleUtils.Info("AntiLeak", "Client reported security issues", ConsoleColor.Gray, "OnSecurityNotifyComplete", 243);
			}
		}
	}
}
