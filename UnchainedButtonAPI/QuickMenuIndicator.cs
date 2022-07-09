using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Tooltips;

namespace UnchainedButtonAPI
{
	internal class QuickMenuIndicator : QuickMenuButtonBase
	{
		private TextMeshProUGUI buttonMainText;

		internal QuickMenuIndicator(QuickMenuButtonRow parentRow, string headerText, string actualText, string tooltip)
		{
			buttonParentName = parentRow.GetParentMenuName();
			InitializeButton(parentRow.GetGameObject().transform, headerText, actualText, tooltip, null);
		}

		internal QuickMenuIndicator(QuickMenuButtonRow parentRow, string headerText, string actualText, string tooltip, Sprite icon)
		{
			buttonParentName = parentRow.GetParentMenuName();
			InitializeButton(parentRow.GetGameObject().transform, headerText, actualText, tooltip, icon);
		}

		internal QuickMenuIndicator(string parentRow, string headerText, string actualText, string tooltip)
		{
			buttonParentName = parentRow;
			InitializeButton(QuickMenuUtils.GetQuickMenu().transform.Find(parentRow + "/ScrollRect/Viewport/VerticalLayoutGroup"), headerText, actualText, tooltip, null);
		}

		internal QuickMenuIndicator(string parentRow, string headerText, string actualText, string tooltip, Sprite icon)
		{
			buttonParentName = parentRow;
			InitializeButton(QuickMenuUtils.GetQuickMenu().transform.Find(parentRow + "/ScrollRect/Viewport/VerticalLayoutGroup"), headerText, actualText, tooltip, icon);
		}

		private void InitializeButton(Transform parent, string headerText, string actualText, string tooltip, Sprite icon)
		{
			buttonObject = Object.Instantiate(QuickMenuTemplates.GetIndicatorButtonTemplate(), parent);
			buttonObject.name = $"IndicatorButton_{QuickMenuUtils.GetQuickMenuIdentifier()}{QuickMenuUtils.GetQuickMenuUniqueIdentifier()}";
			buttonHandler = buttonObject.GetComponent<Button>();
			buttonTooltip = buttonObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
			GameObject gameObject = buttonObject.transform.Find("Icon").gameObject;
			Object.Destroy(gameObject.GetComponent<StyleElement>());
			buttonIcon = gameObject.GetComponent<Image>();
			buttonIcon.color = Color.white;
			buttonText = buttonObject.transform.Find("Text_H4").GetComponent<TextMeshProUGUI>();
			GameObject gameObject2 = buttonObject.transform.Find("Text_H1").gameObject;
			Object.Destroy(gameObject2.GetComponent<ListCountBinding>());
			buttonMainText = gameObject2.GetComponent<TextMeshProUGUI>();
			buttonMainText.fontSize = 54f;
			Object.DestroyImmediate(buttonObject.transform.Find("Text_H1").GetComponent<TextBinding>());
			SetButtonText(headerText);
			SetMainText(actualText);
			SetToolTip(tooltip);
			SetIcon(icon);
			SetActive(active: true);
		}

		internal void SetIcon(Sprite icon)
		{
			if (icon == null)
			{
				buttonIcon.gameObject.SetActive(value: false);
				return;
			}
			buttonIcon.sprite = icon;
			buttonIcon.overrideSprite = icon;
			buttonIcon.gameObject.SetActive(value: true);
		}

		internal void SetMainText(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				buttonMainText.gameObject.SetActive(value: false);
				return;
			}
			string text2 = string.Empty;
			string[] array = text.Split(' ');
			foreach (string text3 in array)
			{
				if (text3.Length > text2.Length)
				{
					text2 = text3;
				}
			}
			buttonMainText.fontSize = 56 - text2.Length * 3;
			buttonMainText.text = text;
			buttonMainText.gameObject.SetActive(value: true);
		}
	}
}
