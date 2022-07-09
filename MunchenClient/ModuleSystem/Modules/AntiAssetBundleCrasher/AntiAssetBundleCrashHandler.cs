using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;

namespace MunchenClient.ModuleSystem.Modules.AntiAssetBundleCrasher
{
	internal class AntiAssetBundleCrashHandler : ModuleComponent
	{
		private static readonly string sandboxCheckerApplicationPath = Configuration.GetClientFolderName() + "\\Dependencies\\AABC\\AntiAssetBundleCrash.exe";

		private static Socket masterSocket = null;

		private static Socket sandboxConnection = null;

		private static Thread sandboxThread = null;

		private static bool abdhPatchesInitialized = false;

		private static int totalCrashes = -1;

		protected override string moduleName => "Anti Asset Bundle Crash Handler";

		internal override void OnApplicationStart()
		{
			InitializeABDHPatcher();
		}

		internal override void OnUIManagerLoaded()
		{
		}

		internal override void OnRoomLeft()
		{
		}

		internal override void OnRoomJoined()
		{
		}

		internal override void OnUpdate()
		{
			if (Configuration.GetAntiCrashConfig().AntiAssetBundleCrash)
			{
			}
		}

		internal static void InitializeABDHPatcher()
		{
			if (Configuration.GetAntiCrashConfig().AntiAssetBundleCrash && !abdhPatchesInitialized)
			{
				if (!AssetBundleDownloadHandlerWrapper.InitializePatches())
				{
					ConsoleUtils.Info("AABC", "Unable to patch ABDH - AABC won't work", ConsoleColor.Gray, "InitializeABDHPatcher", 76);
				}
				else
				{
					abdhPatchesInitialized = true;
				}
			}
		}

		private static void StartSocket()
		{
			try
			{
				sandboxConnection = masterSocket.Accept();
				ConsoleUtils.Info("AABC", "Sandbox connected", ConsoleColor.Gray, "StartSocket", 88);
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("AABC", "ConnectionHandler", e, "StartSocket", 92);
				return;
			}
			byte[] array = new byte[2048];
			while (GeneralUtils.GetMainProcess() != null)
			{
				try
				{
					int num = masterSocket.Receive(array);
					if (num == 0)
					{
						continue;
					}
					string @string = Encoding.ASCII.GetString(array, 0, num);
					if (string.IsNullOrEmpty(@string))
					{
						continue;
					}
					string[] array2 = @string.Split(',');
					string[] array3 = array2;
					foreach (string text in array3)
					{
						if (!string.IsNullOrEmpty(text))
						{
							ConsoleUtils.Info("AABC", "Received command: " + text, ConsoleColor.Gray, "StartSocket", 127);
						}
					}
				}
				catch (SocketException)
				{
					sandboxConnection.Dispose();
					sandboxConnection = null;
					ConsoleUtils.Info("AABC", "Socket was disconnected", ConsoleColor.Gray, "StartSocket", 135);
					break;
				}
			}
			StartSandbox();
		}

		internal static void StartSandbox()
		{
			if (File.Exists(sandboxCheckerApplicationPath))
			{
				StartSandboxProcess();
				return;
			}
			DependencyDownloader.DownloadDependency("AABC", OnDependencyFinished);
			ConsoleUtils.Info("AABC", "Downloading dependency", ConsoleColor.Gray, "StartSandbox", 154);
		}

		private static void OnDependencyFinished()
		{
			ConsoleUtils.Info("AABC", "Dependency downloaded", ConsoleColor.Gray, "OnDependencyFinished", 160);
			StartSandboxProcess();
		}

		private static void StartSandboxProcess()
		{
			totalCrashes++;
			ConsoleUtils.Info("AABC", $"Total Crashes: {totalCrashes}", ConsoleColor.Red, "StartSandboxProcess", 168);
			sandboxThread = new Thread(StartSocket);
			sandboxThread.Start();
			ConsoleUtils.Info("AABC", "Sandbox started", ConsoleColor.Gray, "StartSandboxProcess", 184);
		}
	}
}
