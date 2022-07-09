using System;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using UnityEngine.Video;
using VRC.SDK3.Video.Components;
using VRC.SDK3.Video.Components.AVPro;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;
using VRCSDK2;

namespace MunchenClient.Patching.Patches
{
	internal class VideoPlayerPatch : PatchComponent
	{
		protected override string patchName => "VideoPlayerPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VideoPlayerPatch));
			PatchMethod(typeof(VRC_SyncVideoPlayer).GetMethod("AddURL"), GetLocalPatch("OnVideoPlayerUrlQueuedPatch"), null);
			PatchMethod(typeof(VRC_SyncVideoPlayer).GetMethod("Play"), GetLocalPatch("OnVideoPlayerUrlPlayedPatch"), null);
			PatchMethod(typeof(VRC_SyncVideoPlayer).GetMethod("PlayIndex"), GetLocalPatch("OnVideoPlayerUrlPlayedIndexPatch"), null);
			PatchMethod(typeof(VRCUnityVideoPlayer).GetMethod("LoadURL"), GetLocalPatch("LoadUrlPatch"), null);
			PatchMethod(typeof(VRCUnityVideoPlayer).GetMethod("PlayURL"), GetLocalPatch("PlayUrlPatch"), null);
			PatchMethod(typeof(VRCAVProVideoPlayer).GetMethod("LoadURL"), GetLocalPatch("LoadUrlPatch"), null);
			PatchMethod(typeof(VRCAVProVideoPlayer).GetMethod("PlayURL"), GetLocalPatch("PlayUrlPatch"), null);
			PatchMethod(typeof(BaseVRCVideoPlayer).GetMethod("LoadURL"), GetLocalPatch("LoadUrlPatch"), null);
			PatchMethod(typeof(BaseVRCVideoPlayer).GetMethod("PlayURL"), GetLocalPatch("PlayUrlPatch"), null);
			PatchMethod(typeof(VideoPlayer).GetMethod("Play"), GetLocalPatch("VideoPlayerPlayPatch"), null);
		}

		private static bool IsURLWhitelisted(string url)
		{
			if (!Configuration.GetGeneralConfig().AntiIPLoggingVideoPlayerSafety)
			{
				return true;
			}
			string text = url.Trim();
			if (string.IsNullOrEmpty(text) || text.Length < 7)
			{
				return true;
			}
			int num = text.IndexOf('/');
			if (num != -1 && text[num + 1] == '/')
			{
				string text2 = text.Substring(num + 2);
				int num2 = text2.IndexOf('/');
				if (num2 != -1)
				{
					string text3 = (text2.StartsWith("www") ? text2.Substring(4, num2 - 4) : text2.Substring(0, num2));
					bool flag = MiscUtils.IsSubDomain(text3);
					for (int i = 0; i < MiscUtils.suitableVideoUrls.Count; i++)
					{
						bool flag2 = MiscUtils.IsSubDomain(MiscUtils.suitableVideoUrls[i]);
						if ((flag && flag2) || (!flag && !flag2))
						{
							if (text3 == MiscUtils.suitableVideoUrls[i])
							{
								return true;
							}
						}
						else if (flag && !flag2)
						{
							string text4 = text3.Substring(text3.IndexOf('.') + 1);
							if (text4 == MiscUtils.suitableVideoUrls[i])
							{
								return true;
							}
						}
					}
				}
			}
			ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, LanguageManager.GetUsedLanguage().ModerationVideoPlayerUnsuitable.Replace("{link}", url), ConsoleColor.Gray, "IsURLWhitelisted", 92);
			return false;
		}

		private static bool VideoPlayerPlayPatch(ref VideoPlayer __instance)
		{
			return IsURLWhitelisted(__instance.url);
		}

		private static bool LoadUrlPatch(VRCUrl __0)
		{
			return IsURLWhitelisted(__0.url);
		}

		private static bool PlayUrlPatch(VRCUrl __0)
		{
			return IsURLWhitelisted(__0.url);
		}

		private static bool OnVideoPlayerUrlQueuedPatch(string __0)
		{
			return IsURLWhitelisted(__0);
		}

		private static bool OnVideoPlayerUrlPlayedPatch(VRC_SyncVideoPlayer __instance)
		{
			return __instance.Videos.Length <= 0 || IsURLWhitelisted(__instance.Videos[0].URL);
		}

		private static bool OnVideoPlayerUrlPlayedIndexPatch(int __0, VRC_SyncVideoPlayer __instance)
		{
			return __instance.Videos.Length < __0 || IsURLWhitelisted(__instance.Videos[__0].URL);
		}
	}
}
