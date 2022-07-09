using System;
using System.Collections;
using System.IO;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRCSDK2;

namespace MunchenClient.Menu
{
	internal class VideoPlayerMenu : QuickMenuNestedMenu
	{
		private readonly string exportedVideosFilePath = Configuration.GetClientFolderPath() + "/ExportedVideos.txt";

		private readonly string queueVideosFilePath = Configuration.GetClientFolderPath() + "/Videos.txt";

		private static bool isQueueingVideosFromFile;

		internal VideoPlayerMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerMenuDescription)
		{
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().MiscellaneousCategory);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().QueueVideo, delegate
			{
				GeneralWrappers.ShowInputPopup(LanguageManager.GetUsedLanguage().QueueVideo, string.Empty, InputField.InputType.Standard, isNumeric: false, LanguageManager.GetUsedLanguage().ConfirmText, QueueVideoByUrlCallback);
			}, LanguageManager.GetUsedLanguage().QueueVideoDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().QueueVideosFromFile, delegate
			{
				if (!isQueueingVideosFromFile)
				{
					isQueueingVideosFromFile = true;
					MelonCoroutines.Start(QueueVideosFromFile());
				}
			}, LanguageManager.GetUsedLanguage().QueueVideosFromFileDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().ExportVideosToFile, delegate
			{
				ExportVideosToFile();
			}, LanguageManager.GetUsedLanguage().ExportVideosToFileDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().EarrapeVideo, delegate
			{
				OverrideVideoPlayers("https://cdn.discordapp.com/attachments/862380610013364336/865458390801055774/owowhatsthis.mp4");
			}, LanguageManager.GetUsedLanguage().EarrapeVideoDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().PlayVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer8 = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer8 == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 73);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer8.gameObject, "Play", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().PlayVideoClicked, System.ConsoleColor.Gray, ".ctor", 80);
				}
			}, LanguageManager.GetUsedLanguage().PlayVideoDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().PauseUnpauseVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer7 = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer7 == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 90);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer7.gameObject, "Pause", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().PauseUnpauseClicked, System.ConsoleColor.Gray, ".ctor", 97);
				}
			}, LanguageManager.GetUsedLanguage().PauseUnpauseDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().PreviousVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer6 = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer6 == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 107);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer6.gameObject, "Previous", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().PreviousClicked, System.ConsoleColor.Gray, ".ctor", 114);
				}
			}, LanguageManager.GetUsedLanguage().PreviousDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().NextVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer5 = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer5 == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 124);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer5.gameObject, "Next", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().NextClicked, System.ConsoleColor.Gray, ".ctor", 131);
				}
			}, LanguageManager.GetUsedLanguage().NextDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().StopVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer4 = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer4 == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 143);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer4.gameObject, "Stop", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().StopClicked, System.ConsoleColor.Gray, ".ctor", 150);
				}
			}, LanguageManager.GetUsedLanguage().StopDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().ClearVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer3 = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer3 == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 160);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer3.gameObject, "Clear", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().ClearClicked, System.ConsoleColor.Gray, ".ctor", 167);
				}
			}, LanguageManager.GetUsedLanguage().ClearDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().SpeedUpVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer2 = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer2 == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 177);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer2.gameObject, "SpeedUp", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().SpeedUpClicked, System.ConsoleColor.Gray, ".ctor", 184);
				}
			}, LanguageManager.GetUsedLanguage().SpeedUpDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().SpeedDownVideo, delegate
			{
				VRC_SyncVideoPlayer worldVideoPlayer = GeneralWrappers.GetWorldVideoPlayer();
				if (worldVideoPlayer == null)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, ".ctor", 194);
				}
				else
				{
					Networking.RPC(RPC.Destination.Owner, worldVideoPlayer.gameObject, "SpeedDown", new Il2CppSystem.Object[0]);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().SpeedDownClicked, System.ConsoleColor.Gray, ".ctor", 201);
				}
			}, LanguageManager.GetUsedLanguage().SpeedDownDescription);
		}

		private void QueueVideoByUrlCallback(string videoUrl, List<KeyCode> pressedKeys, Text text)
		{
			QueueVideoByUrl(videoUrl, skipCurrentVideo: false, notify: true);
		}

		private IEnumerator QueueVideosFromFile()
		{
			ConsoleUtils.Info("Video Player", "Starting to queue videos from " + queueVideosFilePath, System.ConsoleColor.Gray, "QueueVideosFromFile", 212);
			string[] songs;
			try
			{
				songs = File.ReadAllLines(queueVideosFilePath);
			}
			catch (System.Exception ex)
			{
				System.Exception e = ex;
				ConsoleUtils.Exception("Video Player", "Queue Videos From File", e, "QueueVideosFromFile", 222);
				yield break;
			}
			for (int i = 0; i < songs.Length; i++)
			{
				QueueVideoByUrl(songs[i], skipCurrentVideo: false, notify: false);
				yield return new WaitForSeconds(0.25f);
			}
			ConsoleUtils.Info("Video Player", $"Queued {songs.Length} videos", System.ConsoleColor.Gray, "QueueVideosFromFile", 234);
			isQueueingVideosFromFile = false;
		}

		internal void QueueVideoByUrl(string videoUrl, bool skipCurrentVideo, bool notify)
		{
			VRC_SyncVideoPlayer worldVideoPlayer = GeneralWrappers.GetWorldVideoPlayer();
			if (worldVideoPlayer == null)
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, "QueueVideoByUrl", 247);
				return;
			}
			Networking.RPC(RPC.Destination.Owner, worldVideoPlayer.gameObject, "AddURL", new Il2CppSystem.Object[1] { (Il2CppSystem.String)videoUrl.Trim() });
			if (skipCurrentVideo)
			{
				Networking.RPC(RPC.Destination.Owner, worldVideoPlayer.gameObject, "Next", new Il2CppSystem.Object[0]);
			}
			if (notify)
			{
				ConsoleUtils.Info("Video Player", "Video queued: " + videoUrl.Trim(), System.ConsoleColor.Gray, "QueueVideoByUrl", 264);
			}
		}

		internal bool OverrideVideoPlayers(string url)
		{
			string value = url.Trim();
			if (string.IsNullOrEmpty(value))
			{
				ConsoleUtils.Info("Video Player", "Can't override video players (URL is null)", System.ConsoleColor.Gray, "OverrideVideoPlayers", 274);
				return false;
			}
			foreach (SyncVideoPlayer item in UnityEngine.Object.FindObjectsOfType<SyncVideoPlayer>())
			{
				if (item != null)
				{
					Networking.LocalPlayer.TakeOwnership(item.gameObject);
					VRC_SyncVideoPlayer field_Private_VRC_SyncVideoPlayer_ = item.field_Private_VRC_SyncVideoPlayer_0;
					field_Private_VRC_SyncVideoPlayer_.Stop();
					field_Private_VRC_SyncVideoPlayer_.Clear();
					field_Private_VRC_SyncVideoPlayer_.AddURL(url);
					field_Private_VRC_SyncVideoPlayer_.Next();
					field_Private_VRC_SyncVideoPlayer_.Play();
				}
			}
			foreach (SyncVideoStream item2 in UnityEngine.Object.FindObjectsOfType<SyncVideoStream>())
			{
				if (item2 != null)
				{
					Networking.LocalPlayer.TakeOwnership(item2.gameObject);
					VRC_SyncVideoStream field_Private_VRC_SyncVideoStream_ = item2.field_Private_VRC_SyncVideoStream_0;
					field_Private_VRC_SyncVideoStream_.Stop();
					field_Private_VRC_SyncVideoStream_.Clear();
					field_Private_VRC_SyncVideoStream_.AddURL(url);
					field_Private_VRC_SyncVideoStream_.Next();
					field_Private_VRC_SyncVideoStream_.Play();
				}
			}
			foreach (VRCUrlInputField item3 in UnityEngine.Object.FindObjectsOfType<VRCUrlInputField>())
			{
				if (item3 != null)
				{
					item3.text = url;
					item3.onEndEdit.Invoke(url);
				}
			}
			return true;
		}

		private void ExportVideosToFile()
		{
			VRC_SyncVideoPlayer worldVideoPlayer = GeneralWrappers.GetWorldVideoPlayer();
			if (worldVideoPlayer == null)
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().VideoPlayerMenuName, LanguageManager.GetUsedLanguage().VideoPlayerNoPlayer, System.ConsoleColor.Gray, "ExportVideosToFile", 327);
				return;
			}
			string[] array = new string[worldVideoPlayer.Videos.Length];
			for (int i = 0; i < worldVideoPlayer.Videos.Length; i++)
			{
				array[i] = worldVideoPlayer.Videos[i].URL;
			}
			try
			{
				File.WriteAllLines(exportedVideosFilePath, array);
			}
			catch (System.Exception e)
			{
				ConsoleUtils.Exception("Video Player", "Export Videos To File", e, "ExportVideosToFile", 345);
				return;
			}
			ConsoleUtils.Info("Video Player", $"Exported {array.Length} videos to {exportedVideosFilePath}", System.ConsoleColor.Gray, "ExportVideosToFile", 350);
		}
	}
}
