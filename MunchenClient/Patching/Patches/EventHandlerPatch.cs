using System;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using Il2CppSystem;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Menu.Player;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnhollowerBaseLib;
using UnityEngine;
using VRC.Core;
using VRC.Networking;
using VRC.SDKBase;

namespace MunchenClient.Patching.Patches
{
	internal class EventHandlerPatch : PatchComponent
	{
		private static readonly Dictionary<int, Dictionary<int, float>> eventFilter = new Dictionary<int, Dictionary<int, float>>();

		private static readonly float eventFilterTimer = 30f;

		private static readonly Dictionary<int, float> lastSpawnEmojiTime = new Dictionary<int, float>();

		private static readonly List<string> whitelistedRpcs = new List<string>
		{
			"initUSpeakSenderRPC", "SendVoiceSetupToPlayerRPC", "ReceiveVoiceStatsSyncRPC", "InteractWithStationRPC", "_InstantiateObject", "_SendOnSpawn", "_DestroyObject", "SanityCheck", "ChangeVisibility", "Respawn",
			"ReapObject", "TakeOwnership", "SendStrokeRPC", "SpawnEmojiRPC", "PlayEmoteRPC", "TeleportRPC", "IncrementPortalPlayerCountRPC", "SyncWorldInstanceIdRPC", "SetTimerRPC", "CancelRPC",
			"ConfigurePortal", "UdonSyncRunProgramAsRPC", "PhotoCapture", "TimerBloop", "PlayEffect", "ReloadAvatarNetworkedRPC", "InternalApplyOverrideRPC", "InformOfBadConnection", "AddURL", "Play",
			"Pause", "Clear", "RemoteClear", "NetworkedQg", "NetworkedQuack", "NetworkedException", "NetworkedCowboy", "NetworkedAllah", "NetworkedGay"
		};

