using System;
using System.Runtime.InteropServices;
using MunchenClient.Core.Compatibility;
using MunchenClient.Utils;

namespace MunchenClient.ModuleSystem.Modules.AntiAssetBundleCrasher
{
	internal class AssetBundleDownloadHandlerWrapper
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int PrepareDelegate(IntPtr thisPtr);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void VoidDelegate(IntPtr thisPtr);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr DestructorDelegate(IntPtr thisPtr, long unk);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int ReceiveDelegate(IntPtr thisPtr, IntPtr bytes, int length);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private unsafe delegate IntPtr CreateCachedDelegate(IntPtr thisPtr, UnmanagedUtils.NativeString* url, UnmanagedUtils.NativeString* name, IntPtr hash, int crc);

		private static CreateCachedDelegate originalCreateCached;

		private static AssetBundleDownloadHandlerVTablePrefix originalVTable;

		internal static bool InitializePatches()
		{
			if (!PatchABDHVTable())
			{
				return false;
			}
			if (!PatchCreateCachedFunction())
			{
				return false;
			}
			return true;
		}

		private unsafe static bool PatchABDHVTable()
		{
			int assetBundleDownloadHandlerVTable = CompatibilityLayer.GetUnityPlayerOffsets().assetBundleDownloadHandlerVTable;
			if (assetBundleDownloadHandlerVTable == 0)
			{
				return false;
			}
			IntPtr intPtr = IntPtr.Add(CompatibilityLayer.GetUnityPlayerBaseAddress(), assetBundleDownloadHandlerVTable);
			originalVTable = *(AssetBundleDownloadHandlerVTablePrefix*)(void*)intPtr;
			AssetBundleDownloadHandlerVTablePrefix assetBundleDownloadHandlerVTablePrefix = originalVTable;
			assetBundleDownloadHandlerVTablePrefix.Prepare = (delegate* unmanaged[Cdecl]<IntPtr, int>)(void*)GetDelegatePointerAndPin<PrepareDelegate>(ABDHPreparePatch);
			assetBundleDownloadHandlerVTablePrefix.Destructor = (delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr>)(void*)GetDelegatePointerAndPin<DestructorDelegate>(ABDHDestructorPatch);
			assetBundleDownloadHandlerVTablePrefix.OnCompleteContent = (delegate* unmanaged[Cdecl]<IntPtr, void>)(void*)GetDelegatePointerAndPin<VoidDelegate>(ABDHCompletePatch);
			assetBundleDownloadHandlerVTablePrefix.OnReceiveData = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int, int>)(void*)GetDelegatePointerAndPin<ReceiveDelegate>(ABDHReceivePatch);
			UnmanagedUtils.VirtualProtect(intPtr, (UIntPtr)(ulong)Marshal.SizeOf<AssetBundleDownloadHandlerVTablePrefix>(), UnmanagedUtils.Protection.PAGE_READWRITE, out var lpflOldProtect);
			*(AssetBundleDownloadHandlerVTablePrefix*)(void*)intPtr = assetBundleDownloadHandlerVTablePrefix;
			UnmanagedUtils.VirtualProtect(intPtr, (UIntPtr)(ulong)Marshal.SizeOf<AssetBundleDownloadHandlerVTablePrefix>(), lpflOldProtect, out var _);
			return true;
		}

		private unsafe static bool PatchCreateCachedFunction()
		{
			int assetBundleDownloadHandlerCreateCached = CompatibilityLayer.GetUnityPlayerOffsets().assetBundleDownloadHandlerCreateCached;
			if (assetBundleDownloadHandlerCreateCached == 0)
			{
				return false;
			}
			IntPtr intPtr = IntPtr.Add(CompatibilityLayer.GetUnityPlayerBaseAddress(), assetBundleDownloadHandlerCreateCached);
			UnmanagedUtils.PatchEngineOffset<CreateCachedDelegate>(assetBundleDownloadHandlerCreateCached, CreateCachedPatch, out originalCreateCached);
			return true;
		}

		private static IntPtr GetDelegatePointerAndPin<T>(T input) where T : MulticastDelegate
		{
			MainUtils.antiGCList.Add(input);
			return Marshal.GetFunctionPointerForDelegate(input);
		}

		private unsafe static IntPtr CreateCachedPatch(IntPtr scriptingObjectPtr, UnmanagedUtils.NativeString* url, UnmanagedUtils.NativeString* name, IntPtr hash, int crc)
		{
			return originalCreateCached(scriptingObjectPtr, url, name, hash, crc);
		}

		internal unsafe static int ABDHPreparePatch(IntPtr thisPtr)
		{
			return originalVTable.Prepare(thisPtr);
		}

		internal unsafe static IntPtr ABDHDestructorPatch(IntPtr thisPtr, long unk)
		{
			return originalVTable.Destructor(thisPtr, unk);
		}

		internal unsafe static int ABDHReceivePatch(IntPtr thisPtr, IntPtr bytes, int byteCount)
		{
			return originalVTable.OnReceiveData(thisPtr, bytes, byteCount);
		}

		internal unsafe static void ABDHCompletePatch(IntPtr thisPtr)
		{
			originalVTable.OnCompleteContent(thisPtr);
		}
	}
}
