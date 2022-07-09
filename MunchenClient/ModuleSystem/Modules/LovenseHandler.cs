using System;
using System.Collections.Generic;
using MunchenClient.Menu.Player;
using MunchenClient.Misc;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class LovenseHandler : ModuleComponent
	{
		private readonly List<LovenseBoneType> lovenseBones = new List<LovenseBoneType>
		{
			new LovenseBoneType
			{
				lovenseBoneName = "Mouth",
				lovenseBoneType = LovenseType.Feel,
				boneType = "Head",
				boneTypeFallback = HumanBodyBones.Head,
				boneOffset = new Vector3(0.055f, 0.04f, 0f),
				boneSize = new Vector3(0.05f, 0.05f, 0.05f)
			},
			new LovenseBoneType
			{
				lovenseBoneName = "Pussy",
				lovenseBoneType = LovenseType.Feel,
				boneType = "Hips",
				boneTypeFallback = HumanBodyBones.Hips,
				boneOffset = new Vector3(0f, -0.125f, 0f),
				boneSize = new Vector3(0.1f, 0.1f, 0.1f)
			},
			new LovenseBoneType
			{
				lovenseBoneName = "Dick",
				lovenseBoneType = LovenseType.Feel,
				boneType = "Dick",
				boneTypeFallback = HumanBodyBones.Jaw,
				boneOffset = new Vector3(0f, 0f, 0f),
				boneSize = new Vector3(0.075f, 0.075f, 0.075f)
			},
			new LovenseBoneType
			{
				lovenseBoneName = "Right Finger",
				lovenseBoneType = LovenseType.Touch,
				boneType = "Right Finger",
				boneTypeFallback = HumanBodyBones.RightMiddleDistal,
				boneOffset = Vector3.zero,
				boneSize = new Vector3(0.1f, 0.1f, 0.1f)
			},
			new LovenseBoneType
			{
				lovenseBoneName = "Left Finger",
				lovenseBoneType = LovenseType.Touch,
				boneType = "Left Finger",
				boneTypeFallback = HumanBodyBones.LeftMiddleDistal,
				boneOffset = Vector3.zero,
				boneSize = new Vector3(0.1f, 0.1f, 0.1f)
			},
			new LovenseBoneType
			{
				lovenseBoneName = "Left Foot",
				lovenseBoneType = LovenseType.Touch,
				boneType = "Left Foot",
				boneTypeFallback = HumanBodyBones.LeftFoot,
				boneOffset = Vector3.zero,
				boneSize = new Vector3(0.1f, 0.1f, 0.1f)
			},
			new LovenseBoneType
			{
				lovenseBoneName = "Right Foot",
				lovenseBoneType = LovenseType.Touch,
				boneType = "Right Foot",
				boneTypeFallback = HumanBodyBones.RightFoot,
				boneOffset = Vector3.zero,
				boneSize = new Vector3(0.1f, 0.1f, 0.1f)
			}
		};

		private static readonly List<MunchenLovenseBoneSystem> currentlyCollidingBones = new List<MunchenLovenseBoneSystem>();

		private static byte lovenseIntensity = 0;

		private static float lovenseLastDistance = 0f;

		private static bool lovenseSettingIntensity = false;

		internal static bool lovenseDebug = false;

		internal static readonly float Penetrator = 0.09f;

		internal static readonly float PenetratorZawooCompat = 0.06f;

		internal static readonly float Oriface = 0.05f;

		internal static readonly float OrifaceAlt = 0.01f;

		internal static readonly float OrifaceSecondary = 0.02f;

		protected override string moduleName => "Lovense Handler";

		internal override void OnApplicationStart()
		{
			ClassInjector.RegisterTypeInIl2Cpp<MunchenLovenseBoneSystem>();
		}

		internal override void OnUpdate()
		{
			if (!LovenseConnectAPI.IsLovenseConnected() || currentlyCollidingBones.Count == 0)
			{
				return;
			}
			float num = 0f;
			foreach (MunchenLovenseBoneSystem currentlyCollidingBone in currentlyCollidingBones)
			{
				float num2 = Vector3.Distance(currentlyCollidingBone.lastCalculatedPosition, currentlyCollidingBone.transform.position);
				currentlyCollidingBone.lastCalculatedPosition = currentlyCollidingBone.transform.position;
				if (num2 > num)
				{
					num = num2;
				}
			}
			float num3 = num * 20f;
			float num4 = Mathf.Lerp(lovenseLastDistance, num3, Time.deltaTime * 2f);
			byte b = MathUtils.Clamp((byte)num4, (byte)0, (byte)20);
			ConsoleUtils.FlushToConsole("Lovense", "---------------------------------", ConsoleColor.Gray, "OnUpdate", 202);
			ConsoleUtils.FlushToConsole("Lovense", $"Current: {lovenseLastDistance}", ConsoleColor.Gray, "OnUpdate", 203);
			ConsoleUtils.FlushToConsole("Lovense", $"Target: {num3}", ConsoleColor.Gray, "OnUpdate", 204);
			ConsoleUtils.FlushToConsole("Lovense", $"Smoothed: {num4}", ConsoleColor.Gray, "OnUpdate", 205);
			ConsoleUtils.FlushToConsole("Lovense", $"Old Intensity: {lovenseIntensity}", ConsoleColor.Gray, "OnUpdate", 206);
			ConsoleUtils.FlushToConsole("Lovense", $"New Intensity: {b}", ConsoleColor.Gray, "OnUpdate", 207);
			if (lovenseIntensity != b && !lovenseSettingIntensity)
			{
				lovenseSettingIntensity = true;
				ConsoleUtils.FlushToConsole("Lovense", $"Applying new intensity: {b} (Old: {lovenseIntensity})", ConsoleColor.Gray, "OnUpdate", 213);
				LovenseConnectAPI.VibrateLovense(b, OnIntensityChangeDone);
				lovenseLastDistance = num4;
			}
		}

		internal override void OnAvatarLoaded(string playerId, string playerName, ref GameObject avatar)
		{
			if (!LovenseConnectAPI.IsLovenseConnected() && !lovenseDebug)
			{
				return;
			}
			Player playerByName = PlayerWrappers.GetPlayerByName(playerName);
			if (playerByName == null)
			{
				ConsoleUtils.Info("Lovense", playerName + " doesn't have a valid player script", ConsoleColor.Gray, "OnAvatarLoaded", 231);
				return;
			}
			Animator componentInChildren = avatar.GetComponentInChildren<Animator>();
			if (componentInChildren == null)
			{
				if (lovenseDebug)
				{
					ConsoleUtils.Info("Lovense", playerName + " doesn't have an animator", ConsoleColor.Gray, "OnAvatarLoaded", 254);
				}
				return;
			}
			foreach (LovenseBoneType lovenseBone in lovenseBones)
			{
				Transform boneTransform = componentInChildren.GetBoneTransform(lovenseBone.boneTypeFallback);
				if (boneTransform != null)
				{
					if (CheckTouchZone(boneTransform, out var _))
					{
						SetupLovenseBone(playerByName, avatar.transform.localScale.y, lovenseBone, boneTransform);
						continue;
					}
					if (CheckDPS(boneTransform, out var bone2) != 0)
					{
						if (bone2 == null)
						{
							SetupLovenseBone(playerByName, avatar.transform.localScale.y, lovenseBone, boneTransform);
						}
						else
						{
							SetupLovenseBone(playerByName, lovenseBone, bone2);
						}
						continue;
					}
					if (CheckRealFeel(boneTransform, out var _))
					{
					}
					ConsoleUtils.Info("Lovense", "No custom systems found at " + boneTransform.name, ConsoleColor.Gray, "OnAvatarLoaded", 298);
					List<Transform> list = MiscUtils.FindAllComponentsInGameObject<Transform>(avatar, includeInactive: true, searchParent: false);
					foreach (Transform item in list)
					{
						string text = item.name.ToLower();
						string value = lovenseBone.boneType.ToLower();
						if (text.Contains(value))
						{
							ConsoleUtils.Info("Lovense", "Used fallback system on " + boneTransform.name, ConsoleColor.Gray, "OnAvatarLoaded", 308);
							SetupLovenseBone(playerByName, avatar.transform.localScale.y, lovenseBone, boneTransform);
						}
					}
					SetupLovenseBone(playerByName, avatar.transform.localScale.y, lovenseBone, boneTransform);
				}
				else if (lovenseDebug)
				{
					ConsoleUtils.Info("Lovense", $"{lovenseBone.boneTypeFallback} doesn't exist - skipping setting up bone", ConsoleColor.Gray, "OnAvatarLoaded", 322);
				}
			}
		}

		private void SetupLovenseBone(Player parentPlayer, float avatarHeight, LovenseBoneType boneInformation, Transform parentBone)
		{
			Vector3 vector = boneInformation.boneOffset * (avatarHeight / 2f);
			Vector3 localScale = boneInformation.boneSize * avatarHeight;
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.name = "MunchenClient Lovense (" + boneInformation.lovenseBoneName + ")";
			gameObject.transform.localScale = localScale;
			gameObject.transform.position = parentBone.position + vector;
			gameObject.transform.parent = parentBone;
			gameObject.GetComponent<Collider>().isTrigger = true;
			if (!lovenseDebug)
			{
				UnityEngine.Object.Destroy(gameObject.GetComponent<MeshRenderer>());
				UnityEngine.Object.Destroy(gameObject.GetComponent<MeshFilter>());
			}
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			MunchenLovenseBoneSystem munchenLovenseBoneSystem = gameObject.AddComponent<MunchenLovenseBoneSystem>();
			munchenLovenseBoneSystem.owner = parentPlayer;
			munchenLovenseBoneSystem.boneInformation = boneInformation;
			if (lovenseDebug)
			{
				ConsoleUtils.Info("Lovense", "Setup " + boneInformation.lovenseBoneName + " on " + parentPlayer.field_Private_VRCPlayerApi_0.displayName, ConsoleColor.Gray, "SetupLovenseBone", 364);
			}
		}

		private void SetupLovenseBone(Player parentPlayer, LovenseBoneType boneInformation, GameObject bone)
		{
			BoxCollider boxCollider = bone.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			Rigidbody rigidbody = bone.AddComponent<Rigidbody>();
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			MunchenLovenseBoneSystem munchenLovenseBoneSystem = bone.AddComponent<MunchenLovenseBoneSystem>();
			munchenLovenseBoneSystem.owner = parentPlayer;
			munchenLovenseBoneSystem.boneInformation = boneInformation;
			if (lovenseDebug)
			{
				ConsoleUtils.Info("Lovense", "Setup " + boneInformation.lovenseBoneName + " on " + parentPlayer.field_Private_VRCPlayerApi_0.displayName, ConsoleColor.Gray, "SetupLovenseBone", 386);
			}
		}

		internal static void RegisterCollidingBone(MunchenLovenseBoneSystem bone, MunchenLovenseBoneSystem remoteBone)
		{
			if (!currentlyCollidingBones.Contains(bone))
			{
				ConsoleUtils.FlushToConsole("Lovense", remoteBone.owner.prop_VRCPlayerApi_0.displayName + " started touching your " + bone.boneInformation.lovenseBoneName + " with their " + remoteBone.boneInformation.lovenseBoneName, ConsoleColor.Gray, "RegisterCollidingBone", 397);
				currentlyCollidingBones.Add(bone);
			}
		}

		internal static void UnregisterCollidingBone(MunchenLovenseBoneSystem bone, MunchenLovenseBoneSystem remoteBone)
		{
			if (currentlyCollidingBones.Contains(bone))
			{
				ConsoleUtils.FlushToConsole("Lovense", remoteBone.owner.prop_VRCPlayerApi_0.displayName + " stopped touching your " + bone.boneInformation.lovenseBoneName + " with their " + remoteBone.boneInformation.lovenseBoneName, ConsoleColor.Gray, "UnregisterCollidingBone", 409);
				currentlyCollidingBones.Remove(bone);
				if (currentlyCollidingBones.Count == 0 && !lovenseSettingIntensity)
				{
					lovenseSettingIntensity = true;
					ConsoleUtils.FlushToConsole("Lovense", "No more bones colliding - stopping vibrations", ConsoleColor.Gray, "UnregisterCollidingBone", 417);
					LovenseConnectAPI.VibrateLovense(0, OnIntensityChangeDone);
				}
			}
		}

		internal static void OnIntensityChangeDone(byte intensity, bool success, string response)
		{
			if (success)
			{
				if (currentlyCollidingBones.Count == 0 && intensity != 0)
				{
					ConsoleUtils.FlushToConsole("Lovense", $"Vibration stopping with: {intensity}", ConsoleColor.Gray, "OnIntensityChangeDone", 429);
					LovenseConnectAPI.VibrateLovense(0, OnIntensityChangeDone);
					return;
				}
				LovenseMenu.currentIntensityIndicator.SetMainText($"{intensity * 5}%");
				lovenseSettingIntensity = false;
				lovenseIntensity = intensity;
				ConsoleUtils.FlushToConsole("Lovense", $"Vibration success with: {intensity}", ConsoleColor.Gray, "OnIntensityChangeDone", 442);
			}
			else
			{
				ConsoleUtils.FlushToConsole("Lovense", $"Vibration failed with: {intensity}", ConsoleColor.Gray, "OnIntensityChangeDone", 447);
				LovenseConnectAPI.VibrateLovense(intensity, OnIntensityChangeDone);
			}
		}

		private DPSType CheckDPS(Transform parentBone, out GameObject bone)
		{
			Light[] array = parentBone.GetComponentsInChildren<Light>(includeInactive: true);
			Light light = null;
			Light[] array2 = array;
			foreach (Light light2 in array2)
			{
				if (MiscUtils.GetChildDepth(light2.transform, parentBone) <= 3)
				{
					light = light2;
					break;
				}
			}
			if (light == null)
			{
				bone = null;
				return DPSType.None;
			}
			if (IsDPSLight(light, Penetrator) || IsDPSLight(light, PenetratorZawooCompat))
			{
				bone = light.transform.parent.GetComponentInChildren<MeshRenderer>()?.gameObject;
				return DPSType.Penetrator;
			}
			if (IsDPSLight(light, Oriface) || IsDPSLight(light, OrifaceAlt) || IsDPSLight(light, OrifaceSecondary))
			{
				bone = null;
				return DPSType.Oriface;
			}
			bone = null;
			return DPSType.None;
		}

		private static bool IsDPSLight(Light light, float id)
		{
			return light.color.maxColorComponent < 0.01f && Mathf.Abs(light.range % 0.1f - id) < 0.001f;
		}

		private bool CheckTouchZone(Transform parentBone, out Transform bone)
		{
			Camera componentInChildren = parentBone.GetComponentInChildren<Camera>(includeInactive: true);
			if (componentInChildren == null)
			{
				bone = null;
				return false;
			}
			if (IsTouchZone(componentInChildren))
			{
				bone = componentInChildren.transform;
				return true;
			}
			bone = null;
			return false;
		}

		private static bool IsTouchZone(Camera touchZone)
		{
			return touchZone.name.ToLower().Contains("touchzone");
		}

		private bool CheckRealFeel(Transform parentBone, out GameObject bone)
		{
			MeshRenderer[] array = parentBone.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
			MeshRenderer meshRenderer = null;
			MeshRenderer[] array2 = array;
			foreach (MeshRenderer meshRenderer2 in array2)
			{
				string text = meshRenderer2.material.shader.name.ToLower();
				string text2 = meshRenderer2.material.name.ToLower();
				ConsoleUtils.Info("RealFeel", "Shader Name: " + text + " | Material Name: " + text2, ConsoleColor.Gray, "CheckRealFeel", 548);
				if (text.Contains("realfeel"))
				{
					meshRenderer = meshRenderer2;
					break;
				}
			}
			if (meshRenderer == null)
			{
				bone = null;
				return false;
			}
			ConsoleUtils.Info("RealFeel", "Found RealFeel at " + meshRenderer.name + " on bone " + parentBone.name, ConsoleColor.Gray, "CheckRealFeel", 563);
			bone = null;
			return false;
		}
	}
}
