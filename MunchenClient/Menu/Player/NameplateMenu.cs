using System.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Player
{
	internal class NameplateMenu : QuickMenuNestedMenu
	{
		internal NameplateMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().NameplateMenuName, LanguageManager.GetUsedLanguage().NameplateMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().NameplatesCategory);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().CustomRanks, Configuration.GetGeneralConfig().RanksCustomRanks, delegate
			{
				Configuration.GetGeneralConfig().RanksCustomRanks = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().CustomRanksDescription, delegate
			{
				Configuration.GetGeneralConfig().RanksCustomRanks = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().CustomRanksDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().NameplateWallhack, Configuration.GetGeneralConfig().NameplateWallhack, delegate
			{
				Configuration.GetGeneralConfig().NameplateWallhack = true;
				Configuration.SaveGeneralConfig();
				GeneralUtils.SetNameplateWallhack(state: true);
			}, LanguageManager.GetUsedLanguage().NameplateWallhackDescription, delegate
			{
				Configuration.GetGeneralConfig().NameplateWallhack = false;
				Configuration.SaveGeneralConfig();
				GeneralUtils.SetNameplateWallhack(state: false);
			}, LanguageManager.GetUsedLanguage().NameplateWallhackDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().NameplateMoreInfo, Configuration.GetGeneralConfig().NameplateMoreInfo, delegate
			{
				Configuration.GetGeneralConfig().NameplateMoreInfo = true;
				Configuration.SaveGeneralConfig();
				foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
				{
					if (!playerCaching.Value.isLocalPlayer)
					{
						playerCaching.Value.customNameplateObject.SetActive(value: true);
					}
				}
			}, LanguageManager.GetUsedLanguage().NameplateMoreInfoDescription, delegate
			{
				Configuration.GetGeneralConfig().NameplateMoreInfo = false;
				Configuration.SaveGeneralConfig();
				foreach (KeyValuePair<string, PlayerInformation> playerCaching2 in PlayerUtils.playerCachingList)
				{
					if (!playerCaching2.Value.isLocalPlayer)
					{
						playerCaching2.Value.customNameplateObject.SetActive(value: false);
					}
				}
			}, LanguageManager.GetUsedLanguage().NameplateMoreInfoDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().NameplateRankColor, Configuration.GetGeneralConfig().NameplateRankColor, delegate
			{
				Configuration.GetGeneralConfig().NameplateRankColor = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().NameplateRankColorDescription, delegate
			{
				Configuration.GetGeneralConfig().NameplateRankColor = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().NameplateRankColorDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().ShowInstanceOwner, Configuration.GetGeneralConfig().ShowInstanceOwner, delegate
			{
				Configuration.GetGeneralConfig().ShowInstanceOwner = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ShowInstanceOwnerDescription, delegate
			{
				Configuration.GetGeneralConfig().ShowInstanceOwner = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ShowInstanceOwnerDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().ShowActorID, Configuration.GetGeneralConfig().ShowActorID, delegate
			{
				Configuration.GetGeneralConfig().ShowActorID = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ShowActorIDDescription, delegate
			{
				Configuration.GetGeneralConfig().ShowActorID = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ShowActorIDDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().ShowAvatarStatus, Configuration.GetGeneralConfig().ShowAvatarStatus, delegate
			{
				Configuration.GetGeneralConfig().ShowAvatarStatus = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ShowAvatarStatusDescription, delegate
			{
				Configuration.GetGeneralConfig().ShowAvatarStatus = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().ShowAvatarStatusDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().DetectLagOrCrash, Configuration.GetGeneralConfig().DetectLagOrCrash, delegate
			{
				Configuration.GetGeneralConfig().DetectLagOrCrash = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DetectLagOrCrashDescription, delegate
			{
				Configuration.GetGeneralConfig().DetectLagOrCrash = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DetectLagOrCrashDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().DetectClientUser, Configuration.GetGeneralConfig().DetectClientUser, delegate
			{
				Configuration.GetGeneralConfig().DetectClientUser = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DetectClientUserDescription, delegate
			{
				Configuration.GetGeneralConfig().DetectClientUser = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DetectClientUserDescription);
		}
	}
}
