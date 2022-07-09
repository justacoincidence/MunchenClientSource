using UnityEngine;

namespace UnchainedButtonAPI
{
	internal class QuickMenuSpacers : QuickMenuButtonBase
	{
		internal QuickMenuSpacers(QuickMenuNestedMenu menu)
		{
			buttonParentName = menu.GetMenuName();
			InitializeButtonRow(menu.GetGameObject().transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));
		}

		internal QuickMenuSpacers(QuickMenus menu)
		{
			buttonParentName = QuickMenuUtils.GetMenuUIPage(menu).field_Public_String_0;
			InitializeButtonRow(QuickMenuUtils.GetMenuUIPage(menu).transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));
		}

		internal QuickMenuSpacers(string parentMenu)
		{
			buttonParentName = parentMenu;
			InitializeButtonRow(QuickMenuUtils.GetQuickMenu().transform.Find(parentMenu + "/ScrollRect/Viewport/VerticalLayoutGroup"));
		}

		private void InitializeButtonRow(Transform parent)
		{
			buttonObject = Object.Instantiate(QuickMenuTemplates.GetSpacersTemplate(), parent);
			buttonObject.name = $"Spacers_8pt_{QuickMenuUtils.GetQuickMenuIdentifier()}{QuickMenuUtils.GetQuickMenuUniqueIdentifier()}";
			SetActive(active: true);
		}
	}
}
