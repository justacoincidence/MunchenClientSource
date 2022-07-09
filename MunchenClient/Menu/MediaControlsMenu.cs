using MunchenClient.Core;
using MunchenClient.Utils;
using UnchainedButtonAPI;

namespace MunchenClient.Menu
{
	internal class MediaControlsMenu : QuickMenuNestedMenu
	{
		internal MediaControlsMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().MediaControlsMenuName, LanguageManager.GetUsedLanguage().MediaControlsMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().MediaControlsPauseUnpause, delegate
			{
				UnmanagedUtils.PlayOrPause();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MediaControlsPauseUnpauseClicked);
			}, LanguageManager.GetUsedLanguage().MediaControlsPauseUnpauseDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().MediaControlsNext, delegate
			{
				UnmanagedUtils.Next();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MediaControlsNextClicked);
			}, LanguageManager.GetUsedLanguage().MediaControlsNextDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().MediaControlsPrevious, delegate
			{
				UnmanagedUtils.Previous();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MediaControlsPreviousClicked);
			}, LanguageManager.GetUsedLanguage().MediaControlsPreviousDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().MediaControlsMuteUnmute, delegate
			{
				UnmanagedUtils.MuteOrUnmute();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MediaControlsMuteUnmuteClicked);
			}, LanguageManager.GetUsedLanguage().MediaControlsMuteUnmuteDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().MediaControlsVolumeDown, delegate
			{
				UnmanagedUtils.VolumeDown();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MediaControlsVolumeDownClicked);
			}, LanguageManager.GetUsedLanguage().MediaControlsVolumeDownDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().MediaControlsVolumeUp, delegate
			{
				UnmanagedUtils.VolumeUp();
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MediaControlsVolumeUpClicked);
			}, LanguageManager.GetUsedLanguage().MediaControlsVolumeUpDescription);
		}
	}
}
