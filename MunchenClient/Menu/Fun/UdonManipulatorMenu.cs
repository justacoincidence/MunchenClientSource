using System.Collections.Generic;
using MunchenClient.Core;
using UnchainedButtonAPI;
using UnityEngine;
using VRC.Udon;

namespace MunchenClient.Menu.Fun
{
	internal class UdonManipulatorMenu : QuickMenuNestedMenu
	{
		private readonly List<QuickMenuButtonRow> buttonRows = new List<QuickMenuButtonRow>();

		internal static UdonBehaviour[] udonBehaviours;

		private UdonManipulatorOptionsMenu instanceHistoryWorldOptions;

		internal UdonManipulatorMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().UdonManipulatorMenuName, LanguageManager.GetUsedLanguage().UdonManipulatorMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
		}

		private void UdonClicked(QuickMenuSingleButton button)
		{
			instanceHistoryWorldOptions.udonBehaviourIndex = (int)button.customData;
			instanceHistoryWorldOptions.ShowMenu();
		}

		internal override void OnMenuShownCallback()
		{
			for (int i = 0; i < buttonRows.Count; i++)
			{
				buttonRows[i].Dispose();
			}
			buttonRows.Clear();
			int num = 0;
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			buttonRows.Add(quickMenuButtonRow);
			instanceHistoryWorldOptions?.Dispose();
			instanceHistoryWorldOptions = new UdonManipulatorOptionsMenu(quickMenuButtonRow);
			udonBehaviours = Object.FindObjectsOfType<UdonBehaviour>();
			for (int j = 0; j < udonBehaviours.Length; j++)
			{
				QuickMenuSingleButton button = new QuickMenuSingleButton(quickMenuButtonRow, udonBehaviours[j].name, null, string.Empty)
				{
					customData = j
				};
				button.SetAction(delegate
				{
					UdonClicked(button);
				});
				num++;
				if (num >= 4)
				{
					num = 0;
					quickMenuButtonRow = new QuickMenuButtonRow(this);
					buttonRows.Add(quickMenuButtonRow);
				}
			}
		}
	}
}
