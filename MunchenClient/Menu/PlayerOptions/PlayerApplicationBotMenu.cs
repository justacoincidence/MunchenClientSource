using MunchenClient.Menu.Fun;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;

namespace MunchenClient.Menu.PlayerOptions
{
	internal class PlayerApplicationBotMenu : QuickMenuNestedMenu
	{
		internal PlayerApplicationBotMenu(QuickMenuButtonRow parent)
			: base(parent, "Application Bots", "Lmao")
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, "Follow", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					PlayerInformation selectedPlayer4 = PlayerWrappers.GetSelectedPlayer();
					if (selectedPlayer4 != null)
					{
						ApplicationBotHandler.userToFollow = selectedPlayer4;
						if (selectedPlayer4.id != PlayerWrappers.GetLocalPlayerInformation().id)
						{
							ApplicationBotHandler.SendCommandToBots("FollowUser:" + selectedPlayer4.id);
						}
						ApplicationBotsMenu.followButton.SetButtonText("Unfollow");
					}
				}
			}, "Whenever the bots should follow you around");
			new QuickMenuSingleButton(parentRow, "Imitate", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					PlayerInformation selectedPlayer3 = PlayerWrappers.GetSelectedPlayer();
					if (selectedPlayer3 != null)
					{
						ApplicationBotHandler.userToImitate = selectedPlayer3;
						if (selectedPlayer3.id != PlayerWrappers.GetLocalPlayerInformation().id)
						{
							ApplicationBotHandler.SendCommandToBots("ImitateUser:" + selectedPlayer3.id);
						}
						ApplicationBotsMenu.imitateButton.SetButtonText("Unimitate");
					}
				}
			}, "Whenever the bots should imitate your voice");
			new QuickMenuSingleButton(parentRow, "Change To Avatar", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					PlayerInformation selectedPlayer2 = PlayerWrappers.GetSelectedPlayer();
					if (selectedPlayer2 != null)
					{
						ApplicationBotHandler.SendCommandToBots("ChangeAvatar:" + selectedPlayer2.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id);
					}
				}
			}, "Whenever the bots should imitate your voice");
			new QuickMenuSingleButton(parentRow, "Teleport To", delegate
			{
				if (ApplicationBotHandler.IsBotsFullyConnected())
				{
					PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
					if (selectedPlayer != null)
					{
						Vector3 position = selectedPlayer.vrcPlayer.transform.position;
						Quaternion rotation = selectedPlayer.vrcPlayer.transform.rotation;
						ApplicationBotHandler.SendCommandToBots($"Teleport:{position.x}:{position.y}:{position.z}");
						ApplicationBotHandler.SendCommandToBots($"Rotate:{rotation.x}:{rotation.y}:{rotation.z}:{rotation.w}");
					}
				}
			}, "Whenever the bots should imitate your voice");
		}
	}
}
