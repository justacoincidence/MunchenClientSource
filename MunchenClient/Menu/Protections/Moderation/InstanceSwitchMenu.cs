using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class InstanceSwitchMenu : QuickMenuNestedMenu
	{
		internal InstanceSwitchMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationInstanceSwitchMenuName, LanguageManager.GetUsedLanguage().ModerationInstanceSwitchMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToConsole, Configuration.GetModerationsConfig().LogModerationsInstanceSwitch, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceSwitch = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceSwitch = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToConsoleDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsInstanceSwitchHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceSwitchHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceSwitchHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
