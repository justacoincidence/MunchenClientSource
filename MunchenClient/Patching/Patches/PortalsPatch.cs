using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using MunchenClient.ModuleSystem;
using MunchenClient.Utils;
using UnhollowerBaseLib;

namespace MunchenClient.Patching.Patches
{
	internal class PortalsPatch : PatchComponent
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void OnPortalEnteredDelegate(IntPtr thisPtr);

		internal static OnPortalEnteredDelegate originalPortalEntered;

		protected override string patchName => "PortalsPatch";

		internal unsafe override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(PortalsPatch));
			PatchMethod(typeof(PortalInternal).GetMethod("OnDestroy"), null, GetLocalPatch("OnPortalDestroyed"));
			PatchMethod(typeof(PortalInternal).GetMethod("ConfigurePortal"), null, GetLocalPatch("OnPortalCreated"));
			PatchMethod(typeof(PortalInternal).GetMethod("SetTimerRPC"), null, GetLocalPatch("OnPortalSetTimerPatch"));
			MethodInfo method = (from mb in typeof(PortalInternal).GetMethods()
				where mb.Name.StartsWith("Method_Public_Void_") && mb.Name.Length <= 21 && CheckUsed(mb, "OnTriggerEnter")
				select mb).First();
			IntPtr ptr = *(IntPtr*)(void*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(method).GetValue(null);
			OnPortalEnteredDelegate onPortalEnteredDelegate = delegate(IntPtr thisPtr)
			{
				OnPortalEnteredPatch(thisPtr);
			};
			MainUtils.antiGCList.Add(onPortalEnteredDelegate);
			MelonUtils.NativeHookAttach((IntPtr)(&ptr), Marshal.GetFunctionPointerForDelegate(onPortalEnteredDelegate));
			originalPortalEntered = Marshal.GetDelegateForFunctionPointer<OnPortalEnteredDelegate>(ptr);
		}

		private static void OnPortalDestroyed(ref PortalInternal __instance)
		{
			ModuleManager.OnPortalDestroyed(ref __instance);
		}

		private static void OnPortalCreated(ref PortalInternal __instance, string __0, string __1, int __2)
		{
			ModuleManager.OnPortalCreated(ref __instance, __0, __1, __2);
		}

		private static void OnPortalSetTimerPatch(ref PortalInternal __instance, float __0)
		{
			ModuleManager.OnPortalSetTimer(ref __instance, __0);
		}

		private static void OnPortalEnteredPatch(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				originalPortalEntered(thisPtr);
				return;
			}
			PortalInternal portal = new PortalInternal(thisPtr);
			if (ModuleManager.OnPortalEntered(ref portal))
			{
				originalPortalEntered(thisPtr);
			}
		}
	}
}
