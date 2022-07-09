using System;
using System.Collections.Generic;
using System.IO;
using MunchenClient.Config;
using MunchenClient.Config.Modules;
using MunchenClient.Core;
using MunchenClient.Menu;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerAPI.Utils;
using UnityEngine;
using VRC.Core;

namespace ServerAPI.Core
{
	internal class ServerAPICore
	{
		private static ServerAPICore instance;

		internal readonly bool debugMode = false;

		internal readonly string baseUrl = string.Empty;

		private int newestServerUpdate = 0;

		private int currentServerUpdate = 0;

		private bool currentServerUpdateForce = false;

		private float nextUpdateCheck = 0f;

		private bool nextUpdateCheckDone = true;

		private float nextUpdateSend = 0f;

		private bool nextUpdateSendDone = true;

		private bool reloggingIntoClient = false;

		private bool requestUserDataPending = false;

		private Action<FavoriteAvatar[]> onDatabaseSearchFinished;

		private TempUploadContainer tempUploadContainer;

		internal static ServerAPICore GetInstance()
		{
			return instance;
		}

		internal static bool InitializeInstance(string baseUrl, bool debugMode)
		{
			if (instance == null)
			{
				instance = new ServerAPICore(baseUrl, debugMode);
				return true;
			}
			return false;
		}

		internal ServerAPICore(string baseUrl, bool debugMode)
		{
			this.baseUrl = baseUrl;
			this.debugMode = debugMode;
			nextUpdateCheck = 0f;
			nextUpdateCheckDone = true;
			nextUpdateSend = 0f;
			nextUpdateSendDone = true;
		}

		internal void OnUIManagerInit()
		{
			if (!debugMode)
			{
				if (!reloggingIntoClient)
				{
					RequestUserData(0.1f);
				}
				else
				{
					requestUserDataPending = true;
				}
			}
		}

		internal void OnUpdate()
		{
			if (debugMode || reloggingIntoClient)
			{
				return;
			}
			if (Time.realtimeSinceStartup >= nextUpdateCheck && nextUpdateCheckDone && nextUpdateSendDone)
			{
				nextUpdateCheck = Time.realtimeSinceStartup + 15f;
				nextUpdateCheckDone = false;
				HttpClientWrapper.SendPostRequest(baseUrl + "fetch.php", new Dictionary<string, string> { { "fetch_type", "1" } }, encryptOnSend: true, decryptOnReceive: true, 0f, OnUpdateCheckFetched);
			}
			if (Time.realtimeSinceStartup >= nextUpdateSend)
			{
				nextUpdateSend = Time.realtimeSinceStartup + 0.5f;
				if (nextUpdateCheckDone && nextUpdateSendDone)
				{
					nextUpdateSendDone = false;
					SendUpdates();
				}
			}
		}

		internal void OnUpdateCheckFetched(bool error, string response)
		{
			if (error)
			{
				ConsoleUtils.Info("ServerAPI", "Failed checking for update (" + response + ")", ConsoleColor.Gray, "OnUpdateCheckFetched", 145);
				nextUpdateCheckDone = true;
			}
			else if (!int.TryParse(response, out newestServerUpdate))
			{
				nextUpdateCheckDone = true;
			}
			else if (newestServerUpdate == currentServerUpdate && !currentServerUpdateForce)
			{
				nextUpdateCheckDone = true;
			}
			else
			{
				HttpClientWrapper.SendPostRequest(baseUrl + "fetch.php", new Dictionary<string, string> { { "fetch_type", "2" } }, encryptOnSend: true, decryptOnReceive: true, 0.5f, OnUpdateFetched);
			}
		}

