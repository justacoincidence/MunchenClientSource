using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Misc;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Wrappers;
using SharpNeatLib.Maths;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRCSDK2;

namespace MunchenClient.Utils
{
	internal class GeneralUtils
	{
		internal static readonly FastRandom fastRandom = new FastRandom();

		private const string glyphs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPVWXYZ0123456789";

		internal static readonly Il2CppSystem.Object[] rpcParameters = new Il2CppSystem.Object[0];

		internal static readonly Color defaultMenuColor = new Color(0.05f, 0.65f, 0.68f);

		private const string clientName = "Munchen Client";

		private const string clientDevelopers = "Killer_bigpoint";

		private const string clientVersion = "2.2.4";

		internal static bool clientBetaBranch = false;

		internal static bool clientSpecialBenefits = false;

		private static Process mainProcess = null;

		private static int screenRefreshRate = -1;

		internal static int targetedRefreshRate = 90;

		internal static bool voiceImitation = false;

		internal static int voiceImitationPlayerKey = -1;

		internal static bool inverseKinematicMimic = false;

		internal static int inverseKinematicMimicPlayerKey = -1;

		internal static bool portableMirror = false;

		internal static bool hideSelf = false;

		internal static bool flight = false;

		internal static bool capsuleHider = false;

		internal static bool itemOrbit = false;

		internal static bool localAvatarClone = false;

		internal static bool ghostJoin = false;

		internal static bool serialization = false;

		internal static bool lockInstance = false;

		internal static bool fakelag = false;

		internal static int fakelagTimer = 0;

		private static bool gameCloserExploitRunning = false;

		internal static readonly System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, bool>> notificationTracker = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, bool>>();

		internal static bool isQuickMenuOpen = false;

		internal static bool isConnectedToInstance = false;

		internal static void AssignMainProcess(Process process)
		{
			if (mainProcess != null)
			{
				ConsoleUtils.Info("Main", "Cannot reassign main process", System.ConsoleColor.Gray, "AssignMainProcess", 87);
			}
			else
			{
				mainProcess = process;
			}
		}

		internal static Process GetMainProcess()
		{
			return mainProcess;
		}

		internal static bool HasSpecialBenefits()
		{
			return clientSpecialBenefits;
		}

		internal static bool IsBetaClient()
		{
			return clientBetaBranch;
		}

		internal static string GetClientDevelopers()
		{
			return "Killer_bigpoint";
		}

		internal static string GetClientName()
		{
			return "Munchen Client";
		}

		internal static string GetClientVersion()
		{
			if (IsBetaClient())
			{
				return "v2.2.4 Beta";
			}
			return "v2.2.4 Stable";
		}

		internal static bool CheckNotification(string playerName, string notification)
		{
			if (!notificationTracker.ContainsKey(playerName))
			{
				return false;
			}
			if (!notificationTracker[playerName].ContainsKey(notification))
			{
				return false;
			}
			return notificationTracker[playerName][notification];
		}

		internal static void ToggleNotification(string playerName, string notification, bool state)
		{
			if (!notificationTracker.ContainsKey(playerName))
			{
				notificationTracker.Add(playerName, new System.Collections.Generic.Dictionary<string, bool>());
			}
			notificationTracker[playerName][notification] = state;
		}

