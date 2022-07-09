using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using Newtonsoft.Json;
using Transmtn;
using UnhollowerBaseLib;
using VRC.Core;

namespace MunchenClient.Patching.Patches
{
	internal class WebSocketPatch : PatchComponent
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void ProcessPipelineDelegate(System.IntPtr instancePtr, System.IntPtr senderPtr, System.IntPtr argsPtr);

		private static readonly System.Collections.Generic.Dictionary<string, PlayerState> cachedPlayerStates = new System.Collections.Generic.Dictionary<string, PlayerState>();

		private static ProcessPipelineDelegate processPipelineDelegate = null;

		protected override string patchName => "WebSocketPatch";

		internal unsafe override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(WebSocketPatch));
			MethodInfo method = typeof(WebsocketPipeline).GetMethod("_ProcessPipe_b__21_0");
			System.IntPtr ptr = *(System.IntPtr*)(void*)(System.IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(method).GetValue(null);
			MelonUtils.NativeHookAttach((System.IntPtr)(&ptr), GetLocalPatch("OnDataReceivedPatch").method.MethodHandle.GetFunctionPointer());
			processPipelineDelegate = Marshal.GetDelegateForFunctionPointer<ProcessPipelineDelegate>(ptr);
		}

		private static void CachePlayer(string userId, VRCUser user, string userLocation)
		{
			if (!cachedPlayerStates.ContainsKey(userId))
			{
				cachedPlayerStates.Add(userId, new PlayerState
				{
					username = user.username,
					joinState = user.state,
					location = userLocation
				});
			}
		}

		private unsafe static void OnDataReceivedPatch(System.IntPtr instancePtr, System.IntPtr senderPtr, System.IntPtr argsPtr)
		{
			processPipelineDelegate(instancePtr, senderPtr, argsPtr);
			if (argsPtr == System.IntPtr.Zero)
			{
				return;
			}
			string value;
			try
			{
				nint num = (nint)argsPtr + 16;
				value = IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			catch (System.Exception)
			{
				return;
			}
			VRCWebSocketObject vRCWebSocketObject;
			try
			{
				vRCWebSocketObject = JsonConvert.DeserializeObject<VRCWebSocketObject>(value);
			}
			catch (System.Exception)
			{
				return;
			}
			if (vRCWebSocketObject == null || vRCWebSocketObject.type == "notification")
			{
				return;
			}
			VRCWebSocketContent vRCWebSocketContent;
			try
			{
				vRCWebSocketContent = JsonConvert.DeserializeObject<VRCWebSocketContent>(vRCWebSocketObject.content);
			}
			catch (System.Exception)
			{
				return;
			}
			if (APIUser.CurrentUser == null || vRCWebSocketContent == null || vRCWebSocketContent.userId == APIUser.CurrentUser.id)
			{
				return;
			}
			if (vRCWebSocketContent.user != null)
			{
				string userLocation = ((!string.IsNullOrEmpty(vRCWebSocketContent.travelingToLocation)) ? vRCWebSocketContent.travelingToLocation : ((vRCWebSocketContent.world != null) ? vRCWebSocketContent.world.name : string.Empty));
				CachePlayer(vRCWebSocketContent.user.id, vRCWebSocketContent.user, userLocation);
			}
			switch (vRCWebSocketObject.type)
			{
			case "friend-location":
			{
				string text3 = (string.IsNullOrEmpty(vRCWebSocketContent.travelingToLocation) ? vRCWebSocketContent.world.name : vRCWebSocketContent.travelingToLocation);
				if (text3 != cachedPlayerStates[vRCWebSocketContent.user.id].location)
				{
					cachedPlayerStates[vRCWebSocketContent.user.id].location = text3;
					string text4 = "<color=purple>" + vRCWebSocketContent.user.displayName + " <color=white>went to <color=yellow>" + text3;
					if (Configuration.GetModerationsConfig().LogModerationsInstanceSwitch)
					{
						ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text4, System.ConsoleColor.Yellow, "OnDataReceivedPatch", 215);
					}
					if (Configuration.GetModerationsConfig().LogModerationsInstanceSwitchHUD)
					{
						GeneralUtils.InformHudTextThreaded(LanguageManager.GetUsedLanguage().ModerationMenuName, text4);
					}
				}
				break;
			}
			case "friend-online":
			{
				string text = "<color=purple>" + vRCWebSocketContent.user.displayName + " <color=white>went <color=green>online";
				if (Configuration.GetModerationsConfig().LogModerationsOnline)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text, System.ConsoleColor.Green, "OnDataReceivedPatch", 233);
				}
				if (Configuration.GetModerationsConfig().LogModerationsOnlineHUD)
				{
					GeneralUtils.InformHudTextThreaded(LanguageManager.GetUsedLanguage().ModerationMenuName, text);
				}
				break;
			}
			case "friend-offline":
				if (cachedPlayerStates.ContainsKey(vRCWebSocketContent.userId))
				{
					string text2 = "<color=purple>" + cachedPlayerStates[vRCWebSocketContent.userId].username + " <color=white>went <color=red>offline";
					if (Configuration.GetModerationsConfig().LogModerationsOffline)
					{
						ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text2, System.ConsoleColor.Red, "OnDataReceivedPatch", 252);
					}
					if (Configuration.GetModerationsConfig().LogModerationsOfflineHUD)
					{
						GeneralUtils.InformHudTextThreaded(LanguageManager.GetUsedLanguage().ModerationMenuName, text2);
					}
				}
				else
				{
					API.SendGetRequest("users/" + vRCWebSocketContent.userId, (ApiContainer)new ApiModelContainer<APIUser>
					{
						OnSuccess = (System.Action<ApiContainer>)OnPlayerFetched,
						OnError = (System.Action<ApiContainer>)OnPlayerFailedFetch
					}, (Il2CppSystem.Collections.Generic.IReadOnlyDictionary<string, BestHTTP.JSON.Json.Token>)null, false, 0f, (API.CredentialsBundle)null);
				}
				break;
			case "friend-update":
			case "friend-active":
				if (vRCWebSocketContent.user.state != cachedPlayerStates[vRCWebSocketContent.user.id].joinState)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, vRCWebSocketContent.user.displayName + " State: " + vRCWebSocketContent.user.state, System.ConsoleColor.Green, "OnDataReceivedPatch", 277);
				}
				break;
			}
		}

		private static void OnPlayerFetched(ApiContainer container)
		{
			ApiModelContainer<APIUser> apiModelContainer = container.Cast<ApiModelContainer<APIUser>>();
			string text = string.Concat("<color=purple>", new Il2CppSystem.String(((ApiDictContainer)apiModelContainer).ResponseDictionary["displayName"].Pointer), " <color=white>went <color=red>offline");
			if (Configuration.GetModerationsConfig().LogModerationsOffline)
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text, System.ConsoleColor.Red, "OnPlayerFetched", 293);
			}
			if (Configuration.GetModerationsConfig().LogModerationsOfflineHUD)
			{
				GeneralUtils.InformHudTextThreaded(LanguageManager.GetUsedLanguage().ModerationMenuName, text);
			}
		}

		private static void OnPlayerFailedFetch(ApiContainer container)
		{
		}
	}
}
