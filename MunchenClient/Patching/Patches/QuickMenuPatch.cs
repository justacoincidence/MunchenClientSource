using System.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using UnchainedButtonAPI;
using VRC.UI.Elements;

namespace MunchenClient.Patching.Patches
{
	internal class QuickMenuPatch : PatchComponent
	{
		private static string lastOpenedMenu = string.Empty;

		protected override string patchName => "QuickMenuPatch";

		internal override void OnInitializeOnUIInit()
		{
			InitializeLocalPatchHandler(typeof(QuickMenuPatch));
			PatchMethod(typeof(VRC.UI.Elements.QuickMenu).GetMethod("OnEnable"), null, GetLocalPatch("OnQuickMenuOpenPatch"));
			PatchMethod(typeof(VRC.UI.Elements.QuickMenu).GetMethod("OnDisable"), null, GetLocalPatch("OnQuickMenuClosePatch"));
		}

		private static void OnQuickMenuOpenPatch()
		{
			foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
			{
				if (!playerCaching.Value.isLocalPlayer && playerCaching.Value.customNameplateTransform != null)
				{
					playerCaching.Value.customNameplateTransform.localPosition = MiscUtils.GetNameplateOffset(open: true);
				}
			}
			if (Configuration.GetGeneralConfig().NameplateRankColor)
			{
				PlayerUtils.RefreshAllPlayerColorCache();
			}
			if (Configuration.GetGeneralConfig().PersistentQuickMenu && !string.IsNullOrEmpty(lastOpenedMenu))
			{
				QuickMenuUtils.OpenMenu(lastOpenedMenu);
			}
			CameraFeaturesHandler.ChangeCameraClipping(nearClipping: false);
			GeneralUtils.isQuickMenuOpen = true;
		}

		private static void OnQuickMenuClosePatch()
		{
			foreach (KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
			{
				if (!playerCaching.Value.isLocalPlayer && playerCaching.Value.customNameplateTransform != null)
				{
					playerCaching.Value.customNameplateTransform.localPosition = MiscUtils.GetNameplateOffset(open: false);
				}
			}
			lastOpenedMenu = QuickMenuUtils.GetQuickMenu().prop_MenuStateController_0.field_Private_UIPage_0.field_Public_String_0;
			CameraFeaturesHandler.ChangeCameraClipping(Configuration.GetGeneralConfig().MinimumCameraClippingDistance);
			GeneralUtils.isQuickMenuOpen = false;
		}
	}
}
