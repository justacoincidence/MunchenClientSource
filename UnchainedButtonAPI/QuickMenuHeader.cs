using TMPro;
using UnityEngine;

namespace UnchainedButtonAPI
{
	internal class QuickMenuHeader : QuickMenuButtonBase
	{
		internal QuickMenuHeader(QuickMenuNestedMenu menu, string text)
		{
			buttonParentName = menu.GetParentMenuName();
			InitializeHeader(menu.GetGameObject().transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"), text);
		}

		internal QuickMenuHeader(QuickMenus menu, string text)
		{
			buttonParentName = QuickMenuUtils.GetMenuUIPage(menu).field_Public_String_0;
			InitializeHeader(QuickMenuUtils.GetMenuUIPage(menu).transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"), text);
		}

		internal QuickMenuHeader(string parentMenu, string text)
		{
			buttonParentName = parentMenu;
			InitializeHeader(QuickMenuUtils.GetQuickMenu().transform.Find(parentMenu + "/ScrollRect/Viewport/VerticalLayoutGroup"), text);
		}

		private void InitializeHeader(Transform parent, string text)
		{
			buttonObject = Object.Instantiate(QuickMenuTemplates.GetHeaderTemplate(), parent);
			buttonObject.name = $"Header_{QuickMenuUtils.GetQuickMenuIdentifier()}{text}{QuickMenuUtils.GetQuickMenuUniqueIdentifier()}";
			GameObject gameObject = buttonObject.transform.Find("LeftItemContainer/Text_Title").gameObject;
			gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(1000f, -23f);
			buttonText = gameObject.GetComponent<TextMeshProUGUI>();
			SetHeaderText(text);
			SetActive(active: true);
		}

		internal void SetHeaderText(string text)
		{
			buttonText.text = text;
		}
	}
}
