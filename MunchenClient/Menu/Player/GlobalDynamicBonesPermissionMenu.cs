using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Player
{
	internal class GlobalDynamicBonesPermissionMenu : QuickMenuNestedMenu
	{
		internal void OnConfigurationChanged()
		{
			if (Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBones)
			{
				PlayerUtils.ReloadAllAvatars();
			}
		}

		internal GlobalDynamicBonesPermissionMenu(QuickMenuButtonRow parent, PlayerRankStatus rank, string rankName)
			: base(parent, rankName, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankDescription.Replace("{rankName}", rankName))
		{
			GlobalDynamicBonesPermissionMenu globalDynamicBonesPermissionMenu = this;
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().EnableDynamicBones, Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].EnableDynamicBones, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].EnableDynamicBones = true;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().EnableDynamicBonesDescription, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].EnableDynamicBones = false;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().EnableDynamicBonesDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PerformanceMode, Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].PerformanceMode, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].PerformanceMode = true;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().PerformanceModeDescription, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].PerformanceMode = false;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().PerformanceModeDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().DisableAtDistance, Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].DistanceDisable, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].DistanceDisable = true;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().DisableAtDistanceDescription, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].DistanceDisable = false;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().DisableAtDistanceDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().VisibilityCheck, Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].VisibilityCheck, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].VisibilityCheck = true;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().VisibilityCheckDescription, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].VisibilityCheck = false;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().VisibilityCheckDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().OptimizeOnly, Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].OptimizeOnly, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].OptimizeOnly = true;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().OptimizeOnlyDescription, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[rank].OptimizeOnly = false;
				Configuration.SaveGlobalDynamicBonesConfig();
				globalDynamicBonesPermissionMenu.OnConfigurationChanged();
			}, LanguageManager.GetUsedLanguage().OptimizeOnlyDescription);
		}
	}
}
