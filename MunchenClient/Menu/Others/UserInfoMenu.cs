using System;
using MenuPanelAPI;
using MunchenClient.Config;
using MunchenClient.Core.Compatibility;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;

namespace MunchenClient.Menu.Others
{
	internal class UserInfoMenu
	{
		internal static MenuPanelButton teleportButton;

		internal static MenuPanelButton favoriteAvatarButton;

		internal static MenuPanelButton stealAvatarButton;

		internal UserInfoMenu()
		{
			if (Configuration.GetGeneralConfig().DisableMenuPlayerButtons)
			{
				return;
			}
			if (CompatibilityLayer.IsEmmInstalled())
			{
				ConsoleUtils.Info("CompatibilityLayer", "emmVRC installed - Disabling user page buttons", ConsoleColor.Gray, ".ctor", 26);
				return;
			}
			teleportButton = new MenuPanelButton("Teleport", delegate
			{
				PlayerInformation playerInformationByName3 = PlayerWrappers.GetPlayerInformationByName(GeneralWrappers.GetPageUserInfo().field_Private_APIUser_0.displayName);
				if (playerInformationByName3 != null)
				{
					PlayerWrappers.GetCurrentPlayer().transform.position = playerInformationByName3.vrcPlayer.transform.position;
					GeneralWrappers.CloseMenu();
				}
				else
				{
					GeneralWrappers.AlertPopup("Error", "User is not in instance");
				}
			}, interactable: true, "Buttons/RightSideButtons/RightUpperButtonColumn/PlaylistsButton", "Buttons/RightSideButtons/RightUpperButtonColumn");
			favoriteAvatarButton = new MenuPanelButton("Favorite Avatar", delegate
			{
				PlayerInformation playerInformationByName2 = PlayerWrappers.GetPlayerInformationByName(GeneralWrappers.GetPageUserInfo().field_Private_APIUser_0.displayName);
				if (playerInformationByName2 != null)
				{
					if (playerInformationByName2.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2 != null)
					{
						if (playerInformationByName2.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.releaseStatus != "private")
						{
							AvatarFavoritesHandler.AddAvatarByID(playerInformationByName2.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id, null, null);
						}
						else
						{
							ConsoleUtils.Info("Player", "Can't add avatar since it's private", ConsoleColor.Gray, ".ctor", 61);
							GeneralWrappers.AlertPopup("Warning", "Can't add avatar since it's private");
						}
					}
					else
					{
						ConsoleUtils.Info("Player", "Can't add avatar since player is either switching or is a robot", ConsoleColor.Gray, ".ctor", 67);
						GeneralWrappers.AlertPopup("Warning", "Can't add avatar since player is either switching or is a robot");
					}
				}
				else
				{
					GeneralWrappers.AlertPopup("Error", "User is not in instance");
				}
			}, interactable: true, "Buttons/RightSideButtons/RightUpperButtonColumn/PlaylistsButton", "Buttons/RightSideButtons/RightUpperButtonColumn");
			stealAvatarButton = new MenuPanelButton("Clone Avatar", delegate
			{
				PlayerInformation playerInformationByName = PlayerWrappers.GetPlayerInformationByName(GeneralWrappers.GetPageUserInfo().field_Private_APIUser_0.displayName);
				if (playerInformationByName != null)
				{
					if (playerInformationByName.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2 != null)
					{
						if (playerInformationByName.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.releaseStatus != "private")
						{
							PlayerUtils.ChangePlayerAvatar(playerInformationByName.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id, logErrorOnHud: true);
							GeneralWrappers.CloseMenu();
						}
						else
						{
							ConsoleUtils.Info("Player", "Can't add avatar since it's private", ConsoleColor.Gray, ".ctor", 93);
							GeneralWrappers.AlertPopup("Warning", "Can't add avatar since it's private");
						}
					}
					else
					{
						ConsoleUtils.Info("Player", "Can't clone avatar since player is either switching or is a robot", ConsoleColor.Gray, ".ctor", 100);
						GeneralWrappers.AlertPopup("Warning", "Can't clone avatar since player is either switching or is a robot");
					}
				}
				else
				{
					GeneralWrappers.AlertPopup("Error", "User is not in instance");
				}
			}, interactable: true, "Buttons/RightSideButtons/RightUpperButtonColumn/PlaylistsButton", "Buttons/RightSideButtons/RightUpperButtonColumn");
		}
	}
}