		internal void OnUpdateFetched(bool error, string response)
		{
			if (error)
			{
				ConsoleUtils.Info("ServerAPI", "Failed updating (" + response + ")", ConsoleColor.Gray, "OnUpdateFetched", 176);
				nextUpdateCheckDone = true;
				return;
			}
			string text = response.Trim();
			if (string.IsNullOrEmpty(text))
			{
				ConsoleUtils.Info("ServerAPI", "Update responded with no data (Server down maybe?)", ConsoleColor.Gray, "OnUpdateFetched", 187);
				nextUpdateCheckDone = true;
				return;
			}
			JObject jObject = JObject.Parse(text);
			switch (JsonParser.GetStatusCode(jObject))
			{
			case 207:
				ReloginToClient();
				nextUpdateCheckDone = true;
				break;
			case 200:
			{
				MiscUtils.SetCrashingAvatarPC((string?)jObject["CrashingAvatarPC"]);
				MiscUtils.SetCrashingAvatarQuest((string?)jObject["CrashingAvatarQuest"]);
				MiscUtils.SetClientDiscordLink(DataProtector.Base64Decode((string?)jObject["ClientDiscordLink"]));
				MiscUtils.blacklistedAvatarIds.Clear();
				MiscUtils.blacklistedAuthorIds.Clear();
				PlayerUtils.playerCustomRank.Clear();
				AvatarFavoritesHandler.favoriteAvatars.Clear();
				JArray jArray = (JArray)jObject["BlacklistedAvatars"];
				for (int i = 0; i < jArray.Count; i++)
				{
					MiscUtils.blacklistedAvatarIds.Add((string?)jArray[i]);
				}
				JArray jArray2 = (JArray)jObject["BlacklistedAuthors"];
				for (int j = 0; j < jArray2.Count; j++)
				{
					MiscUtils.blacklistedAuthorIds.Add((string?)jArray2[j]);
				}
				JArray jArray3 = (JArray)jObject["Users"];
				for (int k = 0; k < jArray3.Count; k++)
				{
					string text2 = ((string?)jArray3[k]["vrchat_id"])?.Trim();
					string text3 = ((string?)jArray3[k]["custom_rank"])?.Trim();
					string text4 = ((string?)jArray3[k]["custom_rank_color"])?.Trim();
					Color color = default(Color);
					bool flag = false;
					bool customRankEnabled = false;
					if (text4 != null && !string.IsNullOrEmpty(text4))
					{
						flag = ColorUtility.TryParseHtmlString(text4, out color);
					}
					if (text3 != null)
					{
						customRankEnabled = !string.IsNullOrEmpty(text3);
					}
					CustomRankInfo value = new CustomRankInfo
					{
						customRankEnabled = customRankEnabled,
						customRank = text3,
						customRankColorEnabled = flag,
						customRankColor = color
					};
					PlayerUtils.playerCustomRank.Add(text2, value);
					PlayerInformation playerInformationByID = PlayerWrappers.GetPlayerInformationByID(text2);
					if (playerInformationByID != null && flag)
					{
						PlayerUtils.playerColorCache[playerInformationByID.displayName] = color;
					}
				}
				JArray jArray4 = (JArray)jObject["Avatars"];
				for (int l = 0; l < jArray4.Count; l++)
				{
					FavoriteAvatar favoriteAvatar = new FavoriteAvatar
					{
						AvatarSortIndex = (int)jArray4[l]["avatar_sort_index"],
						AvatarVersionSystem = (int)jArray4[l]["avatar_version_system"],
						AvatarName = (string?)jArray4[l]["avatar_name"],
						AvatarID = (string?)jArray4[l]["avatar_id"],
						AvatarDescription = (string?)jArray4[l]["avatar_description"],
						AvatarVersion = (int)jArray4[l]["avatar_version"],
						AvatarApiVersion = (int)jArray4[l]["avatar_api_version"],
						AvatarAssetUrl = (string?)jArray4[l]["avatar_asset_url"],
						AvatarImageUrl = (string?)jArray4[l]["avatar_thumbnail"],
						AvatarReleaseStatus = "public",
						AvatarAuthorID = (string?)jArray4[l]["avatar_author_id"],
						AvatarAuthorName = (string?)jArray4[l]["avatar_author_name"],
						AvatarPlatform = (string?)jArray4[l]["avatar_platform"],
						AvatarSupportedPlatforms = (ApiModel.SupportedPlatforms)(int)jArray4[l]["avatar_supported_platforms"]
					};
					if (favoriteAvatar.AvatarVersionSystem == 2)
					{
						if (!AvatarFavoritesHandler.favoriteAvatars.ContainsKey(favoriteAvatar.AvatarID))
						{
							AvatarFavoritesHandler.favoriteAvatars.Add(favoriteAvatar.AvatarID, favoriteAvatar);
						}
						else
						{
							AvatarFavoritesHandler.favoriteAvatars[favoriteAvatar.AvatarID] = favoriteAvatar;
						}
						continue;
					}
					if (AvatarFavoritesHandler.favoriteAvatars.ContainsKey(favoriteAvatar.AvatarID))
					{
						AvatarFavoritesHandler.favoriteAvatars.Remove(favoriteAvatar.AvatarID);
					}
					DeleteAvatarFromDatabase(favoriteAvatar);
					AvatarFavoritesHandler.AddAvatarByIDSilent(favoriteAvatar.AvatarID, null, null);
				}
				if (AvatarFavoritesHandler.favoriteAvatars.Count > 0)
				{
					AvatarFavoritesHandler.RefreshList(string.Empty);
				}
				currentServerUpdateForce = false;
				currentServerUpdate = newestServerUpdate;
				nextUpdateCheckDone = true;
				ConsoleUtils.Info("ServerAPI", $"Newest ranks & avatars fetched (Update: {newestServerUpdate})", ConsoleColor.Gray, "OnUpdateFetched", 324);
				break;
			}
			default:
				ConsoleUtils.Info("ServerAPI", $"Update failed - server responded with Status Code: {JsonParser.GetStatusCode(jObject)} ({JsonParser.GetStatusError(jObject)}) - Report to Killer", ConsoleColor.Gray, "OnUpdateFetched", 329);
				nextUpdateCheckDone = true;
				break;
			}
		}

