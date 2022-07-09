using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Core.Compatibility;
using MunchenClient.Utils;

namespace MunchenClient.Patching.Patches
{
	internal class AntiCrashPatch : PatchComponent
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr AudioMixerReadDelegate(IntPtr thisPtr, IntPtr readerPtr);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private unsafe delegate void FloatReadDelegate(IntPtr readerPtr, float* result, byte* fieldName);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void OutOfBoundsCheckDelegate(IntPtr thisPtr, long a, long b);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void DebugAssertDelegate(IntPtr data);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private unsafe delegate long CountNodesDeepDelegate(UnmanagedUtils.NodeContainer* thisPtr);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private unsafe delegate IntPtr StringReallocateDelegate(UnmanagedUtils.NativeString* str, long newSize);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate ulong ReadObjectDelegate(long a1, long a2, IntPtr a3, IntPtr a4, IntPtr a5, IntPtr a6, IntPtr a7);

		private static AudioMixerReadDelegate audioMixerReadDelegate;

		private static bool allowReadInvalidFloatValues = true;

		private static bool allowReadAudioMixers = true;

		private static bool allowReadObjectChecks = false;

		private const float allowedValueMax = 3.402823E+07f;

		private const float allowedValueMin = -3.402823E+07f;

		private static readonly string[] whitelistedFields = new string[6] { "m_BreakForce", "m_BreakTorque", "collisionSphereDistance", "maxDistance", "inSlope", "outSlope" };

		private static FloatReadDelegate floatReadDelegate;

		private static OutOfBoundsCheckDelegate outOfBoundsCheck;

		[ThreadStatic]
		private static int outOfBoundsCurrentDepth;

		private static DebugAssertDelegate debugAssertCheck;

		private static CountNodesDeepDelegate countNodesDeep;

		private static StringReallocateDelegate reallocateString;

		[ThreadStatic]
		private unsafe static UnmanagedUtils.NativeString* previousReallocatedString;

		[ThreadStatic]
		private static int previousReallocationCount;

		private static ReadObjectDelegate readObject;

		protected override string patchName => "AntiCrashPatch";

