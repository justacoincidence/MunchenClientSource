using System.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Player
{
	internal class AvatarHistoryMenu : QuickMenuNestedMenu
	{
		private readonly List<QuickMenuButtonRow> buttonRows = new List<QuickMenuButtonRow>();

		private AvatarHistoryWorldOptions avatarHistoryWorldOptions;

		internal AvatarHistoryMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().AvatarHistoryMenuName, LanguageManager.GetUsedLanguage().AvatarHistoryMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
		}

		private void AvatarClicked(QuickMenuSingleButton button)
		{
			avatarHistoryWorldOptions.avatarIndex = (int)button.customData;
			avatarHistoryWorldOptions.ShowMenu();
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
			avatarHistoryWorldOptions?.Dispose();
			avatarHistoryWorldOptions = new AvatarHistoryWorldOptions(quickMenuButtonRow);
			for (int num2 = Configuration.GetAvatarHistoryConfig().AvatarHistory.Count - 1; num2 > 0; num2--)
			{
				if (!string.IsNullOrEmpty(Configuration.GetAvatarHistoryConfig().AvatarHistory[num2].Name))
				{
					QuickMenuSingleButton button = new QuickMenuSingleButton(quickMenuButtonRow, Configuration.GetAvatarHistoryConfig().AvatarHistory[num2].Name ?? "", null, Configuration.GetAvatarHistoryConfig().AvatarHistory[num2].ID)
					{
						customData = num2
					};
					button.SetAction(delegate
					{
						AvatarClicked(button);
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
