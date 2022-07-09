using MunchenClient.Core;
using MunchenClient.Menu.Others;
using MunchenClient.ModuleSystem.Modules;
using UnchainedButtonAPI;

namespace MunchenClient.Menu
{
	internal class MicrophoneMenu : QuickMenuNestedMenu
	{
		internal static bool microphoneMenuInitialized;

		internal static QuickMenuIndicator currentGainIndicator;

		internal MicrophoneMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().MicrophoneMenuName, LanguageManager.GetUsedLanguage().MicrophoneMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().IndicatorsCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			currentGainIndicator = new QuickMenuIndicator(parentRow, "Current Gain", "100%", "Your current gain on your microphone");
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().MicrophoneIncreaseVolume, delegate
			{
				PlayerHandler.IncrementPlayerVolume(0.1f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MicrophoneIncreaseVolumeClicked);
			}, LanguageManager.GetUsedLanguage().MicrophoneIncreaseVolumeDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().MicrophoneDecreaseVolume, delegate
			{
				PlayerHandler.DecrementPlayerVolume(0.1f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MicrophoneDecreaseVolumeClicked);
			}, LanguageManager.GetUsedLanguage().MicrophoneDecreaseVolumeDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().MicrophoneIncreaseVolume10x, delegate
			{
				PlayerHandler.IncrementPlayerVolume(1f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MicrophoneIncreaseVolume10xClicked);
			}, LanguageManager.GetUsedLanguage().MicrophoneIncreaseVolume10xDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().MicrophoneDecreaseVolume10x, delegate
			{
				PlayerHandler.DecrementPlayerVolume(1f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MicrophoneDecreaseVolume10xClicked);
			}, LanguageManager.GetUsedLanguage().MicrophoneDecreaseVolume10xDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().MicrophoneEarrapeVolume, delegate
			{
				PlayerHandler.SetPlayerVolume(float.MaxValue);
				ActionWheelMenu.earrapeButton.SetButtonText("Earrape Mic: On");
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MicrophoneEarrapeVolumeClicked);
			}, LanguageManager.GetUsedLanguage().MicrophoneEarrapeVolumeDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().MicrophoneResetVolume, delegate
			{
				PlayerHandler.SetPlayerVolume(1f);
				ActionWheelMenu.earrapeButton.SetButtonText("Earrape Mic: Off");
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().MicrophoneResetVolumeClicked);
			}, LanguageManager.GetUsedLanguage().MicrophoneResetVolumeDescription);
			microphoneMenuInitialized = true;
		}
	}
}
