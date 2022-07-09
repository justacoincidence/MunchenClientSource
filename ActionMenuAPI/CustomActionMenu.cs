using System;
using System.Collections.Generic;
using Il2CppSystem;
using MunchenClient.Config;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace ActionMenuAPI
{
	internal static class CustomActionMenu
	{
		internal enum ActionMenuBaseMenu
		{
			MainMenu = 1
		}

		internal class ActionMenuPage
		{
			internal List<ActionMenuButton> buttons = new List<ActionMenuButton>();

			internal ActionMenuPage previousPage;

			internal ActionMenuButton menuEntryButton;

			internal ActionMenuPage(ActionMenuBaseMenu baseMenu, string buttonText, Texture2D buttonIcon = null)
			{
				if (baseMenu == ActionMenuBaseMenu.MainMenu)
				{
					menuEntryButton = new ActionMenuButton(ActionMenuBaseMenu.MainMenu, buttonText, delegate
					{
						OpenMenu();
					}, buttonIcon);
				}
			}

			internal ActionMenuPage(ActionMenuPage basePage, string buttonText, Texture2D buttonIcon = null)
			{
				previousPage = basePage;
				menuEntryButton = new ActionMenuButton(previousPage, buttonText, delegate
				{
					OpenMenu();
				}, buttonIcon);
			}

			internal void OpenMenu()
			{
				GetActionMenuOpener().field_Public_ActionMenu_0.Method_Public_Page_Action_Action_Texture2D_String_0((System.Action)delegate
				{
					foreach (ActionMenuButton button in buttons)
					{
						PedalOption pedalOption = GetActionMenuOpener().field_Public_ActionMenu_0.Method_Private_PedalOption_0();
						pedalOption.prop_String_0 = button.buttonText;
						pedalOption.field_Public_Func_1_Boolean_0 = DelegateSupport.ConvertDelegate<Il2CppSystem.Func<bool>>(button.buttonAction);
						if (button.buttonIcon != null)
						{
							pedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(button.buttonIcon);
						}
						button.currentPedalOption = pedalOption;
					}
				});
			}
		}

		internal class ActionMenuButton
		{
			internal PedalOption currentPedalOption;

			internal string buttonText;

			internal Texture2D buttonIcon;

			internal System.Func<bool> buttonAction;

			internal ActionMenuButton(ActionMenuBaseMenu baseMenu, string text, System.Action action, Texture2D icon = null)
			{
				buttonText = text;
				buttonIcon = icon;
				buttonAction = delegate
				{
					action();
					return true;
				};
				if (baseMenu == ActionMenuBaseMenu.MainMenu)
				{
					mainMenuButtons.Add(this);
				}
			}

			internal ActionMenuButton(ActionMenuPage basePage, string text, System.Action action, Texture2D icon = null)
			{
				buttonText = text;
				buttonIcon = icon;
				buttonAction = delegate
				{
					action();
					return true;
				};
				basePage.buttons.Add(this);
			}

			internal void SetButtonText(string newText)
			{
				buttonText = newText;
				if (currentPedalOption != null)
				{
					currentPedalOption.prop_String_0 = newText;
				}
			}

			internal void SetIcon(Texture2D newTexture)
			{
				buttonIcon = newTexture;
				if (currentPedalOption != null)
				{
					currentPedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(newTexture);
				}
			}
		}

		private static readonly List<ActionMenuButton> mainMenuButtons = new List<ActionMenuButton>();

		internal static ActionMenu activeActionMenu;

		internal static bool IsOpen(this ActionMenuOpener actionMenuOpener)
		{
			return actionMenuOpener.field_Private_Boolean_0;
		}

		private static ActionMenuOpener GetActionMenuOpener()
		{
			if (!ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.IsOpen() && ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.IsOpen())
			{
				return ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1;
			}
			if (ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.IsOpen() && !ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.IsOpen())
			{
				return ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0;
			}
			return null;
		}

		internal static void OpenMainPage(ActionMenu menu)
		{
			activeActionMenu = menu;
			if (Configuration.GetGeneralConfig().DisableActionMenuIntegration)
			{
				return;
			}
			foreach (ActionMenuButton mainMenuButton in mainMenuButtons)
			{
				PedalOption pedalOption = activeActionMenu.Method_Private_PedalOption_0();
				pedalOption.prop_String_0 = mainMenuButton.buttonText;
				pedalOption.field_Public_Func_1_Boolean_0 = DelegateSupport.ConvertDelegate<Il2CppSystem.Func<bool>>(mainMenuButton.buttonAction);
				if (mainMenuButton.buttonIcon != null)
				{
					pedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(mainMenuButton.buttonIcon);
				}
				mainMenuButton.currentPedalOption = pedalOption;
			}
		}
	}
}
