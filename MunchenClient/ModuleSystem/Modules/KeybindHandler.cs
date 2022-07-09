using MunchenClient.Config;
using MunchenClient.Core.Compatibility;
using MunchenClient.Menu;
using MunchenClient.Menu.Others;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class KeybindHandler : ModuleComponent
	{
		protected override string moduleName => "Keybind Handler";

		internal override void OnUpdate()
		{
			if (Configuration.GetGeneralConfig().DisableAllKeybinds)
			{
				return;
			}
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
			{
				GeneralUtils.ToggleFlight(!GeneralUtils.flight);
				MovementMenu.flightButton.SetToggleState(GeneralUtils.flight);
				ActionWheelMenu.flightButton.SetButtonText(GeneralUtils.flight ? "Flight: <color=green>On" : "Flight: <color=red>Off");
			}
			if (!CompatibilityLayer.IsEmmInstalled())
			{
				if ((Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T)) || (Input.GetKey(KeyCode.T) && Input.GetKeyDown(KeyCode.LeftControl)))
				{
					CameraFeaturesHandler.ChangeCameraState();
				}
				else if ((Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G)) || (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.LeftControl)))
				{
					CameraFeaturesHandler.UseFreezeCamera();
				}
			}
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis != 0f)
			{
				CameraFeaturesHandler.ApplyThirdpersonSmoothZoom(axis > 0f);
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				if (!GeneralWrappers.IsInVR() && Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
				{
					Ray ray = new Ray(GeneralWrappers.GetPlayerCamera().transform.position, GeneralWrappers.GetPlayerCamera().transform.forward);
					if (Physics.Raycast(ray, out var hitInfo))
					{
						PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.transform.position = hitInfo.point;
					}
				}
				if (Input.GetMouseButtonDown(2))
				{
					CameraFeaturesHandler.ApplyCameraSmoothZoom(incremental: false, 60f);
				}
				else if (axis != 0f)
				{
					CameraFeaturesHandler.ApplyCameraSmoothZoom(incremental: true, axis * 30f);
				}
			}
			else if (Input.GetKeyDown(KeyCode.LeftAlt))
			{
				CameraFeaturesHandler.ChangeCameraActualZoomState(zoom: true);
			}
			else if (Input.GetKeyUp(KeyCode.LeftAlt))
			{
				CameraFeaturesHandler.ChangeCameraActualZoomState(zoom: false);
			}
		}
	}
}
