using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;

namespace UnchainedButtonAPI
{
	internal class QuickMenuPageButton : QuickMenuButtonBase
	{
		private MenuTab menuTab;

		internal QuickMenuPageButton(QuickMenuNestedMenu menu, string text, string tooltip, Sprite icon)
		{
			InitializePageButton(QuickMenuUtils.GetQuickMenu().transform.Find("Container/Window/Page_Buttons_QM/HorizontalLayoutGroup"), menu.GetMenuName(), text, tooltip, icon);
			SetAction(delegate
			{
				menu.ShowMenu();
			});
		}

		internal QuickMenuPageButton(string menuName, string text, string tooltip, Sprite icon)
		{
			InitializePageButton(QuickMenuUtils.GetQuickMenu().transform.Find("Container/Window/Page_Buttons_QM/HorizontalLayoutGroup"), "QuickMenu" + menuName, text, tooltip, icon);
		}

		private void InitializePageButton(Transform parent, string menuName, string text, string tooltip, Sprite icon)
		{
			buttonObject = Object.Instantiate(QuickMenuTemplates.GetPageButtonTemplate(), parent);
			buttonObject.name = $"Page_{QuickMenuUtils.GetQuickMenuIdentifier()}{text}{QuickMenuUtils.GetQuickMenuUniqueIdentifier()}";
			buttonHandler = buttonObject.GetComponent<Button>();
			buttonTooltip = buttonObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
			menuTab = buttonObject.GetComponent<MenuTab>();
			menuTab.field_Public_String_0 = menuName;
			GameObject gameObject = buttonObject.transform.Find("Icon").gameObject;
			Object.Destroy(gameObject.GetComponent<StyleElement>());
			buttonBackground = gameObject.GetComponent<Image>();
			buttonBackground.color = Color.white;
			buttonObject.transform.Find("Badge").gameObject.SetActive(value: false);
			SetToolTip(tooltip);
			SetPageIcon(icon);
			SetActive(active: true);
		}

		internal void SetPageIcon(Sprite icon)
		{
			buttonBackground.sprite = icon;
			buttonBackground.overrideSprite = icon;
		}
	}
}
