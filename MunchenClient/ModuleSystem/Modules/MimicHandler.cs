using ExitGames.Client.Photon;
using Il2CppSystem;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Photon.Realtime;
using UnhollowerBaseLib;
using VRC.SDKBase;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class MimicHandler : ModuleComponent
	{
		private bool blockEvent7FromSending = false;

		protected override string moduleName => "Mimic Handler";

		internal override bool OnEventReceived(ref EventData eventData)
		{
			if (eventData.Code == 1 && GeneralUtils.voiceImitation && eventData.Sender == GeneralUtils.voiceImitationPlayerKey)
			{
				byte[] unpackedData = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
				Event1Wrapper.SetActorID(ref unpackedData, PlayerWrappers.GetCurrentPlayer().prop_VRCPlayerApi_0.playerId);
				Event1Wrapper.SetServerTime(ref unpackedData, Networking.GetServerTimeInMilliseconds());
				PhotonUtils.OpRaiseEvent(1, unpackedData, new RaiseEventOptions
				{
					field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
					field_Public_EventCaching_0 = EventCaching.DoNotCache
				}, default(SendOptions));
			}
			if (eventData.Code == 7)
			{
				PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
				if (playerInformationByInstagatorID != null && GeneralUtils.inverseKinematicMimic && GeneralUtils.inverseKinematicMimicPlayerKey == eventData.Sender)
				{
					byte[] unpackedData2 = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
					if (Event7Wrapper.GetActorID(ref unpackedData2) == playerInformationByInstagatorID.actorIdData)
					{
						PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
						Event7Wrapper.SetActorID(ref unpackedData2, localPlayerInformation.actorIdData);
						Event7Wrapper.SetPosition(ref unpackedData2, localPlayerInformation.vrcPlayer.transform.position);
						Event7Wrapper.SetRotation(ref unpackedData2, localPlayerInformation.vrcPlayer.transform.rotation);
						Event7Wrapper.SetPing(ref unpackedData2, localPlayerInformation.GetPing());
						Event7Wrapper.SetFPS(ref unpackedData2, localPlayerInformation.GetFPSRaw());
						blockEvent7FromSending = false;
						PhotonUtils.OpRaiseEvent(7, unpackedData2, new RaiseEventOptions
						{
							field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
							field_Public_EventCaching_0 = EventCaching.DoNotCache
						}, default(SendOptions));
						blockEvent7FromSending = true;
					}
				}
			}
			return true;
		}

		internal override bool OnEventSent(byte eventCode, ref Object eventData, ref RaiseEventOptions raiseEventOptions)
		{
			if (eventCode == 7 && GeneralUtils.inverseKinematicMimic)
			{
				return !blockEvent7FromSending;
			}
			return true;
		}
	}
}
