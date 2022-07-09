using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu.Others;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;

namespace MunchenClient.Menu
{
	internal class MovementMenu : QuickMenuNestedMenu
	{
		internal static bool movementMenuInitialized;

		internal static QuickMenuIndicator currentRunModifierIndicator;

		internal static QuickMenuIndicator currentWalkModifierIndicator;

		internal static QuickMenuIndicator currentGravityModifierIndicator;

		internal static QuickMenuIndicator currentJumpModifierIndicator;

		internal static QuickMenuIndicator currentFlightSpeedIndicator;

		internal static QuickMenuToggleButton flightButton;

		internal MovementMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().MovementMenuName, LanguageManager.GetUsedLanguage().MovementMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().IndicatorsCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			currentRunModifierIndicator = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().RunSpeedIndicator, "0", LanguageManager.GetUsedLanguage().RunSpeedIndicatorDescription);
			currentWalkModifierIndicator = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().WalkSpeedIndicator, "0", LanguageManager.GetUsedLanguage().WalkSpeedIndicatorDescription);
			currentGravityModifierIndicator = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().GravityIndicator, "0", LanguageManager.GetUsedLanguage().GravityIndicatorDescription);
			currentJumpModifierIndicator = new QuickMenuIndicator(parentRow, LanguageManager.GetUsedLanguage().JumpPowerIndicator, "0", LanguageManager.GetUsedLanguage().JumpPowerIndicatorDescription);
			currentFlightSpeedIndicator = new QuickMenuIndicator(parentRow2, LanguageManager.GetUsedLanguage().FlightSpeedIndicator, "0", LanguageManager.GetUsedLanguage().FlightSpeedIndicatorDescription);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow4 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow5 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow6 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow7 = new QuickMenuButtonRow(this);
			flightButton = new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().Flight, GeneralUtils.flight, delegate
			{
				GeneralUtils.ToggleFlight(state: true);
				ActionWheelMenu.flightButton.SetButtonText("Flight: <color=green>On");
			}, LanguageManager.GetUsedLanguage().FlightDescription, delegate
			{
				GeneralUtils.ToggleFlight(state: false);
				ActionWheelMenu.flightButton.SetButtonText("Flight: <color=red>Off");
			}, LanguageManager.GetUsedLanguage().FlightDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().AutoBhop, Configuration.GetGeneralConfig().AutoBhop, delegate
			{
				Configuration.GetGeneralConfig().AutoBhop = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoBhopDescription, delegate
			{
				Configuration.GetGeneralConfig().AutoBhop = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoBhopDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().InfiniteJump, Configuration.GetGeneralConfig().InfiniteJump, delegate
			{
				Configuration.GetGeneralConfig().InfiniteJump = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().InfiniteJumpDescription, delegate
			{
				Configuration.GetGeneralConfig().InfiniteJump = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().InfiniteJumpDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().Jetpack, Configuration.GetGeneralConfig().Jetpack, delegate
			{
				Configuration.GetGeneralConfig().Jetpack = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JetpackDescription, delegate
			{
				Configuration.GetGeneralConfig().Jetpack = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().JetpackDescription);
			new QuickMenuToggleButton(parentRow4, LanguageManager.GetUsedLanguage().FlightDoubleJump, Configuration.GetGeneralConfig().FlightDoubleJump, delegate
			{
				Configuration.GetGeneralConfig().FlightDoubleJump = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().FlightDoubleJumpDescription, delegate
			{
				Configuration.GetGeneralConfig().FlightDoubleJump = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().FlightDoubleJumpDescription);
			new QuickMenuSingleButton(parentRow4, LanguageManager.GetUsedLanguage().IncreaseSpeed, delegate
			{
				PlayerHandler.IncreasePlayerSpeed(PlayerWrappers.GetLocalPlayerInformation(), 2f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().IncreaseSpeedClicked);
			}, LanguageManager.GetUsedLanguage().IncreaseSpeedDescription);
			new QuickMenuSingleButton(parentRow4, LanguageManager.GetUsedLanguage().DecreaseSpeed, delegate
			{
				PlayerHandler.DecreasePlayerSpeed(PlayerWrappers.GetLocalPlayerInformation(), 2f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().DecreaseSpeedClicked);
			}, LanguageManager.GetUsedLanguage().DecreaseSpeedDescription);
			new QuickMenuSingleButton(parentRow4, LanguageManager.GetUsedLanguage().IncreaseSpeed4x, delegate
			{
				PlayerHandler.IncreasePlayerSpeed(PlayerWrappers.GetLocalPlayerInformation(), 8f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().IncreaseSpeed4xClicked);
			}, LanguageManager.GetUsedLanguage().IncreaseSpeed4xDescription);
			new QuickMenuSingleButton(parentRow5, LanguageManager.GetUsedLanguage().DecreaseSpeed4x, delegate
			{
				PlayerHandler.DecreasePlayerSpeed(PlayerWrappers.GetLocalPlayerInformation(), 8f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().DecreaseSpeed4xClicked);
			}, LanguageManager.GetUsedLanguage().DecreaseSpeed4xDescription);
			new QuickMenuSingleButton(parentRow5, LanguageManager.GetUsedLanguage().IncreaseGravity, delegate
			{
				if (!GeneralUtils.flight)
				{
					Vector3 gravity2 = Physics.gravity;
					gravity2.y -= 1f;
					Physics.gravity = gravity2;
					QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().IncreaseGravityClicked);
				}
			}, LanguageManager.GetUsedLanguage().IncreaseGravityDescription);
			new QuickMenuSingleButton(parentRow5, LanguageManager.GetUsedLanguage().DecreaseGravity, delegate
			{
				if (!GeneralUtils.flight)
				{
					Vector3 gravity = Physics.gravity;
					gravity.y += 1f;
					Physics.gravity = gravity;
					QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().DecreaseGravityClicked);
				}
			}, LanguageManager.GetUsedLanguage().DecreaseGravityDescription);
			new QuickMenuSingleButton(parentRow5, LanguageManager.GetUsedLanguage().IncreaseJumpPower, delegate
			{
				PlayerHandler.IncreasePlayerJumpPower(PlayerWrappers.GetLocalPlayerInformation(), 0.5f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().IncreaseJumpPowerClicked);
			}, LanguageManager.GetUsedLanguage().IncreaseJumpPowerDescription);
			new QuickMenuSingleButton(parentRow6, LanguageManager.GetUsedLanguage().DecreaseJumpPower, delegate
			{
				PlayerHandler.DecreasePlayerJumpPower(PlayerWrappers.GetLocalPlayerInformation(), 0.5f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().DecreaseJumpPowerClicked);
			}, LanguageManager.GetUsedLanguage().DecreaseJumpPowerDescription);
			new QuickMenuSingleButton(parentRow6, LanguageManager.GetUsedLanguage().ResetGravity, delegate
			{
				if (!GeneralUtils.flight)
				{
					PlayerHandler.ResetGravity();
					QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().ResetGravityClicked);
				}
			}, LanguageManager.GetUsedLanguage().ResetGravityDescription);
			new QuickMenuSingleButton(parentRow6, LanguageManager.GetUsedLanguage().ResetSpeeds, delegate
			{
				PlayerHandler.ResetPlayerSpeed(PlayerWrappers.GetLocalPlayerInformation());
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().ResetSpeedsClicked);
			}, LanguageManager.GetUsedLanguage().ResetSpeedsDescription);
			new QuickMenuSingleButton(parentRow6, LanguageManager.GetUsedLanguage().ResetJumpPower, delegate
			{
				PlayerHandler.ResetJumpPower(PlayerWrappers.GetLocalPlayerInformation());
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().ResetJumpPowerClicked);
			}, LanguageManager.GetUsedLanguage().ResetJumpPowerDescription);
			new QuickMenuSingleButton(parentRow7, LanguageManager.GetUsedLanguage().IncreaseFlightSpeed, delegate
			{
				PlayerHandler.IncreasePlayerFlightSpeed(1f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().IncreaseFlightSpeedClicked);
			}, LanguageManager.GetUsedLanguage().IncreaseFlightSpeedDescription);
			new QuickMenuSingleButton(parentRow7, LanguageManager.GetUsedLanguage().DecreaseFlightSpeed, delegate
			{
				PlayerHandler.DecreasePlayerFlightSpeed(1f);
				QuickMenuUtils.ShowAlert(LanguageManager.GetUsedLanguage().DecreaseFlightSpeedClicked);
			}, LanguageManager.GetUsedLanguage().DecreaseFlightSpeedDescription);
			movementMenuInitialized = true;
		}
	}
}
