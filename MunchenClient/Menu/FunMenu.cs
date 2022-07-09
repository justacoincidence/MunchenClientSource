using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu.Fun;
using MunchenClient.Menu.Others;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using VRC.SDKBase;

namespace MunchenClient.Menu
{
	internal class FunMenu : QuickMenuNestedMenu
	{
		internal static QuickMenuToggleButton itemOrbitButton;

		internal static QuickMenuToggleButton capsuleHiderButton;

		internal FunMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().FunMenuName, LanguageManager.GetUsedLanguage().FunMenuDescription)
		{
			QuickMenuButtonRow parent2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new ApplicationBotsMenu(parent2);
			new MurderAmongUsMenu(parent2);
			new PortableMirrorMenu(parent2);
			new UdonManipulatorMenu(parent2);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().WorldTriggers, Configuration.GetGeneralConfig().WorldTriggers, delegate
			{
				Configuration.GetGeneralConfig().WorldTriggers = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().WorldTriggersDescription, delegate
			{
				Configuration.GetGeneralConfig().WorldTriggers = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().WorldTriggersDescription);
			capsuleHiderButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().CapsuleHider, GeneralUtils.capsuleHider, delegate
			{
				PlayerUtils.ChangeCapsuleState(state: true);
				GeneralUtils.capsuleHider = true;
				ActionWheelMenu.colliderHiderButton.SetButtonText("Collider Hider: <color=green>On");
			}, LanguageManager.GetUsedLanguage().CapsuleHiderDescription, delegate
			{
				PlayerUtils.ChangeCapsuleState(state: false);
				GeneralUtils.capsuleHider = false;
				ActionWheelMenu.colliderHiderButton.SetButtonText("Collider Hider: <color=red>Off");
			}, LanguageManager.GetUsedLanguage().CapsuleHiderDescription);
			itemOrbitButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ItemOrbit, GeneralUtils.itemOrbit, delegate
			{
				GeneralUtils.itemOrbit = true;
			}, LanguageManager.GetUsedLanguage().ItemOrbitDescription, delegate
			{
				GeneralUtils.itemOrbit = false;
			}, LanguageManager.GetUsedLanguage().ItemOrbitDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().UdonGodmode, Configuration.GetGeneralConfig().UdonGodmode, delegate
			{
				Configuration.GetGeneralConfig().UdonGodmode = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().UdonGodmodeDescription, delegate
			{
				Configuration.GetGeneralConfig().UdonGodmode = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().UdonGodmodeDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().InteractWithAllPickups, delegate
			{
				VRC_Pickup[] array = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int i = 0; i < array.Length; i++)
				{
					Networking.SetOwner(PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0, array[i].gameObject);
					array[i].transform.position = PlayerWrappers.GetCurrentPlayer().gameObject.transform.position + Vector3.up;
				}
			}, LanguageManager.GetUsedLanguage().InteractWithAllPickupsDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().PCCrasher, delegate
			{
				GeneralWrappers.AlertAction("Notice", "Are you sure you want to crash all PC users in this instance?", "Crash", delegate
				{
					GeneralUtils.RunGameCloseExploit(quest: false);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().PCCrasherDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().QuestCrasher, delegate
			{
				GeneralWrappers.AlertAction("Notice", "Are you sure you want to crash all Quest users in this instance?", "Crash", delegate
				{
					GeneralUtils.RunGameCloseExploit(quest: true);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}, LanguageManager.GetUsedLanguage().QuestCrasherDescription);
		}
	}
}
