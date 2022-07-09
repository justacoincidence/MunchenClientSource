using System.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.World
{
	internal class InstanceHistoryMenu : QuickMenuNestedMenu
	{
		private readonly List<QuickMenuButtonRow> buttonRows = new List<QuickMenuButtonRow>();

		private InstanceHistoryWorldOptions instanceHistoryWorldOptions;

		internal InstanceHistoryMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().InstanceHistoryMenuName, LanguageManager.GetUsedLanguage().InstanceHistoryMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
		}

		private void InstanceClicked(QuickMenuSingleButton button)
		{
			instanceHistoryWorldOptions.instanceIndex = (int)button.customData;
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
			instanceHistoryWorldOptions = new InstanceHistoryWorldOptions(quickMenuButtonRow);
			for (int num2 = Configuration.GetInstanceHistoryConfig().InstanceHistory.Count - 1; num2 > 0; num2--)
			{
				if (!string.IsNullOrEmpty(Configuration.GetInstanceHistoryConfig().InstanceHistory[num2].Tags))
				{
					string instanceID = InstanceHistoryHandler.GetInstanceID(Configuration.GetInstanceHistoryConfig().InstanceHistory[num2].Tags);
					string text = Configuration.GetInstanceHistoryConfig().InstanceHistory[num2].Name + " (" + instanceID.Substring(0, (instanceID.Length > 5) ? 5 : instanceID.Length) + ")";
					string tooltip = LanguageManager.GetUsedLanguage().InstanceHistoryButtonDescription.Replace("{WorldRegion}", Configuration.GetInstanceHistoryConfig().InstanceHistory[num2].Region.ToString().Replace("_", " "));
					QuickMenuSingleButton button = new QuickMenuSingleButton(quickMenuButtonRow, text, null, tooltip)
					{
						customData = num2
					};
					button.SetAction(delegate
					{
						InstanceClicked(button);
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
}
