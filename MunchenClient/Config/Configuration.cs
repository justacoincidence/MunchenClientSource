using System;
using System.Diagnostics;
using System.IO;
using MunchenClient.Config.Modules;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;

namespace MunchenClient.Config
{
	internal static class Configuration
	{
		private static readonly string clientDirectoryName = "MünchenClient";

		private static string gameDirectory = string.Empty;

		private static string clientDirectory = string.Empty;

		private static string configDirectory = string.Empty;

		internal static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			ContractResolver = new CustomContractResolver("normalized")
		};

		private static GeneralConfig generalConfig;

		private static bool generalConfigSaving = false;

		private static bool generalConfigSavePending = false;

		private static InstanceHistoryConfig instanceHistoryConfig;

		private static bool instanceHistoryConfigSaving = false;

		private static bool instanceHistoryConfigSavePending = false;

		private static AvatarHistoryConfig avatarHistoryConfig;

		private static bool avatarHistoryConfigSaving = false;

		private static bool avatarHistoryConfigSavePending = false;

		private static AvatarCalibrationsConfig avatarCalibrationsConfig;

		private static bool avatarCalibrationsConfigSaving = false;

		private static bool avatarCalibrationsConfigSavePending = false;

		private static GlobalDynamicBonesConfig globalDynamicBonesConfig;

		private static bool globalDynamicBonesConfigSaving = false;

		private static bool globalDynamicBonesConfigSavePending = false;

		private static AntiCrashConfig antiCrashConfig;

		private static bool antiCrashConfigSaving = false;

		private static bool antiCrashConfigSavePending = false;

		private static ModerationsConfig moderationsConfig;

		private static bool moderationsConfigSaving = false;

		private static bool moderationsConfigSavePending = false;

		private static UploadQueueConfig uploadQueueConfig;

		private static bool uploadQueueConfigSaving = false;

		private static bool uploadQueueConfigSavePending = false;

		private static LovenseConfig lovenseConfig;

		private static bool lovenseConfigSaving = false;

		private static bool lovenseConfigSavePending = false;

		private static TestedAssetBundlesConfig testedAssetBundlesConfig;

		private static bool testedAssetBundlesConfigSaving = false;

		private static bool testedAssetBundlesConfigSavePending = false;

		internal static string GetClientFolderName()
		{
			return clientDirectoryName;
		}

		internal static string GetClientFolderPath()
		{
			return clientDirectory;
		}

		internal static string GetConfigFolderPath()
		{
			return configDirectory;
		}

		internal static string GetGameFolderPath()
		{
			return gameDirectory;
		}

		internal static void OnApplicationStart()
		{
			string fileName = Process.GetCurrentProcess().MainModule.FileName;
			int length = fileName.LastIndexOf('\\');
			gameDirectory = fileName.Substring(0, length);
			clientDirectory = gameDirectory + "\\" + GetClientFolderName() + "\\";
			configDirectory = clientDirectory + "\\Config\\";
		}

		internal static void OnUpdate()
		{
			if (!generalConfigSaving && generalConfigSavePending)
			{
				SaveGeneralConfig();
			}
			if (!instanceHistoryConfigSaving && instanceHistoryConfigSavePending)
			{
				SaveInstanceHistoryConfig();
			}
			if (!avatarHistoryConfigSaving && avatarHistoryConfigSavePending)
			{
				SaveAvatarHistoryConfig();
			}
			if (!avatarCalibrationsConfigSaving && avatarCalibrationsConfigSavePending)
			{
				SaveAvatarCalibrationsConfig();
			}
			if (!globalDynamicBonesConfigSaving && globalDynamicBonesConfigSavePending)
			{
				SaveGlobalDynamicBonesConfig();
			}
			if (!antiCrashConfigSaving && antiCrashConfigSavePending)
			{
				SaveAntiCrashConfig();
			}
			if (!moderationsConfigSaving && moderationsConfigSavePending)
			{
				SaveModerationsConfig();
			}
			if (!uploadQueueConfigSaving && uploadQueueConfigSavePending)
			{
				SaveUploadQueueConfig();
			}
			if (!lovenseConfigSaving && lovenseConfigSavePending)
			{
				SaveLovenseConfig();
			}
			if (!testedAssetBundlesConfigSaving && testedAssetBundlesConfigSavePending)
			{
				SaveTestedAssetBundlesConfig();
			}
		}

