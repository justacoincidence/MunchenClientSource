using UnityEngine;

namespace UnchainedButtonAPI
{
	internal class QuickMenuButtonRow : QuickMenuButtonBase
	{
		internal readonly QuickMenuNestedMenu parentCustomMenu;

		internal QuickMenuButtonRow(QuickMenuNestedMenu menu)
		{
			parentCustomMenu = menu;
			buttonParentName = menu.GetMenuName();
			InitializeButtonRow(menu.GetGameObject().transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));
		}

		internal QuickMenuButtonRow(QuickMenus menu)
		{
			buttonParentName = QuickMenuUtils.GetMenuUIPage(menu).field_Public_String_0;
			InitializeButtonRow(QuickMenuUtils.GetMenuUIPage(menu).transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));
		}

		internal QuickMenuButtonRow(string parentMenu)
		{
			buttonParentName = parentMenu;
			InitializeButtonRow(QuickMenuUtils.GetQuickMenu().transform.Find(parentMenu + "/ScrollRect/Viewport/VerticalLayoutGroup"));
		}

		private void InitializeButtonRow(Transform parent)
		{
			buttonObject = Object.Instantiate(QuickMenuTemplates.GetButtonRowTemplate(), parent);
			buttonObject.name = $"Buttons_{QuickMenuUtils.GetQuickMenuIdentifier()}{QuickMenuUtils.GetQuickMenuUniqueIdentifier()}";
			for (int i = 0; i < buttonObject.transform.GetChildCount(); i++)
			{
				Object.Destroy(buttonObject.transform.GetChild(i).gameObject);
			}
			SetActive(active: true);
		}
	}
}
