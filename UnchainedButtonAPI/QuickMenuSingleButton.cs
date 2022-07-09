using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Tooltips;

namespace UnchainedButtonAPI
{
	internal class QuickMenuSingleButton : QuickMenuButtonBase
	{
		internal object customData;

		internal QuickMenuSingleButton(QuickMenuButtonRow parentRow, string text, Action action, string tooltip, Sprite icon = null)
		{
			buttonParentName = parentRow.GetParentMenuName();
			InitializeButton(parentRow.GetGameObject().transform, text, action, tooltip, icon);
		}

		internal QuickMenuSingleButton(string parentRow, string text, Action action, string tooltip, Sprite icon = null)
		{
			buttonParentName = parentRow;
			InitializeButton(QuickMenuUtils.GetQuickMenu().transform.Find(parentRow + "/ScrollRect/Viewport/VerticalLayoutGroup"), text, action, tooltip, icon);
		}

		private void InitializeButton(Transform parent, string text, Action action, string tooltip, Sprite icon)
		{
			buttonObject = UnityEngine.Object.Instantiate(QuickMenuTemplates.GetSingleButtonTemplate(), parent);
			buttonObject.name = $"Button_{QuickMenuUtils.GetQuickMenuIdentifier()}{QuickMenuUtils.GetQuickMenuUniqueIdentifier()}";
			buttonText = buttonObject.transform.Find("Text_H4").GetComponent<TextMeshProUGUI>();
			buttonHandler = buttonObject.GetComponent<Button>();
			buttonTooltip = buttonObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
			GameObject gameObject = buttonObject.transform.Find("Icon").gameObject;
			UnityEngine.Object.Destroy(gameObject.GetComponent<StyleElement>());
			buttonIcon = gameObject.GetComponent<Image>();
			buttonIcon.color = Color.white;
			SetButtonText(text);
			SetToolTip(tooltip);
			SetAction(action);
			SetIcon(icon);
			SetActive(active: true);
		}

		internal void SetIcon(Sprite icon)
		{
			if (icon == null)
			{
				buttonIcon.gameObject.SetActive(value: false);
				string text = string.Empty;
				string[] array = buttonText.text.Split(' ');
				foreach (string text2 in array)
				{
					if (text2.Length > text.Length)
					{
						text = text2;
					}
				}
				buttonText.fontSize = 37 - text.Length;
				buttonText.transform.localPosition = new Vector3(0f, -25f, 0f);
			}
			else
			{
				buttonIcon.gameObject.SetActive(value: true);
				buttonIcon.sprite = icon;
				buttonIcon.overrideSprite = icon;
				buttonText.fontSize = 24f;
				buttonText.transform.localPosition = new Vector3(0f, -76f, 0f);
			}
		}
	}
}
