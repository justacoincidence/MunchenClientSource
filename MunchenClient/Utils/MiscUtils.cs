using System;
using System.Collections.Generic;
using MunchenClient.Core.Compatibility;
using UnityEngine;
using VRC.Core;

namespace MunchenClient.Utils
{
	internal class MiscUtils
	{
		private static string crashingAvatarPC = string.Empty;

		private static string crashingAvatarQuest = string.Empty;

		internal static readonly ApiAvatar robotAvatar = new ApiAvatar
		{
			id = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11",
			name = "Robot",
			releaseStatus = "public",
			assetUrl = "https://api.vrchat.cloud/api/1/file/file_3c521ce5-e662-4a5d-a2f1-d9088cfde086/1/file",
			version = 1,
			authorName = "vrchat",
			authorId = "8JoV9XEdpo",
			description = "Beep Boop",
			thumbnailImageUrl = "https://api.vrchat.cloud/api/1/file/file_0e8c4e32-7444-44ea-ade4-313c010d4bae/1/file",
			assetVersion = new AssetVersion
			{
				UnityVersion = "2017.4.15f1",
				ApiVersion = 1
			},
			platform = "standalonewindows"
		};

		private static string clientDiscordLink = string.Empty;

		internal static readonly List<string> blacklistedAuthorIds = new List<string>();

		internal static readonly List<string> blacklistedAvatarIds = new List<string>();

		internal static readonly List<string> suitableImageThumbnailUrls = new List<string> { "https://vrchat.com/", "https://assets.vrchat.com/", "https://api.vrchat.cloud/", "https://files.vrchat.cloud/", "https://s3-us-west-2.amazonaws.com/vrc-uploads/", "https://s3-us-west-2.amazonaws.com/vrc-managed-files/", "https://s3.amazonaws.com/files.vrchat.cloud/", "https://d348imysud55la.cloudfront.net/", "https://imgur.com/", "https://vt.miinc.ru/" };

		internal static readonly List<string> suitableVideoUrls = new List<string>
		{
			"youtube.com", "youtu.be", "twitch.tv", "dropbox.com", "cdn.discordapp.com", "soundcloud.com", "player.vimeo.com", "shintostudios.net", "rootworld.xyz", "vrcm.nl",
			"lolisociety.com", "sec1.gencily.xyz", "weba.scrittonic.xyz", "t-ne.x0.to", "stream.vrcdn.live", "nepu.io", "dai.ly", "dailymotion.com", "googlevideo.com"
		};

		internal static List<string> motherboardList = new List<string>
		{
			"B650 AORUS PRO (Gigabyte Technology Co., Ltd.)", "B550 AORUS PRO (Gigabyte Technology Co., Ltd.)", "B450 AORUS PRO (Gigabyte Technology Co., Ltd.)", "B350 AORUS PRO (Gigabyte Technology Co., Ltd.)", "B250 AORUS PRO (Gigabyte Technology Co., Ltd.)", "B150 AORUS PRO (Gigabyte Technology Co., Ltd.)", "Gigabyte B650M DS3H", "Gigabyte B550M DS3H", "Gigabyte B450M DS3H", "Gigabyte B350M DS3H",
			"Gigabyte B250M DS3H", "Gigabyte B150M DS3H", "Asus AM4 TUF Gaming X670-Plus", "Asus AM4 TUF Gaming X570-Plus", "Asus AM4 TUF Gaming X470-Plus", "Asus AM4 TUF Gaming X370-Plus", "Asus AM4 TUF Gaming X270-Plus", "Asus AM4 TUF Gaming X170-Plus", "ASRock Z670 Taichi", "ASRock Z570 Taichi",
			"ASRock Z470 Taichi", "ASRock Z370 Taichi", "ASRock Z270 Taichi", "ASRock Z170 Taichi"
		};