		internal static void LoadAllConfigurations()
		{
			if (!Directory.Exists(configDirectory))
			{
				Directory.CreateDirectory(configDirectory);
				CreateConfigurations();
				return;
			}
			LoadGeneralConfig();
			LoadInstanceHistoryConfig();
			LoadAvatarHistoryConfig();
			LoadAvatarCalibrationsConfig();
			LoadGlobalDynamicBonesConfig();
			LoadAntiCrashConfig();
			LoadModerationsConfig();
			LoadUploadQueueConfig();
			LoadLovenseConfig();
			LoadTestedAssetBundlesConfig();
		}

		internal static void LoadGeneralConfig()
		{
			if (!File.Exists(GeneralConfig.ConfigLocation))
			{
				CreateGeneralConfig();
				return;
			}
			try
			{
				generalConfig = JsonConvert.DeserializeObject<GeneralConfig>(File.ReadAllText(GeneralConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (GeneralConfig)", ConsoleColor.Gray, "LoadGeneralConfig", 194);
			}
		}

		internal static void LoadInstanceHistoryConfig()
		{
			if (!File.Exists(InstanceHistoryConfig.ConfigLocation))
			{
				CreateInstanceHistoryConfig();
				return;
			}
			try
			{
				instanceHistoryConfig = JsonConvert.DeserializeObject<InstanceHistoryConfig>(File.ReadAllText(InstanceHistoryConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (InstanceHistoryConfig)", ConsoleColor.Gray, "LoadInstanceHistoryConfig", 214);
			}
		}

		internal static void LoadAvatarHistoryConfig()
		{
			if (!File.Exists(AvatarHistoryConfig.ConfigLocation))
			{
				CreateAvatarHistoryConfig();
				return;
			}
			try
			{
				avatarHistoryConfig = JsonConvert.DeserializeObject<AvatarHistoryConfig>(File.ReadAllText(AvatarHistoryConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (AvatarHistoryConfig)", ConsoleColor.Gray, "LoadAvatarHistoryConfig", 234);
			}
		}

		internal static void LoadAvatarCalibrationsConfig()
		{
			if (!File.Exists(AvatarCalibrationsConfig.ConfigLocation))
			{
				CreateAvatarCalibrationsConfig();
				return;
			}
			try
			{
				avatarCalibrationsConfig = JsonConvert.DeserializeObject<AvatarCalibrationsConfig>(File.ReadAllText(AvatarCalibrationsConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (AvatarCalibrationsConfig)", ConsoleColor.Gray, "LoadAvatarCalibrationsConfig", 254);
			}
		}

		internal static void LoadGlobalDynamicBonesConfig()
		{
			if (!File.Exists(GlobalDynamicBonesConfig.ConfigLocation))
			{
				CreateGlobalDynamicBonesConfig();
				return;
			}
			try
			{
				globalDynamicBonesConfig = JsonConvert.DeserializeObject<GlobalDynamicBonesConfig>(File.ReadAllText(GlobalDynamicBonesConfig.ConfigLocation));
				CheckGlobalDynamicBonesConfig();
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (GlobalDynamicBonesConfig)", ConsoleColor.Gray, "LoadGlobalDynamicBonesConfig", 276);
			}
		}

		internal static void LoadAntiCrashConfig()
		{
			if (!File.Exists(AntiCrashConfig.ConfigLocation))
			{
				CreateAntiCrashConfig();
				return;
			}
			try
			{
				antiCrashConfig = JsonConvert.DeserializeObject<AntiCrashConfig>(File.ReadAllText(AntiCrashConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (AntiCrashConfig)", ConsoleColor.Gray, "LoadAntiCrashConfig", 296);
			}
		}

		internal static void LoadModerationsConfig()
		{
			if (!File.Exists(ModerationsConfig.ConfigLocation))
			{
				CreateModerationsConfig();
				return;
			}
			try
			{
				moderationsConfig = JsonConvert.DeserializeObject<ModerationsConfig>(File.ReadAllText(ModerationsConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (ModerationsConfig)", ConsoleColor.Gray, "LoadModerationsConfig", 316);
			}
		}

		internal static void LoadUploadQueueConfig()
		{
			if (!File.Exists(UploadQueueConfig.ConfigLocation))
			{
				CreateUploadQueueConfig();
				return;
			}
			try
			{
				uploadQueueConfig = JsonConvert.DeserializeObject<UploadQueueConfig>(File.ReadAllText(UploadQueueConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (UploadQueueConfig)", ConsoleColor.Gray, "LoadUploadQueueConfig", 336);
			}
		}

		internal static void LoadLovenseConfig()
		{
			if (!File.Exists(LovenseConfig.ConfigLocation))
			{
				CreateLovenseConfig();
				return;
			}
			try
			{
				lovenseConfig = JsonConvert.DeserializeObject<LovenseConfig>(File.ReadAllText(LovenseConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (LovenseConfig)", ConsoleColor.Gray, "LoadLovenseConfig", 356);
			}
		}

		internal static void LoadTestedAssetBundlesConfig()
		{
			if (!File.Exists(TestedAssetBundlesConfig.ConfigLocation))
			{
				CreateTestedAssetBundlesConfig();
				return;
			}
			try
			{
				testedAssetBundlesConfig = JsonConvert.DeserializeObject<TestedAssetBundlesConfig>(File.ReadAllText(TestedAssetBundlesConfig.ConfigLocation));
			}
			catch (Exception)
			{
				ConsoleUtils.Info("Config", "Corrupt file couldn't be loaded (TestedAssetBundlesConfig)", ConsoleColor.Gray, "LoadTestedAssetBundlesConfig", 376);
			}
		}

		internal static void SaveAllConfigurations()
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveAllConfigurations", e, "SaveAllConfigurations", 398);
			}
			SaveGeneralConfig();
			SaveInstanceHistoryConfig();
			SaveAvatarHistoryConfig();
			SaveAvatarCalibrationsConfig();
			SaveGlobalDynamicBonesConfig();
			SaveAntiCrashConfig();
			SaveModerationsConfig();
			SaveUploadQueueConfig();
			SaveLovenseConfig();
			SaveTestedAssetBundlesConfig();
		}

		internal static void SaveGeneralConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (generalConfigSaving && !forceSave)
			{
				generalConfigSavePending = true;
				return;
			}
			generalConfigSavePending = false;
			generalConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveGeneralConfig - Integrity", e, "SaveGeneralConfig", 439);
			}
			try
			{
				File.WriteAllText(GeneralConfig.ConfigLocation, JsonConvert.SerializeObject(generalConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveGeneralConfig - Save", e2, "SaveGeneralConfig", 448);
			}
			generalConfigSaving = false;
		}

		internal static void SaveInstanceHistoryConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (instanceHistoryConfigSaving && !forceSave)
			{
				instanceHistoryConfigSavePending = true;
				return;
			}
			instanceHistoryConfigSavePending = false;
			instanceHistoryConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveInstanceHistoryConfig - Integrity", e, "SaveInstanceHistoryConfig", 480);
			}
			try
			{
				File.WriteAllText(InstanceHistoryConfig.ConfigLocation, JsonConvert.SerializeObject(instanceHistoryConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveInstanceHistoryConfig - Save", e2, "SaveInstanceHistoryConfig", 489);
			}
			instanceHistoryConfigSaving = false;
		}

		internal static void SaveAvatarHistoryConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (avatarHistoryConfigSaving && !forceSave)
			{
				avatarHistoryConfigSavePending = true;
				return;
			}
			avatarHistoryConfigSavePending = false;
			avatarHistoryConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveAvatarHistoryConfig - Integrity", e, "SaveAvatarHistoryConfig", 521);
			}
			try
			{
				File.WriteAllText(AvatarHistoryConfig.ConfigLocation, JsonConvert.SerializeObject(avatarHistoryConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveAvatarHistoryConfig - Save", e2, "SaveAvatarHistoryConfig", 530);
			}
			avatarHistoryConfigSaving = false;
		}

		internal static void SaveAvatarCalibrationsConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (avatarCalibrationsConfigSaving && !forceSave)
			{
				avatarCalibrationsConfigSavePending = true;
				return;
			}
			avatarCalibrationsConfigSavePending = false;
			avatarCalibrationsConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveAvatarCalibrationsConfig - Integrity", e, "SaveAvatarCalibrationsConfig", 562);
			}
			try
			{
				File.WriteAllText(AvatarCalibrationsConfig.ConfigLocation, JsonConvert.SerializeObject(avatarCalibrationsConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveAvatarCalibrationsConfig - Save", e2, "SaveAvatarCalibrationsConfig", 571);
			}
			avatarCalibrationsConfigSaving = false;
		}

		internal static void SaveGlobalDynamicBonesConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (globalDynamicBonesConfigSaving && !forceSave)
			{
				globalDynamicBonesConfigSavePending = true;
				return;
			}
			globalDynamicBonesConfigSavePending = false;
			globalDynamicBonesConfigSaving = true;
			CheckGlobalDynamicBonesConfig();
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveGlobalDynamicBonesConfig - Integrity", e, "SaveGlobalDynamicBonesConfig", 605);
			}
			try
			{
				File.WriteAllText(GlobalDynamicBonesConfig.ConfigLocation, JsonConvert.SerializeObject(globalDynamicBonesConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveGlobalDynamicBonesConfig - Save", e2, "SaveGlobalDynamicBonesConfig", 614);
			}
			globalDynamicBonesConfigSaving = false;
		}

		internal static void SaveAntiCrashConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (antiCrashConfigSaving && !forceSave)
			{
				antiCrashConfigSavePending = true;
				return;
			}
			antiCrashConfigSavePending = false;
			antiCrashConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveAntiCrashConfig - Integrity", e, "SaveAntiCrashConfig", 646);
			}
			try
			{
				File.WriteAllText(AntiCrashConfig.ConfigLocation, JsonConvert.SerializeObject(antiCrashConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveAntiCrashConfig - Save", e2, "SaveAntiCrashConfig", 655);
			}
			antiCrashConfigSaving = false;
		}

		internal static void SaveModerationsConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (moderationsConfigSaving && !forceSave)
			{
				moderationsConfigSavePending = true;
				return;
			}
			moderationsConfigSavePending = false;
			moderationsConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveModerationsConfig - Integrity", e, "SaveModerationsConfig", 687);
			}
			try
			{
				File.WriteAllText(ModerationsConfig.ConfigLocation, JsonConvert.SerializeObject(moderationsConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveModerationsConfig - Save", e2, "SaveModerationsConfig", 696);
			}
			moderationsConfigSaving = false;
		}

		internal static void SaveUploadQueueConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (uploadQueueConfigSaving && !forceSave)
			{
				uploadQueueConfigSavePending = true;
				return;
			}
			uploadQueueConfigSavePending = false;
			uploadQueueConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveUploadQueueConfig - Integrity", e, "SaveUploadQueueConfig", 728);
			}
			try
			{
				File.WriteAllText(UploadQueueConfig.ConfigLocation, JsonConvert.SerializeObject(uploadQueueConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveUploadQueueConfig - Save", e2, "SaveUploadQueueConfig", 737);
			}
			uploadQueueConfigSaving = false;
		}

		internal static void SaveLovenseConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (lovenseConfigSaving && !forceSave)
			{
				lovenseConfigSavePending = true;
				return;
			}
			lovenseConfigSavePending = false;
			lovenseConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveLovenseConfig - Integrity", e, "SaveLovenseConfig", 769);
			}
			try
			{
				File.WriteAllText(LovenseConfig.ConfigLocation, JsonConvert.SerializeObject(lovenseConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveLovenseConfig - Save", e2, "SaveLovenseConfig", 778);
			}
			lovenseConfigSaving = false;
		}

		internal static void SaveTestedAssetBundlesConfig(bool forceSave = false)
		{
			if (ApplicationBotHandler.IsBot())
			{
				return;
			}
			if (testedAssetBundlesConfigSaving && !forceSave)
			{
				testedAssetBundlesConfigSavePending = true;
				return;
			}
			testedAssetBundlesConfigSavePending = false;
			testedAssetBundlesConfigSaving = true;
			try
			{
				if (!Directory.Exists(configDirectory))
				{
					Directory.CreateDirectory(configDirectory);
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Config", "SaveTestedAssetBundlesConfig - Integrity", e, "SaveTestedAssetBundlesConfig", 810);
			}
			try
			{
				File.WriteAllText(TestedAssetBundlesConfig.ConfigLocation, JsonConvert.SerializeObject(testedAssetBundlesConfig, Formatting.Indented, jsonSerializerSettings));
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Config", "SaveTestedAssetBundlesConfig - Save", e2, "SaveTestedAssetBundlesConfig", 819);
			}
			testedAssetBundlesConfigSaving = false;
		}

		internal static void CreateConfigurations()
		{
			CreateGeneralConfig();
			CreateInstanceHistoryConfig();
			CreateAvatarHistoryConfig();
			CreateAvatarCalibrationsConfig();
			CreateGlobalDynamicBonesConfig();
			CreateAntiCrashConfig();
			CreateModerationsConfig();
			CreateUploadQueueConfig();
			CreateLovenseConfig();
			CreateTestedAssetBundlesConfig();
		}

		internal static void CreateGeneralConfig()
		{
			generalConfig = new GeneralConfig();
			SaveGeneralConfig();
		}

		internal static void CreateInstanceHistoryConfig()
		{
			instanceHistoryConfig = new InstanceHistoryConfig();
			SaveInstanceHistoryConfig();
		}

		internal static void CreateAvatarHistoryConfig()
		{
			avatarHistoryConfig = new AvatarHistoryConfig();
			SaveAvatarHistoryConfig();
		}

		internal static void CreateAvatarCalibrationsConfig()
		{
			avatarCalibrationsConfig = new AvatarCalibrationsConfig();
			CheckGlobalDynamicBonesConfig();
			SaveAvatarCalibrationsConfig();
		}

		internal static void CreateGlobalDynamicBonesConfig()
		{
			globalDynamicBonesConfig = new GlobalDynamicBonesConfig();
			SaveGlobalDynamicBonesConfig();
		}

		internal static void CreateAntiCrashConfig()
		{
			antiCrashConfig = new AntiCrashConfig();
			SaveAntiCrashConfig();
		}

		internal static void CreateModerationsConfig()
		{
			moderationsConfig = new ModerationsConfig();
			SaveModerationsConfig();
		}

		internal static void CreateUploadQueueConfig()
		{
			uploadQueueConfig = new UploadQueueConfig();
			SaveUploadQueueConfig();
		}

		internal static void CreateLovenseConfig()
		{
			lovenseConfig = new LovenseConfig();
			SaveLovenseConfig();
		}

		internal static void CreateTestedAssetBundlesConfig()
		{
			testedAssetBundlesConfig = new TestedAssetBundlesConfig();
			SaveTestedAssetBundlesConfig();
		}

		internal static GeneralConfig GetGeneralConfig()
		{
			if (generalConfig == null)
			{
				CreateGeneralConfig();
			}
			return generalConfig;
		}

		internal static InstanceHistoryConfig GetInstanceHistoryConfig()
		{
			if (instanceHistoryConfig == null)
			{
				CreateInstanceHistoryConfig();
			}
			return instanceHistoryConfig;
		}

		internal static AvatarHistoryConfig GetAvatarHistoryConfig()
		{
			if (avatarHistoryConfig == null)
			{
				CreateAvatarHistoryConfig();
			}
			return avatarHistoryConfig;
		}

		internal static AvatarCalibrationsConfig GetAvatarCalibrationsConfig()
		{
			if (avatarCalibrationsConfig == null)
			{
				CreateAvatarCalibrationsConfig();
			}
			return avatarCalibrationsConfig;
		}

		internal static GlobalDynamicBonesConfig GetGlobalDynamicBonesConfig()
		{
			if (globalDynamicBonesConfig == null)
			{
				CreateGlobalDynamicBonesConfig();
			}
			return globalDynamicBonesConfig;
		}

		internal static AntiCrashConfig GetAntiCrashConfig()
		{
			if (antiCrashConfig == null)
			{
				CreateAntiCrashConfig();
			}
			return antiCrashConfig;
		}

		internal static ModerationsConfig GetModerationsConfig()
		{
			if (moderationsConfig == null)
			{
				CreateModerationsConfig();
			}
			return moderationsConfig;
		}

		internal static UploadQueueConfig GetUploadQueueConfig()
		{
			if (uploadQueueConfig == null)
			{
				CreateUploadQueueConfig();
			}
			return uploadQueueConfig;
		}

		internal static LovenseConfig GetLovenseConfig()
		{
			if (lovenseConfig == null)
			{
				CreateLovenseConfig();
			}
			return lovenseConfig;
		}

		internal static TestedAssetBundlesConfig GetTestedAssetBundlesConfig()
		{
			if (testedAssetBundlesConfig == null)
			{
				CreateTestedAssetBundlesConfig();
			}
			return testedAssetBundlesConfig;
		}

		internal static void ApplyConfigSettings()
		{
			if (globalDynamicBonesConfig.GlobalDynamicBones)
			{
				PlayerPrefs.SetInt("VRC_LIMIT_DYNAMIC_BONE_USAGE", 0);
			}
			if (generalConfig.PerformanceLowGraphics)
			{
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				Graphics.activeTier = GraphicsTier.Tier1;
			}
			if (generalConfig.PerformanceNoAntiAliasing)
			{
				QualitySettings.antiAliasing = 1;
			}
			if (generalConfig.PerformanceNoShadows)
			{
				QualitySettings.shadows = ShadowQuality.Disable;
			}
			if (generalConfig.PerformanceInputDelayReducer)
			{
				QualitySettings.maxQueuedFrames = 1;
			}
			GeneralUtils.ChangeGameProcessPriority(generalConfig.PerformanceHighPriority);
			GeneralUtils.ChangeGameCoreAffinity(generalConfig.PerformanceNoHyperThreading);
		}

		internal static void CheckGlobalDynamicBonesConfig()
		{
			if (globalDynamicBonesConfig != null)
			{
				if (!globalDynamicBonesConfig.GlobalDynamicBonesPermissions.ContainsKey(PlayerRankStatus.Local))
				{
					globalDynamicBonesConfig.GlobalDynamicBonesPermissions.Add(PlayerRankStatus.Local, new DynamicBonePermission());
				}
				if (!globalDynamicBonesConfig.GlobalDynamicBonesPermissions.ContainsKey(PlayerRankStatus.Visitor))
				{
					globalDynamicBonesConfig.GlobalDynamicBonesPermissions.Add(PlayerRankStatus.Visitor, new DynamicBonePermission());
				}
				if (!globalDynamicBonesConfig.GlobalDynamicBonesPermissions.ContainsKey(PlayerRankStatus.NewUser))
				{
					globalDynamicBonesConfig.GlobalDynamicBonesPermissions.Add(PlayerRankStatus.NewUser, new DynamicBonePermission());
				}
				if (!globalDynamicBonesConfig.GlobalDynamicBonesPermissions.ContainsKey(PlayerRankStatus.User))
				{
					globalDynamicBonesConfig.GlobalDynamicBonesPermissions.Add(PlayerRankStatus.User, new DynamicBonePermission());
				}
				if (!globalDynamicBonesConfig.GlobalDynamicBonesPermissions.ContainsKey(PlayerRankStatus.Known))
				{
					globalDynamicBonesConfig.GlobalDynamicBonesPermissions.Add(PlayerRankStatus.Known, new DynamicBonePermission());
				}
				if (!globalDynamicBonesConfig.GlobalDynamicBonesPermissions.ContainsKey(PlayerRankStatus.Trusted))
				{
					globalDynamicBonesConfig.GlobalDynamicBonesPermissions.Add(PlayerRankStatus.Trusted, new DynamicBonePermission());
				}
				if (!globalDynamicBonesConfig.GlobalDynamicBonesPermissions.ContainsKey(PlayerRankStatus.Friend))
				{
					globalDynamicBonesConfig.GlobalDynamicBonesPermissions.Add(PlayerRankStatus.Friend, new DynamicBonePermission());
				}
			}
		}
	}
}
