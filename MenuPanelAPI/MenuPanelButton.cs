using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MenuPanelAPI
{
	internal class MenuPanelButton
	{
		private GameObject button;

		private Text buttonText;

		private Button buttonHandler;

		private Action onButtonClick;

		internal MenuPanelButton(string text, Action onClick, bool interactable, string buttonTemplate, string parentColumn)
		{
			InitButton(text, onClick, interactable, buttonTemplate, parentColumn);
		}

		private void InitButton(string text, Action onClick, bool interactable, string buttonTemplate, string parentColumn)
		{
			button = UnityEngine.Object.Instantiate(MenuPanelAPI.GetMenuButtonTemplate(buttonTemplate), MenuPanelAPI.GetTabParent(parentColumn).transform);
			UnityEngine.Object.Destroy(button.GetComponent<VRCUiButton>());
			buttonText = button.GetComponentInChildren<Text>();
			buttonHandler = button.GetComponent<Button>();
			SetText(text);
			SetAction(onClick);
			SetInteractable(interactable);
			SetActive(isActive: true);
		}

		internal void SetText(string text)
		{
			button.name = text;
			buttonText.text = text;
		}

		internal void SetAction(Action buttonAction)
		{
			onButtonClick = buttonAction;
			buttonHandler.onClick = new Button.ButtonClickedEvent();
			buttonHandler.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>((Action)delegate
			{
				onButtonClick();
			}));
		}

		internal void SetActive(bool isActive)
		{
			button.gameObject.SetActive(isActive);
		}

		internal void SetInteractable(bool isInteractable)
		{
			buttonHandler.interactable = isInteractable;
		}
	}
}
