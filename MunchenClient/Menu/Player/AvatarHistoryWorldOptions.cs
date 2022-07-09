using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.Player
{
	internal class AvatarHistoryWorldOptions : QuickMenuNestedMenu
	{
		internal int avatarIndex = 0;

		internal void SwitchToAvatar()
		{
			PlayerUtils.ChangePlayerAvatar(Configuration.GetAvatarHistoryConfig().AvatarHistory[avatarIndex].ID, logErrorOnHud: true);
		}

		internal void UseOnBots()
		{
			if (ApplicationBotHandler.IsBotsFullyConnected())
			{
				ApplicationBotHandler.SendCommandToBots("ChangeAvatar:" + Configuration.GetAvatarHistoryConfig().AvatarHistory[avatarIndex].ID);
			}
		}

		internal void RemoveInstanceFromList()
		{
			Configuration.GetAvatarHistoryConfig().AvatarHistory.RemoveAt(avatarIndex);
			Configuration.SaveAvatarHistoryConfig();
			buttonHandler.Press();
		}

		internal AvatarHistoryWorldOptions(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().AvatarHistoryMenuName, string.Empty, createButtonOnParent: false)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AvatarHistoryOptionsSwitch, delegate
			{
				SwitchToAvatar();
			}, LanguageManager.GetUsedLanguage().AvatarHistoryOptionsSwitchDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AvatarHistoryOptionsBots, delegate
			{
				UseOnBots();
			}, LanguageManager.GetUsedLanguage().AvatarHistoryOptionsBotsDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AvatarHistoryOptionsRemove, delegate
			{
				RemoveInstanceFromList();
			}, LanguageManager.GetUsedLanguage().AvatarHistoryOptionsRemoveDescription);
		}
	}
}
