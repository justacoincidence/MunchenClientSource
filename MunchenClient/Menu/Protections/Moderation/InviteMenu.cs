using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class InviteMenu : QuickMenuNestedMenu
	{
		internal InviteMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationInviteMenuName, LanguageManager.GetUsedLanguage().ModerationInviteMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogFriends, Configuration.GetModerationsConfig().LogModerationsInvite, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInvite = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInvite = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsInviteHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInviteHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsInviteHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
