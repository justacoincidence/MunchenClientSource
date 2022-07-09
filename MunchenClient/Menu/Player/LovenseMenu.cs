using System;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace MunchenClient.Menu.Player
{
	internal class LovenseMenu : QuickMenuNestedMenu
	{
		internal static QuickMenuSingleButton connectButton;

		internal static QuickMenuIndicator currentDeviceIndicator;

		internal static QuickMenuIndicator currentBatteryIndicator;

		internal static QuickMenuIndicator currentIntensityIndicator;

		private bool lastConnectionState = false;

		internal LovenseMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().LovenseMenuName, LanguageManager.GetUsedLanguage().LovenseMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().IndicatorsCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			connectButton = new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().ConnectToLovense, delegate
			{
				if (!LovenseConnectAPI.IsLovenseConnecting())
				{
					if (LovenseConnectAPI.IsLovenseConnected())
					{
						if (LovenseConnectAPI.DisconnectFromLovense())
						{
							connectButton.SetButtonText(LanguageManager.GetUsedLanguage().ConnectToLovense);
							currentDeviceIndicator.SetMainText("None");
							currentBatteryIndicator.SetMainText("0%");
							currentIntensityIndicator.SetMainText("0%");
						}
					}
					else
					{
						GeneralWrappers.ShowInputPopup("Lovense IP", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, ConnectLovenseAPI, null, "Enter IP here...");
					}
				}
			}, LanguageManager.GetUsedLanguage().ConnectToLovenseDescription);
			currentDeviceIndicator = new QuickMenuIndicator(parentRow, "Device", "None", "Your current Lovense device");
			currentBatteryIndicator = new QuickMenuIndicator(parentRow, "Battery", "0%", "Your current device's battery percentage");
			currentIntensityIndicator = new QuickMenuIndicator(parentRow, "Intensity", "0%", "Your current device's vibrating intensity");
			new QuickMenuToggleButton(parentRow2, "Debug Mode", LovenseHandler.lovenseDebug, delegate
			{
				LovenseHandler.lovenseDebug = true;
			}, "Enables debug mode", delegate
			{
				LovenseHandler.lovenseDebug = false;
			}, "Enables debug mode");
			new QuickMenuSingleButton(parentRow2, "Test Vibrate Local", delegate
			{
				PerformVibranceTest();
			}, "Test the vibration system");
			new QuickMenuSingleButton(parentRow2, "Test Vibrate Remote", delegate
			{
				foreach (System.Collections.Generic.KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
				{
					Networking.RPC(playerCaching.Value.playerApi, playerCaching.Value.vrcPlayer.gameObject, "SendVibrateTest", GeneralUtils.rpcParameters);
				}
			}, "Test the vibration system");
		}

		private void ConnectLovenseAPI(string ip, Il2CppSystem.Collections.Generic.List<KeyCode> pressedKeys, Text text)
		{
			connectButton.SetButtonText(LanguageManager.GetUsedLanguage().ConnectingToLovense);
			LovenseConnectAPI.ConnectToLovense(ip, OnLovenseConnected);
		}

		private void OnLovenseConnected(LovenseDevice lovense)
		{
			ConsoleUtils.Info("Lovense", $"Connected: {lovense != null}", ConsoleColor.Gray, "OnLovenseConnected", 92);
			if (lastConnectionState != (lovense != null))
			{
				PlayerUtils.ReloadAllAvatars();
				lastConnectionState = lovense != null;
			}
			if (lovense == null)
			{
				connectButton.SetButtonText(LanguageManager.GetUsedLanguage().ConnectToLovense);
				currentDeviceIndicator.SetMainText("None");
				currentBatteryIndicator.SetMainText("0%");
				currentIntensityIndicator.SetMainText("0%");
			}
			else
			{
				connectButton.SetButtonText(LanguageManager.GetUsedLanguage().DisconnectFromLovense);
				currentDeviceIndicator.SetMainText(lovense.GetName());
				currentBatteryIndicator.SetMainText($"{lovense.battery}%");
				currentIntensityIndicator.SetMainText($"{lovense.intensity * 5}%");
			}
		}

		internal static void PerformVibranceTest()
		{
			LovenseConnectAPI.VibrateLovense(20, OnVibranceTestDone);
		}

		private static void OnVibranceTestDone(byte intensity, bool success, string response)
		{
			if (success)
			{
				LovenseConnectAPI.VibrateLovense(0, OnVibranceTestDoneSecond);
			}
		}

		private static void OnVibranceTestDoneSecond(byte intensity, bool success, string response)
		{
			if (!success)
			{
				LovenseConnectAPI.VibrateLovense(0, OnVibranceTestDoneSecond);
			}
			else
			{
				ConsoleUtils.FlushToConsole("Lovense", "Vibration test succeded", ConsoleColor.Gray, "OnVibranceTestDoneSecond", 140);
			}
		}
	}
}
