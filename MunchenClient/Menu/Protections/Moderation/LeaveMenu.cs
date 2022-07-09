using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class LeaveMenu : QuickMenuNestedMenu
	{
		internal LeaveMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationLeaveMenuName, LanguageManager.GetUsedLanguage().ModerationLeaveMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogFriends, Configuration.GetModerationsConfig().LogModerationsLeftFriends, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsLeftFriends = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsLeftFriends = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogOthers, Configuration.GetModerationsConfig().LogModerationsLeftOthers, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsLeftOthers = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogOthersDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsLeftOthers = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogOthersDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsLeftHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsLeftHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsLeftHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
