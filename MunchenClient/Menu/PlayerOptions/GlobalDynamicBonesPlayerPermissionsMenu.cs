using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.PlayerOptions
{
	internal class GlobalDynamicBonesPlayerPermissionsMenu : QuickMenuNestedMenu
	{
		private readonly QuickMenuToggleButton overrideButton;

		private readonly QuickMenuToggleButton enableBonesButton;

		private readonly QuickMenuToggleButton performanceButton;

		private readonly QuickMenuToggleButton distanceDisableButton;

		private readonly QuickMenuToggleButton visibilityCheckButton;

		private readonly QuickMenuToggleButton optimizeOnlyButton;

		internal void OnConfigurationChanged()
		{
			if (Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBones)
			{
				PlayerUtils.ReloadAllAvatars();
			}
		}

		private void PrepareUserInList()
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null && !Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides.ContainsKey(PlayerWrappers.GetSelectedPlayer().id))
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides.Add(PlayerWrappers.GetSelectedPlayer().id, new DynamicBonePermissionOverride());
			}
		}

		private void ChangeOverrideState(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList();
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].EnableOverride = state;
				Configuration.SaveGlobalDynamicBonesConfig();
				OnConfigurationChanged();
			}
		}

		private void ChangeEnableState(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList();
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].EnableDynamicBones = state;
				Configuration.SaveGlobalDynamicBonesConfig();
				OnConfigurationChanged();
			}
		}

		private void ChangePerformanceState(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList();
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].PerformanceMode = state;
				Configuration.SaveGlobalDynamicBonesConfig();
				OnConfigurationChanged();
			}
		}

		private void ChangeDistanceDisableState(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList();
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].DistanceDisable = state;
				Configuration.SaveGlobalDynamicBonesConfig();
				OnConfigurationChanged();
			}
		}

		private void ChangeVisibilityCheckState(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList();
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].VisibilityCheck = state;
				Configuration.SaveGlobalDynamicBonesConfig();
				OnConfigurationChanged();
			}
		}

		private void ChangeOptimizeState(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList();
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].OptimizeOnly = state;
				Configuration.SaveGlobalDynamicBonesConfig();
				OnConfigurationChanged();
			}
		}

		internal override void OnMenuShownCallback()
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				if (Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides.ContainsKey(PlayerWrappers.GetSelectedPlayer().id))
				{
					overrideButton.SetToggleState(Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].EnableOverride);
					enableBonesButton.SetToggleState(Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].EnableDynamicBones);
					performanceButton.SetToggleState(Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].PerformanceMode);
					distanceDisableButton.SetToggleState(Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].DistanceDisable);
					visibilityCheckButton.SetToggleState(Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].VisibilityCheck);
					optimizeOnlyButton.SetToggleState(Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[PlayerWrappers.GetSelectedPlayer().id].OptimizeOnly);
				}
				else
				{
					overrideButton.SetToggleState(state: false);
					enableBonesButton.SetToggleState(state: false);
					performanceButton.SetToggleState(state: false);
					distanceDisableButton.SetToggleState(state: false);
					visibilityCheckButton.SetToggleState(state: false);
					optimizeOnlyButton.SetToggleState(state: false);
				}
			}
		}

		internal GlobalDynamicBonesPlayerPermissionsMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().GlobalDynamicBonesPlayerMenuName, LanguageManager.GetUsedLanguage().GlobalDynamicBonesPlayerMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			overrideButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().EnableOverride, state: false, delegate
			{
				ChangeOverrideState(state: true);
			}, LanguageManager.GetUsedLanguage().EnableOverrideDescription, delegate
			{
				ChangeOverrideState(state: false);
			}, LanguageManager.GetUsedLanguage().EnableOverrideDescription);
			enableBonesButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().EnableDynamicBones, state: false, delegate
			{
				ChangeEnableState(state: true);
			}, LanguageManager.GetUsedLanguage().EnableDynamicBonesDescription, delegate
			{
				ChangeEnableState(state: false);
			}, LanguageManager.GetUsedLanguage().EnableDynamicBonesDescription);
			performanceButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PerformanceMode, state: false, delegate
			{
				ChangePerformanceState(state: true);
			}, LanguageManager.GetUsedLanguage().PerformanceModeDescription, delegate
			{
				ChangePerformanceState(state: false);
			}, LanguageManager.GetUsedLanguage().PerformanceModeDescription);
			distanceDisableButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().DisableAtDistance, state: false, delegate
			{
				ChangeDistanceDisableState(state: true);
			}, LanguageManager.GetUsedLanguage().DisableAtDistanceDescription, delegate
			{
				ChangeDistanceDisableState(state: false);
			}, LanguageManager.GetUsedLanguage().DisableAtDistanceDescription);
			visibilityCheckButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().VisibilityCheck, state: false, delegate
			{
				ChangeVisibilityCheckState(state: true);
			}, LanguageManager.GetUsedLanguage().VisibilityCheckDescription, delegate
			{
				ChangeVisibilityCheckState(state: false);
			}, LanguageManager.GetUsedLanguage().VisibilityCheckDescription);
			optimizeOnlyButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().OptimizeOnly, state: false, delegate
			{
				ChangeOptimizeState(state: true);
			}, LanguageManager.GetUsedLanguage().OptimizeOnlyDescription, delegate
			{
				ChangeOptimizeState(state: false);
			}, LanguageManager.GetUsedLanguage().OptimizeOnlyDescription);
		}
	}
}
