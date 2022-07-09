using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class InstanceMasterMenu : QuickMenuNestedMenu
	{
		internal InstanceMasterMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationInstanceMasterMenuName, LanguageManager.GetUsedLanguage().ModerationInstanceMasterMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogFriends, Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeFriends, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeFriends = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeFriends = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogOthers, Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeOthers, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeOthers = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogOthersDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeOthers = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogOthersDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
