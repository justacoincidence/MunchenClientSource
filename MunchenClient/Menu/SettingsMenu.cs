using System;
using System.Windows.Forms;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu.Settings;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using ServerAPI.Core;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace MunchenClient.Menu
{
	internal class SettingsMenu : QuickMenuNestedMenu
	{
		private readonly QuickMenuSliderButton aspectRatioSlider;

		internal SettingsMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().SettingsMenuName, LanguageManager.GetUsedLanguage().SettingsMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			new PerformanceMenu(quickMenuButtonRow);
			new ColorChangerMenu(quickMenuButtonRow);
			new DisableFeaturesMenu(quickMenuButtonRow);
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().RestartGame, delegate
			{
				MainUtils.RestartGame();
			}, LanguageManager.GetUsedLanguage().RestartGameDescription);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().ReloadAvatars, delegate
			{
				PlayerUtils.ReloadAllAvatars();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().ReloadAvatarsReloaded);
			}, LanguageManager.GetUsedLanguage().ReloadAvatarsDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().CopyAvatarData, delegate
			{
				PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
				if (localPlayerInformation != null)
				{
					ApiAvatar apiAvatar = localPlayerInformation.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2;
					Clipboard.SetText("ID: " + apiAvatar.id + "\nName: " + apiAvatar.name + "\nStatus: " + apiAvatar.releaseStatus + "\nAsset Url: " + apiAvatar.assetUrl + "\nAuthor Name: " + apiAvatar.authorName + "\nAuthor ID: " + apiAvatar.authorId + "\nDescription: " + apiAvatar.description + "\nPreview: " + apiAvatar.imageUrl);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().CopyAvatarDataCopied, ConsoleColor.Gray, ".ctor", 67);
					QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().CopyAvatarDataCopied);
				}
			}, LanguageManager.GetUsedLanguage().CopyAvatarDataDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().MinimumCameraClippingDistance, Configuration.GetGeneralConfig().MinimumCameraClippingDistance, delegate
			{
				Configuration.GetGeneralConfig().MinimumCameraClippingDistance = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().MinimumCameraClippingDistanceDescription, delegate
			{
				Configuration.GetGeneralConfig().MinimumCameraClippingDistance = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().MinimumCameraClippingDistanceDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().LocalizedClient, Configuration.GetGeneralConfig().LocalizedClient, delegate
			{
				Configuration.GetGeneralConfig().LocalizedClient = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().FeatureRequiresRestartText, LanguageManager.GetUsedLanguage().RestartText, delegate
				{
					MainUtils.RestartGame();
				}, LanguageManager.GetUsedLanguage().RestartLaterText, delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().LocalizedClientDescription, delegate
			{
				Configuration.GetGeneralConfig().LocalizedClient = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().LocalizedClientDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().LocalizedClientCustom, Configuration.GetGeneralConfig().LocalizedClientCustom, delegate
			{
				Configuration.GetGeneralConfig().LocalizedClientCustom = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().FeatureRequiresRestartText, LanguageManager.GetUsedLanguage().RestartText, delegate
				{
					MainUtils.RestartGame();
				}, LanguageManager.GetUsedLanguage().RestartLaterText, delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().LocalizedClientCustomDescription, delegate
			{
				Configuration.GetGeneralConfig().LocalizedClientCustom = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().LocalizedClientCustomDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AutoClearCache, Configuration.GetGeneralConfig().AutoClearCache, delegate
			{
				Configuration.GetGeneralConfig().AutoClearCache = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoClearCacheDescription, delegate
			{
				Configuration.GetGeneralConfig().AutoClearCache = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoClearCacheDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().SetCustomCrasher, delegate
			{
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().SetCustomCrasherQuestion, LanguageManager.GetUsedLanguage().PCText, delegate
				{
					GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().SetCustomCrasherPC, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetCrasherPC, null, LanguageManager.GetUsedLanguage().SetCustomCrasherPlaceholderText);
				}, LanguageManager.GetUsedLanguage().QuestText, delegate
				{
					GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().SetCustomCrasherQuest, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetCrasherQuest, null, LanguageManager.GetUsedLanguage().SetCustomCrasherPlaceholderText);
				});
			}, LanguageManager.GetUsedLanguage().SetCustomCrasherDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AvatarDownloadLogging, Configuration.GetGeneralConfig().AvatarDownloadLogging, delegate
			{
				Configuration.GetGeneralConfig().AvatarDownloadLogging = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AvatarDownloadLoggingDescription, delegate
			{
				Configuration.GetGeneralConfig().AvatarDownloadLogging = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AvatarDownloadLoggingDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().ComfyVRMenu, Configuration.GetGeneralConfig().ComfyVRMenu, delegate
			{
				Configuration.GetGeneralConfig().ComfyVRMenu = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ComfyVRMenuDescription, delegate
			{
				Configuration.GetGeneralConfig().ComfyVRMenu = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ComfyVRMenuDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().PersistentQuickMenu, Configuration.GetGeneralConfig().PersistentQuickMenu, delegate
			{
				Configuration.GetGeneralConfig().PersistentQuickMenu = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PersistentQuickMenuDescription, delegate
			{
				Configuration.GetGeneralConfig().PersistentQuickMenu = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PersistentQuickMenuDescription);
			ServerAPICore instance = ServerAPICore.GetInstance();
			if (instance != null && !instance.IsDebugMode())
			{
				new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().ForceServerSync, delegate
				{
					ServerAPICore.GetInstance().ForceUpdateFromServer();
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, LanguageManager.GetUsedLanguage().ForceServerSyncClicked, ConsoleColor.Gray, ".ctor", 184);
					QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().ForceServerSyncClicked);
				}, LanguageManager.GetUsedLanguage().ForceServerSyncDescription);
			}
			if (!GeneralWrappers.IsInVR())
			{
				new QuickMenuSpacers(this);
				new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().AspectRatioCategory);
				aspectRatioSlider = new QuickMenuSliderButton(this, "Aspect Ratio", 0.5f, 1.5f, 1f, delegate(float value)
				{
					GeneralWrappers.GetPlayerCamera().ResetAspect();
					GeneralWrappers.GetUICamera().ResetAspect();
					CameraFeaturesHandler.GetFrontCamera().ResetAspect();
					CameraFeaturesHandler.GetBackCamera().ResetAspect();
					CameraFeaturesHandler.GetFreezeCamera().ResetAspect();
					float aspect = GeneralWrappers.GetPlayerCamera().aspect * value;
					GeneralWrappers.GetPlayerCamera().aspect = aspect;
					GeneralWrappers.GetUICamera().aspect = aspect;
					CameraFeaturesHandler.GetFrontCamera().aspect = aspect;
					CameraFeaturesHandler.GetBackCamera().aspect = aspect;
					CameraFeaturesHandler.GetFreezeCamera().aspect = aspect;
					aspectRatioSlider.SetSecondaryButtonText($"{(int)(value * 100f)}%");
				}, "100%", "Changes the stretchness of your game");
			}
		}

		private void SetCrasherPC(string crasherId, List<KeyCode> pressedKeys, Text text)
		{
			string crasherIdTrimmed = crasherId.Trim();
			if (string.IsNullOrEmpty(crasherIdTrimmed))
			{
				Configuration.GetGeneralConfig().CrasherPC = string.Empty;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessResetPC);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().CrasherCategory, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessResetPC, ConsoleColor.Gray, "SetCrasherPC", 226);
				return;
			}
			PlayerUtils.IsAvatarValid(crasherIdTrimmed, delegate
			{
				Configuration.GetGeneralConfig().CrasherPC = crasherIdTrimmed;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessSetPC);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().CrasherCategory, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessSetPC, ConsoleColor.Gray, "SetCrasherPC", 237);
			}, delegate(string error)
			{
				string text2 = LanguageManager.GetUsedLanguage().SetCustomCrasherFailedPC.Replace("{error}", error);
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, text2);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, text2, ConsoleColor.Gray, "SetCrasherPC", 243);
			});
		}

		private void SetCrasherQuest(string crasherId, List<KeyCode> pressedKeys, Text text)
		{
			string crasherIdTrimmed = crasherId.Trim();
			if (string.IsNullOrEmpty(crasherIdTrimmed))
			{
				Configuration.GetGeneralConfig().CrasherQuest = string.Empty;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessResetQuest);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().CrasherCategory, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessResetQuest, ConsoleColor.Gray, "SetCrasherQuest", 257);
				return;
			}
			PlayerUtils.IsAvatarValid(crasherIdTrimmed, delegate
			{
				Configuration.GetGeneralConfig().CrasherQuest = crasherIdTrimmed;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessSetQuest);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().CrasherCategory, LanguageManager.GetUsedLanguage().SetCustomCrasherSuccessSetQuest, ConsoleColor.Gray, "SetCrasherQuest", 268);
			}, delegate(string error)
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, LanguageManager.GetUsedLanguage().SetCustomCrasherFailedQuest.Replace("{error}", error), ConsoleColor.Gray, "SetCrasherQuest", 271);
			});
		}
	}
}
