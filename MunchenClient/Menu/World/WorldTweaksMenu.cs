using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.World
{
	internal class WorldTweaksMenu : QuickMenuNestedMenu
	{
		internal WorldTweaksMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().WorldTweaksMenuName, LanguageManager.GetUsedLanguage().WorldTweaksMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().TheGreatPugRemoveUnnecessaryDoors, Configuration.GetGeneralConfig().TheGreatPugRemoveUnnecessaryDoors, delegate
			{
				Configuration.GetGeneralConfig().TheGreatPugRemoveUnnecessaryDoors = true;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixUnnecessaryDoorsInTheGreatPug(enableFix: true);
			}, LanguageManager.GetUsedLanguage().TheGreatPugRemoveUnnecessaryDoorsDescription, delegate
			{
				Configuration.GetGeneralConfig().TheGreatPugRemoveUnnecessaryDoors = false;
				Configuration.SaveGeneralConfig();
				WorldUtils.FixUnnecessaryDoorsInTheGreatPug(enableFix: false);
			}, LanguageManager.GetUsedLanguage().TheGreatPugRemoveUnnecessaryDoorsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().JustBClubDoorBypass, Configuration.GetGeneralConfig().JustBClubDoorBypass, delegate
			{
				Configuration.GetGeneralConfig().JustBClubDoorBypass = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JustBClubDoorBypassDescription, delegate
			{
				Configuration.GetGeneralConfig().JustBClubDoorBypass = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JustBClubDoorBypassDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().JustBClubIntroFix, Configuration.GetGeneralConfig().JustBClubIntroFix, delegate
			{
				Configuration.GetGeneralConfig().JustBClubIntroFix = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JustBClubIntroFixDescription, delegate
			{
				Configuration.GetGeneralConfig().JustBClubIntroFix = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JustBClubIntroFixDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().JustBClubVIPBypass, Configuration.GetGeneralConfig().JustBClubVIPBypass, delegate
			{
				Configuration.GetGeneralConfig().JustBClubVIPBypass = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JustBClubVIPBypassDescription, delegate
			{
				Configuration.GetGeneralConfig().JustBClubVIPBypass = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JustBClubVIPBypassDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().FBTHeavenDoorBypass, Configuration.GetGeneralConfig().FBTHeavenDoorBypass, delegate
			{
				Configuration.GetGeneralConfig().FBTHeavenDoorBypass = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().FBTHeavenDoorBypassDescription, delegate
			{
				Configuration.GetGeneralConfig().FBTHeavenDoorBypass = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().FBTHeavenDoorBypassDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().VoidClubAnnoyingEntryDoorFix, Configuration.GetGeneralConfig().VoidClubAnnoyingEntryDoorFix, delegate
			{
				Configuration.GetGeneralConfig().VoidClubAnnoyingEntryDoorFix = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().VoidClubAnnoyingEntryDoorFixDescription, delegate
			{
				Configuration.GetGeneralConfig().VoidClubAnnoyingEntryDoorFix = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().VoidClubAnnoyingEntryDoorFixDescription);
		}
	}
}