		internal static void ToggleCollidersOnPlayer(bool toggle)
		{
			VRCPlayer currentPlayer = PlayerWrappers.GetCurrentPlayer();
			if (!(currentPlayer == null))
			{
				System.Collections.Generic.List<Collider> list = MiscUtils.FindAllComponentsInGameObject<Collider>(currentPlayer.gameObject, includeInactive: true, searchParent: false);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].enabled = toggle;
				}
			}
		}

		internal static void ToggleCollidersOnItems(bool toggle)
		{
			Collider[] array = UnityEngine.Object.FindObjectsOfType<Collider>();
			Component component = ((System.Collections.Generic.IEnumerable<Component>)PlayerWrappers.GetCurrentPlayer().GetComponents<Collider>()).FirstOrDefault();
			foreach (Collider collider in array)
			{
				if ((collider.GetComponent<PlayerSelector>() != null || collider.GetComponent<QuickMenu>() != null || (bool)collider.GetComponent<VRC.SDKBase.VRC_AvatarPedestal>() || collider.GetComponent<VRC.SDKBase.VRC_Pickup>() != null || collider.GetComponent<VRC_Station>() != null) && collider != component)
				{
					collider.enabled = toggle;
				}
			}
		}

		internal static void SetNameplateWallhack(bool state)
		{
			if (state)
			{
				foreach (System.Collections.Generic.KeyValuePair<string, PlayerInformation> playerCaching in PlayerUtils.playerCachingList)
				{
					playerCaching.Value.nameplateCanvas.layer = 19;
				}
				return;
			}
			foreach (System.Collections.Generic.KeyValuePair<string, PlayerInformation> playerCaching2 in PlayerUtils.playerCachingList)
			{
				playerCaching2.Value.nameplateCanvas.layer = 12;
			}
		}

		internal static PlayerRankStatus GetUserRank(PlayerInformation user, bool ignoreLocal)
		{
			if (user.isLocalPlayer && !ignoreLocal)
			{
				return PlayerRankStatus.Local;
			}
			if (user.IsFriends())
			{
				return PlayerRankStatus.Friend;
			}
			if (user.apiUser.hasVeteranTrustLevel)
			{
				return PlayerRankStatus.Trusted;
			}
			if (user.apiUser.hasTrustedTrustLevel)
			{
				return PlayerRankStatus.Known;
			}
			if (user.apiUser.hasKnownTrustLevel)
			{
				return PlayerRankStatus.User;
			}
			if (user.apiUser.hasBasicTrustLevel)
			{
				return PlayerRankStatus.NewUser;
			}
			return PlayerRankStatus.Visitor;
		}

		internal static void ChangeGameProcessPriority(bool highPriority)
		{
			if (highPriority)
			{
				mainProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
			}
			else
			{
				mainProcess.PriorityClass = ProcessPriorityClass.Normal;
			}
			if (IsBetaClient())
			{
				ConsoleUtils.Info("Performance", highPriority ? "Enabling High Process Priority" : "Enabling Normal Process Priority", System.ConsoleColor.Gray, "ChangeGameProcessPriority", 253);
			}
		}

		internal static void ChangeGameCoreAffinity(bool skipHyperThreading)
		{
			long num = 0L;
			int num2 = System.Environment.ProcessorCount - 1;
			int num3 = ((!skipHyperThreading) ? 1 : 2);
			for (int i = 0; i < System.Environment.ProcessorCount; i++)
			{
				if (num2 <= 0)
				{
					break;
				}
				num |= 1L << num2;
				num2 -= num3;
			}
			UnmanagedUtils.SetProcessAffinityMask(mainProcess.Handle, new System.IntPtr(num));
			if (IsBetaClient())
			{
				ConsoleUtils.Info("Performance", skipHyperThreading ? "Disabling HyperThreading" : "Enabling HyperThreading", System.ConsoleColor.Gray, "ChangeGameCoreAffinity", 274);
			}
		}

		internal static void ToggleFlight(bool state)
		{
			if (state)
			{
				Physics.gravity = Vector3.zero;
				flight = true;
				ToggleCollidersOnPlayer(toggle: false);
			}
			else
			{
				PlayerHandler.ResetGravity();
				flight = false;
				ToggleCollidersOnPlayer(toggle: true);
			}
		}

		internal static void InformHudTextThreaded(string identifier, string text, bool logToConsole = false)
		{
			MainUtils.mainThreadQueue.Enqueue(delegate
			{
				InformHudText(identifier, text, logToConsole);
			});
		}

		internal static void InformHudText(string identifier, string text, bool logToConsole = false)
		{
			try
			{
				UserInterface.AddNotificationToHud(text);
			}
			catch (System.Exception e)
			{
				ConsoleUtils.Exception("Utils", "Create HUD Notification", e, "InformHudText", 312);
			}
			if (!logToConsole)
			{
				return;
			}
			int num = text.LastIndexOf("<color=");
			if (num != -1)
			{
				int num2 = text.IndexOf('>', num);
				string htmlString = text.Substring(num + 7, num2 - (num + 7));
				if (ColorUtility.TryParseHtmlString(htmlString, out var color))
				{
					ConsoleUtils.Info(identifier, text, ConsoleUtils.ClosestConsoleColor((byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f)), "InformHudText", 326);
				}
				else
				{
					ConsoleUtils.Info(identifier, text, System.ConsoleColor.Gray, "InformHudText", 330);
				}
			}
			else
			{
				ConsoleUtils.Info(identifier, text, System.ConsoleColor.Gray, "InformHudText", 335);
			}
		}

		internal static bool SaveTextureToDisk(Texture texture, string fileDirectory, string fileName, bool includeCRC32InFileName = false)
		{
			try
			{
				Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, mipChain: false);
				RenderTexture active = RenderTexture.active;
				RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 32);
				Graphics.Blit(texture, temporary);
				RenderTexture.active = temporary;
				texture2D.ReadPixels(new Rect(0f, 0f, temporary.width, temporary.height), 0, 0);
				texture2D.Apply();
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
				byte[] bytes = ImageConversion.EncodeToPNG(texture2D);
				if (!Directory.Exists(fileDirectory))
				{
					Directory.CreateDirectory(fileDirectory);
				}
				if (!includeCRC32InFileName)
				{
					File.WriteAllBytes(fileDirectory + "/" + fileName + ".png", bytes);
				}
				else
				{
					File.WriteAllBytes(fileDirectory + "/" + fileName + " - " + Crc32.CalculateCRC(bytes) + ".png", bytes);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		internal static void ClearVRAM()
		{
			AssetBundleDownloadManager assetBundleDownloadManager = AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0;
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			PlayerManager playerManager = PlayerManager.prop_PlayerManager_0;
			Player[] P = playerManager.field_Private_List_1_Player_0.ToArray();
			for (int i = 0; i < P.Length; i++)
			{
				if (P[i] != null && P[i].prop_ApiAvatar_0 != null)
				{
					list.Add(P[i].prop_ApiAvatar_0.assetUrl);
				}
			}
			System.Collections.Generic.Dictionary<string, AssetBundleDownload> dictionary = new System.Collections.Generic.Dictionary<string, AssetBundleDownload>();
			Il2CppSystem.Collections.Generic.Dictionary<string, AssetBundleDownload>.KeyCollection.Enumerator enumerator = assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				dictionary.Add(current, assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0[current]);
			}
			foreach (string key in dictionary.Keys)
			{
				AssetBundleDownload assetBundleDownload = assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0[key];
				if (!assetBundleDownload.field_Private_String_0.Contains("wrld_") && !list.Contains(key))
				{
					if (assetBundleDownload.prop_GameObject_0 != null)
					{
						UnityEngine.Object.DestroyImmediate(assetBundleDownload.prop_GameObject_0, allowDestroyingAssets: true);
					}
					assetBundleDownload.field_Private_AssetBundle_0?.Unload(unloadAllLoadedObjects: true);
					assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0.Remove(key);
				}
			}
			dictionary.Clear();
			list.Clear();
			Resources.UnloadUnusedAssets();
			Il2CppSystem.GC.Collect(0, Il2CppSystem.GCCollectionMode.Forced, blocking: true, compacting: true);
			Il2CppSystem.GC.Collect(1, Il2CppSystem.GCCollectionMode.Forced, blocking: true, compacting: true);
			System.GC.Collect(0, System.GCCollectionMode.Forced, blocking: true, compacting: true);
			System.GC.Collect(1, System.GCCollectionMode.Forced, blocking: true, compacting: true);
			System.GC.WaitForPendingFinalizers();
		}

		internal static int GetRefreshRate()
		{
			if (screenRefreshRate == -1)
			{
				if (int.TryParse(MainUtils.GetLaunchParameter("--fps"), out var result))
				{
					targetedRefreshRate = result;
				}
				for (int i = 0; i < Screen.resolutions.Length; i++)
				{
					if (Screen.resolutions[i].refreshRate + 1 > targetedRefreshRate && Screen.resolutions[i].refreshRate + 1 > screenRefreshRate)
					{
						screenRefreshRate = Screen.resolutions[i].refreshRate + 1;
					}
				}
				if (screenRefreshRate == -1)
				{
					screenRefreshRate = targetedRefreshRate;
				}
			}
			return screenRefreshRate;
		}

		internal static bool IsStreamerModeEnabled()
		{
			return VRCInputManager.Method_Public_Static_Boolean_InputSetting_0(VRCInputManager.InputSetting.StreamerModeEnabled);
		}

		internal static void ChangeHideSelfState(bool state)
		{
			hideSelf = state;
			GeneralWrappers.GetAvatarPreviewBase().SetActive(!state);
			PlayerWrappers.GetCurrentPlayer().prop_VRCAvatarManager_0.gameObject.SetActive(!state);
			AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0.gameObject.SetActive(!state);
			if (!state)
			{
				PlayerUtils.ReloadAvatar(PlayerRankStatus.Local);
			}
		}

		internal static void RunGameCloseExploit(bool quest, APIUser user = null)
		{
			if (!gameCloserExploitRunning)
			{
				gameCloserExploitRunning = true;
				MelonCoroutines.Start(GameCloseExploitEnumerator(quest, user));
				GeneralWrappers.ClosePopup();
			}
		}

		private static IEnumerator GameCloseExploitEnumerator(bool quest, APIUser user)
		{
			ConsoleUtils.Info("Crasher", "Trying to crash " + ((user != null) ? user.displayName : "world"), System.ConsoleColor.Gray, "GameCloseExploitEnumerator", 502);
			string backupId = PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id;
			ChangeHideSelfState(state: true);
			System.Collections.Generic.List<PlayerInformation> playersToUnblockAfterExploit = new System.Collections.Generic.List<PlayerInformation>();
			if (user != null)
			{
				for (int i = 0; i < PlayerUtils.playerCachingList.Count; i++)
				{
					PlayerInformation playerInfo = PlayerUtils.playerCachingList.ElementAt(i).Value;
					if (!playerInfo.isLocalPlayer && playerInfo.apiUser != user)
					{
						try
						{
							PlayerUtils.ToggleBlockOnPlayer(playerInfo.apiUser);
							playersToUnblockAfterExploit.Add(playerInfo);
						}
						catch (System.Exception ex)
						{
							System.Exception e2 = ex;
							ConsoleUtils.Exception("Crasher", "Target Crash Block", e2, "GameCloseExploitEnumerator", 527);
						}
						yield return new WaitForSeconds(0.1f);
					}
				}
				yield return new WaitForSeconds(5f);
			}
			string crasherPC = (string.IsNullOrEmpty(Configuration.GetGeneralConfig().CrasherPC) ? MiscUtils.GetCrashingAvatarPC() : Configuration.GetGeneralConfig().CrasherPC);
			string crasherQuest = (string.IsNullOrEmpty(Configuration.GetGeneralConfig().CrasherQuest) ? MiscUtils.GetCrashingAvatarQuest() : Configuration.GetGeneralConfig().CrasherQuest);
			PlayerUtils.ChangePlayerAvatar(quest ? crasherQuest : crasherPC, logErrorOnHud: false);
			yield return new WaitForSeconds(15f);
			PlayerUtils.ChangePlayerAvatar(backupId, logErrorOnHud: false);
			yield return new WaitForSeconds(5f);
			for (int j = 0; j < playersToUnblockAfterExploit.Count; j++)
			{
				if (playersToUnblockAfterExploit[j] != null)
				{
					try
					{
						PlayerUtils.ToggleBlockOnPlayer(playersToUnblockAfterExploit[j].apiUser);
					}
					catch (System.Exception ex)
					{
						System.Exception e = ex;
						ConsoleUtils.Exception("Crasher", "Target Crash Unblock", e, "GameCloseExploitEnumerator", 562);
					}
				}
				yield return new WaitForSeconds(0.1f);
			}
			ChangeHideSelfState(state: false);
			gameCloserExploitRunning = false;
			ConsoleUtils.Info("Crasher", "Done crashing " + ((user != null) ? user.displayName : "world"), System.ConsoleColor.Gray, "GameCloseExploitEnumerator", 573);
		}

		internal static void RemoveAvatarFromCache(string avatarId)
		{
			AssetBundleDownloadManager assetBundleDownloadManager = AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0;
			for (int i = 0; i < assetBundleDownloadManager.field_Private_Queue_1_AssetBundleDownload_0.Count; i++)
			{
				AssetBundleDownload assetBundleDownload = assetBundleDownloadManager.field_Private_Queue_1_AssetBundleDownload_0.Dequeue();
				if (assetBundleDownload.field_Private_String_0 != avatarId)
				{
					assetBundleDownloadManager.field_Private_Queue_1_AssetBundleDownload_0.Enqueue(assetBundleDownload);
				}
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			Il2CppSystem.Collections.Generic.Dictionary<string, AssetBundleDownload>.KeyCollection.Enumerator enumerator = assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				list.Add(current);
			}
			foreach (string item in list)
			{
				if (assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0[item].field_Private_String_0 == avatarId)
				{
					UnityEngine.Object.DestroyImmediate(assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0[item].prop_GameObject_0, allowDestroyingAssets: true);
					assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0[item].prop_AssetBundle_0.Unload(unloadAllLoadedObjects: true);
					assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0.Remove(item);
				}
			}
			Resources.UnloadUnusedAssets();
			Il2CppSystem.GC.Collect(0, Il2CppSystem.GCCollectionMode.Forced, blocking: true, compacting: true);
			Il2CppSystem.GC.Collect(1, Il2CppSystem.GCCollectionMode.Forced, blocking: true, compacting: true);
			System.GC.Collect(0, System.GCCollectionMode.Forced, blocking: true, compacting: true);
			System.GC.Collect(1, System.GCCollectionMode.Forced, blocking: true, compacting: true);
			System.GC.WaitForPendingFinalizers();
		}

		internal static async void DownloadFileToPath(string url, string category, string name, string fileType, System.Action<bool> onFinished = null)
		{
			HttpClient client = new HttpClient
			{
				DefaultRequestHeaders = 
				{
					{ "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36" },
					{
						"X-Client-Version",
						Application.version
					}
				}
			};
			try
			{
				string downloadPath = Configuration.GetClientFolderPath() + category + "/";
				if (!Directory.Exists(downloadPath))
				{
					Directory.CreateDirectory(downloadPath);
				}
				HttpResponseMessage response = await client.GetAsync(url);
				using FileStream fileStream = new FileStream(downloadPath + name + "." + fileType, FileMode.CreateNew);
				await response.Content.CopyToAsync(fileStream);
				onFinished?.Invoke(obj: true);
			}
			catch (System.Exception ex)
			{
				System.Exception e = ex;
				ConsoleUtils.Exception("Utils", "DownloadFileToPath", e, "DownloadFileToPath", 643);
				onFinished?.Invoke(obj: false);
			}
		}

		internal static void RefreshVRCUiList(UiVRCList list)
		{
			if (!(list == null))
			{
				list.Method_Public_Void_0();
				list.Method_Public_Void_1();
				list.Method_Public_Void_PDM_0();
				list.Method_Public_Void_PDM_1();
			}
		}

		internal static string RandomString(FastRandom fastRandom, int length)
		{
			string text = string.Empty;
			for (int i = 0; i < length; i++)
			{
				text += "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPVWXYZ0123456789"[fastRandom.Next(0, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPVWXYZ0123456789".Length)];
			}
			return text;
		}
	}
}
