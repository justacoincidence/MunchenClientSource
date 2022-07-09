using System;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.UI.Elements.Tooltips;

namespace UnchainedButtonAPI
{
	internal class QuickMenuButtonBase
	{
		protected string buttonParentName;

		protected GameObject buttonObject;

		protected Image buttonBackground;

		protected Image buttonIcon;

		protected TextMeshProUGUI buttonText;

		protected Button buttonHandler;

		protected VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;

		internal QuickMenuButtonBase()
		{
			QuickMenuUtils.RegisterQuickMenuItem(this);
		}

		internal GameObject GetGameObject()
		{
			return buttonObject;
		}

		internal string GetParentMenuName()
		{
			return buttonParentName;
		}

		internal void SetActive(bool active)
		{
			buttonObject.SetActive(active);
		}

		internal void SetButtonText(string text)
		{
			buttonText.text = text;
			if (buttonIcon != null)
			{
				buttonIcon.transform.localPosition = (string.IsNullOrEmpty(text) ? new Vector3(0f, 54f, 0f) : new Vector3(0f, 56f, 0f));
				buttonIcon.transform.localScale = (string.IsNullOrEmpty(text) ? new Vector3(1.5f, 1.5f, 1.5f) : new Vector3(1f, 1f, 1f));
			}
		}

		internal void SetToolTip(string tooltip)
		{
			buttonTooltip.field_Public_String_0 = tooltip;
			buttonTooltip.enabled = !string.IsNullOrEmpty(tooltip);
		}

		internal void SetAction(Action action)
		{
			buttonHandler.onClick = new Button.ButtonClickedEvent();
			buttonHandler.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(action));
		}

		internal void Dispose()
		{
			QuickMenuUtils.UnregisterQuickMenuItem(this);
			UnityEngine.Object.Destroy(buttonObject);
		}

		internal virtual void OnQuickMenuInitialized()
		{
		}
	}
}
