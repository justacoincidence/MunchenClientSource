using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class JoinMenu : QuickMenuNestedMenu
	{
		internal JoinMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationJoinMenuName, LanguageManager.GetUsedLanguage().ModerationJoinMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogFriends, Configuration.GetModerationsConfig().LogModerationsJoinFriends, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsJoinFriends = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsJoinFriends = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogOthers, Configuration.GetModerationsConfig().LogModerationsJoinOthers, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsJoinOthers = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogOthersDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsJoinOthers = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogOthersDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsJoinHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsJoinHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsJoinHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
