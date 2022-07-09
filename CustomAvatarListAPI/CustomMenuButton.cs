using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAvatarListAPI
{
	internal class CustomMenuButton
	{
		private static GameObject avatarButton;

		private readonly GameObject buttonObj;

		private readonly Button buttonHandler;

		private readonly Text buttonText;

		internal static GameObject ButtonTemplate
		{
			get
			{
				if (avatarButton == null)
				{
					GameObject gameObject = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Favorite Button");
					GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, gameObject.transform.parent);
					gameObject2.GetComponent<Button>().onClick.RemoveAllListeners();
					gameObject2.SetActive(value: false);
					Vector3 localPosition = gameObject2.transform.localPosition;
					gameObject2.transform.localPosition = new Vector3(localPosition.x, localPosition.y + 150f);
					avatarButton = gameObject2;
					Transform transform = gameObject2.transform.Find("Horizontal");
					IEnumerator enumerator = transform.GetEnumerator();
					while (enumerator.MoveNext())
					{
						Il2CppSystem.Object current = enumerator.Current;
						Transform transform2 = current.Cast<Transform>();
						if (transform2 != null && transform2.name != "FavoriteActionText")
						{
							UnityEngine.Object.Destroy(transform2.gameObject);
						}
					}
				}
				return avatarButton;
			}
		}

		private CustomMenuButton()
		{
		}

		internal CustomMenuButton(GameObject buttonGameObject)
		{
			buttonObj = buttonGameObject;
			buttonHandler = buttonGameObject.GetComponentInChildren<Button>();
			buttonText = buttonGameObject.GetComponentInChildren<Text>();
		}

		internal void SetAction(System.Action onClick)
		{
			buttonHandler.onClick = new Button.ButtonClickedEvent();
			buttonHandler.onClick.AddListener(onClick);
		}

		internal GameObject GetButtonObj()
		{
			return buttonObj;
		}

		internal Button GetButtonHandler()
		{
			return buttonHandler;
		}

		internal Text GetButtonText()
		{
			return buttonText;
		}

		internal static CustomMenuButton Create(string title, float x, float y, System.Action onClick = null, float scale = -1f, bool showNew = false)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(ButtonTemplate.gameObject, ButtonTemplate.transform.parent);
			gameObject.name = title;
			CustomMenuButton customMenuButton = new CustomMenuButton(gameObject);
			customMenuButton.GetButtonHandler().onClick.RemoveAllListeners();
			Vector3 localPosition = customMenuButton.GetButtonObj().transform.localPosition;
			customMenuButton.GetButtonObj().transform.localPosition = new Vector3(localPosition.x + x, localPosition.y + 80f * y);
			customMenuButton.GetButtonText().text = title;
			if (!showNew)
			{
				Image[] array = customMenuButton.GetButtonObj().GetComponentsInChildren<Image>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].name == "Icon_New")
					{
						UnityEngine.Object.DestroyImmediate(array[i]);
					}
				}
			}
			if (scale != -1f)
			{
				Vector3 localScale = customMenuButton.GetButtonObj().transform.localScale;
				customMenuButton.GetButtonObj().transform.localScale = new Vector3(localScale.x / scale, localScale.y / scale, localScale.z / scale);
			}
			if (onClick != null)
			{
				customMenuButton.GetButtonHandler().onClick = new Button.ButtonClickedEvent();
				customMenuButton.GetButtonHandler().onClick.AddListener(onClick);
			}
			customMenuButton.GetButtonObj().SetActive(value: true);
			return customMenuButton;
		}
	}
}
