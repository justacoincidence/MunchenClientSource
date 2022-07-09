using System;
using System.Runtime.InteropServices;

namespace MunchenClient.ModuleSystem.Modules.AntiAssetBundleCrasher
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 112)]
	internal struct AssetBundleDownloadHandlerVTablePrefix
	{
		private IntPtr destructorValue;

		private readonly IntPtr unknown1;

		private readonly IntPtr unknown2;

		private IntPtr onReceiveDataValue;

		private readonly IntPtr unknown3;

		private readonly IntPtr unknown4;

		private IntPtr onCompleteContentValue;

		private readonly IntPtr unknown5;

		private readonly IntPtr unknown6;

		private readonly IntPtr getMemorySize2Value;

		private readonly IntPtr getMemorySize1Value;

		private readonly IntPtr getProgressValue;

		private IntPtr prepareValue;

		private IntPtr onAbortValue;

		internal unsafe delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> Destructor
		{
			get
			{
				return (delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr>)(void*)destructorValue;
			}
			set
			{
				destructorValue = (IntPtr)value;
			}
		}

		internal unsafe delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int, int> OnReceiveData
		{
			get
			{
				return (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int, int>)(void*)onReceiveDataValue;
			}
			set
			{
				onReceiveDataValue = (IntPtr)value;
			}
		}

		internal unsafe delegate* unmanaged[Cdecl]<IntPtr, void> OnCompleteContent
		{
			get
			{
				return (delegate* unmanaged[Cdecl]<IntPtr, void>)(void*)onCompleteContentValue;
			}
			set
			{
				onCompleteContentValue = (IntPtr)value;
			}
		}

		internal unsafe delegate* unmanaged[Cdecl]<IntPtr, int> Prepare
		{
			get
			{
				return (delegate* unmanaged[Cdecl]<IntPtr, int>)(void*)prepareValue;
			}
			set
			{
				prepareValue = (IntPtr)value;
			}
		}

		internal unsafe delegate* unmanaged[Cdecl]<IntPtr, void> OnAbort
		{
			get
			{
				return (delegate* unmanaged[Cdecl]<IntPtr, void>)(void*)onAbortValue;
			}
			set
			{
				onAbortValue = (IntPtr)value;
			}
		}
	}
}