		internal unsafe override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(AntiCrashPatch));
			if (CompatibilityLayer.GetUnityPlayerBaseAddress() == IntPtr.Zero)
			{
				ConsoleUtils.Info("AntiCrash", "UnityPlayer not found - AntiCrash will be *HEAVILY* limited", ConsoleColor.Gray, "OnInitializeOnStart", 71);
				return;
			}
			UnmanagedUtils.PatchEngineOffset<AudioMixerReadDelegate>(CompatibilityLayer.GetUnityPlayerOffsets().loadAudioMixer, AudioMixerReadPatch, out audioMixerReadDelegate);
			UnmanagedUtils.PatchEngineOffset<FloatReadDelegate>(CompatibilityLayer.GetUnityPlayerOffsets().readFloatValue, FloatTransferPatch, out floatReadDelegate);
			UnmanagedUtils.PatchEngineOffset<CountNodesDeepDelegate>(CompatibilityLayer.GetUnityPlayerOffsets().countNodes, CountNodesDeepPatch, out countNodesDeep);
			UnmanagedUtils.PatchEngineOffset<DebugAssertDelegate>(CompatibilityLayer.GetUnityPlayerOffsets().debugAssert, DebugAssertPatch, out debugAssertCheck);
			UnmanagedUtils.PatchEngineOffset<OutOfBoundsCheckDelegate>(CompatibilityLayer.GetUnityPlayerOffsets().outOfBoundsCheck, ReaderOobPatch, out outOfBoundsCheck);
			UnmanagedUtils.PatchEngineOffset<StringReallocateDelegate>(CompatibilityLayer.GetUnityPlayerOffsets().reallocateString, ReallocateStringPatch, out reallocateString);
			UnmanagedUtils.PatchEngineOffset<ReadObjectDelegate>(CompatibilityLayer.GetUnityPlayerOffsets().readObject, ReadObjectPatch, out readObject);
			PatchGameCloserExploit(Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash);
		}

		internal static void OnRoomLeft()
		{
			allowReadAudioMixers = true;
			allowReadInvalidFloatValues = true;
			allowReadObjectChecks = false;
		}

		internal static void OnRoomJoined()
		{
			allowReadAudioMixers = false;
			allowReadInvalidFloatValues = false;
			allowReadObjectChecks = true;
		}

		internal static void PatchGameCloserExploit(bool state)
		{
			IntPtr address = IntPtr.Add(CompatibilityLayer.GetUnityPlayerBaseAddress(), CompatibilityLayer.GetUnityPlayerOffsets().assertLoadAssetBundle);
			UnmanagedUtils.ChangeInstructionAtAddress(address, (byte)(state ? 144u : 204u));
		}

		private unsafe static void FloatTransferPatch(IntPtr reader, float* result, byte* fieldName)
		{
			while (floatReadDelegate == null)
			{
				Thread.Sleep(10);
			}
			floatReadDelegate(reader, result, fieldName);
			if (!Configuration.GetAntiCrashConfig().AntiInvalidFloatsCrash || (*result > -3.402823E+07f && *result < 3.402823E+07f) || allowReadInvalidFloatValues || !float.IsNaN(*result))
			{
				return;
			}
			bool flag = false;
			if (fieldName != null)
			{
				string[] array = whitelistedFields;
				foreach (string text in array)
				{
					for (int j = 0; j < text.Length; j++)
					{
						if (fieldName[j] != 0 && fieldName[j] == text[j])
						{
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				*result = 0f;
			}
		}

		private static IntPtr AudioMixerReadPatch(IntPtr thisPtr, IntPtr readerPtr)
		{
			if (Configuration.GetAntiCrashConfig().AntiAvatarAudioMixerCrash && !allowReadAudioMixers)
			{
				return IntPtr.Zero;
			}
			while (audioMixerReadDelegate == null)
			{
				Thread.Sleep(10);
			}
			return audioMixerReadDelegate(thisPtr, readerPtr);
		}

		private static void ReaderOobPatch(IntPtr thisPtr, long a, long b)
		{
			if (!Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash)
			{
				while (outOfBoundsCheck == null)
				{
					Thread.Sleep(10);
				}
				outOfBoundsCheck(thisPtr, a, b);
				return;
			}
			outOfBoundsCurrentDepth++;
			try
			{
				while (outOfBoundsCheck == null)
				{
					Thread.Sleep(10);
				}
				outOfBoundsCheck(thisPtr, a, b);
			}
			finally
			{
				outOfBoundsCurrentDepth--;
			}
		}

		private unsafe static void DebugAssertPatch(IntPtr data)
		{
			if (Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash)
			{
				string text = Marshal.PtrToStringAnsi((IntPtr)(*(void**)(void*)data));
				if (text != null && text.Contains("is corrupted"))
				{
					ConsoleUtils.FlushToConsole(LanguageManager.GetUsedLanguage().ProtectionsMenuName, "Corrupt text found - Report to Killer - (" + text + ")", ConsoleColor.Gray, "DebugAssertPatch", 216);
					void* intPtr = (void*)(data + 48);
					*(byte*)intPtr = (byte)(*(byte*)intPtr & 0xEF);
				}
				else if (outOfBoundsCurrentDepth > 0)
				{
					void* intPtr2 = (void*)(data + 48);
					*(byte*)intPtr2 = (byte)(*(byte*)intPtr2 & 0xEF);
				}
			}
			while (debugAssertCheck == null)
			{
				Thread.Sleep(10);
			}
			debugAssertCheck(data);
		}

		private unsafe static long CountNodesDeepPatch(UnmanagedUtils.NodeContainer* thisPtr)
		{
			if (!Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash)
			{
				while (countNodesDeep == null)
				{
					Thread.Sleep(10);
				}
				return countNodesDeep(thisPtr);
			}
			try
			{
				return CountNodesDeep(thisPtr, new HashSet<IntPtr>());
			}
			catch (Exception)
			{
				return 1L;
			}
		}

		private unsafe static long CountNodesDeep(UnmanagedUtils.NodeContainer* thisPtr, HashSet<IntPtr> parents)
		{
			if (thisPtr == null)
			{
				return 1L;
			}
			long directSubCount = thisPtr->DirectSubCount;
			long num = 1L;
			if (directSubCount <= 0)
			{
				return num;
			}
			parents.Add((IntPtr)thisPtr);
			UnmanagedUtils.NodeContainer** subs = thisPtr->Subs;
			if (subs == null)
			{
				thisPtr->DirectSubCount = 0L;
				return num;
			}
			for (int i = 0; i < directSubCount; i++)
			{
				UnmanagedUtils.NodeContainer* ptr = subs[i];
				if (ptr == null)
				{
					thisPtr->DirectSubCount = 0L;
					return num;
				}
				if (parents.Contains((IntPtr)ptr))
				{
					ptr->DirectSubCount = (thisPtr->DirectSubCount = 0L);
					return num;
				}
				num += CountNodesDeep(ptr, parents);
			}
			return num;
		}

		private unsafe static IntPtr ReallocateStringPatch(UnmanagedUtils.NativeString* nativeString, long newSize)
		{
			if (Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash && nativeString != null && newSize > 128 && nativeString->Data != IntPtr.Zero)
			{
				if (previousReallocatedString != nativeString)
				{
					previousReallocatedString = nativeString;
					previousReallocationCount = 0;
				}
				else
				{
					previousReallocationCount++;
					if (previousReallocationCount >= 8 && newSize <= nativeString->Capacity + 16 && nativeString->Capacity > 16)
					{
						newSize = nativeString->Capacity * 2;
					}
				}
			}
			while (reallocateString == null)
			{
				Thread.Sleep(10);
			}
			return reallocateString(nativeString, newSize);
		}

		private unsafe static ulong ReadObjectPatch(long a1, long a2, IntPtr a3, IntPtr a4, IntPtr a5, IntPtr a6, IntPtr a7)
		{
			if (Configuration.GetAntiCrashConfig().AntiAvatarLoadingCrash && allowReadObjectChecks && a2 == 1)
			{
				long num = (long)(nuint)(*(nint*)(a1 + 104));
				long num2 = (long)(nuint)(*(nint*)(a1 + 96));
				long num3 = (num - num2) / 24;
				if (num3 < 13 || num3 > 10000)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().ProtectionsMenuName, $"Prevented ReadObject ({num3})", ConsoleColor.Gray, "ReadObjectPatch", 343);
					return 0uL;
				}
			}
			while (readObject == null)
			{
				Thread.Sleep(10);
			}
			return readObject(a1, a2, a3, a4, a5, a6, a7);
		}
	}
}
