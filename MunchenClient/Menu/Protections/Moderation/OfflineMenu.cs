using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class OfflineMenu : QuickMenuNestedMenu
	{
		internal OfflineMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationOfflineMenuName, LanguageManager.GetUsedLanguage().ModerationOfflineMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToConsole, Configuration.GetModerationsConfig().LogModerationsOffline, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsOffline = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsOffline = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsOfflineHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsOfflineHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsOfflineHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
