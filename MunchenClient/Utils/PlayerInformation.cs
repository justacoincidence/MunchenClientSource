using TMPro;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace MunchenClient.Utils
{
	internal class PlayerInformation
	{
		internal int actorId;

		internal int actorIdData;

		internal int actorIdDataOther;

		internal string id;

		internal string displayName;

		internal bool isLocalPlayer;

		internal bool isInstanceMaster;

		internal bool isVRChatStaff;

		internal bool isVRUser;

		internal bool isQuestUser;

		internal bool isClientUser;

		internal bool blockedLocalPlayer;

		internal PlayerRankStatus rankStatus;

		internal Player player;

		internal VRCPlayerApi playerApi;

		internal VRCPlayer vrcPlayer;

		internal APIUser apiUser;

		internal VRCNetworkBehaviour networkBehaviour;

		internal USpeaker uSpeaker;

		internal GamelikeInputController input;

		internal bool detectedFirstGround;

		internal int airstuckDetections;

		internal int lastNetworkedUpdatePacketNumber;

		internal float lastNetworkedUpdateTime;

		internal float lastNetworkedVoicePacket;

		internal int lagBarrier;

		internal GameObject nameplateCanvas;

		internal ImageThreeSlice nameplateBackground;

		internal Image nameplateIconBackground;

		internal GameObject customNameplateObject;

		internal RectTransform customNameplateTransform;

		internal TextMeshProUGUI customNameplateText;

		internal bool IsFriends()
		{
			return APIUser.IsFriendsWith(id);
		}

		internal short GetPing()
		{
			return vrcPlayer.prop_PlayerNet_0.prop_Int16_1;
		}

		internal int GetFPS()
		{
			return (int)(1000f / (float)(int)vrcPlayer.prop_PlayerNet_0.field_Private_Byte_0);
		}

		internal byte GetFPSRaw()
		{
			return vrcPlayer.prop_PlayerNet_0.field_Private_Byte_0;
		}

		internal bool IsGrounded()
		{
			if (playerApi == null)
			{
				return false;
			}
			try
			{
				return playerApi.IsPlayerGrounded();
			}
			catch (Il2CppException)
			{
				return false;
			}
		}

		internal Vector3 GetVelocity()
		{
			if (playerApi == null)
			{
				return Vector3.zero;
			}
			try
			{
				return playerApi.GetVelocity();
			}
			catch (Il2CppException)
			{
				return Vector3.zero;
			}
		}

		internal GameObject GetAvatar()
		{
			return vrcPlayer.field_Internal_GameObject_0;
		}
	}
}
