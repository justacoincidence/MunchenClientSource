using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu.Protections;
using UnchainedButtonAPI;

namespace MunchenClient.Menu
{
	internal class ProtectionsMenu : QuickMenuNestedMenu
	{
		internal ProtectionsMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().ProtectionsMenuName, LanguageManager.GetUsedLanguage().ProtectionsMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ProtectionsOtherMenuName);
			QuickMenuButtonRow parent2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parent3 = new QuickMenuButtonRow(this);
			new PortalsMenu(parent2);
			new ModerationMenu(parent2);
			new AntiCrashMenu(parent2);
			new AntiExploitMenu(parent2);
			new SpoofersMenu(parent3);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ProtectionsMenuName);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AntiAvatarIntroMusic, Configuration.GetGeneralConfig().AntiAvatarIntroMusic, delegate
			{
				Configuration.GetGeneralConfig().AntiAvatarIntroMusic = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AntiAvatarIntroMusicDescription, delegate
			{
				Configuration.GetGeneralConfig().AntiAvatarIntroMusic = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AntiAvatarIntroMusicDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().BlockAllRPCEvents, Configuration.GetGeneralConfig().BlockAllRPCEvents, delegate
			{
				Configuration.GetGeneralConfig().BlockAllRPCEvents = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().BlockAllRPCEventsDescription, delegate
			{
				Configuration.GetGeneralConfig().BlockAllRPCEvents = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().BlockAllRPCEventsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().BlockAllUdonEvents, Configuration.GetGeneralConfig().BlockAllUdonEvents, delegate
			{
				Configuration.GetGeneralConfig().BlockAllUdonEvents = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().BlockAllUdonEventsDescription, delegate
			{
				Configuration.GetGeneralConfig().BlockAllUdonEvents = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().BlockAllUdonEventsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().BlockAllPhotonEvents, Configuration.GetGeneralConfig().BlockAllPhotonEvents, delegate
			{
				Configuration.GetGeneralConfig().BlockAllPhotonEvents = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().BlockAllPhotonEventsDescription, delegate
			{
				Configuration.GetGeneralConfig().BlockAllPhotonEvents = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().BlockAllPhotonEventsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AntiIPLoggingVideoPlayerSafety, Configuration.GetGeneralConfig().AntiIPLoggingVideoPlayerSafety, delegate
			{
				Configuration.GetGeneralConfig().AntiIPLoggingVideoPlayerSafety = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AntiIPLoggingVideoPlayerSafetyDescription, delegate
			{
				Configuration.GetGeneralConfig().AntiIPLoggingVideoPlayerSafety = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AntiIPLoggingVideoPlayerSafetyDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AntiIPLoggingImageThumbnailSafety, Configuration.GetGeneralConfig().AntiIPLoggingImageThumbnailSafety, delegate
			{
				Configuration.GetGeneralConfig().AntiIPLoggingImageThumbnailSafety = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AntiIPLoggingImageThumbnailSafetyDescription, delegate
			{
				Configuration.GetGeneralConfig().AntiIPLoggingImageThumbnailSafety = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AntiIPLoggingImageThumbnailSafetyDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().InvisibleDetection, Configuration.GetGeneralConfig().InvisibleDetection, delegate
			{
				Configuration.GetGeneralConfig().InvisibleDetection = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().InvisibleDetectionDescription, delegate
			{
				Configuration.GetGeneralConfig().InvisibleDetection = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().InvisibleDetectionDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().SerializationDetection, Configuration.GetGeneralConfig().SerializationDetection, delegate
			{
				Configuration.GetGeneralConfig().SerializationDetection = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().SerializationDetectionDescription, delegate
			{
				Configuration.GetGeneralConfig().SerializationDetection = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().SerializationDetectionDescription);
		}
	}
}
