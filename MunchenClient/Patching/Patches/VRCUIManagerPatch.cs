using System.Linq;
using System.Reflection;
using MunchenClient.Config;
using MunchenClient.Wrappers;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal class VRCUIManagerPatch : PatchComponent
	{
		protected override string patchName => "VRCUIManagerPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VRCUIManagerPatch));
			foreach (XrefInstance item in XrefScanner.XrefScan(typeof(VRCUiManager).GetMethod("LateUpdate")))
			{
				if (item.Type == XrefType.Method && item.TryResolve() != null && item.TryResolve().GetParameters().Length == 2 && item.TryResolve().GetParameters().All((ParameterInfo a) => a.ParameterType == typeof(bool)))
				{
					PatchMethod((MethodInfo)item.TryResolve(), GetLocalPatch("OnInterfacePlacedPatch"), null);
					break;
				}
			}
		}

		private static bool OnInterfacePlacedPatch(ref VRCUiManager __instance, bool __0)
		{
			if (!Configuration.GetGeneralConfig().ComfyVRMenu)
			{
				return true;
			}
			if (!GeneralWrappers.IsInVR())
			{
				return true;
			}
			float num = ((GeneralWrappers.GetVRCTrackingManager() != null) ? GeneralWrappers.GetVRCTrackingManager().transform.localScale.x : 1f);
			if (num <= 0f)
			{
				num = 1f;
			}
			Transform transform = __instance.transform;
			Transform transform2 = __instance.transform.Find("UnscaledUI");
			transform.position = GeneralWrappers.GetPlayerCamera().transform.position;
			Vector3 eulerAngles = GeneralWrappers.GetPlayerCamera().transform.rotation.eulerAngles;
			Vector3 euler = new Vector3(eulerAngles.x - 30f, eulerAngles.y, 0f);
			if (PlayerWrappers.GetCurrentPlayer() == null)
			{
				euler.x = (euler.z = 0f);
			}
			if (!__0)
			{
				transform.rotation = Quaternion.Euler(euler);
			}
			else
			{
				Quaternion quaternion = Quaternion.Euler(euler);
				if (!(Quaternion.Angle(transform.rotation, quaternion) < 15f))
				{
					if (!(Quaternion.Angle(transform.rotation, quaternion) < 25f))
					{
						transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, 5f);
					}
					else
					{
						transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, 1f);
					}
				}
			}
			if (num >= 0f)
			{
				transform.localScale = num * Vector3.one;
			}
			else
			{
				transform.localScale = Vector3.one;
			}
			if (num > float.Epsilon)
			{
				transform2.localScale = 1f / num * Vector3.one;
			}
			else
			{
				transform2.localScale = Vector3.one;
			}
			return false;
		}
	}
}
