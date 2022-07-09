using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using UnchainedButtonAPI;
using UnityEngine;

namespace MunchenClient.Menu.Player
{
	internal class FlashlightBoneSelectorMenu : QuickMenuNestedMenu
	{
		internal FlashlightBoneSelectorMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().FlashlightBoneSelectorMenuName, LanguageManager.GetUsedLanguage().FlashlightBoneSelectorMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachHead, delegate
			{
				Configuration.GetGeneralConfig().FlashlightAttachedBone = HumanBodyBones.Head;
				FlashlightHandler.SetupFlashlightBone();
			}, LanguageManager.GetUsedLanguage().AttachHead);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachChest, delegate
			{
				Configuration.GetGeneralConfig().FlashlightAttachedBone = HumanBodyBones.Chest;
				FlashlightHandler.SetupFlashlightBone();
			}, LanguageManager.GetUsedLanguage().AttachChest);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachRightHand, delegate
			{
				Configuration.GetGeneralConfig().FlashlightAttachedBone = HumanBodyBones.RightHand;
				FlashlightHandler.SetupFlashlightBone();
			}, LanguageManager.GetUsedLanguage().AttachRightHand);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AttachLeftHand, delegate
			{
				Configuration.GetGeneralConfig().FlashlightAttachedBone = HumanBodyBones.LeftHand;
				FlashlightHandler.SetupFlashlightBone();
			}, LanguageManager.GetUsedLanguage().AttachLeftHand);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().AttachRightFoot, delegate
			{
				Configuration.GetGeneralConfig().FlashlightAttachedBone = HumanBodyBones.RightFoot;
				FlashlightHandler.SetupFlashlightBone();
			}, LanguageManager.GetUsedLanguage().AttachRightFoot);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().AttachLeftFoot, delegate
			{
				Configuration.GetGeneralConfig().FlashlightAttachedBone = HumanBodyBones.LeftFoot;
				FlashlightHandler.SetupFlashlightBone();
			}, LanguageManager.GetUsedLanguage().AttachLeftFoot);
		}
	}
}
