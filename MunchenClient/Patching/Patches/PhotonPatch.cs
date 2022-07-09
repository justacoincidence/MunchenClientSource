using System.Linq;
using System.Reflection;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using Il2CppSystem;
using MunchenClient.Config;
using MunchenClient.ModuleSystem;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal class PhotonPatch : PatchComponent
	{
		private static bool lastPositionRevealed;

		protected override string patchName => "PhotonPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(PhotonPatch));
			PatchMethod(typeof(PhotonPeer).GetProperty("RoundTripTime").GetGetMethod(), GetLocalPatch("GetRoundTripTimePatch"), null);
			PatchMethod(typeof(PhotonPeer).GetMethods().First((MethodInfo mb) => mb.Name.StartsWith("SendOperation") && mb.GetParameters()[1].ParameterType == typeof(ParameterDictionary)), GetLocalPatch("SendOperationPatch"), null);
			PatchMethod(typeof(LoadBalancingClient).GetMethod("Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0"), GetLocalPatch("PhotonRaiseEventPatch"), null);
		}

		private static bool SendOperationPatch(ParameterDictionary __1)
		{
			StructWrapper<byte> structWrapper = __1[244].Cast<StructWrapper<byte>>();
			if (structWrapper.Unwrap() == 7)
			{
				if (GeneralUtils.serialization)
				{
					if (!Configuration.GetGeneralConfig().OnKeySerialization)
					{
						if (!lastPositionRevealed)
						{
							lastPositionRevealed = true;
							PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
						}
						return false;
					}
					if (GeneralWrappers.IsInVR())
					{
						if (Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.5f)
						{
							if (!lastPositionRevealed)
							{
								lastPositionRevealed = true;
								PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
							}
							return false;
						}
					}
					else if (Input.GetKey(KeyCode.LeftControl))
					{
						if (!lastPositionRevealed)
						{
							lastPositionRevealed = true;
							PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
						}
						return false;
					}
				}
				if (GeneralUtils.fakelag)
				{
					GeneralUtils.fakelagTimer++;
					if (GeneralUtils.fakelagTimer < (GeneralWrappers.IsInVR() ? 20 : 10))
					{
						if (!lastPositionRevealed)
						{
							lastPositionRevealed = true;
							PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
						}
						return false;
					}
					GeneralUtils.fakelagTimer = 0;
				}
				if (lastPositionRevealed)
				{
					PlayerUtils.ClearClone(PlayerWrappers.GetLocalPlayerInformation());
				}
				lastPositionRevealed = false;
			}
			return true;
		}

		private static bool GetRoundTripTimePatch(ref int __result)
		{
			if (!Configuration.GetGeneralConfig().SpooferPing)
			{
				return true;
			}
			int num = ((Configuration.GetGeneralConfig().SpooferPingCustom != -1) ? Configuration.GetGeneralConfig().SpooferPingCustom : 1337);
			if (Configuration.GetGeneralConfig().SpooferRealisticMode)
			{
				Configuration.GetGeneralConfig().SpooferPing = false;
				num += PhotonNetwork.field_Public_Static_LoadBalancingClient_0.prop_LoadBalancingPeer_0.RoundTripTime / 3;
				Configuration.GetGeneralConfig().SpooferPing = true;
			}
			__result = num;
			return false;
		}

		private static bool PhotonRaiseEventPatch(byte __0, ref Il2CppSystem.Object __1, ref RaiseEventOptions __2)
		{
			return ModuleManager.OnEventSent(__0, ref __1, ref __2);
		}
	}
}
