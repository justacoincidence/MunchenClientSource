using System;
using System.Runtime.InteropServices;
using MunchenClient.Utils;

namespace AntiShaderCrashAPI
{
	internal static class AntiShaderCrashProxy
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void SetFilterStateDelegate(bool limitLoops, bool limitGeometry, bool limitTesselation);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void SetTesselationDelegate(float value);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void SetLoopsDelegate(int value);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void SetGeometryDelegate(int value);

		internal const string antiCrashLibrary = "MünchenClient.AntiShaderCrashModule";

		private static SetFilterStateDelegate setFilterStateDelegate;

		private static SetTesselationDelegate setTesselationDelegate;

		private static SetLoopsDelegate setLoopsDelegate;

		private static SetGeometryDelegate setGeometryDelegate;

		internal static void Init(IntPtr modulePointer)
		{
			setFilterStateDelegate = Marshal.GetDelegateForFunctionPointer<SetFilterStateDelegate>(UnmanagedUtils.GetProcAddress(modulePointer, "SetFilteringState"));
			setTesselationDelegate = Marshal.GetDelegateForFunctionPointer<SetTesselationDelegate>(UnmanagedUtils.GetProcAddress(modulePointer, "SetMaxTesselationPower"));
			setLoopsDelegate = Marshal.GetDelegateForFunctionPointer<SetLoopsDelegate>(UnmanagedUtils.GetProcAddress(modulePointer, "SetLoopLimit"));
			setGeometryDelegate = Marshal.GetDelegateForFunctionPointer<SetGeometryDelegate>(UnmanagedUtils.GetProcAddress(modulePointer, "SetGeometryLimit"));
		}

		internal static void SetFilteringState(bool limitLoops, bool limitGeometry, bool limitTesselation)
		{
			setFilterStateDelegate(limitLoops, limitGeometry, limitTesselation);
		}

		internal static void SetMaxTesselationPower(float maxTesselation)
		{
			setTesselationDelegate(maxTesselation);
		}

		internal static void SetLoopLimit(int maxLoops)
		{
			setLoopsDelegate(maxLoops);
		}

		internal static void SetGeometryLimit(int geometryLimit)
		{
			setGeometryDelegate(geometryLimit);
		}
	}
}
