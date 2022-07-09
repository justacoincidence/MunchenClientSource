using UnityEngine;
using UnityEngine.UI;

namespace UnchainedButtonAPI
{
	internal class QuickMenuTemplates
	{
		private static GameObject pageButtonReference;

		private static GameObject disabledButtonReference;

		private static GameObject headerReference;

		private static GameObject buttonRowReference;

		private static GameObject spacersReference;

		private static GameObject singleButtonReference;

		private static GameObject toggleButtonReference;

		private static GameObject nestedMenuReference;

		private static GameObject modalReference;

		private static GameObject sliderReference;

		internal static GameObject GetPageButtonTemplate()
		{
			if (pageButtonReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Page_Settings")
					{
						pageButtonReference = transform.gameObject;
						break;
					}
				}
			}
			return pageButtonReference;
		}

		internal static GameObject GetIndicatorButtonTemplate()
		{
			if (disabledButtonReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Button_Build")
					{
						disabledButtonReference = transform.gameObject;
						break;
					}
				}
			}
			return disabledButtonReference;
		}

		internal static GameObject GetHeaderTemplate()
		{
			if (headerReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Header_QuickActions")
					{
						headerReference = transform.gameObject;
						break;
					}
				}
			}
			return headerReference;
		}

		internal static GameObject GetButtonRowTemplate()
		{
			if (buttonRowReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Buttons_QuickActions")
					{
						buttonRowReference = transform.gameObject;
						break;
					}
				}
			}
			return buttonRowReference;
		}

		internal static GameObject GetSpacersTemplate()
		{
			if (spacersReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Spacer_8pt")
					{
						spacersReference = transform.gameObject;
						break;
					}
				}
			}
			return spacersReference;
		}

		internal static GameObject GetSingleButtonTemplate()
		{
			if (singleButtonReference == null)
			{
				Button[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Button>(includeInactive: true);
				Button[] array2 = array;
				foreach (Button button in array2)
				{
					if (button.name == "Button_Respawn")
					{
						singleButtonReference = button.gameObject;
						break;
					}
				}
			}
			return singleButtonReference;
		}

		internal static GameObject GetToggleButtonTemplate()
		{
			if (toggleButtonReference == null)
			{
				Toggle[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Toggle>(includeInactive: true);
				Toggle[] array2 = array;
				foreach (Toggle toggle in array2)
				{
					if (toggle.name == "Button_ToggleTooltips")
					{
						toggleButtonReference = toggle.gameObject;
						break;
					}
				}
			}
			return toggleButtonReference;
		}

		internal static GameObject GetNestedMenuTemplate()
		{
			if (nestedMenuReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Menu_Dashboard")
					{
						nestedMenuReference = transform.gameObject;
						break;
					}
				}
			}
			return nestedMenuReference;
		}

		internal static GameObject GetModalTemplate()
		{
			if (modalReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Modal_AddMessage")
					{
						modalReference = transform.gameObject;
						break;
					}
				}
			}
			return modalReference;
		}

		internal static GameObject GetSliderTemplate()
		{
			if (sliderReference == null)
			{
				Transform[] array = QuickMenuUtils.GetQuickMenu().GetComponentsInChildren<Transform>(includeInactive: true);
				Transform[] array2 = array;
				foreach (Transform transform in array2)
				{
					if (transform.name == "Mic")
					{
						sliderReference = transform.gameObject;
						break;
					}
				}
			}
			return sliderReference;
		}
	}
}
