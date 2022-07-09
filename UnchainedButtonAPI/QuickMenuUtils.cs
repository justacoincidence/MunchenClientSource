using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Patching;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace UnchainedButtonAPI
{
	internal class QuickMenuUtils
	{
		internal delegate void ShowLinkAlertAction(string link);

		internal delegate void ShowAlertAction(string text);

		private static string quickMenuIdentifier = "UninitializedQuickMenu";

		private static int quickMenuUniqueIdentifier = 0;

		private static Sprite toggleIconOn;

		private static Sprite toggleIconOff;

		private static VRC.UI.Elements.QuickMenu quickMenuCached;

		private static readonly System.Collections.Generic.List<QuickMenuNestedMenu> quickMenuQueuedChangesUIPage = new System.Collections.Generic.List<QuickMenuNestedMenu>();

		private static readonly System.Collections.Generic.List<QuickMenuButtonBase> quickMenuItems = new System.Collections.Generic.List<QuickMenuButtonBase>();

		private static readonly System.Collections.Generic.Dictionary<QuickMenus, UIPage> quickMenuUIPages = new System.Collections.Generic.Dictionary<QuickMenus, UIPage>();

		private static readonly System.Collections.Generic.Dictionary<QuickMenus, string> quickMenuPaths = new System.Collections.Generic.Dictionary<QuickMenus, string>
		{
			{
				QuickMenus.LaunchPad,
				"Menu_Dashboard"
			},
			{
				QuickMenus.Notifications,
				"Menu_Notifications"
			},
			{
				QuickMenus.Camera,
				"Menu_Camera"
			},
			{
				QuickMenus.Here,
				"Menu_Here"
			},
			{
				QuickMenus.AudioSettings,
				"Menu_AudioSettings"
			},
			{
				QuickMenus.ChangeAudioInputDevice,
				"Menu_ChangeAudioInputDevice"
			},
			{
				QuickMenus.Settings,
				"Menu_Settings"
			},
			{
				QuickMenus.Emojis,
				"Menu_QM_Emojis"
			},
			{
				QuickMenus.DevTools,
				"Menu_DevTools"
			},
			{
				QuickMenus.SelectedUser,
				"Menu_SelectedUser_Local"
			},
			{
				QuickMenus.HoveredUser,
				"Modal_HoveredUser"
			}
		};

		internal static ShowLinkAlertAction showLinkAlertAction;

		internal static ShowAlertAction showAlertAction;

		internal static ShowLinkAlertAction ShowLinkAlert
		{
			get
			{
				if (showLinkAlertAction != null)
				{
					return showLinkAlertAction;
				}
				MethodInfo method = (from mb in typeof(UIMenu).GetMethods()
					where mb.Name.StartsWith("Method_Public_Virtual_Final_New_Void_String_") && PatchManager.CheckMethod(mb, "Open Web Browser")
					select mb).First();
				showLinkAlertAction = (ShowLinkAlertAction)Delegate.CreateDelegate(typeof(ShowLinkAlertAction), GetQuickMenu(), method);
				return showLinkAlertAction;
			}
		}

		internal static ShowAlertAction ShowAlert
		{
			get
			{
				if (showAlertAction != null)
				{
					return showAlertAction;
				}
				MethodInfo method = (from mb in typeof(UIMenu).GetMethods()
					where mb.Name.StartsWith("Method_Public_Virtual_Final_New_Void_String_") && PatchManager.CheckNonGlobalMethod(mb, "Method_Public_Void_String_", 27)
					select mb).First();
				showAlertAction = (ShowAlertAction)Delegate.CreateDelegate(typeof(ShowAlertAction), GetQuickMenu(), method);
				return showAlertAction;
			}
		}

		internal static void InitializeButtonAPI(string clientName, Sprite iconOn, Sprite iconOff)
		{
			quickMenuIdentifier = clientName;
			toggleIconOn = iconOn;
			toggleIconOff = iconOff;
			QuickMenuPatches.InitializeButtonPatches(clientName);
		}

		internal static string GetQuickMenuIdentifier()
		{
			return quickMenuIdentifier;
		}

		internal static int GetQuickMenuUniqueIdentifier()
		{
			quickMenuUniqueIdentifier++;
			return quickMenuUniqueIdentifier;
		}

		internal static Sprite GetToggleIconOn()
		{
			return toggleIconOn;
		}

		internal static Sprite GetToggleIconOff()
		{
			return toggleIconOff;
		}

		internal static void RegisterQuickMenuItem(QuickMenuButtonBase item)
		{
			quickMenuItems.Add(item);
		}

		internal static void UnregisterQuickMenuItem(QuickMenuButtonBase item)
		{
			quickMenuItems.Remove(item);
			if (item is QuickMenuNestedMenu nestedMenu)
			{
				RemoveMenuFromController(nestedMenu);
			}
		}

		internal static int GetFirstModalIndex()
		{
			return QuickMenuTemplates.GetModalTemplate().transform.GetSiblingIndex();
		}

		internal static VRC.UI.Elements.QuickMenu GetQuickMenu()
		{
			if (quickMenuCached == null)
			{
				VRC.UI.Elements.QuickMenu[] array = Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>();
				if (array.Length != 0)
				{
					quickMenuCached = array[0];
				}
			}
			return quickMenuCached;
		}

		internal static UIPage GetMenuUIPage(QuickMenus menu)
		{
			if (!quickMenuUIPages.ContainsKey(menu))
			{
				GameObject gameObject = FindObject(GetQuickMenu().gameObject, quickMenuPaths[menu]);
				Il2CppSystem.Collections.Generic.List<Component> list = new Il2CppSystem.Collections.Generic.List<Component>();
				gameObject.GetComponents(list);
				Il2CppSystem.Collections.Generic.List<Component>.Enumerator enumerator = list.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Component current = enumerator.Current;
					if (!(current == null))
					{
						UIPage uIPage = current.TryCast<UIPage>();
						if (!(uIPage == null))
						{
							quickMenuUIPages.Add(menu, uIPage);
							break;
						}
					}
				}
			}
			return quickMenuUIPages.ContainsKey(menu) ? quickMenuUIPages[menu] : null;
		}

		internal static void ChangeScrollState(ScrollRect scrollRect, bool state)
		{
			if (!(scrollRect == null))
			{
				RectMask2D component = scrollRect.viewport.GetComponent<RectMask2D>();
				if (!(component == null))
				{
					scrollRect.enabled = state;
					component.enabled = state;
				}
			}
		}

		internal static void ChangeScrollState(QuickMenus menu, bool state)
		{
			GameObject gameObject = null;
			for (int i = 0; i < GetMenuUIPage(menu).transform.transform.childCount; i++)
			{
				if (GetMenuUIPage(menu).transform.GetChild(i).name.ToLower().Contains("scrollrect"))
				{
					gameObject = GetMenuUIPage(menu).transform.GetChild(i).gameObject;
					break;
				}
			}
			if (!(gameObject == null))
			{
				ChangeScrollState(gameObject.GetComponent<ScrollRect>(), state);
			}
		}

		internal static void ChangeScrollState(UIPage menu, bool state)
		{
			GameObject gameObject = null;
			for (int i = 0; i < menu.transform.childCount; i++)
			{
				if (menu.transform.GetChild(i).name.ToLower().Contains("scrollrect"))
				{
					gameObject = menu.transform.GetChild(i).gameObject;
					break;
				}
			}
			if (!(gameObject == null))
			{
				ChangeScrollState(gameObject.GetComponent<ScrollRect>(), state);
			}
		}

		internal static void ChangeScrollState(QuickMenuNestedMenu menu, bool state)
		{
			GameObject gameObject = null;
			for (int i = 0; i < menu.GetGameObject().transform.childCount; i++)
			{
				if (menu.GetGameObject().transform.GetChild(i).name.ToLower().Contains("scrollrect"))
				{
					gameObject = menu.GetGameObject().transform.GetChild(i).gameObject;
					break;
				}
			}
			if (!(gameObject == null))
			{
				ChangeScrollState(gameObject.GetComponent<ScrollRect>(), state);
			}
		}

		internal static void OpenMenu(QuickMenuNestedMenu menu, bool clearStackPage = false)
		{
			OpenMenu(menu.GetMenuName(), clearStackPage);
		}

		internal static void OpenMenu(string menuName, bool clearStackPage = false)
		{
			string text = menuName;
			if (text == "QuickMenuSelectedUserLocal")
			{
				text = "QuickMenuHere";
			}
			for (int i = 0; i < GetQuickMenu().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0.Count; i++)
			{
				if (!(GetQuickMenu().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0[i] == null) && GetQuickMenu().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0[i].field_Public_String_0 == text)
				{
					GetQuickMenu().prop_MenuStateController_0.ShowTabContent(i, clearStackPage);
					break;
				}
			}
		}

		internal static void AddMenuToController(QuickMenuNestedMenu nestedMenu)
		{
			if (!QuickMenuPatches.HasMenuChangesBeenInitialized())
			{
				quickMenuQueuedChangesUIPage.Add(nestedMenu);
				return;
			}
			System.Collections.Generic.List<UIPage> list = GetQuickMenu().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0.ToList();
			foreach (QuickMenuNestedMenu item in quickMenuQueuedChangesUIPage)
			{
				list.Add(item.uiPage);
			}
			if (nestedMenu != null)
			{
				list.Add(nestedMenu.uiPage);
			}
			quickMenuQueuedChangesUIPage.Clear();
			GetQuickMenu().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0 = list.ToArray();
		}

		internal static void RemoveMenuFromController(QuickMenuNestedMenu nestedMenu)
		{
			System.Collections.Generic.List<UIPage> list = GetQuickMenu().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0.ToList();
			foreach (UIPage item in list.ToList())
			{
				if (item == null || item == nestedMenu.uiPage)
				{
					list.Remove(item);
				}
			}
			GetQuickMenu().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0 = list.ToArray();
		}

		internal static void OnUIPageDisabled(UIPage page)
		{
			if (!QuickMenuPatches.HasMenuChangesBeenInitialized())
			{
				return;
			}
			foreach (QuickMenuButtonBase quickMenuItem in quickMenuItems)
			{
				if (quickMenuItem is QuickMenuNestedMenu quickMenuNestedMenu && quickMenuNestedMenu.uiPage == page)
				{
					quickMenuNestedMenu.OnMenuUnshown();
				}
			}
		}

		internal static void OnMenuInitialized()
		{
			AddMenuToController(null);
			foreach (QuickMenuButtonBase quickMenuItem in quickMenuItems)
			{
				quickMenuItem.OnQuickMenuInitialized();
			}
		}

		internal static GameObject FindObject(GameObject parent, string name)
		{
			Transform[] array = parent.GetComponentsInChildren<Transform>(includeInactive: true);
			Transform[] array2 = array;
			foreach (Transform transform in array2)
			{
				if (transform.name == name)
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		internal static Texture2D CreateTextureFromBase64(string data)
		{
			Texture2D texture2D = new Texture2D(2, 2);
			ImageConversion.LoadImage(texture2D, Convert.FromBase64String(data));
			texture2D.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			return texture2D;
		}

		internal static Sprite CreateSpriteFromBase64(string data)
		{
			Texture2D texture2D = CreateTextureFromBase64(data);
			Rect rect = new Rect(0f, 0f, texture2D.width, texture2D.height);
			Vector2 pivot = new Vector2(0.5f, 0.5f);
			Vector4 border = Vector4.zero;
			Sprite sprite = Sprite.CreateSprite_Injected(texture2D, ref rect, ref pivot, 100f, 0u, SpriteMeshType.Tight, ref border, generateFallbackPhysicsShape: false);
			sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			return sprite;
		}
	}
}
