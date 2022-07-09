using System.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.Rendering;

namespace MunchenClient.Menu.Settings
{
	internal class PerformanceMenu : QuickMenuNestedMenu
	{
		internal PerformanceMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().PerformanceMenuName, LanguageManager.GetUsedLanguage().PerformanceMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PerformanceLimitFPSToMonitor, Configuration.GetGeneralConfig().PerformanceUnlimitedFPS, delegate
			{
				Configuration.GetGeneralConfig().PerformanceUnlimitedFPS = true;
				Configuration.SaveGeneralConfig();
				if (!GeneralWrappers.IsInVR())
				{
					Application.targetFrameRate = GeneralUtils.GetRefreshRate();
				}
			}, LanguageManager.GetUsedLanguage().PerformanceLimitFPSToMonitorDescription.Replace("{RefreshRate}", GeneralUtils.GetRefreshRate().ToString()), delegate
			{
				Configuration.GetGeneralConfig().PerformanceUnlimitedFPS = false;
				Configuration.SaveGeneralConfig();
				if (!GeneralWrappers.IsInVR())
				{
					Application.targetFrameRate = GeneralUtils.targetedRefreshRate;
				}
			}, LanguageManager.GetUsedLanguage().PerformanceLimitFPSToMonitorDescription.Replace("{RefreshRate}", GeneralUtils.GetRefreshRate().ToString()));
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PerformanceHighPriority, Configuration.GetGeneralConfig().PerformanceHighPriority, delegate
			{
				Configuration.GetGeneralConfig().PerformanceHighPriority = true;
				Configuration.SaveGeneralConfig();
				GeneralUtils.ChangeGameProcessPriority(highPriority: true);
			}, LanguageManager.GetUsedLanguage().PerformanceHighPriorityDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceHighPriority = false;
				Configuration.SaveGeneralConfig();
				GeneralUtils.ChangeGameProcessPriority(highPriority: false);
			}, LanguageManager.GetUsedLanguage().PerformanceHighPriorityDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PerformanceImageCache, Configuration.GetGeneralConfig().PerformanceImageCache, delegate
			{
				Configuration.GetGeneralConfig().PerformanceImageCache = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PerformanceImageCacheDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceImageCache = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PerformanceImageCacheDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PerformanceLowGraphicsMode, Configuration.GetGeneralConfig().PerformanceLowGraphics, delegate
			{
				Configuration.GetGeneralConfig().PerformanceLowGraphics = true;
				Configuration.SaveGeneralConfig();
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				Graphics.activeTier = GraphicsTier.Tier1;
			}, LanguageManager.GetUsedLanguage().PerformanceLowGraphicsModeDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceLowGraphics = false;
				Configuration.SaveGeneralConfig();
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
				Graphics.activeTier = GraphicsTier.Tier3;
			}, LanguageManager.GetUsedLanguage().PerformanceLowGraphicsModeDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().PerformanceNoAA, Configuration.GetGeneralConfig().PerformanceNoAntiAliasing, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoAntiAliasing = true;
				Configuration.SaveGeneralConfig();
				QualitySettings.antiAliasing = 1;
			}, LanguageManager.GetUsedLanguage().PerformanceNoAADescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoAntiAliasing = false;
				Configuration.SaveGeneralConfig();
				QualitySettings.antiAliasing = 4;
			}, LanguageManager.GetUsedLanguage().PerformanceNoAADescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().PerformanceNoShadows, Configuration.GetGeneralConfig().PerformanceNoShadows, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoShadows = true;
				Configuration.SaveGeneralConfig();
				QualitySettings.shadows = ShadowQuality.Disable;
			}, LanguageManager.GetUsedLanguage().PerformanceNoShadowsDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoShadows = false;
				Configuration.SaveGeneralConfig();
				QualitySettings.shadows = ShadowQuality.All;
			}, LanguageManager.GetUsedLanguage().PerformanceNoShadowsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().PerformanceNoHT, Configuration.GetGeneralConfig().PerformanceNoHyperThreading, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoHyperThreading = true;
				Configuration.SaveGeneralConfig();
				GeneralUtils.ChangeGameCoreAffinity(skipHyperThreading: true);
			}, LanguageManager.GetUsedLanguage().PerformanceNoHTDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoHyperThreading = false;
				Configuration.SaveGeneralConfig();
				GeneralUtils.ChangeGameCoreAffinity(skipHyperThreading: false);
			}, LanguageManager.GetUsedLanguage().PerformanceNoHTDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().PerformanceNoPerfStats, Configuration.GetGeneralConfig().PerformanceNoPerformanceStats, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoPerformanceStats = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PerformanceNoPerfStatsDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceNoPerformanceStats = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PerformanceNoPerfStatsDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().AdvancedAvatarHider, Configuration.GetGeneralConfig().AdvancedAvatarHider, delegate
			{
				Configuration.GetGeneralConfig().AdvancedAvatarHider = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AdvancedAvatarHiderDescription, delegate
			{
				Configuration.GetGeneralConfig().AdvancedAvatarHider = false;
				Configuration.SaveGeneralConfig();
				foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
				{
					playerCaching.Value.GetAvatar().SetActive(value: true);
				}
			}, LanguageManager.GetUsedLanguage().AdvancedAvatarHiderDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().PerformanceSmartFPS, Configuration.GetGeneralConfig().PerformanceSmartFPS, delegate
			{
				Configuration.GetGeneralConfig().PerformanceSmartFPS = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PerformanceSmartFPSDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceSmartFPS = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PerformanceSmartFPSDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().PerformanceInputDelayReducer, Configuration.GetGeneralConfig().PerformanceInputDelayReducer, delegate
			{
				Configuration.GetGeneralConfig().PerformanceInputDelayReducer = true;
				Configuration.SaveGeneralConfig();
				QualitySettings.maxQueuedFrames = 1;
			}, LanguageManager.GetUsedLanguage().PerformanceInputDelayReducerDescription, delegate
			{
				Configuration.GetGeneralConfig().PerformanceInputDelayReducer = false;
				Configuration.SaveGeneralConfig();
				QualitySettings.maxQueuedFrames = 2;
			}, LanguageManager.GetUsedLanguage().PerformanceInputDelayReducerDescription);
		}
	}
}
