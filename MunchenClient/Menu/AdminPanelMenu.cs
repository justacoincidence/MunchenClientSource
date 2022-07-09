using System;
using System.Collections;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using ServerAPI.Core;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.Menu
{
	internal class AdminPanelMenu : QuickMenuNestedMenu
	{
		internal AdminPanelMenu(QuickMenuButtonRow parent)
			: base(parent, "Staff Panel", "Several moderation features for general features")
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, "Change PC Crasher", delegate
			{
				GeneralWrappers.ShowInputPopup("Change PC Crasher", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetCrasherPC, null, "Enter new avatar id...");
			}, "Changes the global PC crasher");
			new QuickMenuSingleButton(parentRow, "Change Quest Crasher", delegate
			{
				GeneralWrappers.ShowInputPopup("Change Quest Crasher", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetCrasherQuest, null, "Enter new avatar id...");
			}, "Changes the global Quest crasher");
			new QuickMenuSingleButton(parentRow, "Change Discord Link", delegate
			{
				GeneralWrappers.ShowInputPopup("Change Discord Link", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SetDiscordLink, null, "Enter new link...");
			}, "Changes the global discord link");
			new QuickMenuSingleButton(parentRow, "Globally Blacklist Avatar", delegate
			{
				GeneralWrappers.ShowInputPopup("Globally Blacklist Avatar", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, BlacklistAvatarGlobally, null, "Enter avatar id...");
			}, "Blacklists an avatar on a global basis for everyone");
			new QuickMenuSingleButton(parentRow2, "Globally Blacklist Author", delegate
			{
				GeneralWrappers.ShowInputPopup("Globally Blacklist Author", string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, BlacklistAuthorGlobally, null, "Enter author id...");
			}, "Blacklists an author on a global basis for everyone");
		}

		private void SetCrasherPC(string crasherId, List<KeyCode> pressedKeys, Text text)
		{
			string crasherIdTrimmed = crasherId.Trim();
			if (string.IsNullOrEmpty(crasherIdTrimmed))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Can't update PC crasher with empty avatar id");
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, "Can't update PC crasher with empty avatar id", ConsoleColor.Gray, "SetCrasherPC", 89);
				return;
			}
			PlayerUtils.IsAvatarValid(crasherIdTrimmed, delegate
			{
				ServerAPICore.GetInstance().UploadAvatarCrasher(crasherIdTrimmed, quest: false);
				MiscUtils.SetCrashingAvatarPC(crasherIdTrimmed);
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, "PC crasher updated successfully");
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, "PC crasher updated successfully", ConsoleColor.Gray, "SetCrasherPC", 100);
			}, delegate(string error)
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Failed updating PC crasher with the following message: " + error);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, "Failed updating PC crasher with the following message: " + error, ConsoleColor.Gray, "SetCrasherPC", 104);
			});
		}

		private void SetCrasherQuest(string crasherId, List<KeyCode> pressedKeys, Text text)
		{
			string crasherIdTrimmed = crasherId.Trim();
			if (string.IsNullOrEmpty(crasherIdTrimmed))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Can't update Quest crasher with empty avatar id");
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, "Can't update Quest crasher with empty avatar id", ConsoleColor.Gray, "SetCrasherQuest", 115);
				return;
			}
			PlayerUtils.IsAvatarValid(crasherIdTrimmed, delegate
			{
				ServerAPICore.GetInstance().UploadAvatarCrasher(crasherIdTrimmed, quest: true);
				MiscUtils.SetCrashingAvatarQuest(crasherIdTrimmed);
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, "Quest crasher updated successfully");
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, "Quest crasher updated successfully", ConsoleColor.Gray, "SetCrasherQuest", 126);
			}, delegate(string error)
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Failed updating Quest crasher with the following message: " + error);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, "Failed updating Quest crasher with the following message: " + error, ConsoleColor.Gray, "SetCrasherQuest", 130);
			});
		}

		private void SetDiscordLink(string link, List<KeyCode> pressedKeys, Text text)
		{
			string text2 = link.Trim();
			if (string.IsNullOrEmpty(text2))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Can't update discord link with empty link");
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, "Can't update discord link with empty link", ConsoleColor.Gray, "SetDiscordLink", 141);
				return;
			}
			ServerAPICore.GetInstance().UploadDiscordLink(text2);
			MiscUtils.SetClientDiscordLink(text2);
			GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, "Discord link updated successfully");
			ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, "Discord link updated successfully", ConsoleColor.Gray, "SetDiscordLink", 150);
		}

		private void BlacklistAvatarGlobally(string crasherId, List<KeyCode> pressedKeys, Text text)
		{
			MelonCoroutines.Start(BlacklistAvatarGloballyEnumerator(crasherId));
		}

		private IEnumerator BlacklistAvatarGloballyEnumerator(string crasherId)
		{
			yield return new WaitForEndOfFrame();
			string crasherIdTrimmed = crasherId.Trim();
			if (string.IsNullOrEmpty(crasherIdTrimmed))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Can't blacklist with empty avatar id");
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, "Can't blacklist with empty avatar id", ConsoleColor.Gray, "BlacklistAvatarGloballyEnumerator", 169);
			}
			else if (MiscUtils.blacklistedAvatarIds.Contains(crasherIdTrimmed))
			{
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, "Are you sure you want to unblacklist this avatar?", "Unblacklist", delegate
				{
					ServerAPICore.GetInstance().RemoveAvatarToBlacklistDatabase(crasherIdTrimmed);
					MiscUtils.blacklistedAvatarIds.Remove(crasherIdTrimmed);
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, "Avatar successfully removed from blacklist");
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, "Avatar successfully removed from blacklist", ConsoleColor.Gray, "BlacklistAvatarGloballyEnumerator", 183);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}
			else
			{
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, "Are you sure you want to blacklist this avatar?", "Blacklist", delegate
				{
					ServerAPICore.GetInstance().UploadAvatarToBlacklistDatabase(crasherIdTrimmed);
					MiscUtils.blacklistedAvatarIds.Add(crasherIdTrimmed);
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, "Avatar successfully blacklisted");
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, "Avatar successfully blacklisted", ConsoleColor.Gray, "BlacklistAvatarGloballyEnumerator", 199);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}
		}

		private void BlacklistAuthorGlobally(string auhtorId, List<KeyCode> pressedKeys, Text text)
		{
			MelonCoroutines.Start(BlacklistAuthorGloballyEnumerator(auhtorId));
		}

		private IEnumerator BlacklistAuthorGloballyEnumerator(string auhtorId)
		{
			yield return new WaitForEndOfFrame();
			string auhtorIdTrimmed = auhtorId.Trim();
			if (string.IsNullOrEmpty(auhtorIdTrimmed))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Can't blacklist with empty author id");
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, "Can't blacklist with empty author id", ConsoleColor.Gray, "BlacklistAuthorGloballyEnumerator", 224);
			}
			else if (MiscUtils.blacklistedAuthorIds.Contains(auhtorIdTrimmed))
			{
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, "Are you sure you want to unblacklist this author?", "Unblacklist", delegate
				{
					ServerAPICore.GetInstance().RemoveAuthorToBlacklistDatabase(auhtorIdTrimmed);
					MiscUtils.blacklistedAuthorIds.Remove(auhtorIdTrimmed);
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, "Author successfully removed from blacklist");
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, "Author successfully removed from blacklist", ConsoleColor.Gray, "BlacklistAuthorGloballyEnumerator", 238);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}
			else
			{
				GeneralWrappers.AlertAction(LanguageManager.GetUsedLanguage().NoticeText, "Are you sure you want to blacklist this author?", "Blacklist", delegate
				{
					ServerAPICore.GetInstance().UploadAuthorToBlacklistDatabase(auhtorIdTrimmed);
					MiscUtils.blacklistedAuthorIds.Add(auhtorIdTrimmed);
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, "Author successfully blacklisted");
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().SuccessText, "Author successfully blacklisted", ConsoleColor.Gray, "BlacklistAuthorGloballyEnumerator", 254);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}
		}
	}
}
