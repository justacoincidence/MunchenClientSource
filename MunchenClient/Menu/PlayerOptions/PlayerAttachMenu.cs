using System;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;

namespace MunchenClient.Menu.PlayerOptions
{
	internal class PlayerAttachMenu : QuickMenuNestedMenu
	{
		private void SelectBodyBoneToAttach(HumanBodyBones bodypart, bool offset, bool orbit)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer == null)
			{
				ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, "SelectBodyBoneToAttach", 18);
				return;
			}
			if (selectedPlayer.isLocalPlayer)
			{
				ConsoleUtils.Info("Player", "Can't target local player", ConsoleColor.Gray, "SelectBodyBoneToAttach", 25);
				return;
			}
			PlayerTargetHandler.SelectNewPlayerToAttach(selectedPlayer, bodypart, offset, orbit);
			PlayerTargetHandler.attachToPlayer = true;
			PlayerMenu.attachToPlayerButton.SetToggleState(state: true);
		}

		internal PlayerAttachMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().PlayerAttachMenuName, LanguageManager.GetUsedLanguage().PlayerAttachMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachHead, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.Head, offset: false, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachHeadDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachChest, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.Chest, offset: false, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachChestDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachRightHand, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.RightHand, offset: false, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachRightHandDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachLeftHand, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.LeftHand, offset: false, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachLeftHandDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().AttachHips, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.Hips, offset: false, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachHipsDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().AttachBack, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.Neck, offset: true, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachBackDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().AttachRightFoot, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.RightFoot, offset: false, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachRightHandDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().AttachLeftFoot, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.LeftFoot, offset: false, orbit: false);
			}, LanguageManager.GetUsedLanguage().AttachLeftHandDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().AttachOrbit, delegate
			{
				SelectBodyBoneToAttach(HumanBodyBones.Head, offset: false, orbit: true);
			}, LanguageManager.GetUsedLanguage().AttachOrbitDescription);
		}
	}
}
