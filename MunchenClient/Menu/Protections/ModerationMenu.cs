using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu.Protections.Moderation;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Protections
{
	internal class ModerationMenu : QuickMenuNestedMenu
	{
		internal ModerationMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationMenuDescription)
		{
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parent2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parent3 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parent4 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parent5 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().VRChatStaffWarning, Configuration.GetModerationsConfig().LogModerationsWarnAboutModerators, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsWarnAboutModerators = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().EnableModerationLoggingDescription, delegate
			{
				Configuration.GetModerationsConfig().LogModerationsWarnAboutModerators = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().EnableModerationLoggingDescription);
			new QuickMenuToggleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().ModerationSounds, Configuration.GetModerationsConfig().ModerationSounds, delegate
			{
				Configuration.GetModerationsConfig().ModerationSounds = true;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationSoundsDescription, delegate
			{
				Configuration.GetModerationsConfig().ModerationSounds = false;
				Configuration.SaveModerationsConfig();
			}, LanguageManager.GetUsedLanguage().ModerationSoundsDescription);
			new JoinMenu(quickMenuButtonRow);
			new LeaveMenu(quickMenuButtonRow);
			new WarnMenu(parent2);
			new MicOffMenu(parent2);
			new MuteMenu(parent2);
			new BlockMenu(parent2);
			new InviteMenu(parent3);
			new InviteRequestMenu(parent3);
			new InviteRequestDeniedMenu(parent3);
			new FriendRequestMenu(parent3);
			new InstanceMasterMenu(parent4);
			new VotekickMenu(parent4);
			new OnlineMenu(parent4);
			new OfflineMenu(parent4);
			new InstanceSwitchMenu(parent5);
		}
	}
}
