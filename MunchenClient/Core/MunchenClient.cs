using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Config.Modules;
using MunchenClient.Core.Compatibility;
using MunchenClient.Menu;
using MunchenClient.Menu.Others;
using MunchenClient.Menu.PlayerOptions;
using MunchenClient.Misc;
using MunchenClient.ModuleSystem;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Patching;
using MunchenClient.Patching.Patches;
using MunchenClient.Security;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using ServerAPI.Core;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MunchenClient.Core
{
	public static class MunchenClient
	{
		private static readonly string assetPath = Configuration.GetClientFolderName() + "\\Dependencies\\ClientAssets\\ClientAssets.assetbundle";

		private static bool shouldRunClient = false;

		public static void OnApplicationStart(HttpClient httpClient, List<string> parameters, string baseUrl, bool debugMode)
		{
			ConsoleUtils.Info("Download Link", "https://github.com/PhotonBot");
			PerformanceProfiler.StartProfiling("OnApplicationStart");
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
				HttpClientCustom.SetHttpClient(httpClient);
				GeneralUtils.clientSpecialBenefits = parameters.Contains("specialBenefits");
				GeneralUtils.clientBetaBranch = parameters.Contains("betaBranch");
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Main", "OnApplicationStart - Assign Parameters", e, "OnApplicationStart", 51);
			}
			try
			{
				ApplicationBotHandler.PreCheckBotHandler();
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Main", "OnApplicationStart - PreCheckBotHandler", e2, "OnApplicationStart", 60);
			}
			try
			{
				GeneralUtils.AssignMainProcess(Process.GetCurrentProcess());
			}
			catch (Exception e3)
			{
				ConsoleUtils.Exception("Main", "OnApplicationStart - Assign Process", e3, "OnApplicationStart", 70);
			}
			try
			{
				AntiLeak.InitializeAntiLeak();
			}
			catch (Exception e4)
			{
				ConsoleUtils.Exception("Main", "OnApplicationStart - AntiLeak", e4, "OnApplicationStart", 80);
			}
			try
			{
				Configuration.OnApplicationStart();
			}
			catch (Exception e5)
			{
				ConsoleUtils.Exception("Main", "OnApplicationStart - Initialize", e5, "OnApplicationStart", 89);
			}
			if (!ApplicationBotHandler.IsBot())
			{
				try
				{
					CompatibilityLayer.CheckCompatiblity();
				}
				catch (Exception e6)
				{
					ConsoleUtils.Exception("Main", "OnApplicationStart - CompatibilityLayer", e6, "OnApplicationStart", 102);
				}
				try
				{
					ServerAPICore.InitializeInstance(baseUrl, debugMode);
				}
				catch (Exception e7)
				{
					ConsoleUtils.Exception("Main", "OnApplicationStart - ServerAPI", e7, "OnApplicationStart", 112);
				}
				try
				{
					Configuration.LoadAllConfigurations();
				}
				catch (Exception e8)
				{
					ConsoleUtils.Exception("Main", "OnApplicationStart - Config Loader", e8, "OnApplicationStart", 121);
				}
				try
				{
					Configuration.ApplyConfigSettings();
				}
				catch (Exception e9)
				{
					ConsoleUtils.Exception("Main", "OnApplicationStart - Config Processor", e9, "OnApplicationStart", 130);
				}
			}
			ModuleManager.ConstructModules();
			ModuleManager.InitializeModules();
			PatchManager.ConstructPatchesOnStart();
			PatchManager.InitializePatchesOnStart();
			try
			{
				ConsoleUtils.SetTitle(ConsoleUtils.GetTitle() + " - " + GeneralUtils.GetClientName() + " " + GeneralUtils.GetClientVersion() + " - Developed by " + GeneralUtils.GetClientDevelopers());
			}
			catch (Exception e10)
			{
				ConsoleUtils.Exception("Main", "OnApplicationStart - Title Changer", e10, "OnApplicationStart", 149);
			}
			SceneManager.add_sceneUnloaded((Action<Scene>)delegate(Scene scene)
			{
				OnLevelWasUnloaded(scene.buildIndex);
			});
			shouldRunClient = true;
			MelonCoroutines.Start(WaitForUserInterfaceInitialization());
			PerformanceProfiler.EndProfiling("OnApplicationStart");
		}

		public static void OnApplicationQuit()
		{
			if (shouldRunClient)
			{
				ModuleManager.ShutdownModules();
				if (Configuration.GetGeneralConfig().AutoClearCache)
				{
					MainUtils.ClearCache();
				}
			}
		}

		public static void OnLevelWasLoaded(int level)
		{
			if (shouldRunClient)
			{
				PerformanceProfiler.StartProfiling("OnLevelWasLoaded");
				ModuleManager.OnLevelWasLoaded(level);
				PerformanceProfiler.EndProfiling("OnLevelWasLoaded");
			}
		}

		public static void OnLevelWasInitialized(int level)
		{
			if (shouldRunClient)
			{
				PerformanceProfiler.StartProfiling("OnLevelWasInitialized");
				ModuleManager.OnLevelWasInitialized(level);
				PerformanceProfiler.EndProfiling("OnLevelWasInitialized");
			}
		}

		public static void OnLevelWasUnloaded(int levelIndex)
		{
			if (shouldRunClient)
			{
				PerformanceProfiler.StartProfiling("OnLevelWasUnloaded");
				ModuleManager.OnLevelWasUnloaded(levelIndex);
				PerformanceProfiler.EndProfiling("OnLevelWasUnloaded");
			}
		}

		public static void OnUpdate()
		{
			if (!shouldRunClient)
			{
				return;
			}
			if (!ApplicationBotHandler.IsBot())
			{
				try
				{
					ServerAPICore.GetInstance().OnUpdate();
				}
				catch (Exception e)
				{
					ConsoleUtils.Exception("Main", "OnUpdate - ServerAPICore", e, "OnUpdate", 238);
				}
				try
				{
					Configuration.OnUpdate();
				}
				catch (Exception e2)
				{
					ConsoleUtils.Exception("Main", "OnUpdate - Configuration", e2, "OnUpdate", 248);
				}
				try
				{
					if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Backspace))
					{
						MainUtils.RestartGame();
					}
				}
				catch (Exception e3)
				{
					ConsoleUtils.Exception("Main", "OnUpdate - Last Resort", e3, "OnUpdate", 261);
				}
			}
			ModuleManager.OnUpdate();
			MainUtils.ProcessMainThreadQueue();
			ConsoleUtils.ProcessWriteQueue();
		}

		public static void OnLateUpdate()
		{
			if (!shouldRunClient)
			{
				return;
			}
			ModuleManager.OnLateUpdate();
			try
			{
				AntiLeak.OnAntiLeakUpdate();
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Main", "OnUpdate - AntiLeak", e, "OnLateUpdate", 287);
			}
		}

		public static void OnFixedUpdate()
		{
			if (shouldRunClient)
			{
				ModuleManager.OnFixedUpdate();
			}
		}

		private static IEnumerator WaitForUserInterfaceInitialization()
		{
			while (GeneralWrappers.GetVRCUiManager() == null)
			{
				yield return new WaitForEndOfFrame();
			}
			OnUserInterfaceInitialized();
		}

		private static void OnUserInterfaceInitialized()
		{
			if (!shouldRunClient)
			{
				ConsoleUtils.FlushToConsole("[Main]", "Client not running due to no initialization", ConsoleColor.Gray, "OnUserInterfaceInitialized", 317);
				return;
			}
			PatchManager.InitializePatchesOnUIInit();
			if (CompatibilityLayer.GetCurrentGameVersion() != 1194 && CompatibilityLayer.GetCurrentGameVersion() != 1194)
			{
				ConsoleUtils.Info("Main", $"GameVersion {CompatibilityLayer.GetCurrentGameVersion()} - compatibility can't be assured", ConsoleColor.Gray, "OnUserInterfaceInitialized", 329);
			}
			if (ApplicationBotHandler.IsBot() || (File.Exists(assetPath) && Configuration.GetGeneralConfig().ClientAssetsVersion == GeneralConfig.ClientAssetsVersionCurrent))
			{
				InitializeMenu();
				return;
			}
			DependencyDownloader.DownloadDependency("ClientAssets", OnMenuDependencyFinished);
			ConsoleUtils.Info("Main", "Downloading client assets", ConsoleColor.Gray, "OnUserInterfaceInitialized", 341);
		}

		private static void OnMenuDependencyFinished()
		{
			ConsoleUtils.Info("Main", "Client assets downloaded", ConsoleColor.Gray, "OnMenuDependencyFinished", 347);
			Configuration.GetGeneralConfig().ClientAssetsVersion = GeneralConfig.ClientAssetsVersionCurrent;
			InitializeMenu();
		}

		private static void InitializeMenu()
		{
			PerformanceProfiler.StartProfiling("OnUIManagerInit");
			try
			{
				GeneralWrappers.InitializeWrappers();
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Main", "OnUIManagerInit - Wrapper Init", e, "InitializeMenu", 364);
			}
			if (!ApplicationBotHandler.IsBot())
			{
				try
				{
					LanguageManager.LoadLanguage();
				}
				catch (Exception e2)
				{
					ConsoleUtils.Exception("Main", "OnUIManagerInit - Language Manager", e2, "InitializeMenu", 377);
				}
				try
				{
					AssetLoader.LoadAssetBundle(assetPath);
					QuickMenuUtils.InitializeButtonAPI(LanguageManager.GetUsedLanguage().ClientName, MainUtils.CreateSprite(AssetLoader.LoadTexture("ToggleIconOn")), MainUtils.CreateSprite(AssetLoader.LoadTexture("ToggleIconOff")));
					new MainClientMenu();
					new QuickMenuHeader(QuickMenus.SelectedUser, LanguageManager.GetUsedLanguage().ClientName);
					QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(QuickMenus.SelectedUser);
					new PlayerOptionsMenu(quickMenuButtonRow);
					new PlayerApplicationBotMenu(quickMenuButtonRow);
					new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().ForceClone, delegate
					{
						PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
						if (selectedPlayer != null)
						{
							PlayerUtils.ChangePlayerAvatar(selectedPlayer.vrcPlayer.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id, logErrorOnHud: true);
						}
						else
						{
							ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, "InitializeMenu", 411);
							GeneralWrappers.AlertPopup("Warning", "Player not found");
						}
					}, LanguageManager.GetUsedLanguage().ForceCloneDescription);
					new UserInfoMenu();
					new ActionWheelMenu();
					if (Configuration.GetGeneralConfig().AutoClearCache)
					{
						MainUtils.ClearCache();
					}
				}
				catch (Exception e3)
				{
					ConsoleUtils.Exception("Main", "OnUIManagerInit - Menu Init", e3, "InitializeMenu", 428);
				}
				try
				{
					ServerAPICore.GetInstance().OnUIManagerInit();
				}
				catch (Exception e4)
				{
					ConsoleUtils.Exception("Main", "OnUIManagerInit - ServerAPICore", e4, "InitializeMenu", 438);
				}
				try
				{
					UserInterface.SetupMenuButtons();
				}
				catch (Exception e5)
				{
					ConsoleUtils.Exception("Main", "OnUIManagerInit - SetupMenuButtons", e5, "InitializeMenu", 447);
				}
				try
				{
					if (Configuration.GetGeneralConfig().PerformanceUnlimitedFPS && !GeneralWrappers.IsInVR())
					{
						Application.targetFrameRate = GeneralUtils.GetRefreshRate();
					}
					MenuColorHandler.FindMenuItems();
				}
				catch (Exception e6)
				{
					ConsoleUtils.Exception("Main", "OnUIManagerInit - Post Config", e6, "InitializeMenu", 462);
				}
			}
			else
			{
				LanguageManager.UseEnglishLanguage();
			}
			ModuleManager.OnUIManagerLoaded();
			try
			{
				NetworkManagerPatch.OnUIManagerInit();
			}
			catch (Exception e7)
			{
				ConsoleUtils.Exception("Main", "OnUIManagerInit - SetupMenuButtons", e7, "InitializeMenu", 479);
			}
			PerformanceProfiler.EndProfiling("OnUIManagerInit");
			if (GeneralUtils.IsBetaClient())
			{
				ConsoleUtils.Info("Performance", string.Format("OnApplicationStart took: {0:F2} ms", PerformanceProfiler.GetProfiling("OnApplicationStart")), ConsoleColor.Gray, "InitializeMenu", 487);
				ConsoleUtils.Info("Performance", string.Format("OnUIManagerInit took: {0:F2} ms", PerformanceProfiler.GetProfiling("OnUIManagerInit")), ConsoleColor.Gray, "InitializeMenu", 488);
			}
		}
	}
}
