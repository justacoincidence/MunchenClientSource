using System.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Menu.Player;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC;
using VRC.Core;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class FlashlightHandler : ModuleComponent
	{
		private static VRCPlayer cachedPlayerHandler;

		private static GameObject cachedPlayerAvatar;

		private static FlashlightStruct cachedFlashlight;

		private static readonly Dictionary<HumanBodyBones, FlashlightOffsets> flashlightOffsets = new Dictionary<HumanBodyBones, FlashlightOffsets>
		{
			{
				HumanBodyBones.Head,
				new FlashlightOffsets
				{
					positionOffset = Vector3.zero,
					rotationOffset = Vector3.zero
				}
			},
			{
				HumanBodyBones.Chest,
				new FlashlightOffsets
				{
					positionOffset = Vector3.zero,
					rotationOffset = Vector3.zero
				}
			},
			{
				HumanBodyBones.LeftHand,
				new FlashlightOffsets
				{
					positionOffset = Vector3.zero,
					rotationOffset = new Vector3(270f, 0f, 0f)
				}
			},
			{
				HumanBodyBones.RightHand,
				new FlashlightOffsets
				{
					positionOffset = Vector3.zero,
					rotationOffset = new Vector3(270f, 0f, 0f)
				}
			},
			{
				HumanBodyBones.LeftFoot,
				new FlashlightOffsets
				{
					positionOffset = new Vector3(0f, 0.15f, 0f),
					rotationOffset = new Vector3(300f, 180f, 140f)
				}
			},
			{
				HumanBodyBones.RightFoot,
				new FlashlightOffsets
				{
					positionOffset = new Vector3(0f, 0.15f, 0f),
					rotationOffset = new Vector3(300f, 200f, 140f)
				}
			}
		};

		protected override string moduleName => "Flashlight Handler";

		internal override void OnUpdate()
		{
			if (FlashlightMenu.flashlightMenuInitialized)
			{
				FlashlightMenu.flashlightAttachedBone.SetMainText(Configuration.GetGeneralConfig().FlashlightAttachedBone.ToString());
				FlashlightMenu.flashlightSpotAngle.SetMainText(Configuration.GetGeneralConfig().FlashlightSpotAngle.ToString());
				FlashlightMenu.flashlightRange.SetMainText(Configuration.GetGeneralConfig().FlashlightRange.ToString());
				FlashlightMenu.flashlightIntensity.SetMainText(Configuration.GetGeneralConfig().FlashlightIntensity.ToString());
			}
		}

		internal override void OnAvatarLoaded(string playerId, string playerName, ref GameObject avatar)
		{
			Player playerByName = PlayerWrappers.GetPlayerByName(playerName);
			if ((cachedPlayerHandler == null || cachedPlayerAvatar == null) && playerByName.prop_APIUser_0.id == APIUser.CurrentUser?.id)
			{
				cachedPlayerHandler = playerByName.prop_VRCPlayer_0;
				cachedPlayerAvatar = avatar;
			}
			if (!(playerByName.prop_VRCPlayer_0 != cachedPlayerHandler))
			{
				SetupFlashlightBone();
			}
		}

		internal static void SetupFlashlightBone()
		{
			if (cachedFlashlight != null)
			{
				Object.DestroyImmediate(cachedFlashlight.flashlightObject);
			}
			Animator componentInChildren = cachedPlayerAvatar.GetComponentInChildren<Animator>();
			FlashlightOffsets flashlightOffsets = FlashlightHandler.flashlightOffsets[Configuration.GetGeneralConfig().FlashlightAttachedBone];
			cachedFlashlight = new FlashlightStruct
			{
				flashlightObject = new GameObject("Munchen Flashlight")
			};
			cachedFlashlight.flashlightComponent = cachedFlashlight.flashlightObject.AddComponent<Light>();
			ApplyFlashlightValues();
			ApplyFlashlightVisibilityState();
			if (componentInChildren != null)
			{
				Transform boneTransform = componentInChildren.GetBoneTransform(Configuration.GetGeneralConfig().FlashlightAttachedBone);
				if (boneTransform != null)
				{
					cachedFlashlight.flashlightObject.transform.parent = boneTransform;
					cachedFlashlight.flashlightObject.transform.position = boneTransform.position + flashlightOffsets.positionOffset;
					cachedFlashlight.flashlightObject.transform.rotation = boneTransform.rotation;
				}
				else
				{
					cachedFlashlight.flashlightObject.transform.parent = cachedPlayerAvatar.transform;
					cachedFlashlight.flashlightObject.transform.position = cachedPlayerAvatar.transform.position + flashlightOffsets.positionOffset;
					cachedFlashlight.flashlightObject.transform.rotation = cachedPlayerAvatar.transform.rotation;
				}
			}
			else
			{
				cachedFlashlight.flashlightObject.transform.parent = cachedPlayerAvatar.transform;
				cachedFlashlight.flashlightObject.transform.position = cachedPlayerAvatar.transform.position + flashlightOffsets.positionOffset;
				cachedFlashlight.flashlightObject.transform.rotation = cachedPlayerAvatar.transform.rotation;
			}
			cachedFlashlight.flashlightObject.transform.Rotate(flashlightOffsets.rotationOffset, Space.Self);
		}

		internal static void ApplyFlashlightValues()
		{
			cachedFlashlight.flashlightComponent.type = LightType.Spot;
			cachedFlashlight.flashlightComponent.spotAngle = MathUtils.Clamp(Configuration.GetGeneralConfig().FlashlightSpotAngle, 1, 179);
			cachedFlashlight.flashlightComponent.range = Configuration.GetGeneralConfig().FlashlightRange;
			cachedFlashlight.flashlightComponent.intensity = Configuration.GetGeneralConfig().FlashlightIntensity;
			cachedFlashlight.flashlightComponent.bounceIntensity = 0f;
		}

		internal static void ApplyFlashlightVisibilityState()
		{
			cachedFlashlight.flashlightObject.SetActive(Configuration.GetGeneralConfig().FlashlightActive);
		}
	}
}
