using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Misc;
using MunchenClient.Utils;
using UnchainedButtonAPI;
using UnityEngine;

namespace MunchenClient.Menu.Fun
{
	internal class PortableMirrorMenu : QuickMenuNestedMenu
	{
		internal static QuickMenuIndicator currentSizeXIndicator;

		internal static QuickMenuIndicator currentSizeYIndicator;

		internal static QuickMenuToggleButton portableMirrorButton;

		internal PortableMirrorMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().PortableMirrorMenuName, LanguageManager.GetUsedLanguage().PortableMirrorMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ResizeCategory);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			portableMirrorButton = new QuickMenuToggleButton(parentRow, "Portable Mirror", GeneralUtils.portableMirror, delegate
			{
				PortableMirror.SpawnMirror(0, Configuration.GetGeneralConfig().PortableMirrorSizeX, Configuration.GetGeneralConfig().PortableMirrorSizeY, Configuration.GetGeneralConfig().PortableMirrorPickupable);
				GeneralUtils.portableMirror = true;
			}, "Mirror go brr", delegate
			{
				PortableMirror.RemoveMirror(0);
				GeneralUtils.portableMirror = false;
			}, "Mirror go brr");
			new QuickMenuToggleButton(parentRow, "Pickupable", Configuration.GetGeneralConfig().PortableMirrorPickupable, delegate
			{
				Configuration.GetGeneralConfig().PortableMirrorPickupable = true;
				Configuration.SaveGeneralConfig();
				Mirror mirror6 = PortableMirror.GetMirror(0);
				if (mirror6 != null)
				{
					mirror6.portableMirrorPickup.pickupable = true;
				}
			}, "", delegate
			{
				Configuration.GetGeneralConfig().PortableMirrorPickupable = false;
				Configuration.SaveGeneralConfig();
				Mirror mirror5 = PortableMirror.GetMirror(0);
				if (mirror5.portableMirrorPickup != null)
				{
					mirror5.portableMirrorPickup.pickupable = false;
				}
			}, "");
			currentSizeXIndicator = new QuickMenuIndicator(parentRow, "Size X", Configuration.GetGeneralConfig().PortableMirrorSizeX.ToString(), LanguageManager.GetUsedLanguage().RunSpeedIndicatorDescription);
			currentSizeYIndicator = new QuickMenuIndicator(parentRow, "Size Y", Configuration.GetGeneralConfig().PortableMirrorSizeY.ToString(), LanguageManager.GetUsedLanguage().WalkSpeedIndicatorDescription);
			new QuickMenuSingleButton(parentRow2, "X+", delegate
			{
				Configuration.GetGeneralConfig().PortableMirrorSizeX += 0.1f;
				Configuration.SaveGeneralConfig();
				currentSizeXIndicator.SetMainText(Configuration.GetGeneralConfig().PortableMirrorSizeX.ToString());
				Mirror mirror4 = PortableMirror.GetMirror(0);
				if (mirror4 != null)
				{
					mirror4.portableMirror.transform.localScale = new Vector3(Configuration.GetGeneralConfig().PortableMirrorSizeX, Configuration.GetGeneralConfig().PortableMirrorSizeY, 1f);
				}
			}, "");
			new QuickMenuSingleButton(parentRow2, "X-", delegate
			{
				Configuration.GetGeneralConfig().PortableMirrorSizeX -= 0.1f;
				Configuration.SaveGeneralConfig();
				currentSizeXIndicator.SetMainText(Configuration.GetGeneralConfig().PortableMirrorSizeX.ToString());
				Mirror mirror3 = PortableMirror.GetMirror(0);
				if (mirror3 != null)
				{
					mirror3.portableMirror.transform.localScale = new Vector3(Configuration.GetGeneralConfig().PortableMirrorSizeX, Configuration.GetGeneralConfig().PortableMirrorSizeY, 1f);
				}
			}, "");
			new QuickMenuSingleButton(parentRow2, "Y+", delegate
			{
				Configuration.GetGeneralConfig().PortableMirrorSizeY += 0.1f;
				Configuration.SaveGeneralConfig();
				currentSizeYIndicator.SetMainText(Configuration.GetGeneralConfig().PortableMirrorSizeY.ToString());
				Mirror mirror2 = PortableMirror.GetMirror(0);
				if (mirror2 != null)
				{
					mirror2.portableMirror.transform.localScale = new Vector3(Configuration.GetGeneralConfig().PortableMirrorSizeX, Configuration.GetGeneralConfig().PortableMirrorSizeY, 1f);
				}
			}, "");
			new QuickMenuSingleButton(parentRow2, "Y-", delegate
			{
				Configuration.GetGeneralConfig().PortableMirrorSizeY -= 0.1f;
				Configuration.SaveGeneralConfig();
				currentSizeYIndicator.SetMainText(Configuration.GetGeneralConfig().PortableMirrorSizeY.ToString());
				Mirror mirror = PortableMirror.GetMirror(0);
				if (mirror != null)
				{
					mirror.portableMirror.transform.localScale = new Vector3(Configuration.GetGeneralConfig().PortableMirrorSizeX, Configuration.GetGeneralConfig().PortableMirrorSizeY, 1f);
				}
			}, "");
		}
	}
}
