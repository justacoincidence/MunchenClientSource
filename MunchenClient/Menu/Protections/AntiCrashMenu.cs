using System;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Patching.Patches;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace MunchenClient.Menu.Protections
{
	internal class AntiCrashMenu : QuickMenuNestedMenu
	{
		internal AntiCrashMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ProtectionsMenuName);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow4 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow5 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow6 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().WhitelistAvatarByID, delegate
			{
				GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().WhitelistAvatarByID, string.Empty, InputField.InputType.Standard, isNumeric: false, "Confirm", WhitelistAvatarByID);
			}, LanguageManager.GetUsedLanguage().WhitelistAvatarByIDDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().ExperimentalAvatarBlocker, Configuration.GetAntiCrashConfig().ExperimentalAvatarBlocker, delegate
			{
				Configuration.GetAntiCrashConfig().ExperimentalAvatarBlocker = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().ExperimentalAvatarBlockerDescription, delegate
			{
				Configuration.GetAntiCrashConfig().ExperimentalAvatarBlocker = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().ExperimentalAvatarBlockerDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().GlobalAvatarBlacklist, Configuration.GetAntiCrashConfig().GlobalAvatarBlacklist, delegate
			{
				Configuration.GetAntiCrashConfig().GlobalAvatarBlacklist = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().GlobalAvatarBlacklistDescription, delegate
			{
				Configuration.GetAntiCrashConfig().GlobalAvatarBlacklist = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().GlobalAvatarBlacklistDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AntiCrashEnablePublic, Configuration.GetAntiCrashConfig().AntiCrashEnablePublic, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnablePublic = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnablePublicDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnablePublic = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnablePublicDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AntiCrashEnableInvite, Configuration.GetAntiCrashConfig().AntiCrashEnableInvite, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableInvite = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableInviteDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableInvite = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableInviteDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AntiCrashEnableInvitePlus, Configuration.GetAntiCrashConfig().AntiCrashEnableInvitePlus, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableInvitePlus = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableInvitePlusDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableInvitePlus = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableInvitePlusDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AntiCrashEnableFriends, Configuration.GetAntiCrashConfig().AntiCrashEnableFriends, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableFriends = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableFriendsDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableFriends = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableFriendsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AntiCrashEnableFriendsPlus, Configuration.GetAntiCrashConfig().AntiCrashEnableFriendsPlus, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableFriendsPlus = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableFriendsPlusDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiCrashEnableFriendsPlus = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiCrashEnableFriendsPlusDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().AntiShaderCrash, Configuration.GetAntiCrashConfig().AntiShaderCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiShaderCrash = true;
				Configuration.SaveAntiCrashConfig();
				AntiShaderCrashHandler.ApplyAntiShaderConfig(state: true);
			}, LanguageManager.GetUsedLanguage().AntiShaderCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiShaderCrash = false;
				Configuration.SaveAntiCrashConfig();
				AntiShaderCrashHandler.ApplyAntiShaderConfig(state: false);
			}, LanguageManager.GetUsedLanguage().AntiShaderCrashDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().AntiAudioCrash, Configuration.GetAntiCrashConfig().AntiAudioCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAudioCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiAudioCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAudioCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiAudioCrashDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().AntiMeshCrash, Configuration.GetAntiCrashConfig().AntiMeshCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiMeshCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiMeshCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiMeshCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiMeshCrashDescription);
			new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().AntiMaterialCrash, Configuration.GetAntiCrashConfig().AntiMaterialCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiMaterialCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiMaterialCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiMaterialCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiMaterialCrashDescription);
			new QuickMenuToggleButton(parentRow4, LanguageManager.GetUsedLanguage().AntiFinalIKCrash, Configuration.GetAntiCrashConfig().AntiFinalIKCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiFinalIKCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiFinalIKCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiFinalIKCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiFinalIKCrashDescription);
			new QuickMenuToggleButton(parentRow4, LanguageManager.GetUsedLanguage().AntiClothCrash, Configuration.GetAntiCrashConfig().AntiClothCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiClothCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiClothCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiClothCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiClothCrashDescription);
			new QuickMenuToggleButton(parentRow4, LanguageManager.GetUsedLanguage().AntiParticleSystemCrash, Configuration.GetAntiCrashConfig().AntiParticleSystemCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiParticleSystemCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiParticleSystemCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiParticleSystemCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiParticleSystemCrashDescription);
			new QuickMenuToggleButton(parentRow4, LanguageManager.GetUsedLanguage().AntiDynamicBoneCrash, Configuration.GetAntiCrashConfig().AntiDynamicBoneCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiDynamicBoneCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiDynamicBoneCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiDynamicBoneCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiDynamicBoneCrashDescription);
			new QuickMenuToggleButton(parentRow5, LanguageManager.GetUsedLanguage().AntiLightSourceCrash, Configuration.GetAntiCrashConfig().AntiLightSourceCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiLightSourceCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiLightSourceCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiLightSourceCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiLightSourceCrashDescription);
			new QuickMenuToggleButton(parentRow5, LanguageManager.GetUsedLanguage().AntiBlendShapeCrash, Configuration.GetAntiCrashConfig().AntiBlendShapeCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiBlendShapeCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiBlendShapeCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiBlendShapeCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiBlendShapeCrashDescription);
			new QuickMenuToggleButton(parentRow5, LanguageManager.GetUsedLanguage().AntiAvatarLoadingCrash, Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash = true;
				Configuration.SaveAntiCrashConfig();
				AntiCrashPatch.PatchGameCloserExploit(state: true);
			}, LanguageManager.GetUsedLanguage().AntiAvatarLoadingCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash = false;
				Configuration.SaveAntiCrashConfig();
				AntiCrashPatch.PatchGameCloserExploit(state: false);
			}, LanguageManager.GetUsedLanguage().AntiAvatarLoadingCrashDescription);
			new QuickMenuToggleButton(parentRow5, LanguageManager.GetUsedLanguage().AntiAvatarAudioMixerCrash, Configuration.GetAntiCrashConfig().AntiAvatarAudioMixerCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAvatarAudioMixerCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiAvatarAudioMixerCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAvatarAudioMixerCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiAvatarAudioMixerCrashDescription);
			new QuickMenuToggleButton(parentRow6, LanguageManager.GetUsedLanguage().AntiPhysicsCrash, Configuration.GetAntiCrashConfig().AntiPhysicsCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiPhysicsCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiPhysicsCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiPhysicsCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiPhysicsCrashDescription);
			new QuickMenuToggleButton(parentRow6, LanguageManager.GetUsedLanguage().AntiInvalidFloatsCrash, Configuration.GetAntiCrashConfig().AntiInvalidFloatsCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiInvalidFloatsCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiInvalidFloatsCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiInvalidFloatsCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiInvalidFloatsCrashDescription);
			new QuickMenuToggleButton(parentRow6, LanguageManager.GetUsedLanguage().AntiAvatarDescriptorCrash, Configuration.GetAntiCrashConfig().AntiAvatarDescriptorCrash, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAvatarDescriptorCrash = true;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiAvatarDescriptorCrashDescription, delegate
			{
				Configuration.GetAntiCrashConfig().AntiAvatarDescriptorCrash = false;
				Configuration.SaveAntiCrashConfig();
			}, LanguageManager.GetUsedLanguage().AntiAvatarDescriptorCrashDescription);
			if (GeneralUtils.HasSpecialBenefits())
			{
				new QuickMenuToggleButton(parentRow6, LanguageManager.GetUsedLanguage().AntiAssetBundleCrash, Configuration.GetAntiCrashConfig().AntiAssetBundleCrash, delegate
				{
					Configuration.GetAntiCrashConfig().AntiAssetBundleCrash = true;
					Configuration.SaveAntiCrashConfig();
				}, LanguageManager.GetUsedLanguage().AntiAssetBundleCrashDescription, delegate
				{
					Configuration.GetAntiCrashConfig().AntiAssetBundleCrash = false;
					Configuration.SaveAntiCrashConfig();
				}, LanguageManager.GetUsedLanguage().AntiAssetBundleCrashDescription);
			}
		}

		private void WhitelistAvatarByID(string avatarId, List<KeyCode> pressedKeys, Text text)
		{
			ApiAvatar apiAvatar = new ApiAvatar();
			apiAvatar.id = avatarId.Trim();
			((ApiModel)apiAvatar).Get((Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)delegate(ApiContainer x)
			{
				AntiCrashUtils.ProcessAvatarWhitelist(x.Model.Cast<ApiAvatar>());
			}, (Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)delegate(ApiContainer x)
			{
				ConsoleUtils.Info("Whitelist", "Whitelist failed with error: " + x.Error, System.ConsoleColor.Gray, "WhitelistAvatarByID", 331);
				GeneralWrappers.AlertPopup("Whitelist", "Whitelist failed with error: " + x.Error);
			}, (Dictionary<string, BestHTTP.JSON.Json.Token>)null, false);
		}
	}
}
