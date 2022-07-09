using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class MicOffMenu : QuickMenuNestedMenu
	{
		internal MicOffMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationMicOffMenuName, LanguageManager.GetUsedLanguage().ModerationMicOffMenuNameDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationMicOffPrevent, Configuration.GetModerationsConfig().LogModerationsMicOffPrevent, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsMicOffPrevent = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationMicOffPreventDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsMicOffPrevent = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationMicOffPreventDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToConsole, Configuration.GetModerationsConfig().LogModerationsMicOffLog, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsMicOffLog = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsMicOffLog = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsMicOffHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsMicOffHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsMicOffHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
