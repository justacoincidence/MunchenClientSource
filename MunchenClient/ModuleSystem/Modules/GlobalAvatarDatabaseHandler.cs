using System;
using CustomAvatarListAPI;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using ServerAPI.Core;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class GlobalAvatarDatabaseHandler : ModuleComponent
	{
		private static CustomAvatarList avatarDatabaseList;

		private static bool searchingAfterAvatars;

		protected override string moduleName => "Global Avatar Database Handler";

		internal override void OnUIManagerLoaded()
		{
			if (!Configuration.GetGeneralConfig().DisableAvatarDatabase)
			{
				avatarDatabaseList = CustomAvatarList.Create(LanguageManager.GetUsedLanguage().GlobalAvatarDatabase, 2, delegate
				{
					GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().SearchAfterAvatar, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SearchAfterAvatar, null, LanguageManager.GetUsedLanguage().LeaveBlankToReset);
				});
			}
		}

		private static void SearchAfterAvatar(string avatarName, List<KeyCode> pressedKeys, Text text)
		{
			if (searchingAfterAvatars)
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, "Already searching for avatars");
				ConsoleUtils.Info("Avatars", "Already searching for avatars", ConsoleColor.Gray, "SearchAfterAvatar", 37);
			}
			else
			{
				searchingAfterAvatars = true;
				ServerAPICore.GetInstance().DoAvatarDatabaseRequest(avatarName, OnSearchAfterAvatarFinished);
			}
		}

		private static void OnSearchAfterAvatarFinished(FavoriteAvatar[] avatars)
		{
			if (avatars == null)
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, LanguageManager.GetUsedLanguage().ErrorSearchForAvatar);
				searchingAfterAvatars = false;
				return;
			}
			if (avatars.Length == 0)
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().ErrorNoAvatarsFound);
				searchingAfterAvatars = false;
				return;
			}
			try
			{
				List<ApiAvatar> list = new List<ApiAvatar>();
				foreach (FavoriteAvatar favoriteAvatar in avatars)
				{
					list.Add(favoriteAvatar.ToApiAvatar());
				}
				avatarDatabaseList.GetAvatarListText().text = $"{LanguageManager.GetUsedLanguage().GlobalAvatarDatabase} ({list.Count})";
				avatarDatabaseList.RefreshAvatarsList(list);
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("AvatarDatabase", "RefreshList", e, "OnSearchAfterAvatarFinished", 81);
			}
			searchingAfterAvatars = false;
		}
	}
}
