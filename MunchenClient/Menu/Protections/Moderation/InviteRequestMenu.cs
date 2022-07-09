using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections.Moderation
{
	internal class InviteRequestMenu : QuickMenuNestedMenu
	{
		internal InviteRequestMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationRequestInviteMenuName, LanguageManager.GetUsedLanguage().ModerationRequestInviteMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogFriends, Configuration.GetModerationsConfig().LogModerationsRequestInvite, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsRequestInvite = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsRequestInvite = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogFriendsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ModerationLogToHud, Configuration.GetModerationsConfig().LogModerationsRequestInviteHUD, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsRequestInviteHUD = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsRequestInviteHUD = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationLogToHudDescription);
		}
	}
}
