using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using TMPro;
using UnchainedButtonAPI;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace MunchenClient.Menu.Protections
{
	internal class PortalsMenu : QuickMenuNestedMenu
	{
		internal PortalsMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().PortalsMenuName, LanguageManager.GetUsedLanguage().PortalsMenuDescription)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().RemoveAllPortals, delegate
			{
				DeleteAllPortals(informHud: false);
			}, LanguageManager.GetUsedLanguage().RemoveAllPortalsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().PortalConfirmation, Configuration.GetGeneralConfig().PortalsProtectionsConfirmation, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsConfirmation = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PortalConfirmationDescription, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsConfirmation = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().PortalConfirmationDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AutoDeleteOtherPortals, Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteNonFriends, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteNonFriends = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoDeleteOtherPortalsDescription, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteNonFriends = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoDeleteOtherPortalsDescription);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().AutoDeleteFriendsPortals, Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteFriends, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteFriends = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoDeleteFriendsPortalsDescription, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsAutoDeleteFriends = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoDeleteFriendsPortalsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AutoDeleteDistantPlacedPortals, Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlaced, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlaced = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoDeleteDistantPlacedPortalsDescription, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlaced = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AutoDeleteDistantPlacedPortalsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().AllowFriendsDistantPlacedPortals, Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlacedAllowFriends, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlacedAllowFriends = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AllowFriendsDistantPlacedPortalsDescription, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteDistantPlacedAllowFriends = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().AllowFriendsDistantPlacedPortalsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().DeleteUnusedPortals, Configuration.GetGeneralConfig().PortalsProtectionsDeleteUnused, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteUnused = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DeleteUnusedPortalsDescription, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteUnused = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DeleteUnusedPortalsDescription);
			new QuickMenuToggleButton(parentRow2, LanguageManager.GetUsedLanguage().DeleteBadlyConfiguredPortals, Configuration.GetGeneralConfig().PortalsProtectionsDeleteBadlyConfigured, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteBadlyConfigured = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DeleteBadlyConfiguredPortalsDescription, delegate
			{
				Configuration.GetGeneralConfig().PortalsProtectionsDeleteBadlyConfigured = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().DeleteBadlyConfiguredPortalsDescription);
		}

		internal static void DeleteAllPortals(bool informHud)
		{
			PortalInternal[] array = Resources.FindObjectsOfTypeAll<PortalInternal>();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] == null))
				{
					TextMeshPro componentInChildren = array[i].GetComponentInChildren<TextMeshPro>();
					if ((componentInChildren.text.Contains("public") || componentInChildren.text.Contains("invite") || componentInChildren.text.Contains("friends")) && !componentInChildren.text.Contains(APIUser.CurrentUser.displayName))
					{
						Networking.Destroy(array[i].gameObject);
						num++;
					}
				}
			}
			if (informHud)
			{
				if (num == 1)
				{
					GeneralUtils.InformHudText("AntiPortal", $"Deleted {num} portal", logToConsole: true);
				}
				else if (num > 1)
				{
					GeneralUtils.InformHudText("AntiPortal", $"Deleted {num} portals", logToConsole: true);
				}
			}
			else if (num == 1)
			{
				QuickMenuUtils.ShowAlert($"Deleted {num} portal");
			}
			else
			{
				QuickMenuUtils.ShowAlert($"Deleted {num} portals");
			}
		}
	}
}