		internal static List<string> graphicsCardVersionList = new List<string>
		{
			"Vulkan 1.3.2 [0x7fd040000]", "Vulkan 1.3.0 [0x7fd040000]", "Vulkan 1.2.0 [0x7fd040000]", "Vulkan 1.1.0 [0x7fd040000]", "Vulkan 1.0.0 [0x7fd040000]", "Direct3D 12.0 [level 12.2]", "Direct3D 12.0 [level 12.1]", "Direct3D 12.0 [level 12.0]", "Direct3D 11.0 [level 11.4]", "Direct3D 11.0 [level 11.3]",
			"Direct3D 11.0 [level 11.2]", "Direct3D 11.0 [level 11.1]", "Direct3D 11.0 [level 11.0]", "Direct3D 10.0 [level 10.0]", "Direct3D 10.0 [level 10.1]", "Direct3D 9.0 [level 9.0]", "Direct3D 9.0 [level 9.0a]", "Direct3D 9.0 [level 9.0b]", "Direct3D 9.0 [level 9.0c]"
		};

		internal static List<string> graphicsCardList = new List<string>
		{
			"NVIDIA Quadro RTX 8000", "NVIDIA Quadro RTX 7000", "NVIDIA Quadro RTX 6000", "NVIDIA Quadro RTX 5000", "NVIDIA Quadro RTX 4000", "NVIDIA Quadro P3000", "NVIDIA Quadro P2000", "NVIDIA Quadro P1000", "NVIDIA GTX Titan RTX", "NVIDIA GTX Titan V",
			"NVIDIA GTX Titan Xp", "NVIDIA GTX Titan X (2016)", "NVIDIA GTX Titan X", "NVIDIA GTX Titan Z", "NVIDIA GTX Titan Black", "NVIDIA GTX Titan", "NVIDIA GeForce RTX 3090", "NVIDIA GeForce RTX 3080 Ti", "NVIDIA GeForce RTX 3080", "NVIDIA GeForce RTX 3080M",
			"NVIDIA GeForce RTX 3070 Ti", "NVIDIA GeForce RTX 3070", "NVIDIA GeForce RTX 3070M", "NVIDIA GeForce RTX 3060", "NVIDIA GeForce RTX 3060M", "NVIDIA GeForce RTX 3050 Ti", "NVIDIA GeForce RTX 3050", "NVIDIA GeForce RTX 3050M", "NVIDIA GeForce RTX 2080 Ti", "NVIDIA GeForce RTX 2080",
			"NVIDIA GeForce RTX 2080M", "NVIDIA GeForce RTX 2070 Ti", "NVIDIA GeForce RTX 2070", "NVIDIA GeForce RTX 2070M", "NVIDIA GeForce RTX 2060", "NVIDIA GeForce RTX 2060M", "NVIDIA GeForce RTX 2050 Ti", "NVIDIA GeForce RTX 2050", "NVIDIA GeForce RTX 2050M", "NVIDIA GeForce GTX 1080 Ti",
			"NVIDIA GeForce GTX 1080", "NVIDIA GeForce GTX 1080M", "NVIDIA GeForce GTX 1070 Ti", "NVIDIA GeForce GTX 1070", "NVIDIA GeForce GTX 1070M", "NVIDIA GeForce GTX 1060 4GB", "NVIDIA GeForce GTX 1060 2GB", "NVIDIA GeForce GTX 1060M", "NVIDIA GeForce GTX 1050 Ti", "NVIDIA GeForce GTX 1050",
			"NVIDIA GeForce GTX 1050M", "NVIDIA GeForce GTX 980 Ti", "NVIDIA GeForce GTX 980", "NVIDIA GeForce GTX 980M", "NVIDIA GeForce GTX 970", "NVIDIA GeForce GTX 970M", "NVIDIA GeForce GTX 960", "NVIDIA GeForce GTX 960M", "NVIDIA GeForce GTX 950 Ti", "NVIDIA GeForce GTX 950",
			"NVIDIA GeForce GTX 780 Ti", "NVIDIA GeForce GTX 780", "NVIDIA GeForce GTX 780M", "NVIDIA GeForce GTX 770", "NVIDIA GeForce GTX 770M", "NVIDIA GeForce GTX 760", "NVIDIA GeForce GTX 760M", "NVIDIA GeForce GTX 750 Ti", "NVIDIA GeForce GTX 750"
		};

