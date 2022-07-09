using ActionMenuAPI;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu.Protections;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;

namespace MunchenClient.Menu.Others
{
	internal class ActionWheelMenu
	{
		internal static CustomActionMenu.ActionMenuPage clientPage;

		internal static CustomActionMenu.ActionMenuButton flightButton;

		internal static CustomActionMenu.ActionMenuButton serializationButton;

		internal static CustomActionMenu.ActionMenuButton wallhackButton;

		internal static CustomActionMenu.ActionMenuButton earrapeButton;

		internal static CustomActionMenu.ActionMenuButton colliderHiderButton;

		internal static CustomActionMenu.ActionMenuButton portalButton;

		internal static CustomActionMenu.ActionMenuButton mediaButton;

		internal ActionWheelMenu()
		{
			clientPage = new CustomActionMenu.ActionMenuPage(CustomActionMenu.ActionMenuBaseMenu.MainMenu, "München", AssetLoader.LoadTexture("MunchenClientLogo"));
			flightButton = new CustomActionMenu.ActionMenuButton(clientPage, "Flight: <color=red>Off", delegate
			{
				GeneralUtils.ToggleFlight(!GeneralUtils.flight);
				MovementMenu.flightButton.SetToggleState(GeneralUtils.flight);
				flightButton.SetButtonText(GeneralUtils.flight ? "Flight: <color=green>On" : "Flight: <color=red>Off");
			}, AssetLoader.LoadTexture("FlightIcon"));
			serializationButton = new CustomActionMenu.ActionMenuButton(clientPage, "Serialization: <color=red>Off", delegate
			{
				GeneralUtils.serialization = !GeneralUtils.serialization;
				serializationButton.SetButtonText(GeneralUtils.serialization ? "Serialization: <color=green>On" : "Serialization: <color=red>Off");
				if (GeneralUtils.serialization)
				{
					GeneralUtils.fakelag = false;
					PhotonExploitsMenu.fakeLagButton.SetToggleState(state: false);
				}
			}, AssetLoader.LoadTexture("SerializationIcon"));
			wallhackButton = new CustomActionMenu.ActionMenuButton(clientPage, "Wallhack: <color=red>Off", delegate
			{
				Configuration.GetGeneralConfig().PlayerWallhack = !Configuration.GetGeneralConfig().PlayerWallhack;
				Configuration.SaveGeneralConfig();
				GeneralWrappers.ApplyAllPlayerWallhack(Configuration.GetGeneralConfig().PlayerWallhack);
				PlayerMenu.wallhackButton.SetToggleState(Configuration.GetGeneralConfig().PlayerWallhack);
				wallhackButton.SetButtonText(Configuration.GetGeneralConfig().PlayerWallhack ? "Wallhack: <color=green>On" : "Wallhack: <color=red>Off");
			}, AssetLoader.LoadTexture("WallhackIcon"));
			wallhackButton.SetButtonText(Configuration.GetGeneralConfig().PlayerWallhack ? "Wallhack: <color=green>On" : "Wallhack: <color=red>Off");
			earrapeButton = new CustomActionMenu.ActionMenuButton(clientPage, "Earrape Mic: <color=red>Off", delegate
			{
				if (PlayerHandler.GetPlayerVolume() > 1.70141173E+38f)
				{
					PlayerHandler.SetPlayerVolume(1f);
					earrapeButton.SetButtonText("Earrape Mic: <color=red>Off");
				}
				else
				{
					PlayerHandler.SetPlayerVolume(float.MaxValue);
					earrapeButton.SetButtonText("Earrape Mic: <color=green>On");
				}
			}, AssetLoader.LoadTexture("EarrapeIcon"));
			colliderHiderButton = new CustomActionMenu.ActionMenuButton(clientPage, "Collider Hider: <color=red>Off", delegate
			{
				GeneralUtils.capsuleHider = !GeneralUtils.capsuleHider;
				PlayerUtils.ChangeCapsuleState(GeneralUtils.capsuleHider);
				FunMenu.capsuleHiderButton.SetToggleState(GeneralUtils.capsuleHider);
				colliderHiderButton.SetButtonText(GeneralUtils.capsuleHider ? "Collider Hider: <color=green>On" : "Collider Hider: <color=red>Off");
			}, AssetLoader.LoadTexture("ColliderHider"));
			portalButton = new CustomActionMenu.ActionMenuButton(clientPage, "Delete Portals", delegate
			{
				PortalsMenu.DeleteAllPortals(informHud: true);
			}, AssetLoader.LoadTexture("PortalIcon"));
			mediaButton = new CustomActionMenu.ActionMenuButton(clientPage, "(Un)Pause Media", delegate
			{
				UnmanagedUtils.PlayOrPause();
			}, AssetLoader.LoadTexture("MediaIcon"));
		}
	}
}
