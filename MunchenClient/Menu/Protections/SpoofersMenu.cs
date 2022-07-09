using System;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Patching.Patches;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.Menu.Protections
{
	internal class SpoofersMenu : QuickMenuNestedMenu
	{
		internal SpoofersMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().SpooferMenuName, LanguageManager.GetUsedLanguage().SpooferMenuDescription)
		{
			new QuickMenuHeader(this, "Miscellaneous");
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AutoRegenerate, Configuration.GetGeneralConfig().SpooferAutoRegenerate, delegate
			{
				Configuration.GetGeneralConfig().SpooferAutoRegenerate = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoRegenerateDescription, delegate
			{
				Configuration.GetGeneralConfig().SpooferAutoRegenerate = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoRegenerateDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().RegenerateHWID, delegate
			{
				GeneralWrappers.AlertAction("Notice", LanguageManager.GetUsedLanguage().RegenerateHWIDNotice, LanguageManager.GetUsedLanguage().RegenerateHWIDText, delegate
				{
					UnityEnginePatch.GenerateHardwareIdentifier(GeneralUtils.fastRandom);
					GeneralWrappers.ClosePopup();
				}, LanguageManager.GetUsedLanguage().CancelText, delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().RegenerateHWIDDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().SetSpoofedFPS, delegate
			{
				GeneralWrappers.ShowInputPopup("Custom Spoofed FPS", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetSpooferCustomFPS, null, "Leave blank to reset...");
			}, LanguageManager.GetUsedLanguage().SetSpoofedFPSDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().SetSpoofedPing, delegate
			{
				GeneralWrappers.ShowInputPopup("Custom Spoofed Ping", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetSpooferCustomPing, null, "Leave blank to reset...");
			}, LanguageManager.GetUsedLanguage().SetSpoofedPingDescription);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, "Spoofers");
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().SpooferRealisticMode, Configuration.GetGeneralConfig().SpooferRealisticMode, delegate
			{
				Configuration.GetGeneralConfig().SpooferRealisticMode = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().SpooferRealisticModeDescription, delegate
			{
				Configuration.GetGeneralConfig().SpooferRealisticMode = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().SpooferRealisticModeDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().HWIDSpoofer, Configuration.GetGeneralConfig().SpooferHWID, delegate
			{
				Configuration.GetGeneralConfig().SpooferHWID = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().HWIDSpooferDescription, delegate
			{
				Configuration.GetGeneralConfig().SpooferHWID = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().HWIDSpooferDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().SteamIDSpoofer, Configuration.GetGeneralConfig().SpooferSteamID, delegate
			{
				Configuration.GetGeneralConfig().SpooferSteamID = true;
				Configuration.SaveGeneralConfig();
				if (!SteamworksPatch.IsSteamPatched())
				{
					GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().FeatureRequiresRestartText, LanguageManager.GetUsedLanguage().RestartText, delegate
					{
						MainUtils.RestartGame();
					}, LanguageManager.GetUsedLanguage().RestartLaterText, delegate
					{
						GeneralWrappers.ClosePopup();
					});
				}
			}, LanguageManager.GetUsedLanguage().SteamIDSpooferDescription, delegate
			{
				Configuration.GetGeneralConfig().SpooferSteamID = false;
				Configuration.SaveGeneralConfig();
				if (SteamworksPatch.IsSteamPatched())
				{
					GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().FeatureRequiresRestartText, LanguageManager.GetUsedLanguage().RestartText, delegate
					{
						MainUtils.RestartGame();
					}, LanguageManager.GetUsedLanguage().RestartLaterText, delegate
					{
						GeneralWrappers.ClosePopup();
					});
				}
			}, LanguageManager.GetUsedLanguage().SteamIDSpooferDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().PeripheralSpoofer, Configuration.GetGeneralConfig().SpooferPeripheral, delegate
			{
				Configuration.GetGeneralConfig().SpooferPeripheral = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PeripheralSpooferDescription, delegate
			{
				Configuration.GetGeneralConfig().SpooferPeripheral = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PeripheralSpooferDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().PingSpoofer, Configuration.GetGeneralConfig().SpooferPing, delegate
			{
				Configuration.GetGeneralConfig().SpooferPing = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PingSpooferDescription, delegate
			{
				Configuration.GetGeneralConfig().SpooferPing = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PingSpooferDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().FPSSpoofer, Configuration.GetGeneralConfig().SpooferFPS, delegate
			{
				Configuration.GetGeneralConfig().SpooferFPS = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().FPSSpooferDescription, delegate
			{
				Configuration.GetGeneralConfig().SpooferFPS = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().FPSSpooferDescription);
		}

		private void SetSpooferCustomFPS(string fpsText, List<KeyCode> pressedKeys, Text text)
		{
			string text2 = fpsText.Trim();
			int result;
			if (string.IsNullOrEmpty(text2))
			{
				Configuration.GetGeneralConfig().SpooferFPSCustom = -1;
				Configuration.SaveGeneralConfig();
				ConsoleUtils.Info("Spoofer", "Succesfully reset FPS", ConsoleColor.Gray, "SetSpooferCustomFPS", 186);
				GeneralWrappers.AlertPopup("Spoofer", "Succesfully reset FPS");
			}
			else if (!int.TryParse(text2, out result))
			{
				ConsoleUtils.Info("Spoofer", "Invalid FPS", ConsoleColor.Gray, "SetSpooferCustomFPS", 194);
				GeneralWrappers.AlertPopup("Spoofer", "Invalid FPS");
			}
			else
			{
				Configuration.GetGeneralConfig().SpooferFPSCustom = result;
				Configuration.SaveGeneralConfig();
				ConsoleUtils.Info("Spoofer", "Succesfully set FPS", ConsoleColor.Gray, "SetSpooferCustomFPS", 203);
				GeneralWrappers.AlertPopup("Spoofer", "Succesfully set FPS");
			}
		}

		private void SetSpooferCustomPing(string pingText, List<KeyCode> pressedKeys, Text text)
		{
			string text2 = pingText.Trim();
			int result;
			if (string.IsNullOrEmpty(text2))
			{
				Configuration.GetGeneralConfig().SpooferPingCustom = -1;
				Configuration.SaveGeneralConfig();
				ConsoleUtils.Info("Spoofer", "Succesfully reset Ping", ConsoleColor.Gray, "SetSpooferCustomPing", 216);
				GeneralWrappers.AlertPopup("Spoofer", "Succesfully reset Ping");
			}
			else if (!int.TryParse(text2, out result))
			{
				ConsoleUtils.Info("Spoofer", "Invalid Ping", ConsoleColor.Gray, "SetSpooferCustomPing", 224);
				GeneralWrappers.AlertPopup("Spoofer", "Invalid Ping");
			}
			else
			{
				Configuration.GetGeneralConfig().SpooferPingCustom = result;
				Configuration.SaveGeneralConfig();
				ConsoleUtils.Info("Spoofer", "Succesfully set Ping", ConsoleColor.Gray, "SetSpooferCustomPing", 233);
				GeneralWrappers.AlertPopup("Spoofer", "Succesfully set Ping");
			}
		}
	}
}
