using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class MenuColorHandler : ModuleComponent
	{
		private static Color rainbowColor = Color.red;

		private static readonly List<Text> menuText = new List<Text>();

		private static readonly List<Image> menuItems = new List<Image>();

		private static readonly List<Image> menuItemsDimmed = new List<Image>();

		private static readonly List<Image> menuItemsDarker = new List<Image>();

		private static readonly List<Slider> menuItemsSliders = new List<Slider>();

		private static readonly List<Button> menuItemsButtons = new List<Button>();

		private static Button menuItemsRowButton;

		private static Image menuItemsInputRectangle;

		private static Image menuItemsInputRectanglePanel;

		private static Image menuItemsInputField;

		private static Text menuItemsInputSearchTitle;

		private static bool userInterfaceFound = false;

		private static bool menuFindingItems = false;

		private static bool menuColorReset = true;

		public static bool menuColorChanged = false;

		protected override string moduleName => "Menu Color Handler";

		internal override void OnUpdate()
		{
			if (!userInterfaceFound)
			{
				return;
			}
			if (!Configuration.GetGeneralConfig().ColorChangerEnable)
			{
				if (!menuColorReset)
				{
					menuColorReset = true;
					UpdateMenuColor(GeneralUtils.defaultMenuColor);
				}
				return;
			}
			menuColorReset = false;
			if (Configuration.GetGeneralConfig().ColorChangerRainbowMode)
			{
				Color.RGBToHSV(rainbowColor, out var H, out var S, out var V);
				rainbowColor = Color.HSVToRGB(H + Time.deltaTime * (Configuration.GetGeneralConfig().ColorChangerRainbowHyperMode ? 0.5f : 0.1f), S, V);
				UpdateMenuColor(rainbowColor);
			}
			else if (menuColorChanged)
			{
				menuColorChanged = false;
				UpdateMenuColor(Configuration.GetGeneralConfig().ColorChangerColor.GetColor());
			}
		}

		internal static void UpdateMenuColor(Color color)
		{
			Color color2 = new Color(color.r, color.g, color.b, 0.7f);
			Color color3 = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f, 0.9f);
			Color color4 = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, 0.9f);
			foreach (Image menuItem in menuItems)
			{
				menuItem.color = color2;
			}
			foreach (Image item in menuItemsDimmed)
			{
				item.color = color3;
			}
			foreach (Image item2 in menuItemsDarker)
			{
				item2.color = color4;
			}
			foreach (Text item3 in menuText)
			{
				item3.color = color;
			}
			Color color5 = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, 0.8f);
			menuItemsInputRectangle.color = color5;
			color.a = 0.8f;
			menuItemsInputRectanglePanel.GetComponent<Image>().color = color;
			color.a = 0.5f;
			menuItemsInputField.GetComponent<Image>().color = color;
			menuItemsInputSearchTitle.color = color;
			color5.a = 0.5f;
			ColorBlock colorBlock = default(ColorBlock);
			colorBlock.colorMultiplier = 1f;
			colorBlock.disabledColor = Color.grey;
			colorBlock.highlightedColor = color * 1.5f;
			colorBlock.normalColor = color / 1.5f;
			colorBlock.pressedColor = Color.grey * 1.5f;
			colorBlock.fadeDuration = 0.1f;
			ColorBlock colors = colorBlock;
			ColorBlock colors2 = colorBlock;
			menuItemsRowButton.colors = colors;
			color.a = 0.5f;
			color5.a = 1f;
			colors2.normalColor = color5;
			foreach (Slider menuItemsSlider in menuItemsSliders)
			{
				if (menuItemsSlider != null)
				{
					menuItemsSlider.colors = colors2;
				}
			}
			foreach (Button menuItemsButton in menuItemsButtons)
			{
				if (menuItemsButton != null)
				{
					menuItemsButton.colors = colors;
				}
			}
		}

		internal static void FindMenuItems()
		{
			if (!menuFindingItems)
			{
				MelonCoroutines.Start(FindMenuItemsEnumerator());
			}
		}

		private static IEnumerator FindMenuItemsEnumerator()
		{
			menuFindingItems = true;
			userInterfaceFound = false;
			Transform userInterface = GameObject.Find("UserInterface").transform;
			Transform menuContent = userInterface.Find("MenuContent").transform;
			Transform menuSafetyScreen = menuContent.transform.Find("Screens/Settings_Safety/");
			menuItems.Add(menuSafetyScreen.Find("_Description_SafetyLevel").GetComponent<Image>());
			menuItems.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_Custom/ON").GetComponent<Image>());
			menuItems.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_None/ON").GetComponent<Image>());
			menuItems.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_Normal/ON").GetComponent<Image>());
			menuItems.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_Maxiumum/ON").GetComponent<Image>());
			yield return new WaitForEndOfFrame();
			Transform menuPopups = menuContent.transform.Find("Popups/");
			menuItems.Add(menuPopups.Find("InputKeypadPopup/Rectangle/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("InputKeypadPopup/InputField").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("StandardPopupV2/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("StandardPopup/InnerDashRing").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("StandardPopup/RingGlow").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("UpdateStatusPopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("UpdateStatusPopup/Popup/InputFieldStatus").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("InputPopup/InputField").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("AdvancedSettingsPopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("AddToFavoriteListPopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("EditFavoriteListPopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("PerformanceSettingsPopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("AlertPopup/Lighter").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("RoomInstancePopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("ReportWorldPopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("ReportUserPopup/Popup/Panel").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("SearchOptionsPopup/Popup/Panel (1)").GetComponent<Image>());
			yield return new WaitForEndOfFrame();
			foreach (Transform transform in from x in menuContent.GetComponentsInChildren<Transform>(includeInactive: true)
				where x != null && x.name.Contains("Panel_Header")
				select x)
			{
				foreach (Image item in transform.GetComponentsInChildren<Image>())
				{
					menuItems.Add(item);
				}
				yield return new WaitForEndOfFrame();
			}
			foreach (Transform transform2 in from x in menuContent.GetComponentsInChildren<Transform>(includeInactive: true)
				where x != null && x.name.Contains("Handle")
				select x)
			{
				foreach (Image item2 in transform2.GetComponentsInChildren<Image>())
				{
					menuItems.Add(item2);
				}
				yield return new WaitForEndOfFrame();
			}
			menuItems.Add(menuPopups.Find("LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").GetComponent<Image>());
			menuItems.Add(menuPopups.Find("LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").GetComponent<Image>());
			yield return new WaitForEndOfFrame();
			menuItemsDimmed.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_Custom/ON/TopPanel_SafetyLevel").GetComponent<Image>());
			menuItemsDimmed.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_None/ON/TopPanel_SafetyLevel").GetComponent<Image>());
			menuItemsDimmed.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_Normal/ON/TopPanel_SafetyLevel").GetComponent<Image>());
			menuItemsDimmed.Add(menuSafetyScreen.Find("_Buttons_SafetyLevel/Button_Maxiumum/ON/TopPanel_SafetyLevel").GetComponent<Image>());
			yield return new WaitForEndOfFrame();
			foreach (Transform transform3 in from x in menuContent.GetComponentsInChildren<Transform>(includeInactive: true)
				where x != null && x.name.Contains("Fill")
				select x)
			{
				foreach (Image item3 in transform3.GetComponentsInChildren<Image>())
				{
					menuItemsDimmed.Add(item3);
				}
				yield return new WaitForEndOfFrame();
			}
			menuItemsDarker.Add(menuPopups.Find("InputKeypadPopup/Rectangle").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("StandardPopupV2/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("StandardPopup/Rectangle").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("StandardPopup/MidRing").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("UpdateStatusPopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("AdvancedSettingsPopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("AddToFavoriteListPopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("EditFavoriteListPopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("PerformanceSettingsPopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("RoomInstancePopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("RoomInstancePopup/Popup/BorderImage (1)").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("ReportWorldPopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("ReportUserPopup/Popup/BorderImage").GetComponent<Image>());
			menuItemsDarker.Add(menuPopups.Find("SearchOptionsPopup/Popup/BorderImage").GetComponent<Image>());
			yield return new WaitForEndOfFrame();
			foreach (Transform transform4 in from x in menuContent.GetComponentsInChildren<Transform>(includeInactive: true)
				where x != null && x.name.Contains("Background")
				select x)
			{
				foreach (Image item4 in transform4.GetComponentsInChildren<Image>())
				{
					menuItemsDarker.Add(item4);
				}
				yield return new WaitForEndOfFrame();
			}
			Transform menuItemsInput = menuPopups.Find("InputPopup");
			foreach (Text item5 in menuItemsInput.Find("Keyboard/Keys").GetComponentsInChildren<Text>(includeInactive: true))
			{
				menuText.Add(item5);
			}
			yield return new WaitForEndOfFrame();
			foreach (Text item6 in menuPopups.Find("InputKeypadPopup/Keyboard/Keys").GetComponentsInChildren<Text>(includeInactive: true))
			{
				menuText.Add(item6);
			}
			yield return new WaitForEndOfFrame();
			menuItemsInputRectangle = menuItemsInput.Find("Rectangle").GetComponent<Image>();
			menuItemsInputRectanglePanel = menuItemsInput.Find("Rectangle/Panel").GetComponent<Image>();
			Transform menuItemsInputAlternate = menuContent.transform.Find("Backdrop/Header/Tabs/ViewPort/Content/Search");
			menuItemsInputField = menuItemsInputAlternate.Find("InputField").GetComponent<Image>();
			menuItemsInputSearchTitle = menuItemsInputAlternate.Find("SearchTitle").GetComponent<Text>();
			yield return new WaitForEndOfFrame();
			menuItemsRowButton = menuContent.GetComponentsInChildren<Transform>(includeInactive: true).FirstOrDefault((Transform x) => x != null && x.name == "Row:4 Column:0").GetComponent<Button>();
			foreach (Slider slider in menuContent.GetComponentsInChildren<Slider>(includeInactive: true))
			{
				menuItemsSliders.Add(slider);
			}
			yield return new WaitForEndOfFrame();
			foreach (Button button in menuContent.GetComponentsInChildren<Button>(includeInactive: true))
			{
				menuItemsButtons.Add(button);
			}
			yield return new WaitForEndOfFrame();
			userInterfaceFound = true;
			menuColorChanged = true;
			menuFindingItems = false;
		}
	}
}
