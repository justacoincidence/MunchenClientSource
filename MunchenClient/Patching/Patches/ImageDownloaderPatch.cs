using System;
using System.Collections.Concurrent;
using Il2CppSystem;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal class ImageDownloaderPatch : PatchComponent
	{
		internal static readonly ConcurrentQueue<ImageDownloadContainer> imageDownloadQueue = new ConcurrentQueue<ImageDownloadContainer>();

		internal static bool imageDownloadCallOriginal = false;

		protected override string patchName => "ImageDownloaderPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(ImageDownloaderPatch));
			PatchMethod(typeof(ImageDownloader).GetMethod("DownloadImageInternal"), GetLocalPatch("OnImageDownloadPatch"), null);
		}

		private static bool OnImageDownloadPatch(string __0, int __1, Il2CppSystem.Action<Texture2D> __2, Il2CppSystem.Action __3, string __4, bool __5)
		{
			if (Configuration.GetGeneralConfig().PerformanceImageCache && !imageDownloadCallOriginal)
			{
				Texture2D cachedImage = CacheUtils.GetCachedImage(__0);
				if (cachedImage != null)
				{
					__2.Invoke(cachedImage);
					return false;
				}
				imageDownloadQueue.Enqueue(new ImageDownloadContainer
				{
					imageUrl = __0,
					imageSize = __1,
					onImageDownload = __2,
					onImageDownloadFailed = __3,
					fallbackImageUrl = __4,
					isRetry = __5
				});
				return false;
			}
			if (Configuration.GetGeneralConfig().AntiIPLoggingImageThumbnailSafety && !string.IsNullOrEmpty(__0))
			{
				for (int i = 0; i < MiscUtils.suitableImageThumbnailUrls.Count; i++)
				{
					if (__0.StartsWith(MiscUtils.suitableImageThumbnailUrls[i]))
					{
						return true;
					}
				}
				string text = LanguageManager.GetUsedLanguage().ModerationImageThumbnailUnsuitable.Replace("{link}", __0);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text, System.ConsoleColor.Gray, "OnImageDownloadPatch", 73);
				GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text);
				return false;
			}
			return true;
		}

		internal static void DownloadImage(string imageUrl, int imageSize, Il2CppSystem.Action<Texture2D> onImageDownload, Il2CppSystem.Action onImageDownloadFailed, string fallbackImageUrl = "", bool isRetry = false)
		{
			imageDownloadCallOriginal = true;
			ImageDownloader.Instance.DownloadImageInternal(imageUrl, imageSize, onImageDownload, onImageDownloadFailed, fallbackImageUrl, isRetry);
			imageDownloadCallOriginal = false;
		}
	}
}
