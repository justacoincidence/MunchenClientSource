using System.Collections;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Misc;
using MunchenClient.Patching.Patches;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class PlayerEventsHandler : ModuleComponent
	{
		private static bool enableModerationAfterCountdown;

		private static bool moderationEventsEnabled;

		protected override string moduleName => "Player Events Handler";

		internal override void OnPlayerJoin(PlayerInformation player)
		{
			if (enableModerationAfterCountdown)
			{
				enableModerationAfterCountdown = false;
				MelonCoroutines.Start(DelayModerationEventsEnumerator());
			}
			if (player.isInstanceMaster)
			{
				if ((player.isLocalPlayer || player.IsFriends()) && Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeFriends)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerMasterJoined.Replace("{username}", player.displayName), Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeHUD);
				}
				else if (Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeOthers)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerMasterJoined.Replace("{username}", player.displayName), Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeHUD);
				}
			}
			if (Configuration.GetGeneralConfig().PlayerWallhack)
			{
				GeneralWrappers.ApplyPlayerWallhack(Configuration.GetGeneralConfig().PlayerWallhack, player);
			}
			if (!moderationEventsEnabled)
			{
				return;
			}
			if (player.IsFriends())
			{
				if (Configuration.GetModerationsConfig().LogModerationsJoinFriends)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerJoined.Replace("{username}", player.displayName), Configuration.GetModerationsConfig().LogModerationsJoinHUD);
					if (Configuration.GetModerationsConfig().ModerationSounds)
					{
						AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("UserConnect"));
					}
				}
			}
			else if (Configuration.GetModerationsConfig().LogModerationsJoinOthers)
			{
				GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerJoined.Replace("{username}", player.displayName), Configuration.GetModerationsConfig().LogModerationsJoinHUD);
				if (Configuration.GetModerationsConfig().ModerationSounds)
				{
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("UserConnect"));
				}
			}
		}

		internal override void OnPlayerLeft(PlayerInformation player)
		{
			if (!moderationEventsEnabled)
			{
				return;
			}
			if (player.IsFriends())
			{
				if (Configuration.GetModerationsConfig().LogModerationsLeftFriends)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerLeft.Replace("{username}", player.displayName), Configuration.GetModerationsConfig().LogModerationsLeftHUD);
					if (Configuration.GetModerationsConfig().ModerationSounds)
					{
						AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("UserDisconnect"));
					}
				}
			}
			else if (Configuration.GetModerationsConfig().LogModerationsLeftOthers)
			{
				GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerLeft.Replace("{username}", player.displayName), Configuration.GetModerationsConfig().LogModerationsLeftHUD);
				if (Configuration.GetModerationsConfig().ModerationSounds)
				{
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("UserDisconnect"));
				}
			}
		}

		internal override void OnRoomMasterChanged(PlayerInformation newMaster)
		{
			if (newMaster.IsFriends())
			{
				if (Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeFriends)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerMasterSwitched.Replace("{username}", newMaster.displayName), Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeHUD);
				}
			}
			else if (Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeOthers)
			{
				GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, LanguageManager.GetUsedLanguage().ModerationPlayerMasterSwitched.Replace("{username}", newMaster.displayName), Configuration.GetModerationsConfig().LogModerationsInstanceMasterChangeHUD);
			}
		}

		internal override void OnLevelWasInitialized(int level)
		{
			enableModerationAfterCountdown = true;
			moderationEventsEnabled = false;
			UserInterface.ClearNotificationHud();
		}

		private IEnumerator DelayModerationEventsEnumerator()
		{
			yield return new WaitForSeconds(1.5f);
			moderationEventsEnabled = true;
			NotificationPatch.firstTimeFriendRequestsReceived = true;
		}

		internal static bool IsModerationEventsEnabled()
		{
			return moderationEventsEnabled;
		}
	}
}