		internal static List<string> cpuList = new List<string>
		{
			"AMD Ryzen 9 5950X 16-Core Processor", "AMD Ryzen 9 5900X 12-Core Processor", "AMD Ryzen 9 5900 12-Core Processor", "AMD Ryzen 7 5800X3D 8-Core Processor", "AMD Ryzen 7 5800X 8-Core Processor", "AMD Ryzen 7 5800 8-Core Processor", "AMD Ryzen 5 5600X 6-Core Processor", "AMD Ryzen 5 5600 6-Core Processor", "AMD Ryzen 9 3950X 16-Core Processor", "AMD Ryzen 9 3900X 12-Core Processor",
			"AMD Ryzen 9 3900 12-Core Processor", "AMD Ryzen 7 3800X 8-Core Processor", "AMD Ryzen 7 3800 8-Core Processor", "AMD Ryzen 5 3600X 6-Core Processor", "AMD Ryzen 5 3600 6-Core Processor", "AMD Ryzen 9 2900X 12-Core Processor", "AMD Ryzen 9 2900 12-Core Processor", "AMD Ryzen 7 2800X 8-Core Processor", "AMD Ryzen 7 2800 8-Core Processor", "AMD Ryzen 5 2600X 6-Core Processor",
			"AMD Ryzen 5 2600 6-Core Processor", "AMD Ryzen 9 1900X 12-Core Processor", "AMD Ryzen 9 1900 12-Core Processor", "AMD Ryzen 7 1800X 8-Core Processor", "AMD Ryzen 7 1800 8-Core Processor", "AMD Ryzen 5 1600X 6-Core Processor", "AMD Ryzen 5 1600 6-Core Processor", "Intel(R) Core(TM) i9-12900K CPU @ 3.20GHZ", "Intel(R) Core(TM) i9-12900 CPU @ 2.50GHZ", "Intel(R) Core(TM) i9-11900K CPU @ 3.50GHZ",
			"Intel(R) Core(TM) i9-11900 CPU @ 2.50GHZ", "Intel(R) Core(TM) i9-10900K CPU @ 3.70GHZ", "Intel(R) Core(TM) i9-10900 CPU @ 2.80GHZ", "Intel(R) Core(TM) i9-9900K CPU @ 3.60GHZ", "Intel(R) Core(TM) i9-9900 CPU @ 3.10GHZ", "Intel(R) Core(TM) i7-12700K CPU @ 3.60GHZ", "Intel(R) Core(TM) i7-12700K CPU @ 2.50GHZ", "Intel(R) Core(TM) i7-11700K CPU @ 3.60GHZ", "Intel(R) Core(TM) i7-11700 CPU @ 2.50GHZ", "Intel(R) Core(TM) i7-10700K CPU @ 3.80GHZ",
			"Intel(R) Core(TM) i7-10700 CPU @ 2.90GHZ", "Intel(R) Core(TM) i7-9700K CPU @ 3.60GHZ", "Intel(R) Core(TM) i7-9700 CPU @ 3.00GHZ", "Intel(R) Core(TM) i7-8700K CPU @ 3.70GHZ", "Intel(R) Core(TM) i7-8700 CPU @ 3.20GHZ", "Intel(R) Core(TM) i7-7700K CPU @ 4.20GHZ", "Intel(R) Core(TM) i7-7700 CPU @ 3.60GHZ", "Intel(R) Core(TM) i7-6700K CPU @ 4.00GHZ", "Intel(R) Core(TM) i7-6700 CPU @ 3.40GHZ", "Intel(R) Core(TM) i7-5700HQ CPU @ 2.70GHZ",
			"Intel(R) Core(TM) i7-4790K CPU @ 4.00GHZ", "Intel(R) Core(TM) i7-4790 CPU @ 3.60GHZ", "Intel(R) Core(TM) i7-4770K CPU @ 3.50GHZ", "Intel(R) Core(TM) i7-4770 CPU @ 3.40GHZ", "Intel(R) Core(TM) i7-3770K CPU @ 3.50GHZ", "Intel(R) Core(TM) i7-3770 CPU @ 3.40GHZ", "Intel(R) Core(TM) i7-2700K CPU @ 3.50GHZ"
		};