		internal void SendUpdates()
		{
			if (Configuration.GetUploadQueueConfig().UploadQueue.Count == 0)
			{
				nextUpdateSendDone = true;
				return;
			}
			tempUploadContainer = Configuration.GetUploadQueueConfig().UploadQueue.Dequeue();
			Dictionary<string, string> parameters;
			if (tempUploadContainer.uploadType == 3)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{ "player_id", tempUploadContainer.player_id },
					{ "player_custom_name", tempUploadContainer.player_custom_name }
				};
			}
			else if (tempUploadContainer.uploadType == 4)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{ "player_id", tempUploadContainer.player_id },
					{ "player_custom_color", tempUploadContainer.player_custom_color }
				};
			}
			else if (tempUploadContainer.uploadType == 5 || tempUploadContainer.uploadType == 6)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{ "player_id", tempUploadContainer.player_id }
				};
			}
			else if (tempUploadContainer.uploadType == 8)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{ "avatar_id", tempUploadContainer.avatar_id },
					{
						"avatar_quest",
						tempUploadContainer.avatar_quest ? "1" : "0"
					}
				};
			}
			else if (tempUploadContainer.uploadType == 9)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{
						"discord_link",
						DataProtector.Base64Encode(tempUploadContainer.discordLink)
					}
				};
			}
			else if (tempUploadContainer.uploadType == 10 || tempUploadContainer.uploadType == 11)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{ "avatar_id", tempUploadContainer.avatar_id }
				};
			}
			else if (tempUploadContainer.uploadType == 12)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{ "player_id", tempUploadContainer.player_id },
					{ "player_name", tempUploadContainer.player_name }
				};
			}
			else if (tempUploadContainer.uploadType == 13 || tempUploadContainer.uploadType == 14)
			{
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{ "player_id", tempUploadContainer.player_id }
				};
			}
			else
			{
				if (tempUploadContainer.saved_avatar == null || string.IsNullOrEmpty(tempUploadContainer.saved_avatar.AvatarAssetUrl) || string.IsNullOrEmpty(tempUploadContainer.saved_avatar.AvatarImageUrl))
				{
					nextUpdateSendDone = true;
					return;
				}
				int avatarSupportedPlatforms = (int)tempUploadContainer.saved_avatar.AvatarSupportedPlatforms;
				parameters = new Dictionary<string, string>
				{
					{
						"upload_type",
						tempUploadContainer.uploadType.ToString()
					},
					{
						"avatar_version_system",
						tempUploadContainer.saved_avatar.AvatarVersionSystem.ToString()
					},
					{
						"avatar_id",
						tempUploadContainer.saved_avatar.AvatarID
					},
					{
						"avatar_name",
						tempUploadContainer.saved_avatar.AvatarName
					},
					{
						"avatar_description",
						tempUploadContainer.saved_avatar.AvatarDescription
					},
					{
						"avatar_version",
						tempUploadContainer.saved_avatar.AvatarVersion.ToString()
					},
					{
						"avatar_api_version",
						tempUploadContainer.saved_avatar.AvatarApiVersion.ToString()
					},
					{
						"avatar_asset_url",
						tempUploadContainer.saved_avatar.AvatarAssetUrl
					},
					{
						"avatar_thumbnail",
						tempUploadContainer.saved_avatar.AvatarImageUrl
					},
					{
						"avatar_release_status",
						tempUploadContainer.saved_avatar.AvatarReleaseStatus
					},
					{
						"avatar_author_id",
						tempUploadContainer.saved_avatar.AvatarAuthorID
					},
					{
						"avatar_author_name",
						tempUploadContainer.saved_avatar.AvatarAuthorName
					},
					{
						"avatar_platform",
						tempUploadContainer.saved_avatar.AvatarPlatform
					},
					{
						"avatar_supported_platforms",
						avatarSupportedPlatforms.ToString()
					}
				};
			}
			HttpClientWrapper.SendPostRequest(baseUrl + "upload.php", parameters, encryptOnSend: true, decryptOnReceive: true, 0.1f, OnUploadFinished);
		}

		internal void OnUploadFinished(bool error, string response)
		{
			if (error)
			{
				Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(tempUploadContainer);
				nextUpdateSendDone = true;
				ConsoleUtils.Info("ServerAPI", "Failed uploading data (" + response + ")", ConsoleColor.Gray, "OnUploadFinished", 456);
				return;
			}
			string text = response.Trim();
			if (string.IsNullOrEmpty(text))
			{
				Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(tempUploadContainer);
				nextUpdateSendDone = true;
				ConsoleUtils.Info("ServerAPI", "Upload to cloud responded with no data (Server down maybe?)", ConsoleColor.Gray, "OnUploadFinished", 468);
				return;
			}
			JObject json = JObject.Parse(text);
			switch (JsonParser.GetStatusCode(json))
			{
			case 207:
				ReloginToClient();
				Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(tempUploadContainer);
				nextUpdateSendDone = true;
				return;
			default:
				Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(tempUploadContainer);
				ConsoleUtils.Info("ServerAPI", $"Upload to cloud failed with Status Code: {JsonParser.GetStatusCode(json)} ({JsonParser.GetStatusError(json)}) - Report to Killer", ConsoleColor.Gray, "OnUploadFinished", 490);
				break;
			case 200:
				break;
			}
			nextUpdateSendDone = true;
		}

		internal void UploadAvatarToDatabase(FavoriteAvatar avatar)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 1;
			tempUploadContainer.saved_avatar = avatar;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void DeleteAvatarFromDatabase(FavoriteAvatar avatar)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 2;
			tempUploadContainer.saved_avatar = avatar;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void ChangePlayerCustomRankName(string playerId, string newCustomRankName)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 3;
			tempUploadContainer.player_id = playerId;
			tempUploadContainer.player_custom_name = newCustomRankName;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void ChangePlayerCustomRankColor(string playerId, string newCustomRankColor)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 4;
			tempUploadContainer.player_id = playerId;
			tempUploadContainer.player_custom_color = newCustomRankColor;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void RemovePlayerCustomRankName(string playerId)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 5;
			tempUploadContainer.player_id = playerId;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void RemovePlayerCustomRankColor(string playerId)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 6;
			tempUploadContainer.player_id = playerId;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void UploadAvatarToGlobalDatabase(FavoriteAvatar avatar)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 7;
			tempUploadContainer.saved_avatar = avatar;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void UploadAvatarCrasher(string avatar, bool quest)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 8;
			tempUploadContainer.avatar_id = avatar;
			tempUploadContainer.avatar_quest = quest;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void UploadDiscordLink(string link)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 9;
			tempUploadContainer.discordLink = link;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void UploadAvatarToBlacklistDatabase(string avatarId)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 10;
			tempUploadContainer.avatar_id = avatarId;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void RemoveAvatarToBlacklistDatabase(string avatarId)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 11;
			tempUploadContainer.avatar_id = avatarId;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void LinkVRChatAccountToAuthKey(string player_id, string player_name)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 12;
			tempUploadContainer.player_id = player_id;
			tempUploadContainer.player_name = player_name;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void UploadAuthorToBlacklistDatabase(string authorId)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 13;
			tempUploadContainer.player_id = authorId;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void RemoveAuthorToBlacklistDatabase(string authorId)
		{
			TempUploadContainer tempUploadContainer = default(TempUploadContainer);
			tempUploadContainer.uploadType = 14;
			tempUploadContainer.player_id = authorId;
			TempUploadContainer item = tempUploadContainer;
			Configuration.GetUploadQueueConfig().UploadQueue.Enqueue(item);
			Configuration.SaveUploadQueueConfig();
		}

		internal void DoAvatarDatabaseRequest(string query, Action<FavoriteAvatar[]> onFinished)
		{
			onDatabaseSearchFinished = onFinished;
			HttpClientWrapper.SendPostRequest(baseUrl + "fetch.php", new Dictionary<string, string>
			{
				{ "fetch_type", "4" },
				{ "searchQuery", query }
			}, encryptOnSend: true, decryptOnReceive: true, 0.1f, OnDatabaseSearchFinished);
		}

		private void OnDatabaseSearchFinished(bool error, string response)
		{
			if (error)
			{
				ConsoleUtils.Info("ServerAPI", "Failed searching avatar database (" + response + ")", ConsoleColor.Gray, "OnDatabaseSearchFinished", 685);
				onDatabaseSearchFinished(null);
				return;
			}
			string text = response.Trim();
			if (string.IsNullOrEmpty(text))
			{
				ConsoleUtils.Info("ServerAPI", "Request responded with no data (Server down maybe?)", ConsoleColor.Gray, "OnDatabaseSearchFinished", 696);
				onDatabaseSearchFinished(null);
				return;
			}
			JObject jObject = JObject.Parse(text);
			switch (JsonParser.GetStatusCode(jObject))
			{
			case 207:
				ReloginToClient();
				onDatabaseSearchFinished(null);
				break;
			default:
				ConsoleUtils.Info("ServerAPI", $"Database request to cloud failed with Status Code: {JsonParser.GetStatusCode(jObject)} ({JsonParser.GetStatusError(jObject)}) - Report to Killer", ConsoleColor.Gray, "OnDatabaseSearchFinished", 717);
				onDatabaseSearchFinished(null);
				break;
			case 200:
			{
				JArray jArray = (JArray)jObject["Avatars"];
				FavoriteAvatar[] array = new FavoriteAvatar[jArray.Count];
				for (int i = 0; i < jArray.Count; i++)
				{
					array[i] = new FavoriteAvatar
					{
						AvatarSortIndex = (int)jArray[i]["avatar_sort_index"],
						AvatarVersionSystem = (int)jArray[i]["avatar_version_system"],
						AvatarName = (string?)jArray[i]["avatar_name"],
						AvatarID = (string?)jArray[i]["avatar_id"],
						AvatarDescription = (string?)jArray[i]["avatar_description"],
						AvatarVersion = (int)jArray[i]["avatar_version"],
						AvatarApiVersion = (int)jArray[i]["avatar_api_version"],
						AvatarAssetUrl = (string?)jArray[i]["avatar_asset_url"],
						AvatarImageUrl = (string?)jArray[i]["avatar_thumbnail"],
						AvatarReleaseStatus = (string?)jArray[i]["avatar_release_status"],
						AvatarAuthorID = (string?)jArray[i]["avatar_author_id"],
						AvatarAuthorName = (string?)jArray[i]["avatar_author_name"],
						AvatarPlatform = (string?)jArray[i]["avatar_platform"],
						AvatarSupportedPlatforms = (ApiModel.SupportedPlatforms)(int)jArray[i]["avatar_supported_platforms"]
					};
				}
				onDatabaseSearchFinished(array);
				break;
			}
			}
		}

		internal void ForceUpdateFromServer()
		{
			currentServerUpdateForce = true;
			nextUpdateCheck = 0f;
		}

		internal bool IsDebugMode()
		{
			return debugMode;
		}

		internal void ReloginToClient()
		{
			reloggingIntoClient = true;
			string text = File.ReadAllText(AuthKeyConfig.ConfigLocation).Trim();
			AuthKeyConfig authKeyConfig = null;
			try
			{
				authKeyConfig = JsonConvert.DeserializeObject<AuthKeyConfig>(text);
			}
			catch (Exception)
			{
				authKeyConfig = new AuthKeyConfig
				{
					Token = DataProtector.EncryptData(text)
				};
			}
			HttpClientWrapper.SendPostRequest(baseUrl + "../../core/login.php", new Dictionary<string, string>
			{
				{ "cheat_id", "1" },
				{
					"auth_token",
					DataProtector.DecryptData(authKeyConfig.Token)
				},
				{
					"hardware_id",
					MainUtils.GetActualHardwareIdentifier()
				},
				{
					"time_current",
					DataProtector.GetCurrentTimeInEpoch().ToString()
				}
			}, encryptOnSend: true, decryptOnReceive: true, 0.1f, OnReloginFinished);
		}

		internal void OnReloginFinished(bool error, string response)
		{
			if (error)
			{
				ConsoleUtils.Info("ServerAPI", "Failed relogging into client (" + response + ")", ConsoleColor.Gray, "OnReloginFinished", 794);
				reloggingIntoClient = false;
				return;
			}
			string text = response.Trim();
			if (string.IsNullOrEmpty(text))
			{
				ConsoleUtils.Info("ServerAPI", "Relogging into client failed (Server down maybe?)", ConsoleColor.Gray, "OnReloginFinished", 805);
				reloggingIntoClient = false;
				return;
			}
			JObject json = JObject.Parse(text);
			if (JsonParser.GetStatusCode(json) != 200)
			{
				ConsoleUtils.Info("ServerAPI", $"Relogging into client failed with Status Code: {JsonParser.GetStatusCode(json)} ({JsonParser.GetStatusError(json)}) - Report to Killer", ConsoleColor.Gray, "OnReloginFinished", 816);
				reloggingIntoClient = false;
				return;
			}
			reloggingIntoClient = false;
			if (requestUserDataPending)
			{
				RequestUserData(0.1f);
			}
			ConsoleUtils.Info("ServerAPI", "Relogging into client successful", ConsoleColor.Gray, "OnReloginFinished", 830);
		}

		internal void RequestUserData(float delay)
		{
			HttpClientWrapper.SendGetRequest(baseUrl + "../../core/account.php", decryptOnReceive: true, delay, OnRequestUserDataFinished);
		}

		internal void OnRequestUserDataFinished(bool error, string response)
		{
			if (error)
			{
				ConsoleUtils.Info("ServerAPI", "Failed requesting user data (" + response + ")", ConsoleColor.Gray, "OnRequestUserDataFinished", 842);
				RequestUserData(5f);
				return;
			}
			string text = response.Trim();
			if (string.IsNullOrEmpty(text))
			{
				ConsoleUtils.Info("ServerAPI", "Requesting user data failed (Server down maybe?)", ConsoleColor.Gray, "OnRequestUserDataFinished", 853);
				RequestUserData(5f);
				return;
			}
			JObject jObject = JObject.Parse(text);
			if (JsonParser.GetStatusCode(jObject) != 200)
			{
				ConsoleUtils.Info("ServerAPI", $"Requesting user data failed with Status Code: {JsonParser.GetStatusCode(jObject)} ({JsonParser.GetStatusError(jObject)}) - Report to Killer", ConsoleColor.Gray, "OnRequestUserDataFinished", 864);
				RequestUserData(5f);
				return;
			}
			MainClientMenu.userProfileHeader.SetHeaderText(LanguageManager.GetUsedLanguage().UserAccountCategory + " - " + (string?)jObject["UserName"] + " (UID: " + (string?)jObject["UserID"] + ")");
			MainClientMenu.userProfilePicture.SetIcon(MainUtils.CreateSprite((string?)jObject["UserPicture"]));
			MainClientMenu.userProfileRank.SetButtonText((string?)jObject["UserRank"]);
			MainClientMenu.userProfileRank.SetToolTip("Your current status on the forums is: " + (string?)jObject["UserRank"]);
			MainClientMenu.userProfileSubExpiry.SetButtonText((string?)jObject["SubExpiry"]);
			MainClientMenu.userProfileSubExpiry.SetToolTip("Your subscription will expire in: " + (string?)jObject["SubExpiry"]);
			requestUserDataPending = false;
		}
	}
}
