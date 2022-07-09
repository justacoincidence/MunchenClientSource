using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Player
{
	internal class GlobalDynamicBonesMenu : QuickMenuNestedMenu
	{
		internal GlobalDynamicBonesMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().GlobalDynamicBonesMenuName, LanguageManager.GetUsedLanguage().GlobalDynamicBonesMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().GlobalDynamicBonesEnable, Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBones, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBones = true;
				Configuration.SaveGlobalDynamicBonesConfig();
				PlayerUtils.ReloadAllAvatars();
			}, LanguageManager.GetUsedLanguage().GlobalDynamicBonesEnableDescription, delegate
			{
				Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBones = false;
				Configuration.SaveGlobalDynamicBonesConfig();
				PlayerUtils.ReloadAllAvatars();
			}, LanguageManager.GetUsedLanguage().GlobalDynamicBonesEnableDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().GlobalDynamicBonesReset, delegate
			{
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().ResetText, LanguageManager.GetUsedLanguage().GlobalDynamicBonesResetConfirmation, LanguageManager.GetUsedLanguage().ResetText, delegate
				{
					Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides.Clear();
					Configuration.SaveGlobalDynamicBonesConfig();
				}, LanguageManager.GetUsedLanguage().CancelText, delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().GlobalDynamicBonesResetDescription);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().RanksCategory);
			QuickMenuButtonRow parent2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parent3 = new QuickMenuButtonRow(this);
			new GlobalDynamicBonesPermissionMenu(parent2, PlayerRankStatus.Local, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankLocalPlayer);
			new GlobalDynamicBonesPermissionMenu(parent2, PlayerRankStatus.Friend, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankFriends);
			new GlobalDynamicBonesPermissionMenu(parent2, PlayerRankStatus.Trusted, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankTrusted);
			new GlobalDynamicBonesPermissionMenu(parent2, PlayerRankStatus.Known, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankKnown);
			new GlobalDynamicBonesPermissionMenu(parent3, PlayerRankStatus.User, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankUser);
			new GlobalDynamicBonesPermissionMenu(parent3, PlayerRankStatus.NewUser, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankNewUser);
			new GlobalDynamicBonesPermissionMenu(parent3, PlayerRankStatus.Visitor, LanguageManager.GetUsedLanguage().GlobalDynamicBonesRankVisitor);
		}
	}
}
