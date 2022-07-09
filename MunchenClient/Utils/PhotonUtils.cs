using ExitGames.Client.Photon;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnhollowerBaseLib;
using VRC.Core;

namespace MunchenClient.Utils
{
	internal class PhotonUtils
	{
		internal static void OpRaiseEvent(byte code, object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
		{
			Object customObject2 = SerializationUtils.FromManagedToIL2CPP<Object>(customObject);
			OpRaiseEvent(code, customObject2, RaiseEventOptions, sendOptions);
		}

		internal static void OpRaiseEvent(byte code, Object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
		{
			PhotonNetwork.field_Public_Static_LoadBalancingClient_0.Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0(code, customObject, RaiseEventOptions, sendOptions);
		}

		internal static ApiAvatar AvatarDictionaryToApiAvatar(Dictionary<string, Object> avatar)
		{
			return new ApiAvatar
			{
				id = new String(avatar["id"].Pointer),
				name = new String(avatar["name"].Pointer),
				releaseStatus = new String(avatar["releaseStatus"].Pointer),
				assetUrl = new String(avatar["assetUrl"].Pointer),
				version = avatar["version"].Unbox<int>(),
				authorName = new String(avatar["authorName"].Pointer),
				authorId = new String(avatar["authorId"].Pointer),
				description = new String(avatar["description"].Pointer),
				thumbnailImageUrl = new String(avatar["thumbnailImageUrl"].Pointer),
				imageUrl = new String(avatar["imageUrl"].Pointer)
			};
		}

		internal static Dictionary<string, Object> ApiAvatarToAvatarDictionary(ApiAvatar avatar)
		{
			Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
			dictionary["id"] = (String)avatar.id;
			dictionary["name"] = (String)avatar.name;
			dictionary["releaseStatus"] = (String)avatar.releaseStatus;
			dictionary["assetUrl"] = (String)"";
			dictionary["version"] = new Int32
			{
				m_value = avatar.version
			}.BoxIl2CppObject();
			dictionary["authorName"] = (String)avatar.authorName;
			dictionary["authorId"] = (String)avatar.authorId;
			dictionary["description"] = (String)avatar.description;
			dictionary["thumbnailImageUrl"] = (String)avatar.thumbnailImageUrl;
			dictionary["imageUrl"] = (String)avatar.imageUrl;
			dictionary["platform"] = (String)avatar.platform;
			Il2CppReferenceArray<Object> il2CppReferenceArray = new Il2CppReferenceArray<Object>(1L);
			Dictionary<string, Object> dictionary2 = new Dictionary<string, Object>();
			string text = avatar.assetVersion.UnityVersion.Substring(0, 4);
			string text2 = avatar.assetVersion.UnityVersion.Substring(5, 1);
			string text3 = avatar.assetVersion.UnityVersion.Substring(7, 2);
			double value = double.Parse(text + "0" + text2 + text3 + "000");
			dictionary2["assetUrl"] = (String)avatar.assetUrl;
			dictionary2["unityVersion"] = (String)avatar.assetVersion.UnityVersion;
			dictionary2["unitySortNumber"] = new Double
			{
				m_value = value
			}.BoxIl2CppObject();
			dictionary2["assetVersion"] = new Int32
			{
				m_value = avatar.assetVersion.ApiVersion
			}.BoxIl2CppObject();
			dictionary2["platform"] = (String)avatar.platform;
			il2CppReferenceArray[0] = dictionary2;
			dictionary["unityPackages"] = il2CppReferenceArray.Cast<Object>();
			return dictionary;
		}
	}
}