		internal static List<string> operatingSystemList = new List<string>
		{
			"Windows 11  (10.0.22449) 64bit", "Windows 11  (10.0.22449) 32bit", "Windows 11  (10.0.22000) 64bit", "Windows 11  (10.0.22000) 32bit", "Windows 11  (10.0.0) 64bit", "Windows 11  (10.0.0) 32bit", "Windows 10  (10.0.19044) 64bit", "Windows 10  (10.0.19044) 32bit", "Windows 10  (10.0.19042) 64bit", "Windows 10  (10.0.19042) 32bit",
			"Windows 10  (10.0.19041) 64bit", "Windows 10  (10.0.19041) 32bit", "Windows 10  (10.0.18363) 64bit", "Windows 10  (10.0.18363) 32bit", "Windows 10  (10.0.18362) 64bit", "Windows 10  (10.0.18362) 32bit", "Windows 10  (10.0.17763) 64bit", "Windows 10  (10.0.17763) 32bit", "Windows 10  (10.0.17134) 64bit", "Windows 10  (10.0.17134) 32bit",
			"Windows 10  (10.0.16299) 64bit", "Windows 10  (10.0.16299) 32bit", "Windows 10  (10.0.15063) 64bit", "Windows 10  (10.0.15063) 32bit", "Windows 10  (10.0.14393) 64bit", "Windows 10  (10.0.14393) 32bit", "Windows 10  (10.0.10586) 64bit", "Windows 10  (10.0.10586) 32bit", "Windows 10  (10.0.0) 64bit", "Windows 10  (10.0.0) 32bit",
			"Windows 8.1  (10.0.0) 64bit", "Windows 8.1  (10.0.0) 32bit", "Windows 8  (10.0.0) 64bit", "Windows 8  (10.0.0) 32bit", "Windows 7  (10.0.0) 64bit", "Windows 7  (10.0.0) 32bit"
		};

		internal static void SetCrashingAvatarPC(string id)
		{
			crashingAvatarPC = id;
		}

		internal static void SetCrashingAvatarQuest(string id)
		{
			crashingAvatarQuest = id;
		}

		internal static void SetClientDiscordLink(string link)
		{
			clientDiscordLink = link;
		}

		internal static string GetCrashingAvatarPC()
		{
			return crashingAvatarPC;
		}

		internal static string GetCrashingAvatarQuest()
		{
			return crashingAvatarQuest;
		}

		internal static string GetClientDiscordLink()
		{
			return clientDiscordLink;
		}

		internal static List<T> FindAllComponentsInGameObject<T>(GameObject gameObject, bool includeInactive = true, bool searchParent = true, bool searchChildren = true) where T : class
		{
			List<T> list = new List<T>();
			if (gameObject == null)
			{
				return list;
			}
			try
			{
				foreach (T component in gameObject.GetComponents<T>())
				{
					list.Add(component);
				}
				if (searchParent && gameObject.transform.parent != null)
				{
					foreach (T item in gameObject.GetComponentsInParent<T>(includeInactive))
					{
						list.Add(item);
					}
				}
				if (searchChildren && gameObject.transform.childCount > 0)
				{
					foreach (T componentsInChild in gameObject.GetComponentsInChildren<T>(includeInactive))
					{
						list.Add(componentsInChild);
					}
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("Misc", "FindAllComponentsInGameObject", e, "FindAllComponentsInGameObject", 388);
			}
			return list;
		}

		internal static Vector3 GetNameplateOffset(bool open)
		{
			if (CompatibilityLayer.IsNotoriousInstalled())
			{
				return open ? new Vector3(0f, -85f, 0f) : new Vector3(0f, -58f, 0f);
			}
			return open ? new Vector3(0f, 60f, 0f) : new Vector3(0f, 30f, 0f);
		}

		internal static int GetChildDepth(Transform child, Transform parent)
		{
			int num = 0;
			if (child == parent)
			{
				return num;
			}
			while (child.parent != null)
			{
				num++;
				if (child.parent == parent)
				{
					return num;
				}
				child = child.parent;
			}
			return -1;
		}

		internal static string RemoveCharacterFromString(string text, char character)
		{
			char[] array = new char[text.Length];
			int num = 0;
			foreach (char c in text)
			{
				if (c != character)
				{
					array[num] = c;
					num++;
				}
			}
			return new string(array, 0, num);
		}

		internal static bool IsSubDomain(string url)
		{
			int num = 0;
			for (int i = 0; i < url.Length; i++)
			{
				if (url[i] == '.')
				{
					num++;
					if (num == 2)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
