using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using MunchenClient.Config;
using MunchenClient.Config.Modules;
using MunchenClient.Utils;
using Newtonsoft.Json;

namespace MunchenClient.Core
{
	internal class LanguageManager
	{
		private static readonly HttpClient requestHandler = new HttpClient();

		private static LanguageConfig currentLanguage;

		internal static void LoadLanguage()
		{
			if (!Configuration.GetGeneralConfig().LocalizedClient)
			{
				UseEnglishLanguage();
				return;
			}
			if (Configuration.GetGeneralConfig().LocalizedClientCustom)
			{
				ConsoleUtils.Info("Language", "Using custom langauge", ConsoleColor.Gray, "LoadLanguage", 27);
				if (!File.Exists(LanguageConfig.ConfigLocation))
				{
					UseEnglishLanguage();
					File.WriteAllText(LanguageConfig.ConfigLocation, JsonConvert.SerializeObject(currentLanguage, Formatting.Indented, Configuration.jsonSerializerSettings));
					ConsoleUtils.Info("Language", "Custom language has been created at: " + LanguageConfig.ConfigLocation, ConsoleColor.Gray, "LoadLanguage", 35);
					return;
				}
				currentLanguage = JsonConvert.DeserializeObject<LanguageConfig>(File.ReadAllText(LanguageConfig.ConfigLocation));
				if (currentLanguage.ConfigVersion != new LanguageConfig().ConfigVersion)
				{
					File.WriteAllText(LanguageConfig.ConfigLocation, JsonConvert.SerializeObject(currentLanguage, Formatting.Indented, Configuration.jsonSerializerSettings));
					ConsoleUtils.Info("Language", "Language config has been migrated and saved to new version", ConsoleColor.Gray, "LoadLanguage", 48);
				}
				return;
			}
			ConsoleUtils.Info("Language", "Downloading Localization for: " + CultureInfo.InstalledUICulture.EnglishName, ConsoleColor.Gray, "LoadLanguage", 54);
			HttpResponseMessage result;
			try
			{
				result = requestHandler.GetAsync("https://raw.githubusercontent.com/killerbigpoint/MunchenClientLanguages/main/Languages/" + CultureInfo.InstalledUICulture.Name + ".json").Result;
			}
			catch (HttpRequestException e)
			{
				UseEnglishLanguage();
				ConsoleUtils.Exception("Language", "Download", e, "LoadLanguage", 67);
				return;
			}
			catch (Exception e2)
			{
				UseEnglishLanguage();
				ConsoleUtils.Exception("Language", "Unknown", e2, "LoadLanguage", 75);
				return;
			}
			if (result.StatusCode != HttpStatusCode.OK)
			{
				UseEnglishLanguage();
				ConsoleUtils.Info("Language", "No language available - falling back to English", ConsoleColor.Gray, "LoadLanguage", 85);
			}
			else
			{
				currentLanguage = JsonConvert.DeserializeObject<LanguageConfig>(result.Content.ReadAsStringAsync().Result);
			}
		}

		internal static ref LanguageConfig GetUsedLanguage()
		{
			return ref currentLanguage;
		}

		internal static void UseEnglishLanguage()
		{
			currentLanguage = new LanguageConfig();
		}
	}
}
