using System;
using System.Collections.Concurrent;
using System.IO;
using MunchenClient.Config;
using UnityEngine;

namespace MunchenClient.Utils
{
	internal class CacheUtils
	{
		private const string DATA_FILE_NAME = "__data";

		private const string INFO_FILE_NAME = "__info";

		private static readonly ConcurrentDictionary<string, Texture2D> cachedImages = new ConcurrentDictionary<string, Texture2D>();

		private static readonly string imageCachePath = Configuration.GetClientFolderPath() + "Image Cache/";

		private static string TrimFileID(string id)
		{
			string result;
			try
			{
				string[] array = id.Split('_');
				if (array.Length == 1)
				{
					int num = array[0].LastIndexOf('/');
					if (num == -1)
					{
						result = "file_" + array[0];
					}
					else
					{
						string text = array[0].Substring(num + 1);
						int length = text.LastIndexOf('.');
						result = "file_" + text.Substring(0, length);
					}
				}
				else
				{
					int num2 = array[1].IndexOf('/');
					result = ((num2 != -1) ? ("file_" + array[1].Substring(0, num2)) : ("file_" + array[1]));
				}
			}
			catch (Exception e)
			{
				result = id;
				ConsoleUtils.Exception("ImageCache", "TrimFileID", e, "TrimFileID", 62);
			}
			return result;
		}

		internal static Texture2D GetCachedImage(string imageName)
		{
			string text = TrimFileID(imageName);
			if (cachedImages.ContainsKey(text))
			{
				return cachedImages[text];
			}
			string path = imageCachePath + text + ".png";
			if (!File.Exists(path))
			{
				return null;
			}
			byte[] array = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2);
			if (!ImageConversion.LoadImage(texture2D, array))
			{
				return null;
			}
			cachedImages.TryAdd(text, texture2D);
			return texture2D;
		}

		internal static void AddCachedImage(string imageName, Texture2D image)
		{
			string text = TrimFileID(imageName);
			if (!cachedImages.ContainsKey(text))
			{
				cachedImages.TryAdd(text, image);
				if (!Directory.Exists(imageCachePath))
				{
					Directory.CreateDirectory(imageCachePath);
				}
				string path = imageCachePath + text + ".png";
				if (!File.Exists(path) && !GeneralUtils.SaveTextureToDisk(image, imageCachePath, text))
				{
					ConsoleUtils.Info("ImageCache", "Failed to save cached image (ID: " + text + ")", ConsoleColor.Gray, "AddCachedImage", 123);
				}
			}
		}

		private static string GetRawCachePathBase(string fileKey, Hash128 fileHash)
		{
			return Path.Combine(Path.GetFullPath(AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.field_Private_Cache_0.path), fileKey, fileHash.ToString());
		}

		internal static string GetRawCacheDataPath(string fileKey, Hash128 fileHash)
		{
			return Path.Combine(GetRawCachePathBase(fileKey, fileHash), "__data");
		}

		internal static void CreateCacheInfoFile(string fileKey, Hash128 fileHash)
		{
			string path = Path.Combine(GetRawCachePathBase(fileKey, fileHash), "__info");
			File.WriteAllText(path, string.Format("-1\n{0}\n1\n{1}\n", ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(), "__data"));
		}
	}
}
