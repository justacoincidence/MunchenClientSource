using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Core.Compatibility;
using MunchenClient.Menu.Others;
using MunchenClient.Menu.Player;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using RootMotion.FinalIK;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRCSDK2;

namespace MunchenClient.Menu
{
	internal class PlayerMenu : QuickMenuNestedMenu
	{
		private readonly QuickMenuNestedMenu lovenseMenu;

		private readonly QuickMenuToggleButton fbtSavingButton;

		internal static QuickMenuToggleButton wallhackButton;

		internal static QuickMenuToggleButton attachToPlayerButton;

		internal static QuickMenuToggleButton localAvatarCloneButton;

		internal static GameObject localAvatarClone;

		internal PlayerMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().PlayerMenuName, LanguageManager.GetUsedLanguage().PlayerMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			new GlobalDynamicBonesMenu(quickMenuButtonRow);
			new NameplateMenu(quickMenuButtonRow);
			lovenseMenu = new LovenseMenu(quickMenuButtonRow);
			if (!GeneralUtils.IsBetaClient())
			{
				lovenseMenu.SetMenuAccessibility(state: false, "Can't access menu - Under construction (coming soon)");
			}
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().ChangeAvatarByID, delegate
			{
				GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().ChangeAvatarByID, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, ChangeAvatarByID);
			}, LanguageManager.GetUsedLanguage().ChangeAvatarByIDDescription);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow quickMenuButtonRow2 = new QuickMenuButtonRow(this);
			fbtSavingButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().FBTCalibrationSaving, !CompatibilityLayer.IsFBTSaverInstalled() && !CompatibilityLayer.IsIKTweaksInstalled() && Configuration.GetAvatarCalibrationsConfig().SaveCalibrations, delegate
			{
				if (!CompatibilityLayer.IsFBTSaverInstalled() && !CompatibilityLayer.IsIKTweaksInstalled())
				{
					Configuration.GetAvatarCalibrationsConfig().SaveCalibrations = true;
					Configuration.SaveAvatarCalibrationsConfig();
				}
				else
				{
					fbtSavingButton.SetToggleState(state: false);
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().FBTCalibrationSavingError);
				}
			}, LanguageManager.GetUsedLanguage().FBTCalibrationSavingDescription, delegate
			{
				Configuration.GetAvatarCalibrationsConfig().SaveCalibrations = false;
				Configuration.SaveAvatarCalibrationsConfig();
			}, LanguageManager.GetUsedLanguage().FBTCalibrationSavingDescription);
			wallhackButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PlayerWallhack, Configuration.GetGeneralConfig().PlayerWallhack, delegate
			{
				Configuration.GetGeneralConfig().PlayerWallhack = true;
				Configuration.SaveGeneralConfig();
				ActionWheelMenu.wallhackButton.SetButtonText("Wallhack: <color=green>On");
				GeneralWrappers.ApplyAllPlayerWallhack(state: true);
			}, LanguageManager.GetUsedLanguage().PlayerWallhackDescription, delegate
			{
				Configuration.GetGeneralConfig().PlayerWallhack = false;
				Configuration.SaveGeneralConfig();
				ActionWheelMenu.wallhackButton.SetButtonText("Wallhack: <color=red>Off");
				GeneralWrappers.ApplyAllPlayerWallhack(state: false);
			}, LanguageManager.GetUsedLanguage().PlayerWallhackDescription);
			attachToPlayerButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AttachToPlayer, PlayerTargetHandler.attachToPlayer, delegate
			{
				PlayerTargetHandler.attachToPlayer = true;
			}, LanguageManager.GetUsedLanguage().AttachToPlayerDescription, delegate
			{
				PlayerTargetHandler.attachToPlayer = false;
				GeneralUtils.flight = false;
				GeneralUtils.ToggleFlight(state: false);
			}, LanguageManager.GetUsedLanguage().AttachToPlayerDescription);
			localAvatarCloneButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().LocalAvatarClone, GeneralUtils.localAvatarClone, delegate
			{
				GeneralUtils.localAvatarClone = true;
				CreateLocalAvatarClone();
			}, LanguageManager.GetUsedLanguage().LocalAvatarCloneDescription, delegate
			{
				GeneralUtils.localAvatarClone = false;
				Object.DestroyImmediate(localAvatarClone);
			}, LanguageManager.GetUsedLanguage().LocalAvatarCloneDescription);
			new AvatarHistoryMenu(quickMenuButtonRow2);
			new FlashlightMenu(quickMenuButtonRow2);
			new QuickMenuToggleButton(quickMenuButtonRow2, "Hidden Avatar Scaler", Configuration.GetGeneralConfig().HiddenAvatarScaler, delegate
			{
				Configuration.GetGeneralConfig().HiddenAvatarScaler = true;
				Configuration.SaveGeneralConfig();
			}, string.Empty, delegate
			{
				Configuration.GetGeneralConfig().HiddenAvatarScaler = false;
				Configuration.SaveGeneralConfig();
			}, string.Empty);
		}

		private void CreateLocalAvatarClone()
		{
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			localAvatarClone = Object.Instantiate(localPlayerInformation.GetAvatar());
			localAvatarClone.name = "Munchen Local Avatar Clone";
			localAvatarClone.transform.position = localPlayerInformation.GetAvatar().transform.position;
			localAvatarClone.transform.rotation = localPlayerInformation.GetAvatar().transform.rotation;
			foreach (Transform componentsInChild in localAvatarClone.GetComponentsInChildren<Transform>(includeInactive: true))
			{
				componentsInChild.gameObject.layer = 0;
				if ((double)componentsInChild.localScale.x < 0.001 && (double)componentsInChild.localScale.y < 0.001 && (double)componentsInChild.localScale.z < 0.001)
				{
					componentsInChild.localScale = new Vector3(1f, 1f, 1f);
				}
			}
			Object.DestroyImmediate(localAvatarClone.GetComponent<VRC_AvatarDescriptor>());
			Object.DestroyImmediate(localAvatarClone.GetComponent<PipelineManager>());
			Object.DestroyImmediate(localAvatarClone.GetComponent<MonoBehaviourPrivateObUnique>());
			Object.DestroyImmediate(localAvatarClone.GetComponent<DynamicBoneController>());
			Object.DestroyImmediate(localAvatarClone.GetComponent<VRIK>());
			Object.DestroyImmediate(localAvatarClone.GetComponent<FullBodyBipedIK>());
			Object.DestroyImmediate(localAvatarClone.GetComponent<LimbIK>());
			System.Collections.Generic.List<DynamicBone> list = MiscUtils.FindAllComponentsInGameObject<DynamicBone>(localAvatarClone);
			System.Collections.Generic.List<DynamicBoneCollider> list2 = MiscUtils.FindAllComponentsInGameObject<DynamicBoneCollider>(localAvatarClone);
			System.Collections.Generic.List<AudioSource> list3 = MiscUtils.FindAllComponentsInGameObject<AudioSource>(localAvatarClone);
			foreach (DynamicBone item in list)
			{
				Object.DestroyImmediate(item);
			}
			foreach (DynamicBoneCollider item2 in list2)
			{
				Object.DestroyImmediate(item2);
			}
			foreach (AudioSource item3 in list3)
			{
				if ((item3.isPlaying || item3.playOnAwake) && item3.enabled && item3.gameObject.activeInHierarchy)
				{
					item3.playOnAwake = false;
					item3.Stop();
				}
			}
		}

		private void ChangeAvatarByID(string avatarId, Il2CppSystem.Collections.Generic.List<KeyCode> pressedKeys, Text text)
		{
			PlayerUtils.ChangePlayerAvatar(avatarId.Trim(), logErrorOnHud: true);
		}
	}
}
