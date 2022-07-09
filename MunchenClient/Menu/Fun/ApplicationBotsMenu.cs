using Il2CppSystem.Collections.Generic;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.Menu.Fun
{
	internal class ApplicationBotsMenu : QuickMenuNestedMenu
	{
		private readonly QuickMenuSingleButton connectionButton;

		internal static QuickMenuSingleButton followButton;

		internal static QuickMenuSingleButton imitateButton;

		private bool botsStartStopping = false;

		internal ApplicationBotsMenu(QuickMenuButtonRow parent)
			: base(parent, "Application Bots", "Haha brr")
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow quickMenuButtonRow2 = new QuickMenuButtonRow(this);
			connectionButton = new QuickMenuSingleButton(quickMenuButtonRow, "Start Bots", delegate
			{
				if (!botsStartStopping)
				{
					if (!ApplicationBotHandler.IsBotsFullyConnected())
					{
						GeneralWrappers.ShowInputPopup("Bot amount", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, StartBots, null, "Enter bot amount here...");
					}
					else
					{
						botsStartStopping = true;
						connectionButton.SetButtonText("Stopping Bots...");
						ApplicationBotHandler.StopPhotonBots(OnBotsStopped);
					}
				}
			}, "Starts the bots");
			new QuickMenuSingleButton(quickMenuButtonRow, "Join", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					ApplicationBotHandler.SendCommandToBots("JoinMe:" + WorldUtils.GetCurrentWorld().id + ":" + WorldUtils.GetCurrentInstance().instanceId);
				}
			}, "Makes the bots join your instance");
			new QuickMenuSingleButton(quickMenuButtonRow, "Leave", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					ApplicationBotHandler.SendCommandToBots("LeaveLobby");
				}
			}, "Makes the bots join your instance");
			new ApplicationBotIndividualMenu(quickMenuButtonRow);
			followButton = new QuickMenuSingleButton(parentRow, "Follow", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					if (ApplicationBotHandler.userToFollow == null)
					{
						ApplicationBotHandler.userToFollow = PlayerWrappers.GetLocalPlayerInformation();
						followButton.SetButtonText("Unfollow");
					}
					else
					{
						ApplicationBotHandler.SendCommandToBots("FollowUser:null");
						ApplicationBotHandler.userToFollow = null;
						followButton.SetButtonText("Follow");
					}
				}
			}, "Whenever the bots should follow you around");
			imitateButton = new QuickMenuSingleButton(parentRow, "Imitate", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					if (ApplicationBotHandler.userToImitate == null)
					{
						ApplicationBotHandler.userToImitate = PlayerWrappers.GetLocalPlayerInformation();
						imitateButton.SetButtonText("Unimitate");
					}
					else
					{
						ApplicationBotHandler.userToImitate = null;
						ApplicationBotHandler.SendCommandToBots("ImitateUser:null");
						imitateButton.SetButtonText("Imitate");
					}
				}
			}, "Whenever the bots should imitate your voice");
			new QuickMenuSingleButton(parentRow, "Use Current Avatar", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					ApplicationBotHandler.SendCommandToBots("ChangeAvatar:" + PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id);
				}
			}, "Use your current avatar for as their own");
			new QuickMenuSingleButton(parentRow, "Use Custom Avatar", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					GeneralWrappers.ShowInputPopup("Use Custom Avatar", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, UseCustomAvatar);
				}
			}, "Whenever the bots should follow you around");
			new QuickMenuSingleButton(parentRow2, "Teleport", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					Vector3 position = PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.transform.position;
					Quaternion rotation = PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.transform.rotation;
					ApplicationBotHandler.SendCommandToBots($"Teleport:{position.x}:{position.y}:{position.z},Rotate:{rotation.x}:{rotation.y}:{rotation.z}:{rotation.w}");
				}
			}, "Whenever the bots should imitate your voice");
			new QuickMenuSingleButton(parentRow2, "Set FPS", delegate
			{
				GeneralWrappers.ShowInputPopup("Set FPS", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetFPS, null, "Enter fps here...");
			}, "Sets the bots maximum fps");
		}

		internal void OnBotsStarted(bool successful)
		{
			botsStartStopping = false;
			if (successful)
			{
				connectionButton.SetButtonText("Stop Bots");
			}
			else
			{
				connectionButton.SetButtonText("Start Bots");
			}
		}

		internal void OnBotsStopped(bool successful)
		{
			botsStartStopping = false;
			if (successful)
			{
				connectionButton.SetButtonText("Start Bots");
			}
			else
			{
				connectionButton.SetButtonText("Stop Bots");
			}
		}

		private void StartBots(string botAmount, List<KeyCode> pressedKeys, Text text)
		{
			if (!int.TryParse(botAmount, out var result))
			{
				OnBotsStarted(successful: false);
				GeneralWrappers.AlertPopup("Bots", "Invalid bot amount specified");
			}
			else
			{
				connectionButton.SetButtonText("Starting Bots...");
				ApplicationBotHandler.StartPhotonBots(OnBotsStarted, result);
			}
		}

		private void SetFPS(string fps, List<KeyCode> pressedKeys, Text text)
		{
			if (!int.TryParse(fps, out var result))
			{
				GeneralWrappers.AlertPopup("Bots", "Invalid fps amount specified");
			}
			else
			{
				ApplicationBotHandler.SendCommandToBots($"SetFPS:{result}");
			}
		}

		private void UseCustomAvatar(string avatarId, List<KeyCode> pressedKeys, Text text)
		{
			ApplicationBotHandler.SendCommandToBots("ChangeAvatar:" + avatarId);
		}
	}
}
