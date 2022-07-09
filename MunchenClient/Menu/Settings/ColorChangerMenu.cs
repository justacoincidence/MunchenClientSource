using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.Menu.Settings
{
	internal class ColorChangerMenu : QuickMenuNestedMenu
	{
		internal ColorChangerMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ColorChangerMenuName, LanguageManager.GetUsedLanguage().ColorChangerMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ColorChangerMenuOptionsHeader);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ColorChanger, Configuration.GetGeneralConfig().ColorChangerEnable, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerEnable = true;
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerDescription, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerEnable = false;
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ColorChangerRainbowMode, Configuration.GetGeneralConfig().ColorChangerRainbowMode, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerRainbowMode = true;
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerRainbowModeDescription, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerRainbowMode = false;
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerRainbowModeDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ColorChangerHyperMode, Configuration.GetGeneralConfig().ColorChangerRainbowHyperMode, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerRainbowHyperMode = true;
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerRainbowModeDescription, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerRainbowHyperMode = false;
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerRainbowModeDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().ColorChangerCustom, delegate
			{
				GeneralWrappers.ShowInputPopup("Custom Menu Color", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, ParseCustomColorInput);
			}, LanguageManager.GetUsedLanguage().ColorChangerCustomDescription);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ColorChangerMenuColorsHeader);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().ColorChangerMagenta, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(Color.magenta);
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerMagentaDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().ColorChangerCyan, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(Color.cyan);
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerCyanDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().ColorChangerBlue, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(Color.blue);
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerBlueDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().ColorChangerRed, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(Color.red);
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerRedDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().ColorChangerGreen, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(Color.green);
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerGreenDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().ColorChangerYellow, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(Color.yellow);
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerYellowDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().ColorChangerOrange, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(new Color(255f, 165f, 0f));
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerOrangeDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().ColorChangerPink, delegate
			{
				Configuration.GetGeneralConfig().ColorChangerColor.SetColor(new Color(254f, 20f, 147f));
				Configuration.SaveGeneralConfig();
				MenuColorHandler.menuColorChanged = true;
			}, LanguageManager.GetUsedLanguage().ColorChangerPinkDescription);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ColorChangerPlayerWallhackHeader);
			QuickMenuButtonRow parentRow4 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow4, LanguageManager.GetUsedLanguage().PlayerWallhackSetColor, delegate
			{
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().PlayerWallhackSetColorQuestion, LanguageManager.GetUsedLanguage().PlayerWallhackSetColorFriends, delegate
				{
					GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().PlayerWallhackSetColorFriends, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetFriendsESPColor);
				}, LanguageManager.GetUsedLanguage().PlayerWallhackSetColorStrangers, delegate
				{
					GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().PlayerWallhackSetColorStrangers, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetStrangersESPColor);
				});
			}, LanguageManager.GetUsedLanguage().PlayerWallhackSetColorDescription);
			new QuickMenuToggleButton(parentRow4, LanguageManager.GetUsedLanguage().ColorChangerRGBFriendsWallhack, Configuration.GetGeneralConfig().PlayerWallhackRGBFriends, delegate
			{
				Configuration.GetGeneralConfig().PlayerWallhackRGBFriends = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ColorChangerRGBFriendsWallhackDescription, delegate
			{
				Configuration.GetGeneralConfig().PlayerWallhackRGBFriends = false;
				Configuration.SaveGeneralConfig();
				PlayerHandler.RefreshFriendsWallhackColors();
			}, LanguageManager.GetUsedLanguage().ColorChangerRGBFriendsWallhackDescription);
			new QuickMenuToggleButton(parentRow4, LanguageManager.GetUsedLanguage().ColorChangerRGBStrangersWallhack, Configuration.GetGeneralConfig().PlayerWallhackRGBStrangers, delegate
			{
				Configuration.GetGeneralConfig().PlayerWallhackRGBStrangers = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ColorChangerRGBStrangersWallhackDescription, delegate
			{
				Configuration.GetGeneralConfig().PlayerWallhackRGBStrangers = false;
				Configuration.SaveGeneralConfig();
				PlayerHandler.RefreshStrangersWallhackColors();
			}, LanguageManager.GetUsedLanguage().ColorChangerRGBStrangersWallhackDescription);
		}

		private void ParseCustomColorInput(string color, List<KeyCode> y, Text z)
		{
			if (!ColorUtility.TryParseHtmlString(color, out var color2))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, LanguageManager.GetUsedLanguage().ColorChangerFailedParsing);
				return;
			}
			Configuration.GetGeneralConfig().ColorChangerColor.SetColor(color2);
			Configuration.SaveGeneralConfig();
			MenuColorHandler.menuColorChanged = true;
		}

		private void SetFriendsESPColor(string color, List<KeyCode> pressedKeys, Text text)
		{
			if (!ColorUtility.TryParseHtmlString(color, out var color2))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, LanguageManager.GetUsedLanguage().ColorChangerFailedParsing);
				return;
			}
			Configuration.GetGeneralConfig().PlayerWallhackFriendsColor.SetColor(color2);
			Configuration.SaveGeneralConfig();
			PlayerHandler.RefreshFriendsWallhackColors();
			GeneralWrappers.ApplyAllPlayerWallhack(state: false);
			GeneralWrappers.ApplyAllPlayerWallhack(state: true);
		}

		private void SetStrangersESPColor(string color, List<KeyCode> pressedKeys, Text text)
		{
			if (!ColorUtility.TryParseHtmlString(color, out var color2))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, LanguageManager.GetUsedLanguage().ColorChangerFailedParsing);
				return;
			}
			Configuration.GetGeneralConfig().PlayerWallhackStrangersColor.SetColor(color2);
			Configuration.SaveGeneralConfig();
			PlayerHandler.RefreshStrangersWallhackColors();
			GeneralWrappers.ApplyAllPlayerWallhack(state: false);
			GeneralWrappers.ApplyAllPlayerWallhack(state: true);
		}
	}
}