		protected override string patchName => "EventHandlerPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(EventHandlerPatch));
			PatchMethod(typeof(VRC_EventHandler).GetMethod("InternalTriggerEvent"), GetLocalPatch("OnEventDataSentPatch"), null);
			PatchMethod(typeof(FlatBufferNetworkSerializer).GetMethod("Method_Public_Void_EventData_0"), GetLocalPatch("FlatBufferNetworkSerializeReceivePatch"), null);
			MethodInfo[] methods = typeof(VRC_EventLog.EventReplicator).GetMethods(BindingFlags.Instance | BindingFlags.Public);
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.Name.StartsWith("Method_Public_Virtual_Final_New_Void_EventData_"))
				{
					PatchMethod(methodInfo, GetLocalPatch("OnEventDataReceivedPatch"), null);
				}
			}
		}

		internal static bool IsPlayerFiltered(int actorId, int eventID)
		{
			if (!eventFilter.ContainsKey(actorId))
			{
				return false;
			}
			if (!eventFilter[actorId].ContainsKey(eventID))
			{
				return false;
			}
			if (eventFilter[actorId][eventID] <= Time.realtimeSinceStartup)
			{
				return false;
			}
			return true;
		}

		internal static void FilterPlayer(int actorId, int eventID)
		{
			PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(actorId);
			string text = LanguageManager.GetUsedLanguage().ModerationEventLagDetected.Replace("{username}", (playerInformationByInstagatorID != null) ? playerInformationByInstagatorID.displayName : $"UserID: {actorId}").Replace("{event}", eventID.ToString());
			if (playerInformationByInstagatorID != null)
			{
				playerInformationByInstagatorID.isClientUser = true;
			}
			ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text, System.ConsoleColor.Red, "FilterPlayer", 126);
			GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ProtectionsMenuName, text);
			if (eventFilter.ContainsKey(actorId))
			{
				if (eventFilter[actorId].ContainsKey(eventID))
				{
					eventFilter[actorId][eventID] = Time.realtimeSinceStartup + eventFilterTimer;
				}
				else
				{
					eventFilter[actorId].Add(eventID, Time.realtimeSinceStartup + eventFilterTimer);
				}
			}
			else
			{
				eventFilter.Add(actorId, new Dictionary<int, float> { 
				{
					eventID,
					Time.realtimeSinceStartup + eventFilterTimer
				} });
			}
		}

		private static bool OnEventDataSentPatch(ref VRC_EventHandler.VrcBroadcastType __1)
		{
			if (Configuration.GetGeneralConfig().WorldTriggers && (__1 != 0 || __1 != VRC_EventHandler.VrcBroadcastType.AlwaysBufferOne || __1 != VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered))
			{
				__1 = VRC_EventHandler.VrcBroadcastType.Always;
			}
			return true;
		}

		private static bool FlatBufferNetworkSerializeReceivePatch(EventData __0)
		{
			if (__0.Code != 9)
			{
				return true;
			}
			if (Configuration.GetGeneralConfig().AntiFreezeExploit && !IsGoodSerializedData(__0))
			{
				return false;
			}
			return true;
		}

		private static bool IsGoodSerializedData(EventData eventData)
		{
			if (IsPlayerFiltered(eventData.Sender, eventData.Code))
			{
				return false;
			}
			Il2CppArrayBase<byte> il2CppArrayBase = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
			if (il2CppArrayBase.Length <= 10)
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			if (System.BitConverter.ToInt32(il2CppArrayBase, 4) == 0)
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			return true;
		}

		private static bool OnEventDataReceivedPatch(EventData __0)
		{
			if (__0.Code != 6)
			{
				return true;
			}
			if (Configuration.GetGeneralConfig().AntiFreezeExploit && !IsGoodRPC(__0))
			{
				return false;
			}
			return true;
		}

		private static bool IsGoodRPC(EventData eventData)
		{
			if (IsPlayerFiltered(eventData.Sender, eventData.Code))
			{
				return false;
			}
			Il2CppSystem.Object param_2;
			try
			{
				Il2CppStructArray<byte> param_ = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer).Cast<Il2CppStructArray<byte>>();
				BinarySerializer.Method_Public_Static_Boolean_ArrayOf_Byte_byref_Object_0(param_, out param_2);
			}
			catch (Il2CppException)
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			if (param_2 == null)
			{
				return false;
			}
			VRC_EventLog.EventLogEntry eventLogEntry = param_2.TryCast<VRC_EventLog.EventLogEntry>();
			if (eventLogEntry.field_Private_Int32_1 != eventData.Sender)
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			VRC_EventHandler.VrcEvent field_Private_VrcEvent_ = eventLogEntry.field_Private_VrcEvent_0;
			if (field_Private_VrcEvent_.EventType > VRC_EventHandler.VrcEventType.CallUdonMethod)
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
			if (playerInformationByInstagatorID != null && playerInformationByInstagatorID.isLocalPlayer)
			{
				return true;
			}
			if (field_Private_VrcEvent_.ParameterString == "SendVibrateTest")
			{
				LovenseMenu.PerformVibranceTest();
				return false;
			}
			if (!ApplicationBotHandler.IsBot() && Configuration.GetGeneralConfig().NetworkedEmotes && playerInformationByInstagatorID != null)
			{
				switch (field_Private_VrcEvent_.ParameterString)
				{
				case "NetworkedQg":
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("QgIsGay"), playerInformationByInstagatorID.vrcPlayer.transform.position);
					return false;
				case "NetworkedQuack":
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Quack"), playerInformationByInstagatorID.vrcPlayer.transform.position);
					return false;
				case "NetworkedException":
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Sizzukie"), playerInformationByInstagatorID.vrcPlayer.transform.position);
					return false;
				case "NetworkedCowboy":
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Niggers"), playerInformationByInstagatorID.vrcPlayer.transform.position);
					return false;
				case "NetworkedAllah":
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Allah"), playerInformationByInstagatorID.vrcPlayer.transform.position);
					return false;
				case "NetworkedGay":
					AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("GayEcho"), playerInformationByInstagatorID.vrcPlayer.transform.position);
					return false;
				}
			}
			if (field_Private_VrcEvent_.EventType != VRC_EventHandler.VrcEventType.SendRPC)
			{
				return true;
			}
			string text = field_Private_VrcEvent_.ParameterString.ToLower();
			if (field_Private_VrcEvent_.EventType == VRC_EventHandler.VrcEventType.AddHealth || text.Length > 70 || text.Contains("enablemeshrpc") || text.Contains("color") || text.Contains("love") || text.Contains("()"))
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			Il2CppReferenceArray<Il2CppSystem.Object> il2CppReferenceArray;
			try
			{
				il2CppReferenceArray = ParameterSerialization.Method_Public_Static_ArrayOf_Object_ArrayOf_Byte_0(field_Private_VrcEvent_.ParameterBytes);
			}
			catch (Il2CppException)
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			if (playerInformationByInstagatorID != null && ApplicationBotHandler.userToFollow != null && ApplicationBotHandler.userToFollow.id == playerInformationByInstagatorID.id)
			{
				Networking.RPC(RPC.Destination.All, PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.gameObject, field_Private_VrcEvent_.ParameterString, il2CppReferenceArray);
			}
			if (il2CppReferenceArray == null)
			{
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			string text2 = ((field_Private_VrcEvent_.ParameterObject != null) ? field_Private_VrcEvent_.ParameterObject.name : "Null");
			if (text2 != "USpeak" && text2 != "SceneEventHandlerAndInstantiator" && !text2.Contains("Portals/PortalInternalDynamic"))
			{
				if (Configuration.GetGeneralConfig().BlockAllRPCEvents)
				{
					return false;
				}
				if (playerInformationByInstagatorID != null && Configuration.GetGeneralConfig().BlockedPlayerEvents.ContainsKey(playerInformationByInstagatorID.id) && Configuration.GetGeneralConfig().BlockedPlayerEvents[playerInformationByInstagatorID.id].BlockedRPC)
				{
					return false;
				}
			}
			if (field_Private_VrcEvent_.ParameterString == "SpawnEmojiRPC")
			{
				if (playerInformationByInstagatorID == null || il2CppReferenceArray[0] == null)
				{
					FilterPlayer(eventData.Sender, eventData.Code);
					return false;
				}
				int num = il2CppReferenceArray[0].Unbox<int>();
				if (num < 0 || num > 56)
				{
					FilterPlayer(eventData.Sender, eventData.Code);
					return false;
				}
				if (lastSpawnEmojiTime.ContainsKey(eventData.Sender))
				{
					float num2 = Time.realtimeSinceStartup - lastSpawnEmojiTime[eventData.Sender];
					lastSpawnEmojiTime[eventData.Sender] = Time.realtimeSinceStartup;
					if (num2 < 1f)
					{
						FilterPlayer(eventData.Sender, eventData.Code);
						return false;
					}
				}
				else
				{
					lastSpawnEmojiTime.Add(eventData.Sender, Time.realtimeSinceStartup);
				}
			}
			if (!whitelistedRpcs.Contains(field_Private_VrcEvent_.ParameterString))
			{
				ConsoleUtils.Info("Event", $"{eventData.Sender} | {field_Private_VrcEvent_.ParameterString}", System.ConsoleColor.Gray, "IsGoodRPC", 425);
				FilterPlayer(eventData.Sender, eventData.Code);
				return false;
			}
			return true;
		}
	}
}
