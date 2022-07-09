using System.Collections;
using System.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class CameraFeaturesHandler : ModuleComponent
	{
		private static bool camerasInitialized = false;

		private static bool worldFullyLoaded = false;

		private static readonly Queue<bool> clippingStateQueue = new Queue<bool>();

		private static Camera cameraBack;

		private static Camera cameraFront;

		private static Camera cameraFreeze;

		private static float zoomOffset = 0f;

		private static float currentFovZoom = 60f;

		private static bool altZoom = false;

		private static int cameraSetup = 0;

		private bool lastApplicationFocusedState = true;

		private static Vector3 freezeCameraTargetPosition = Vector3.zero;

		private static Quaternion freezeCameraTargetRotation = Quaternion.identity;

		private readonly int CULLING_LAYER_HEAD = 262144;

		private readonly int CULLING_LAYER_PLAYERLOCAL = 1024;

		protected override string moduleName => "Camera Features";

		internal override void OnUIManagerLoaded()
		{
			camerasInitialized = false;
			GameObject gameObject = new GameObject("Munchen Back Camera");
			cameraBack = gameObject.AddComponent<Camera>();
			cameraBack.fieldOfView = 60f;
			cameraBack.nearClipPlane = 0.05f;
			cameraBack.transform.parent = GeneralWrappers.GetPlayerCamera().transform;
			cameraBack.transform.rotation = GeneralWrappers.GetPlayerCamera().transform.rotation;
			cameraBack.transform.position = GeneralWrappers.GetPlayerCamera().transform.position;
			cameraBack.transform.position -= cameraBack.transform.forward * 2f;
			cameraBack.cullingMask = GeneralWrappers.GetPlayerCamera().cullingMask;
			cameraBack.cullingMask |= CULLING_LAYER_HEAD;
			cameraBack.cullingMask &= ~CULLING_LAYER_PLAYERLOCAL;
			cameraBack.enabled = false;
			Object.DontDestroyOnLoad(cameraBack.gameObject);
			GameObject gameObject2 = new GameObject("Munchen Front Camera");
			cameraFront = gameObject2.AddComponent<Camera>();
			cameraFront.fieldOfView = 60f;
			cameraFront.nearClipPlane = 0.05f;
			cameraFront.transform.parent = GeneralWrappers.GetPlayerCamera().transform;
			cameraFront.transform.rotation = GeneralWrappers.GetPlayerCamera().transform.rotation;
			cameraFront.transform.Rotate(0f, 180f, 0f);
			cameraFront.transform.position = GeneralWrappers.GetPlayerCamera().transform.position;
			cameraFront.transform.position += -cameraFront.transform.forward * 2f;
			cameraFront.cullingMask = GeneralWrappers.GetPlayerCamera().cullingMask;
			cameraFront.cullingMask |= CULLING_LAYER_HEAD;
			cameraFront.cullingMask &= ~CULLING_LAYER_PLAYERLOCAL;
			cameraFront.enabled = false;
			Object.DontDestroyOnLoad(cameraFront.gameObject);
			GameObject gameObject3 = new GameObject("Munchen Freeze Camera");
			cameraFreeze = gameObject3.AddComponent<Camera>();
			cameraFreeze.fieldOfView = 60f;
			cameraFreeze.nearClipPlane = 0.05f;
			cameraFreeze.transform.parent = GeneralWrappers.GetPlayerCamera().transform;
			cameraFreeze.transform.rotation = GeneralWrappers.GetPlayerCamera().transform.rotation;
			cameraFreeze.transform.position = GeneralWrappers.GetPlayerCamera().transform.position;
			cameraFreeze.cullingMask = GeneralWrappers.GetPlayerCamera().cullingMask;
			cameraFreeze.cullingMask |= CULLING_LAYER_HEAD;
			cameraFreeze.cullingMask &= ~CULLING_LAYER_PLAYERLOCAL;
			cameraFreeze.enabled = false;
			Object.DontDestroyOnLoad(cameraFreeze.gameObject);
			camerasInitialized = true;
		}

		internal override void OnUpdate()
		{
			if (!camerasInitialized)
			{
				return;
			}
			if (worldFullyLoaded && clippingStateQueue.Count > 0)
			{
				bool flag = clippingStateQueue.Dequeue();
				GeneralWrappers.GetPlayerCamera().nearClipPlane = (flag ? 0.001f : 0.05f);
				GeneralWrappers.GetUICamera().nearClipPlane = (flag ? 0.001f : 0.05f);
				GeneralWrappers.GetPhotoCamera().nearClipPlane = (flag ? 0.001f : 0.05f);
				cameraFront.nearClipPlane = (flag ? 0.001f : 0.05f);
				cameraBack.nearClipPlane = (flag ? 0.001f : 0.05f);
				cameraFreeze.nearClipPlane = (flag ? 0.001f : 0.05f);
			}
			if (lastApplicationFocusedState != Application.isFocused)
			{
				lastApplicationFocusedState = Application.isFocused;
				altZoom = false;
				GeneralWrappers.GetReticle().SetActive(value: true);
				if (!Application.isFocused)
				{
					if (Configuration.GetGeneralConfig().PerformanceSmartFPS && !GeneralWrappers.IsInVR())
					{
						Application.targetFrameRate = 20;
					}
				}
				else if (Configuration.GetGeneralConfig().PerformanceSmartFPS && !GeneralWrappers.IsInVR())
				{
					if (Configuration.GetGeneralConfig().PerformanceUnlimitedFPS)
					{
						Application.targetFrameRate = GeneralUtils.GetRefreshRate();
					}
					else
					{
						Application.targetFrameRate = GeneralUtils.targetedRefreshRate;
					}
				}
			}
			if (!Configuration.GetGeneralConfig().DisableAllKeybinds)
			{
				if (cameraSetup == 0)
				{
					float fieldOfView = Mathf.Lerp(GeneralWrappers.GetPlayerCamera().fieldOfView, altZoom ? 10f : currentFovZoom, 20f * Time.deltaTime);
					GeneralWrappers.GetPlayerCamera().fieldOfView = fieldOfView;
					GeneralWrappers.GetUICamera().fieldOfView = fieldOfView;
				}
				else if (cameraSetup == 3)
				{
					cameraFreeze.transform.position = freezeCameraTargetPosition;
					cameraFreeze.transform.rotation = freezeCameraTargetRotation;
				}
			}
		}

		internal override void OnLevelWasInitialized(int level)
		{
			if (level != 0 && level != 1)
			{
				worldFullyLoaded = false;
				MelonCoroutines.Start(DoWorldFullyLoadedCountdown());
				ChangeCameraClipping(Configuration.GetGeneralConfig().MinimumCameraClippingDistance);
			}
		}

		private IEnumerator DoWorldFullyLoadedCountdown()
		{
			yield return new WaitForSecondsRealtime(15f);
			worldFullyLoaded = true;
		}

		internal static void ChangeCameraClipping(bool nearClipping)
		{
			clippingStateQueue.Enqueue(nearClipping);
		}

		internal static void ChangeCameraState()
		{
			if (cameraSetup < 2)
			{
				cameraSetup++;
				zoomOffset = 0f;
				cameraBack.transform.position -= cameraBack.transform.forward * zoomOffset;
				cameraFront.transform.position += cameraBack.transform.forward * zoomOffset;
			}
			else
			{
				cameraSetup = 0;
				zoomOffset = 0f;
				cameraBack.transform.position -= cameraBack.transform.forward * zoomOffset;
				cameraFront.transform.position += cameraBack.transform.forward * zoomOffset;
			}
			OnCameraStateChanged();
		}

		internal static void UseFreezeCamera()
		{
			if (cameraSetup == 3)
			{
				cameraSetup = 0;
			}
			else
			{
				freezeCameraTargetPosition = GeneralWrappers.GetPlayerCamera().transform.position;
				freezeCameraTargetRotation = GeneralWrappers.GetPlayerCamera().transform.rotation;
				cameraSetup = 3;
			}
			OnCameraStateChanged();
		}

		private static void OnCameraStateChanged()
		{
			if (cameraSetup == 0)
			{
				cameraFreeze.enabled = false;
				cameraBack.enabled = false;
				cameraFront.enabled = false;
				GeneralWrappers.GetPlayerCamera().enabled = true;
			}
			else if (cameraSetup == 1)
			{
				cameraFreeze.enabled = false;
				cameraBack.enabled = true;
				cameraFront.enabled = false;
				GeneralWrappers.GetPlayerCamera().enabled = false;
			}
			else if (cameraSetup == 2)
			{
				cameraFreeze.enabled = false;
				cameraBack.enabled = false;
				cameraFront.enabled = true;
				GeneralWrappers.GetPlayerCamera().enabled = false;
			}
			else if (cameraSetup == 3)
			{
				cameraFreeze.transform.rotation = GeneralWrappers.GetPlayerCamera().transform.rotation;
				cameraFreeze.transform.position = GeneralWrappers.GetPlayerCamera().transform.position;
				cameraFreeze.enabled = true;
				cameraBack.enabled = false;
				cameraFront.enabled = false;
				GeneralWrappers.GetPlayerCamera().enabled = false;
			}
			GeneralUtils.SetNameplateWallhack(Configuration.GetGeneralConfig().NameplateWallhack && cameraSetup == 0);
		}

		internal static void ApplyCameraSmoothZoom(bool incremental, float zoom)
		{
			if (cameraSetup == 0)
			{
				if (!incremental)
				{
					currentFovZoom = zoom;
				}
				else
				{
					currentFovZoom -= zoom;
				}
			}
		}

		internal static void ApplyThirdpersonSmoothZoom(bool forward)
		{
			if (cameraSetup == 1 || cameraSetup == 2)
			{
				if (forward)
				{
					cameraBack.transform.position += cameraBack.transform.forward * 0.1f;
					cameraFront.transform.position -= cameraBack.transform.forward * 0.1f;
					zoomOffset += 0.1f;
				}
				else
				{
					cameraBack.transform.position -= cameraBack.transform.forward * 0.1f;
					cameraFront.transform.position += cameraBack.transform.forward * 0.1f;
					zoomOffset -= 0.1f;
				}
			}
		}

		internal static void ChangeCameraActualZoomState(bool zoom)
		{
			if (cameraSetup == 0)
			{
				altZoom = zoom;
				GeneralWrappers.GetReticle().SetActive(!zoom);
			}
		}

		internal static Camera GetActiveCamera()
		{
			if (cameraFront.enabled)
			{
				return cameraFront;
			}
			if (cameraBack.enabled)
			{
				return cameraBack;
			}
			if (cameraFreeze.enabled)
			{
				return cameraFreeze;
			}
			return GeneralWrappers.GetPlayerCamera();
		}

		internal static Camera GetFrontCamera()
		{
			return cameraFront;
		}

		internal static Camera GetBackCamera()
		{
			return cameraBack;
		}

		internal static Camera GetFreezeCamera()
		{
			return cameraFreeze;
		}

		internal static int GetCameraSetup()
		{
			return cameraSetup;
		}
	}
}
