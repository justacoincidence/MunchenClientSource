using System;
using ExitGames.Client.Photon;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnhollowerBaseLib;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class ModerationHandler : ModuleComponent
	{
		protected override string moduleName => "Moderation Handler";

		internal override bool OnEventReceived(ref EventData eventData)
		{
			if (eventData.Code == 33)
			{
				Dictionary<byte, Il2CppSystem.Object> dictionary = eventData.Parameters[eventData.CustomDataKey].Cast<Dictionary<byte, Il2CppSystem.Object>>();
				byte b = dictionary[0].Unbox<byte>();
				if (!System.Enum.IsDefined(typeof(ModerationCodes), b))
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, $"Unknown Moderation Event Found: {b}", System.ConsoleColor.Yellow, "OnEventReceived", 26);
				}
				switch (b)
				{
				case 2:
				{
					string text2 = new Il2CppSystem.String(dictionary[2].Pointer);
					if (text2.StartsWith("You have been warned") && Configuration.GetModerationsConfig().LogModerationsWarningsPrevent)
					{
						PlayerInformation instanceCreatorPlayerInformation2 = PlayerWrappers.GetInstanceCreatorPlayerInformation();
						string text3 = LanguageManager.GetUsedLanguage().ModerationWarnDetected.Replace("{username}", instanceCreatorPlayerInformation2.displayName);
						if (Configuration.GetModerationsConfig().LogModerationsWarningsLog)
						{
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text3, System.ConsoleColor.Gray, "OnEventReceived", 49);
						}
						if (Configuration.GetModerationsConfig().LogModerationsWarningsHUD)
						{
							GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text3);
						}
						return false;
					}
					break;
				}
				case 8:
					if (Configuration.GetModerationsConfig().LogModerationsMicOffPrevent)
					{
						PlayerInformation instanceCreatorPlayerInformation = PlayerWrappers.GetInstanceCreatorPlayerInformation();
						string text = LanguageManager.GetUsedLanguage().ModerationMicOffDetected.Replace("{username}", instanceCreatorPlayerInformation.displayName);
						if (Configuration.GetModerationsConfig().LogModerationsMicOffLog)
						{
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text, System.ConsoleColor.Gray, "OnEventReceived", 76);
						}
						if (Configuration.GetModerationsConfig().LogModerationsMicOffHUD)
						{
							GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text);
						}
						return false;
					}
					break;
				case 21:
					if (dictionary.Count == 4)
					{
						bool flag = dictionary[10].Unbox<bool>();
						bool isMuted = dictionary[11].Unbox<bool>();
						PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(dictionary[1].Unbox<int>());
						if (playerInformationByInstagatorID != null)
						{
							PlayerUtils.OnPlayerBlockStateChanged(playerInformationByInstagatorID, flag);
							PlayerUtils.OnPlayerMuteStateChanged(playerInformationByInstagatorID, isMuted);
							if (flag && Configuration.GetModerationsConfig().LogModerationsBlockPrevent)
							{
								return false;
							}
						}
					}
					else if (dictionary.Count == 3)
					{
						Il2CppStructArray<int> il2CppStructArray = dictionary[10].Cast<Il2CppStructArray<int>>();
						Il2CppStructArray<int> il2CppStructArray2 = dictionary[11].Cast<Il2CppStructArray<int>>();
						for (int i = 0; i < il2CppStructArray.Count; i++)
						{
							PlayerInformation playerInformationByInstagatorID2 = PlayerWrappers.GetPlayerInformationByInstagatorID(il2CppStructArray[i]);
							if (playerInformationByInstagatorID2 != null)
							{
								PlayerUtils.OnPlayerBlockStateChanged(playerInformationByInstagatorID2, isBlocked: true);
							}
						}
						for (int j = 0; j < il2CppStructArray2.Count; j++)
						{
							PlayerInformation playerInformationByInstagatorID3 = PlayerWrappers.GetPlayerInformationByInstagatorID(il2CppStructArray2[j]);
							if (playerInformationByInstagatorID3 != null)
							{
								PlayerUtils.OnPlayerMuteStateChanged(playerInformationByInstagatorID3, isMuted: true);
							}
						}
					}
					else
					{
						ConsoleUtils.Info("Photon", $"{b} Invalid mute/block received (Size: {dictionary.Count})", System.ConsoleColor.Gray, "OnEventReceived", 137);
					}
					break;
				}
			}
			return true;
		}
	}
}
