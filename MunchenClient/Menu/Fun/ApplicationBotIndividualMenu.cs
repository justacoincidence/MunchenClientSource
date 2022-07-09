using System.Collections.Generic;
using MunchenClient.ModuleSystem.Modules;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Fun
{
	internal class ApplicationBotIndividualMenu : QuickMenuNestedMenu
	{
		private readonly List<QuickMenuButtonRow> buttonRows = new List<QuickMenuButtonRow>();

		private ApplicationBotIndividualOptionsMenu individualBotOptions;

		internal ApplicationBotIndividualMenu(QuickMenuButtonRow parent)
			: base(parent, "Individual Bot Selector", "Control the bots individually")
		{
		}

		private void BotClicked(QuickMenuSingleButton button)
		{
			individualBotOptions.botIndex = (int)button.customData;
			individualBotOptions.ShowMenu();
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
			individualBotOptions?.Dispose();
			individualBotOptions = new ApplicationBotIndividualOptionsMenu(quickMenuButtonRow);
			for (int j = 0; j < ApplicationBotHandler.GetBotAmount(); j++)
			{
				QuickMenuSingleButton button = new QuickMenuSingleButton(quickMenuButtonRow, $"Bot: {j}", null, string.Empty)
				{
					customData = j
				};
				button.SetAction(delegate
				{
					BotClicked(button);
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
