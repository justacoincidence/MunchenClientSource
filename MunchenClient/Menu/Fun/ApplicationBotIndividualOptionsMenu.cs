using Il2CppSystem.Collections.Generic;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.Menu.Fun
{
	internal class ApplicationBotIndividualOptionsMenu : QuickMenuNestedMenu
	{
		internal int botIndex = 0;

		internal ApplicationBotIndividualOptionsMenu(QuickMenuButtonRow parent)
			: base(parent, "Individual Bot Selector", string.Empty, createButtonOnParent: false)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, "Use Current Avatar", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					ApplicationBotHandler.SendCommandToBot(botIndex, "ChangeAvatar:" + PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id);
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
					ApplicationBotHandler.SendCommandToBot(botIndex, $"Teleport:{position.x}:{position.y}:{position.z},Rotate:{rotation.x}:{rotation.y}:{rotation.z}:{rotation.w}");
				}
			}, "Whenever the bots should imitate your voice");
		}

		private void UseCustomAvatar(string avatarId, List<KeyCode> pressedKeys, Text text)
		{
			ApplicationBotHandler.SendCommandToBot(botIndex, "ChangeAvatar:" + avatarId);
		}
	}
}
