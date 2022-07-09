using Il2CppSystem.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;

namespace UnchainedButtonAPI
{
	internal class QuickMenuNestedMenu : QuickMenuButtonBase
	{
		internal UIPage uiPage;

		private string nestedMenuName;

		private bool menuAccessible;

		private string menuAccessibleError;

		internal QuickMenuNestedMenu(QuickMenuButtonRow parentRow, string text, string tooltip, bool createButtonOnParent = true)
		{
			QuickMenuNestedMenu quickMenuNestedMenu = this;
			buttonParentName = parentRow.GetParentMenuName();
			InitializeNestedMenu(text);
			buttonHandler = buttonObject.transform.Find("Header_H1/LeftItemContainer/Button_Back").GetComponent<Button>();
			buttonHandler.gameObject.SetActive(value: true);
			SetAction(delegate
			{
				parentRow.parentCustomMenu?.ShowMenu();
				QuickMenuUtils.OpenMenu(parentRow.GetParentMenuName(), clearStackPage: true);
			});
			if (createButtonOnParent)
			{
				new QuickMenuSingleButton(parentRow, text, delegate
				{
					quickMenuNestedMenu.ShowMenu();
				}, tooltip);
			}
		}

		internal QuickMenuNestedMenu(string text)
		{
			InitializeNestedMenu(text);
			buttonParentName = nestedMenuName;
		}

		private void InitializeNestedMenu(string text)
		{
			int quickMenuUniqueIdentifier = QuickMenuUtils.GetQuickMenuUniqueIdentifier();
			buttonObject = Object.Instantiate(QuickMenuTemplates.GetNestedMenuTemplate(), QuickMenuUtils.GetQuickMenu().transform.Find("Container/Window/QMParent"));
			buttonObject.name = $"Menu_{QuickMenuUtils.GetQuickMenuIdentifier()}{text}{quickMenuUniqueIdentifier}";
			buttonObject.transform.SetSiblingIndex(QuickMenuUtils.GetFirstModalIndex());
			buttonText = buttonObject.transform.Find("Header_H1/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUI>();
			nestedMenuName = $"QuickMenu{QuickMenuUtils.GetQuickMenuIdentifier()}{text}{quickMenuUniqueIdentifier}";
			GameObject gameObject = buttonObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").gameObject;
			for (int i = 0; i < gameObject.transform.GetChildCount(); i++)
			{
				Object.Destroy(gameObject.transform.GetChild(i).gameObject);
			}
			Object.DestroyImmediate(buttonObject.GetComponent<LaunchPadQMMenu>());
			uiPage = buttonObject.AddComponent<UIPage>();
			uiPage.field_Public_String_0 = nestedMenuName;
			uiPage.field_Private_Boolean_1 = true;
			uiPage.field_Private_List_1_UIPage_0 = new List<UIPage>();
			uiPage.field_Private_List_1_UIPage_0.Add(uiPage);
			QuickMenuMunchenPage quickMenuMunchenPage = buttonObject.AddComponent<QuickMenuMunchenPage>();
			quickMenuMunchenPage.uiPage = uiPage;
			QuickMenuUtils.AddMenuToController(this);
			menuAccessible = true;
			SetMenuText(text);
			SetActive(active: false);
		}

		internal void SetMenuText(string text)
		{
			buttonText.text = text;
		}

		internal void SetMenuAccessibility(bool state, string accessError)
		{
			menuAccessible = state;
			menuAccessibleError = accessError;
		}

		internal string GetMenuName()
		{
			return nestedMenuName;
		}

		internal void ShowMenu()
		{
			if (!menuAccessible)
			{
				QuickMenuUtils.ShowAlert(menuAccessibleError);
				return;
			}
			if (!QuickMenuUtils.GetQuickMenu().prop_MenuStateController_0.field_Private_UIPage_0.field_Public_String_0.Contains("Player Options"))
			{
				for (int i = 1; i < uiPage.field_Private_List_1_UIPage_0.Count; i++)
				{
					if (uiPage.field_Private_List_1_UIPage_0[i].field_Public_String_0 == "QuickMenuHoveredUser")
					{
						uiPage.field_Private_List_1_UIPage_0[i].gameObject.SetActive(value: false);
						uiPage.field_Private_List_1_UIPage_0.RemoveAt(i);
					}
					else if (uiPage.field_Private_List_1_UIPage_0[i].field_Public_String_0 == "QuickMenuSelectedUserLocal")
					{
						uiPage.field_Private_List_1_UIPage_0[i].gameObject.SetActive(value: false);
						uiPage.field_Private_List_1_UIPage_0.RemoveAt(i);
						uiPage.gameObject.SetActive(value: true);
					}
				}
			}
			QuickMenuUtils.OpenMenu(this, clearStackPage: true);
			OnMenuShownCallback();
		}

		internal void OnMenuUnshown()
		{
		}

		internal virtual void OnMenuShownCallback()
		{
		}
	}
}
