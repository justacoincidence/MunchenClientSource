using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Tooltips;

namespace UnchainedButtonAPI
{
	internal class QuickMenuSliderButton : QuickMenuButtonBase
	{
		private Slider buttonSlider;

		private TextMeshProUGUI buttonTextSecondary;

		private Action<float> onValueChangedCallback;

		private bool callbackAllowed = true;

		internal QuickMenuSliderButton(QuickMenuNestedMenu parentRow, string text, float minValue, float maxValue, float starterValue, Action<float> action, string secondaryText, string tooltip)
		{
			buttonParentName = parentRow.GetMenuName();
			InitializeButton(parentRow.GetGameObject().transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"), text, minValue, maxValue, starterValue, action, secondaryText, tooltip);
		}

		internal QuickMenuSliderButton(string parentRow, string text, float minValue, float maxValue, float starterValue, Action<float> action, string secondaryText, string tooltip)
		{
			buttonParentName = parentRow;
			InitializeButton(QuickMenuUtils.GetQuickMenu().transform.Find(parentRow + "/ScrollRect/Viewport/VerticalLayoutGroup"), text, minValue, maxValue, starterValue, action, secondaryText, tooltip);
		}

		private void InitializeButton(Transform parent, string text, float minValue, float maxValue, float starterValue, Action<float> action, string secondaryText, string tooltip)
		{
			buttonObject = UnityEngine.Object.Instantiate(QuickMenuTemplates.GetSliderTemplate(), parent);
			buttonObject.name = $"Slider_{QuickMenuUtils.GetQuickMenuIdentifier()}{QuickMenuUtils.GetQuickMenuUniqueIdentifier()}";
			buttonObject.transform.Find("CurrentMic").gameObject.SetActive(value: false);
			buttonObject.transform.Find("InputLevel/Sliders/MicLevelSlider").gameObject.SetActive(value: false);
			buttonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(896f, 150f);
			buttonText = buttonObject.transform.Find("InputLevel/Sliders/MicSensitivitySlider/Text_QM_H4").GetComponent<TextMeshProUGUI>();
			GameObject gameObject = buttonObject.transform.Find("InputLevel/Sliders/MicSensitivitySlider/Text_QM_H4 (1)").gameObject;
			UnityEngine.Object.Destroy(gameObject.GetComponent<TextBinding>());
			buttonTextSecondary = gameObject.GetComponent<TextMeshProUGUI>();
			GameObject gameObject2 = buttonObject.transform.Find("InputLevel/Sliders/MicSensitivitySlider/Slider").gameObject;
			UnityEngine.Object.Destroy(gameObject2.GetComponent<SliderBinding>());
			buttonTooltip = gameObject2.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
			buttonSlider = gameObject2.GetComponent<Slider>();
			buttonSlider.onValueChanged = new Slider.SliderEvent();
			buttonSlider.onValueChanged.AddListener((Action<float>)OnValueChanged);
			SetSliderAction(action);
			SetSliderMinValue(minValue);
			SetSliderMaxValue(maxValue);
			SetSliderValue(starterValue, invokeAction: false);
			SetButtonText(text);
			SetSecondaryButtonText(secondaryText);
			SetToolTip(tooltip);
			SetActive(active: true);
		}

		internal void SetSecondaryButtonText(string text)
		{
			buttonTextSecondary.text = text;
		}

		internal void SetSliderMinValue(float value)
		{
			buttonSlider.minValue = value;
		}

		internal void SetSliderMaxValue(float value)
		{
			buttonSlider.maxValue = value;
		}

		internal void SetSliderValue(float value, bool invokeAction = true)
		{
			callbackAllowed = invokeAction;
			buttonSlider.value = value;
			callbackAllowed = true;
		}

		internal void SetSliderAction(Action<float> onValueChanged)
		{
			onValueChangedCallback = onValueChanged;
		}

		private void OnValueChanged(float value)
		{
			if (callbackAllowed)
			{
				onValueChangedCallback(value);
			}
		}
	}
}
