using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Photon.Pun;
using Photon.Realtime;
using UnhollowerBaseLib;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class ProtectionsHandler : ModuleComponent
	{
		private readonly System.Collections.Generic.Dictionary<int, int> lastEvent7Time = new System.Collections.Generic.Dictionary<int, int>();

		private bool previouslyUsedGhostJoin = false;

		protected override string moduleName => "Protections Handler";

		internal override void OnRoomLeft()
		{
			lastEvent7Time.Clear();
			AudioUtils.ClearLoggedVoicePackets();
		}

		internal override bool OnEventReceived(ref EventData eventData)
		{
			if (eventData.Code == 1 && IsEvent1Bad(ref eventData))
			{
				return false;
			}
			if (eventData.Code == 7 && IsEvent7Bad(ref eventData))
			{
				return false;
			}
			if (eventData.Code == 42 && ShouldBlockAvatarSwitch(ref eventData))
			{
				return false;
			}
			if (GeneralUtils.lockInstance && ShouldLockInstance(ref eventData))
			{
				return false;
			}
			if (ShouldBlockPhotonEvent(ref eventData))
			{
				return false;
			}
			return true;
		}

		internal override bool OnEventSent(byte eventCode, ref Il2CppSystem.Object eventData, ref RaiseEventOptions raiseEventOptions)
		{
			HandleGhostJoin(ref raiseEventOptions);
			if (eventCode == 7 && previouslyUsedGhostJoin)
			{
				return false;
			}
			return true;
		}

		internal void HandleGhostJoin(ref RaiseEventOptions raiseEventOptions)
		{
			if (GeneralUtils.ghostJoin)
			{
				raiseEventOptions.field_Public_ReceiverGroup_0 = ReceiverGroup.MasterClient;
				previouslyUsedGhostJoin = true;
			}
			else if (previouslyUsedGhostJoin)
			{
				raiseEventOptions.field_Public_ReceiverGroup_0 = ReceiverGroup.Others;
				previouslyUsedGhostJoin = false;
			}
		}

		internal bool IsEvent1Bad(ref EventData eventData)
		{
			if (Configuration.GetGeneralConfig().AntiEarrapeExploit)
			{
				byte[] voiceData = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
				if (AudioUtils.IsVoiceDataBad(eventData.Sender, voiceData))
				{
					PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
					if (!GeneralUtils.CheckNotification(playerInformationByInstagatorID.displayName, "earrapeExploit"))
					{
						string text = LanguageManager.GetUsedLanguage().ModerationEarrapeDetected.Replace("{username}", playerInformationByInstagatorID.displayName);
						GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text, logToConsole: true);
						GeneralUtils.ToggleNotification(playerInformationByInstagatorID.displayName, "earrapeExploit", state: true);
					}
					return true;
				}
			}
			return false;
		}

		internal bool IsEvent7Bad(ref EventData eventData)
		{
			if (!Configuration.GetGeneralConfig().AntiFreezeExploit)
			{
				return false;
			}
			byte[] unpackedData = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
			int actorID = Event7Wrapper.GetActorID(ref unpackedData);
			if (actorID <= PhotonNetwork.field_Public_Static_Int32_0)
			{
				return false;
			}
			int serverTime = Event7Wrapper.GetServerTime(ref unpackedData);
			if (lastEvent7Time.ContainsKey(actorID))
			{
				if (serverTime <= lastEvent7Time[actorID])
				{
					ConsoleUtils.Info("Anti-Freeze", $"Prevented Event 7 Exploit from Sender: {eventData.Sender} | {actorID} | New: {serverTime} | Old: {lastEvent7Time[actorID]}", System.ConsoleColor.Gray, "IsEvent7Bad", 129);
					PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
					if (playerInformationByInstagatorID != null)
					{
						playerInformationByInstagatorID.isClientUser = true;
					}
					lastEvent7Time[actorID] = serverTime;
					return true;
				}
				lastEvent7Time[actorID] = serverTime;
			}
			else
			{
				lastEvent7Time.Add(actorID, serverTime);
			}
			return false;
		}

		internal bool ShouldBlockAvatarSwitch(ref EventData eventData)
		{
			PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
			if (playerInformationByInstagatorID != null && playerInformationByInstagatorID.blockedLocalPlayer && Configuration.GetModerationsConfig().ModerationBlockPreventAvatarChange)
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, "Prevented blocked user: " + playerInformationByInstagatorID.displayName + " from switching avatar", System.ConsoleColor.Yellow, "ShouldBlockAvatarSwitch", 158);
				return true;
			}
			return false;
		}

		internal bool ShouldLockInstance(ref EventData eventData)
		{
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			if (localPlayerInformation.isInstanceMaster)
			{
				if (eventData.Code == byte.MaxValue)
				{
					Hashtable hashtable = eventData.Parameters[249].Cast<Hashtable>();
					Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> dictionary = hashtable["user"].Cast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
					string text = LanguageManager.GetUsedLanguage().ModerationPreventPlayerJoin.Replace("{username}", new Il2CppSystem.String(dictionary["displayName"].Pointer));
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().PhotonExploitsMenuName, text, System.ConsoleColor.Gray, "ShouldLockInstance", 178);
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().PhotonExploitsMenuName, text);
					return true;
				}
				if (eventData.Code == 202)
				{
					return true;
				}
			}
			return false;
		}

		internal bool ShouldBlockPhotonEvent(ref EventData eventData)
		{
			if (eventData.Code != 1 && eventData.Code != 3 && eventData.Code != 5 && eventData.Code != 7 && eventData.Code != 8 && eventData.Code != 9 && eventData.Code != 33 && eventData.Code != 40 && eventData.Code != 35 && eventData.Code != 34 && eventData.Code != 42 && eventData.Code != 60 && eventData.Code != 202 && eventData.Code != 208 && eventData.Code != 226 && eventData.Code != 253 && eventData.Code != 254 && eventData.Code != byte.MaxValue)
			{
				PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
				if (playerInformationByInstagatorID != null)
				{
					if (Configuration.GetGeneralConfig().BlockAllPhotonEvents && !playerInformationByInstagatorID.isLocalPlayer)
					{
						return true;
					}
					if (Configuration.GetGeneralConfig().BlockedPlayerEvents.ContainsKey(playerInformationByInstagatorID.id) && Configuration.GetGeneralConfig().BlockedPlayerEvents[playerInformationByInstagatorID.id].BlockedPhoton)
					{
						return true;
					}
				}
				else if (Configuration.GetGeneralConfig().BlockAllPhotonEvents)
				{
					return true;
				}
			}
			return false;
		}
	}
}
