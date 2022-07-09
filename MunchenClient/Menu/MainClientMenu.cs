using System;
using MunchenClient.Core;
using MunchenClient.Misc;
using MunchenClient.Utils;
using UnchainedButtonAPI;

namespace MunchenClient.Menu
{
	internal class MainClientMenu : QuickMenuNestedMenu
	{
		internal static QuickMenuHeader userProfileHeader;

		internal static QuickMenuIndicator userProfilePicture;

		internal static QuickMenuIndicator userProfileRank;

		internal static QuickMenuIndicator userProfileSubExpiry;

		internal static QuickMenuIndicator userProfileAvatarsSaved;

		private void OpenDiscordLink()
		{
			QuickMenuUtils.ShowLinkAlert(MiscUtils.GetClientDiscordLink());
		}

		internal MainClientMenu()
			: base(LanguageManager.GetUsedLanguage().ClientName)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
			new QuickMenuPageButton(this, LanguageManager.GetUsedLanguage().ClientName, LanguageManager.GetUsedLanguage().ClientDescription, MainUtils.CreateSprite(AssetLoader.LoadTexture("MunchenClientLogo")));
			userProfileHeader = new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().UserAccountCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			userProfilePicture = new QuickMenuIndicator(parentRow, string.Empty, string.Empty, "This is your current profile picture on the forums");
			userProfileRank = new QuickMenuIndicator(parentRow, string.Empty, "Rank", string.Empty);
			userProfileSubExpiry = new QuickMenuIndicator(parentRow, string.Empty, "Expiry", string.Empty);
			userProfileAvatarsSaved = new QuickMenuIndicator(parentRow, string.Empty, "Avatars Saved", string.Empty);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow quickMenuButtonRow2 = new QuickMenuButtonRow(this);
			new SettingsMenu(quickMenuButtonRow);
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().Discord, delegate
			{
				OpenDiscordLink();
			}, LanguageManager.GetUsedLanguage().DiscordDescription);
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().ClearVRAM, delegate
			{
				PerformanceProfiler.StartProfiling("ClearVRAM");
				GeneralUtils.ClearVRAM();
				PerformanceProfiler.EndProfiling("ClearVRAM");
				string text = LanguageManager.GetUsedLanguage().ClearVRAMClicked.Replace("{TimeToClear}", string.Format("{0:F2}", PerformanceProfiler.GetProfiling("ClearVRAM")));
				ConsoleUtils.Info("Performance", text, ConsoleColor.Gray, ".ctor", 60);
				QuickMenuUtils.ShowAlert(text);
			}, LanguageManager.GetUsedLanguage().ClearVRAMDescription);
			new QuickMenuToggleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().HideYourself, GeneralUtils.hideSelf, delegate
			{
				GeneralUtils.ChangeHideSelfState(state: true);
			}, LanguageManager.GetUsedLanguage().HideYourselfDescription, delegate
			{
				GeneralUtils.ChangeHideSelfState(state: false);
			}, LanguageManager.GetUsedLanguage().HideYourselfDescription);
			new QuickMenuSingleButton(quickMenuButtonRow2, LanguageManager.GetUsedLanguage().ClearHUDMessages, delegate
			{
				UserInterface.ClearNotificationHud();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().ClearHUDMessagesClicked);
			}, LanguageManager.GetUsedLanguage().ClearHUDMessagesDescription);
			new MediaControlsMenu(quickMenuButtonRow2);
			new NetworkedEmotesMenu(quickMenuButtonRow2);
			if (GeneralUtils.HasSpecialBenefits())
			{
				new AdminPanelMenu(quickMenuButtonRow2);
			}
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parent = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parent2 = new QuickMenuButtonRow(this);
			new ProtectionsMenu(parent);
			new WorldMenu(parent);
			new PlayerMenu(parent);
			new FunMenu(parent);
			new MicrophoneMenu(parent2);
			new MovementMenu(parent2);
			new VideoPlayerMenu(parent2);
			new PhotonExploitsMenu(parent2);
		}
	}
}
