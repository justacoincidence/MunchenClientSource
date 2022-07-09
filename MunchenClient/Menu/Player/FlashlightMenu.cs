using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Player
{
	internal class FlashlightMenu : QuickMenuNestedMenu
	{
		internal static bool flashlightMenuInitialized;

		internal static QuickMenuIndicator flashlightAttachedBone;

		internal static QuickMenuIndicator flashlightSpotAngle;

		internal static QuickMenuIndicator flashlightRange;

		internal static QuickMenuIndicator flashlightIntensity;

		internal FlashlightMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().FlashlightMenuName, LanguageManager.GetUsedLanguage().FlashlightMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().IndicatorsCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			flashlightAttachedBone = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().FlashlightIndicatorAttachedBone, string.Empty, LanguageManager.GetUsedLanguage().FlashlightIndicatorAttachedBoneDescription);
			flashlightSpotAngle = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().FlashlightIndicatorSpotAngle, string.Empty, LanguageManager.GetUsedLanguage().FlashlightIndicatorSpotAngleDescription);
			flashlightRange = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().FlashlightIndicatorRange, string.Empty, LanguageManager.GetUsedLanguage().FlashlightIndicatorRangeDescription);
			flashlightIntensity = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().FlashlightIndicatorIntensity, string.Empty, LanguageManager.GetUsedLanguage().FlashlightIndicatorIntensityDescription);
			new QuickMenuToggleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().FlashlightMenuName, Configuration.GetGeneralConfig().FlashlightActive, delegate
			{
				Configuration.GetGeneralConfig().FlashlightActive = true;
				Configuration.SaveGeneralConfig();
				FlashlightHandler.ApplyFlashlightVisibilityState();
			}, LanguageManager.GetUsedLanguage().FlashlightMenuDescription, delegate
			{
				Configuration.GetGeneralConfig().FlashlightActive = false;
				Configuration.SaveGeneralConfig();
				FlashlightHandler.ApplyFlashlightVisibilityState();
			}, LanguageManager.GetUsedLanguage().FlashlightMenuDescription);
			new FlashlightBoneSelectorMenu(quickMenuButtonRow);
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().FlashlightIncreaseSpotAngle, delegate
			{
				if (Configuration.GetGeneralConfig().FlashlightSpotAngle < 179)
				{
					Configuration.GetGeneralConfig().FlashlightSpotAngle++;
					Configuration.SaveGeneralConfig();
					FlashlightHandler.ApplyFlashlightValues();
				}
			}, LanguageManager.GetUsedLanguage().FlashlightIncreaseSpotAngleDescription);
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().FlashlightDecreaseSpotAngle, delegate
			{
				if (Configuration.GetGeneralConfig().FlashlightSpotAngle > 1)
				{
					Configuration.GetGeneralConfig().FlashlightSpotAngle--;
					Configuration.SaveGeneralConfig();
					FlashlightHandler.ApplyFlashlightValues();
				}
			}, LanguageManager.GetUsedLanguage().FlashlightDecreaseSpotAngleDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().FlashlightIncreaseRange, delegate
			{
				Configuration.GetGeneralConfig().FlashlightRange += 1f;
				Configuration.SaveGeneralConfig();
				FlashlightHandler.ApplyFlashlightValues();
			}, LanguageManager.GetUsedLanguage().FlashlightIncreaseRangeDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().FlashlightDecreaseRange, delegate
			{
				Configuration.GetGeneralConfig().FlashlightRange -= 1f;
				Configuration.SaveGeneralConfig();
				FlashlightHandler.ApplyFlashlightValues();
			}, LanguageManager.GetUsedLanguage().FlashlightDecreaseRangeDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().FlashlightIncreaseIntensity, delegate
			{
				Configuration.GetGeneralConfig().FlashlightIntensity += 0.1f;
				Configuration.SaveGeneralConfig();
				FlashlightHandler.ApplyFlashlightValues();
			}, LanguageManager.GetUsedLanguage().FlashlightIncreaseIntensityDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().FlashlightDecreaseIntensity, delegate
			{
				Configuration.GetGeneralConfig().FlashlightIntensity -= 0.1f;
				Configuration.SaveGeneralConfig();
				FlashlightHandler.ApplyFlashlightValues();
			}, LanguageManager.GetUsedLanguage().FlashlightDecreaseIntensityDescription);
			flashlightMenuInitialized = true;
		}
	}
}
