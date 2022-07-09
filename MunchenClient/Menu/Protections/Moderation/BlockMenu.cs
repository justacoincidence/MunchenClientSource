using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class BlockMenu : QuickMenuNestedMenu
	{
		internal BlockMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationBlockMenuName, LanguageManager.GetUsedLanguage().ModerationBlockMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationBlockPrevent, Configuration.GetModerationsConfig().LogModerationsBlockPrevent, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsBlockPrevent = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationBlockPreventDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsBlockPrevent = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationBlockPreventDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationBlockPreventAvatarChange, Configuration.GetModerationsConfig().ModerationBlockPreventAvatarChange, delegate
			{
				Configuration.GetModerationsConfig().ModerationBlockPreventAvatarChange = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationBlockPreventAvatarChangeDescription, delegate
			{
				Configuration.GetModerationsConfig().ModerationBlockPreventAvatarChange = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationBlockPreventAvatarChangeDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToConsole, Configuration.GetModerationsConfig().LogModerationsBlockLog, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsBlockLog = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsBlockLog = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsBlockHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsBlockHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsBlockHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
