using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Settings
{
	internal class DisableFeaturesMenu : QuickMenuNestedMenu
	{
		internal DisableFeaturesMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().DisableFeaturesMenuName, LanguageManager.GetUsedLanguage().DisableFeaturesMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().DisableAllKeybinds, Configuration.GetGeneralConfig().DisableAllKeybinds, delegate
			{
				Configuration.GetGeneralConfig().DisableAllKeybinds = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableAllKeybindsDescription, delegate
			{
				Configuration.GetGeneralConfig().DisableAllKeybinds = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableAllKeybindsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().DisableFBTSavingCompletely, Configuration.GetGeneralConfig().DisableFBTSavingCompletely, delegate
			{
				Configuration.GetGeneralConfig().DisableFBTSavingCompletely = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction("Notice", "This setting requires a restart in order to work - restart now?", "Restart", delegate
				{
					MainUtils.RestartGame();
				}, "Later", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().DisableFBTSavingCompletelyDescription, delegate
			{
				Configuration.GetGeneralConfig().DisableFBTSavingCompletely = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableFBTSavingCompletelyDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().DisableMenuPlayerButtons, Configuration.GetGeneralConfig().DisableMenuPlayerButtons, delegate
			{
				Configuration.GetGeneralConfig().DisableMenuPlayerButtons = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction("Notice", "This setting requires a restart in order to work - restart now?", "Restart", delegate
				{
					MainUtils.RestartGame();
				}, "Later", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().DisableMenuPlayerButtonsDescription, delegate
			{
				Configuration.GetGeneralConfig().DisableMenuPlayerButtons = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableMenuPlayerButtonsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().DisableKeyboardImprovements, Configuration.GetGeneralConfig().DisableKeyboardImprovements, delegate
			{
				Configuration.GetGeneralConfig().DisableKeyboardImprovements = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction("Notice", "This setting requires a restart in order to work - restart now?", "Restart", delegate
				{
					MainUtils.RestartGame();
				}, "Later", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().DisableKeyboardImprovementsDescription, delegate
			{
				Configuration.GetGeneralConfig().DisableKeyboardImprovements = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableKeyboardImprovementsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().DisableAvatarFavoritesSearchButton, Configuration.GetGeneralConfig().DisableAvatarFavoritesSearchButton, delegate
			{
				Configuration.GetGeneralConfig().DisableAvatarFavoritesSearchButton = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction("Notice", "This setting requires a restart in order to work - restart now?", "Restart", delegate
				{
					MainUtils.RestartGame();
				}, "Later", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().DisableAvatarFavoritesSearchButtonDescription, delegate
			{
				Configuration.GetGeneralConfig().DisableAvatarFavoritesSearchButton = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableAvatarFavoritesSearchButtonDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().DisableAvatarFavorites, Configuration.GetGeneralConfig().DisableAvatarFavorites, delegate
			{
				Configuration.GetGeneralConfig().DisableAvatarFavorites = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction("Notice", "This setting requires a restart in order to work - restart now?", "Restart", delegate
				{
					MainUtils.RestartGame();
				}, "Later", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().DisableAvatarFavoritesDescription, delegate
			{
				Configuration.GetGeneralConfig().DisableAvatarFavorites = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableAvatarFavoritesDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().DisableAvatarDatabase, Configuration.GetGeneralConfig().DisableAvatarDatabase, delegate
			{
				Configuration.GetGeneralConfig().DisableAvatarDatabase = true;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.AlertAction("Notice", "This setting requires a restart in order to work - restart now?", "Restart", delegate
				{
					MainUtils.RestartGame();
				}, "Later", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().DisableAvatarDatabaseDescription, delegate
			{
				Configuration.GetGeneralConfig().DisableAvatarDatabase = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DisableAvatarDatabaseDescription);
		}
	}
}
