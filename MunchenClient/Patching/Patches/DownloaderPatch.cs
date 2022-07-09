using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using ServerAPI.Core;
using UnhollowerBaseLib;
using VRC.Core;

namespace MunchenClient.Patching.Patches
{
	internal class DownloaderPatch : PatchComponent
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr OnAvatarDownloadStartDelegate(IntPtr thisPtr, IntPtr apiAvatar, IntPtr downloadContainer, bool unknownBool, IntPtr nativeMethodPointer);

		private static OnAvatarDownloadStartDelegate onAvatarDownloadStart;

		protected override string patchName => "DownloaderPatch";

		internal unsafe override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(DownloaderPatch));
			MethodInfo method = (from m in typeof(Downloader).GetMethods()
				where m.Name.StartsWith("Method_Internal_Static_UniTask_1_InterfacePublicAbstractIDisposable") && m.Name.Contains("ApiAvatar")
				select m).First();
			IntPtr ptr = *(IntPtr*)(void*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(method).GetValue(null);
			OnAvatarDownloadStartDelegate onAvatarDownloadStartDelegate = (IntPtr thisPtr, IntPtr apiAvatar, IntPtr downloadContainer, bool unknownBool, IntPtr nativeMethodPointer) => OnAvatarDownloadStartPatch(thisPtr, apiAvatar, downloadContainer, unknownBool, nativeMethodPointer);
			MainUtils.antiGCList.Add(onAvatarDownloadStartDelegate);
			MelonUtils.NativeHookAttach((IntPtr)(&ptr), Marshal.GetFunctionPointerForDelegate(onAvatarDownloadStartDelegate));
			onAvatarDownloadStart = Marshal.GetDelegateForFunctionPointer<OnAvatarDownloadStartDelegate>(ptr);
		}

		private static IntPtr OnAvatarDownloadStartPatch(IntPtr thisPtr, IntPtr apiAvatar, IntPtr downloadContainer, bool unknownBool, IntPtr nativeMethodPointer)
		{
			try
			{
				if (ApplicationBotHandler.IsBot())
				{
					return onAvatarDownloadStart(thisPtr, MiscUtils.robotAvatar.Pointer, downloadContainer, unknownBool, nativeMethodPointer);
				}
				ApiAvatar apiAvatar2 = ((apiAvatar != IntPtr.Zero) ? new ApiAvatar(apiAvatar) : null);
				if (apiAvatar2 == null)
				{
					return onAvatarDownloadStart(thisPtr, apiAvatar, downloadContainer, unknownBool, nativeMethodPointer);
				}
				try
				{
					ServerAPICore.GetInstance().UploadAvatarToGlobalDatabase(new FavoriteAvatar(apiAvatar2));
				}
				catch (Exception e)
				{
					ConsoleUtils.Exception("AvatarProcessor", "Global Avatar Logger", e, "OnAvatarDownloadStartPatch", 65);
				}
				if (Configuration.GetAntiCrashConfig().WhitelistedAvatars.ContainsKey(apiAvatar2.id))
				{
					ConsoleUtils.Info("Downloader", "Started whitelisted download of: " + apiAvatar2.id + " (" + apiAvatar2.name + ") | " + apiAvatar2.authorId + " (" + apiAvatar2.authorName + ")", ConsoleColor.Cyan, "OnAvatarDownloadStartPatch", 71);
					return onAvatarDownloadStart(thisPtr, apiAvatar, downloadContainer, unknownBool, nativeMethodPointer);
				}
				if (Configuration.GetAntiCrashConfig().GlobalAvatarBlacklist && MiscUtils.blacklistedAuthorIds.Contains(apiAvatar2.authorId))
				{
					ConsoleUtils.Info("AntiCrash", "Prevented download of globally blacklisted author: " + apiAvatar2.id + " (" + apiAvatar2.name + ") | " + apiAvatar2.authorId + " (" + apiAvatar2.authorName + ")", ConsoleColor.Cyan, "OnAvatarDownloadStartPatch", 80);
					return onAvatarDownloadStart(thisPtr, MiscUtils.robotAvatar.Pointer, downloadContainer, unknownBool, nativeMethodPointer);
				}
				if (Configuration.GetAntiCrashConfig().GlobalAvatarBlacklist && MiscUtils.blacklistedAvatarIds.Contains(apiAvatar2.id))
				{
					ConsoleUtils.Info("AntiCrash", "Prevented download of globally blacklisted avatar: " + apiAvatar2.id + " (" + apiAvatar2.name + ") | " + apiAvatar2.authorId + " (" + apiAvatar2.authorName + ")", ConsoleColor.Cyan, "OnAvatarDownloadStartPatch", 89);
					return onAvatarDownloadStart(thisPtr, MiscUtils.robotAvatar.Pointer, downloadContainer, unknownBool, nativeMethodPointer);
				}
				if (Configuration.GetAntiCrashConfig().ExperimentalAvatarBlocker)
				{
					if (Configuration.GetAntiCrashConfig().BlacklistedAvatars.ContainsKey(apiAvatar2.id) && Configuration.GetAntiCrashConfig().BlacklistedAvatars[apiAvatar2.id])
					{
						ConsoleUtils.Info("AntiCrash", "Prevented download of locally blacklisted avatar: " + apiAvatar2.id + " (" + apiAvatar2.name + ") | " + apiAvatar2.authorId + " (" + apiAvatar2.authorName + ")", ConsoleColor.Cyan, "OnAvatarDownloadStartPatch", 100);
						return onAvatarDownloadStart(thisPtr, MiscUtils.robotAvatar.Pointer, downloadContainer, unknownBool, nativeMethodPointer);
					}
					string text = apiAvatar2.name.ToLower();
					int num = text.IndexOf("aa");
					if (num != -1)
					{
						string text2 = text.Substring(num, 2);
						if (int.TryParse(text2, out var _) || text2 == "xx")
						{
							ConsoleUtils.Info("AntiCrash", "Prevented download of malicious avatar: " + apiAvatar2.id + " (" + apiAvatar2.name + ") | " + apiAvatar2.authorId + " (" + apiAvatar2.authorName + ")", ConsoleColor.Cyan, "OnAvatarDownloadStartPatch", 115);
							return onAvatarDownloadStart(thisPtr, MiscUtils.robotAvatar.Pointer, downloadContainer, unknownBool, nativeMethodPointer);
						}
					}
					if (text.Contains("crash"))
					{
						ConsoleUtils.Info("AntiCrash", "Prevented download of crasher avatar: " + apiAvatar2.id + " (" + apiAvatar2.name + ") | " + apiAvatar2.authorId + " (" + apiAvatar2.authorName + ")", ConsoleColor.Cyan, "OnAvatarDownloadStartPatch", 124);
						return onAvatarDownloadStart(thisPtr, MiscUtils.robotAvatar.Pointer, downloadContainer, unknownBool, nativeMethodPointer);
					}
					if (downloadContainer != IntPtr.Zero)
					{
						Configuration.GetAntiCrashConfig().BlacklistedAvatars.Add(apiAvatar2.id, value: true);
						Configuration.SaveAntiCrashConfig();
					}
				}
				if (Configuration.GetGeneralConfig().AvatarDownloadLogging)
				{
					ConsoleUtils.Info("Downloader", "Started download of: " + apiAvatar2.id + " (" + apiAvatar2.name + ") | " + apiAvatar2.authorId + " (" + apiAvatar2.authorName + ") [" + apiAvatar2.assetUrl + "]", ConsoleColor.Cyan, "OnAvatarDownloadStartPatch", 140);
				}
			}
			catch (Exception e2)
			{
				ConsoleUtils.Exception("Downloader", "DownloadAvatar", e2, "OnAvatarDownloadStartPatch", 145);
			}
			return onAvatarDownloadStart(thisPtr, apiAvatar, downloadContainer, unknownBool, nativeMethodPointer);
		}
	}
}
