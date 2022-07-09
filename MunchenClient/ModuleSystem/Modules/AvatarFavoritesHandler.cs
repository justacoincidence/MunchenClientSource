using System;
using System.Collections.Generic;
using System.Linq;
using CustomAvatarListAPI;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using ServerAPI.Core;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.SDKBase.Validation.Performance.Stats;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class AvatarFavoritesHandler : ModuleComponent
	{
		private static CustomAvatarList favoriteAvatarList;

		private static CustomMenuButton avatarFavoriteStateButton;

		internal const int currentAvatarVersionSystem = 2;

		internal static System.Collections.Generic.Dictionary<string, FavoriteAvatar> favoriteAvatars = new System.Collections.Generic.Dictionary<string, FavoriteAvatar>();

		private GameObject avatarPage = null;

		private bool listNeedsRefresh = true;

		protected override string moduleName => "Avatar Favorites Handler";

		private static string GetProperListName(int avatarAmount)
		{
			return $"{LanguageManager.GetUsedLanguage().PersonalAvatarDatabase} ({avatarAmount})";
		}

		internal override void OnUIManagerLoaded()
		{
			if (Configuration.GetGeneralConfig().DisableAvatarFavorites)
			{
				return;
			}
			if (!Configuration.GetGeneralConfig().DisableAvatarFavoritesSearchButton)
			{
				favoriteAvatarList = CustomAvatarList.Create(GetProperListName(favoriteAvatars.Count), 1, delegate
				{
					GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().SearchAfterAvatar, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, SearchAfterAvatar, null, LanguageManager.GetUsedLanguage().LeaveBlankToReset);
				});
			}
			else
			{
				favoriteAvatarList = CustomAvatarList.Create(GetProperListName(favoriteAvatars.Count), 1);
			}
			MainClientMenu.userProfileAvatarsSaved.SetButtonText(favoriteAvatars.Count.ToString());
			MainClientMenu.userProfileAvatarsSaved.SetToolTip($"You currently got {favoriteAvatars.Count} avatars saved");
			avatarPage = GameObject.Find("UserInterface/MenuContent/Screens/Avatar");
			avatarFavoriteStateButton = CustomMenuButton.Create(LanguageManager.GetUsedLanguage().FavoriteAvatar, 820f, 9.59f, OnAvatarStateFavorited);
			favoriteAvatarList.GetAvatarList().field_Public_SimpleAvatarPedestal_0.field_Internal_Action_4_String_GameObject_AvatarPerformanceStats_ObjectPublicBoBoBoBoBoBoBoBoBoBoUnique_0 = (Il2CppSystem.Action<string, GameObject, AvatarPerformanceStats, ObjectPublicBoBoBoBoBoBoBoBoBoBoUnique>)(System.Action<string, GameObject, AvatarPerformanceStats, ObjectPublicBoBoBoBoBoBoBoBoBoBoUnique>)OnAvatarInMenuClicked;
			CustomMenuButton.Create(LanguageManager.GetUsedLanguage().AddAvatarByID, 1160f, 9.59f, delegate
			{
				GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().AddAvatarByID, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, AddAvatarByID);
			});
		}

		internal override void OnUpdate()
		{
			if (avatarPage != null && avatarPage.activeSelf)
			{
				if (listNeedsRefresh)
				{
					RefreshList(string.Empty);
					listNeedsRefresh = false;
				}
			}
			else if (!listNeedsRefresh)
			{
				listNeedsRefresh = true;
			}
		}

		private static void SearchAfterAvatar(string avatarName, Il2CppSystem.Collections.Generic.List<KeyCode> pressedKeys, Text text)
		{
			RefreshList(avatarName);
		}

		internal static void AddAvatarByID(string avatarId, Il2CppSystem.Collections.Generic.List<KeyCode> keycodeList, Text text)
		{
			if (!Configuration.GetGeneralConfig().DisableAvatarFavorites)
			{
				ApiAvatar apiAvatar = new ApiAvatar();
				apiAvatar.id = avatarId;
				((ApiModel)apiAvatar).Get((Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)AvatarFoundHandler, (Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)AvatarNotFoundHandler, (Il2CppSystem.Collections.Generic.Dictionary<string, BestHTTP.JSON.Json.Token>)null, false);
			}
		}

		internal static void AddAvatarByIDSilent(string avatarId, Il2CppSystem.Collections.Generic.List<KeyCode> keycodeList, Text text)
		{
			if (!Configuration.GetGeneralConfig().DisableAvatarFavorites)
			{
				ApiAvatar apiAvatar = new ApiAvatar();
				apiAvatar.id = avatarId;
				((ApiModel)apiAvatar).Get((Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)AvatarFoundHandlerSilent, (Il2CppSystem.Action<ApiContainer>)(System.Action<ApiContainer>)AvatarNotFoundHandlerSilent, (Il2CppSystem.Collections.Generic.Dictionary<string, BestHTTP.JSON.Json.Token>)null, false);
			}
		}

		private static void AvatarNotFoundHandler(ApiContainer apiContainer)
		{
			GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().ErrorText, LanguageManager.GetUsedLanguage().AvatarNotFound);
		}

		private static void AvatarFoundHandler(ApiContainer apiContainer)
		{
			ApiAvatar apiAvatar = apiContainer.Model.Cast<ApiAvatar>();
			if (apiAvatar.authorId != PlayerWrappers.GetLocalPlayerInformation().id && apiAvatar.releaseStatus == "private")
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().AvatarPrivate);
			}
			else if (favoriteAvatars.ContainsKey(apiAvatar.id))
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().AvatarAlreadyFavorited);
			}
			else
			{
				AddRemoveAvatarToFavoriteList(apiAvatar, notifyUser: true);
			}
		}

		private static void AvatarNotFoundHandlerSilent(ApiContainer apiContainer)
		{
			ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ErrorText, LanguageManager.GetUsedLanguage().AvatarNotFound, System.ConsoleColor.Gray, "AvatarNotFoundHandlerSilent", 204);
		}

		private static void AvatarFoundHandlerSilent(ApiContainer apiContainer)
		{
			ApiAvatar apiAvatar = apiContainer.Model.Cast<ApiAvatar>();
			if (apiAvatar.authorId != PlayerWrappers.GetLocalPlayerInformation().id && apiAvatar.releaseStatus == "private")
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().AvatarPrivate, System.ConsoleColor.Gray, "AvatarFoundHandlerSilent", 213);
			}
			else if (favoriteAvatars.ContainsKey(apiAvatar.id))
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().AvatarAlreadyFavorited, System.ConsoleColor.Gray, "AvatarFoundHandlerSilent", 220);
			}
			else
			{
				AddRemoveAvatarToFavoriteList(apiAvatar, notifyUser: false);
			}
		}

		private static void OnAvatarStateFavorited()
		{
			ApiAvatar field_Internal_ApiAvatar_ = favoriteAvatarList.GetAvatarList().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0;
			if (field_Internal_ApiAvatar_.authorId != PlayerWrappers.GetLocalPlayerInformation().id && field_Internal_ApiAvatar_.releaseStatus == "private")
			{
				GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().AvatarPrivate);
				return;
			}
			if (!favoriteAvatars.ContainsKey(field_Internal_ApiAvatar_.id))
			{
				avatarFavoriteStateButton.GetButtonText().text = LanguageManager.GetUsedLanguage().UnfavoriteAvatar;
			}
			else
			{
				avatarFavoriteStateButton.GetButtonText().text = LanguageManager.GetUsedLanguage().FavoriteAvatar;
			}
			AddRemoveAvatarToFavoriteList(field_Internal_ApiAvatar_, notifyUser: true);
		}

		private static void OnAvatarInMenuClicked(string avatar, GameObject y, AvatarPerformanceStats z, ObjectPublicBoBoBoBoBoBoBoBoBoBoUnique o)
		{
			if (favoriteAvatars.ContainsKey(favoriteAvatarList.GetAvatarList().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.id))
			{
				avatarFavoriteStateButton.GetButtonText().text = LanguageManager.GetUsedLanguage().UnfavoriteAvatar;
			}
			else
			{
				avatarFavoriteStateButton.GetButtonText().text = LanguageManager.GetUsedLanguage().FavoriteAvatar;
			}
		}

		internal static void RefreshList(string searchQuery)
		{
			if (Configuration.GetGeneralConfig().DisableAvatarFavorites || favoriteAvatarList == null)
			{
				return;
			}
			Il2CppSystem.Collections.Generic.List<ApiAvatar> list = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
			string value = searchQuery.Trim().ToLower();
			if (!string.IsNullOrEmpty(value))
			{
				foreach (System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> item in favoriteAvatars.ToList().OrderByDescending(delegate(System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> entry)
				{
					System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> keyValuePair2 = entry;
					return keyValuePair2.Value.AvatarSortIndex;
				}))
				{
					if (item.Value.AvatarName.ToLower().Contains(value))
					{
						list.Add(item.Value.ToApiAvatar());
					}
				}
			}
			else
			{
				foreach (System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> item2 in favoriteAvatars.ToList().OrderByDescending(delegate(System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> entry)
				{
					System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> keyValuePair = entry;
					return keyValuePair.Value.AvatarSortIndex;
				}))
				{
					list.Add(item2.Value.ToApiAvatar());
				}
			}
			MainClientMenu.userProfileAvatarsSaved.SetButtonText(favoriteAvatars.Count.ToString());
			MainClientMenu.userProfileAvatarsSaved.SetToolTip($"You currently got {favoriteAvatars.Count} avatars saved");
			favoriteAvatarList.GetAvatarListText().text = GetProperListName(list.Count);
			favoriteAvatarList.RefreshAvatarsList(list);
		}

		internal static bool AddRemoveAvatarToFavoriteList(ApiAvatar avi, bool notifyUser)
		{
			if (avi.authorId != PlayerWrappers.GetLocalPlayerInformation().id && avi.releaseStatus == "private")
			{
				if (notifyUser)
				{
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().NoticeText, LanguageManager.GetUsedLanguage().AvatarPrivate);
				}
				return false;
			}
			if (!favoriteAvatars.ContainsKey(avi.id))
			{
				IOrderedEnumerable<System.Collections.Generic.KeyValuePair<string, FavoriteAvatar>> source = favoriteAvatars.ToList().OrderByDescending(delegate(System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> entry)
				{
					System.Collections.Generic.KeyValuePair<string, FavoriteAvatar> keyValuePair = entry;
					return keyValuePair.Value.AvatarSortIndex;
				});
				int avatarSortIndex = ((source.Count() <= 0) ? 1 : (source.ElementAt(0).Value.AvatarSortIndex + 1));
				FavoriteAvatar favoriteAvatar = new FavoriteAvatar(avi)
				{
					AvatarSortIndex = avatarSortIndex
				};
				favoriteAvatars.Add(avi.id, favoriteAvatar);
				ServerAPICore.GetInstance().UploadAvatarToDatabase(favoriteAvatar);
				if (notifyUser)
				{
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, LanguageManager.GetUsedLanguage().AvatarFavorited.Replace("{name}", avi.name));
				}
			}
			else
			{
				ServerAPICore.GetInstance().DeleteAvatarFromDatabase(favoriteAvatars[avi.id]);
				favoriteAvatars.Remove(avi.id);
				if (notifyUser)
				{
					GeneralWrappers.AlertPopup(LanguageManager.GetUsedLanguage().SuccessText, LanguageManager.GetUsedLanguage().AvatarUnfavorited.Replace("{name}", avi.name));
				}
			}
			RefreshList(string.Empty);
			return true;
		}
	}
}
