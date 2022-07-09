using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Il2CppSystem;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Transmtn.DTO.Notifications;
using UnhollowerBaseLib;
using VRC.Core;

namespace MunchenClient.Patching.Patches
{
	internal class NotificationPatch : PatchComponent
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate System.IntPtr AddNotificationDelegate(System.IntPtr instancePtr, System.IntPtr notificationPtr, System.IntPtr returnedException);

		internal static bool firstTimeFriendRequestsReceived = false;

		private static readonly HashSet<string> handledNotifications = new HashSet<string>();

		private static AddNotificationDelegate addNotificationDelegate = null;

		protected override string patchName => "NotificationPatch";

		internal unsafe override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(NotificationPatch));
			MethodInfo method = typeof(NotificationManager.ObjectNPrivateSealedNoBoVoNoBoNoBoNoBoNo0).GetMethods(BindingFlags.Instance | BindingFlags.Public).First((MethodInfo m) => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(Notification));
			System.IntPtr ptr = *(System.IntPtr*)(void*)(System.IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(method).GetValue(null);
			MelonUtils.NativeHookAttach((System.IntPtr)(&ptr), GetLocalPatch("AddNotificationPatch").method.MethodHandle.GetFunctionPointer());
			addNotificationDelegate = Marshal.GetDelegateForFunctionPointer<AddNotificationDelegate>(ptr);
		}

		private static System.IntPtr AddNotificationPatch(System.IntPtr instancePtr, System.IntPtr notificationPtr, System.IntPtr returnedException)
		{
			if (instancePtr == System.IntPtr.Zero || notificationPtr == System.IntPtr.Zero)
			{
				return System.IntPtr.Zero;
			}
			Notification notification = new Notification(notificationPtr);
			if (notification.id != null && !handledNotifications.Contains(notification.id))
			{
				handledNotifications.Add(notification.id);
				try
				{
					HandleNotification(ref notification);
				}
				catch (System.Exception e)
				{
					ConsoleUtils.Exception("NotificationPatch", "NotificationManager", e, "AddNotificationPatch", 64);
				}
			}
			return addNotificationDelegate(instancePtr, notificationPtr, returnedException);
		}

		private static void HandleNotification(ref Notification notification)
		{
			if (WorldUtils.GetCurrentWorld() == null || WorldUtils.GetCurrentInstance() == null || notification.notificationType == null)
			{
				return;
			}
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			if ((localPlayerInformation != null && localPlayerInformation.isLocalPlayer && localPlayerInformation.apiUser.statusValue == APIUser.UserStatus.DoNotDisturb) || GeneralUtils.IsStreamerModeEnabled())
			{
				return;
			}
			switch (notification.notificationType)
			{
			case "invite":
			{
				if (!Configuration.GetModerationsConfig().LogModerationsInvite)
				{
					break;
				}
				string text3 = LanguageManager.GetUsedLanguage().ModerationInvitation.Replace("{username}", notification.senderUsername).Replace("{world}", new Il2CppSystem.String(notification.details["worldName"].Pointer));
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text3, System.ConsoleColor.Yellow, "HandleNotification", 104);
				if (Configuration.GetModerationsConfig().LogModerationsInviteHUD)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text3);
					if (Configuration.GetModerationsConfig().ModerationSounds)
					{
						AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("NotificationReceived"));
					}
				}
				break;
			}
			case "requestInvite":
			{
				if (!Configuration.GetModerationsConfig().LogModerationsRequestInvite)
				{
					break;
				}
				string text4 = LanguageManager.GetUsedLanguage().ModerationRequestedInvite.Replace("{username}", notification.senderUsername);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text4, System.ConsoleColor.Yellow, "HandleNotification", 127);
				if (Configuration.GetModerationsConfig().LogModerationsRequestInviteHUD)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text4);
					if (Configuration.GetModerationsConfig().ModerationSounds)
					{
						AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("NotificationReceived"));
					}
				}
				break;
			}
			case "requestInviteResponse":
			{
				if (!Configuration.GetModerationsConfig().LogModerationsDeniedRequestInvite)
				{
					break;
				}
				string text5 = LanguageManager.GetUsedLanguage().ModerationInviteRequestDenied.Replace("{username}", notification.senderUsername);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text5, System.ConsoleColor.Yellow, "HandleNotification", 150);
				if (Configuration.GetModerationsConfig().LogModerationsDeniedRequestInviteHUD)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text5);
					if (Configuration.GetModerationsConfig().ModerationSounds)
					{
						AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("NotificationReceived"));
					}
				}
				break;
			}
			case "friendRequest":
			{
				if (!Configuration.GetModerationsConfig().LogModerationsFriendRequest || !firstTimeFriendRequestsReceived)
				{
					break;
				}
				string text2 = LanguageManager.GetUsedLanguage().ModerationFriendRequest.Replace("{username}", notification.senderUsername);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text2, System.ConsoleColor.Green, "HandleNotification", 172);
				if (Configuration.GetModerationsConfig().LogModerationsFriendRequestHUD)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text2);
					if (Configuration.GetModerationsConfig().ModerationSounds)
					{
						AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("NotificationReceived"));
					}
				}
				break;
			}
			case "voteToKick":
			{
				string userId = new Il2CppSystem.String(notification.details["id"].Pointer);
				bool flag = APIUser.IsFriendsWith(userId);
				if ((!flag || !Configuration.GetModerationsConfig().LogModerationsVotekickFriends) && (flag || !Configuration.GetModerationsConfig().LogModerationsVotekickOthers))
				{
					break;
				}
				string newValue = notification.message.Substring(39, notification.message.IndexOf(',') - 39);
				string text = LanguageManager.GetUsedLanguage().ModerationVotekick.Replace("{username}", newValue);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ModerationMenuName, text, System.ConsoleColor.Red, "HandleNotification", 199);
				if (Configuration.GetModerationsConfig().LogModerationsVotekickHUD)
				{
					GeneralUtils.InformHudText(LanguageManager.GetUsedLanguage().ModerationMenuName, text);
					if (Configuration.GetModerationsConfig().ModerationSounds)
					{
						AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("NotificationReceived"));
					}
				}
				break;
			}
			}
		}
	}
}
