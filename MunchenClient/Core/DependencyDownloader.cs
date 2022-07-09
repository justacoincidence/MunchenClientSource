using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using MunchenClient.Config;
using MunchenClient.Utils;

namespace MunchenClient.Core
{
	internal class DependencyDownloader
	{
		private static readonly Dictionary<string, DependencyDownload> onDependencyFinished = new Dictionary<string, DependencyDownload>();

		internal static void DownloadDependency(string name, Action onFinished)
		{
			onDependencyFinished.Add(HttpClientWrapper.SendDownloadRequest("https://shintostudios.net/download/dependencies/" + name + ".zip", 0f, OnDownloadFinished), new DependencyDownload
			{
				name = name,
				onFinished = onFinished
			});
		}

		private static void OnDownloadFinished(string token, bool success, byte[] download)
		{
			string text = Configuration.GetClientFolderPath() + "Dependencies/" + onDependencyFinished[token].name + "/";
			string text2 = text + token + ".zip";
			if (Directory.Exists(text))
			{
				string[] files = Directory.GetFiles(text);
				foreach (string path in files)
				{
					File.Delete(path);
				}
				Directory.Delete(text, recursive: true);
			}
			Directory.CreateDirectory(text);
			File.WriteAllBytes(text2, download);
			ZipFile.ExtractToDirectory(text2, text);
			File.Delete(text2);
			onDependencyFinished[token].onFinished();
			onDependencyFinished.Remove(token);
		}
	}
}
