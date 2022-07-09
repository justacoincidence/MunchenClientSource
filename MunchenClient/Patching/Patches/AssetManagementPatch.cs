using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Core;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRCSDK2;

namespace MunchenClient.Patching.Patches
{
	internal class AssetManagementPatch : PatchComponent
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr ObjectInstantiateDelegate(IntPtr assetPtr, Vector3 pos, Quaternion rot, byte allowCustomShaders, byte isUI, byte validate, IntPtr nativeMethodPointer);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void VoidDelegate(IntPtr thisPtr, IntPtr nativeMethodInfo);

		private static readonly List<IntPtr> previouslyCheckedAvatars = new List<IntPtr>();

		internal static VRCAvatarManager currentlyLoadingAvatar;

		protected override string patchName => "AssetMangementPatch";

		internal unsafe override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(AssetManagementPatch));
			List<MethodInfo> list = (from it in typeof(AssetManagement).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
				where it.Name.StartsWith("Method_Public_Static_Object_Object_Vector3_Quaternion_Boolean_Boolean_Boolean_") && it.GetParameters().Length == 6
				select it).ToList();
			foreach (MethodInfo item in list)
			{
				IntPtr ptr = *(IntPtr*)(void*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(item).GetValue(null);
				ObjectInstantiateDelegate originalInstantiateDelegate = null;
				ObjectInstantiateDelegate objectInstantiateDelegate = (IntPtr assetPtr, Vector3 pos, Quaternion rot, byte allowCustomShaders, byte isUI, byte validate, IntPtr nativeMethodPointer) => OnObjectInstantiatedPatch(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer, originalInstantiateDelegate);
				MainUtils.antiGCList.Add(objectInstantiateDelegate);
				MelonUtils.NativeHookAttach((IntPtr)(&ptr), Marshal.GetFunctionPointerForDelegate(objectInstantiateDelegate));
				originalInstantiateDelegate = Marshal.GetDelegateForFunctionPointer<ObjectInstantiateDelegate>(ptr);
			}
			Type[] nestedTypes = typeof(VRCAvatarManager).GetNestedTypes();
			foreach (Type type in nestedTypes)
			{
				MethodInfo method = type.GetMethod("MoveNext");
				int fieldOffset;
				VoidDelegate originalDelegate;
				if (!(method == null))
				{
					PropertyInfo propertyInfo = type.GetProperties().SingleOrDefault((PropertyInfo it) => it.PropertyType == typeof(VRCAvatarManager));
					if (!(propertyInfo == null))
					{
						fieldOffset = (int)IL2CPP.il2cpp_field_get_offset((IntPtr)UnhollowerUtils.GetIl2CppFieldInfoPointerFieldForGeneratedFieldAccessor(propertyInfo.GetMethod).GetValue(null));
						IntPtr intPtr = *(IntPtr*)(void*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(method).GetValue(null);
						intPtr = XrefScannerLowLevel.JumpTargets(intPtr).First();
						originalDelegate = null;
						VoidDelegate voidDelegate = TaskMoveNextPatch;
						MainUtils.antiGCList.Add(voidDelegate);
						MelonUtils.NativeHookAttach((IntPtr)(&intPtr), Marshal.GetFunctionPointerForDelegate(voidDelegate));
						originalDelegate = Marshal.GetDelegateForFunctionPointer<VoidDelegate>(intPtr);
					}
				}
				unsafe void TaskMoveNextPatch(IntPtr taskPtr, IntPtr nativeMethodInfo)
				{
					IntPtr intPtr2 = *(IntPtr*)(void*)(taskPtr + fieldOffset - 16);
					currentlyLoadingAvatar = new VRCAvatarManager(intPtr2);
					originalDelegate(taskPtr, nativeMethodInfo);
					currentlyLoadingAvatar = null;
				}
			}
		}

		internal static void OnRoomLeft()
		{
			previouslyCheckedAvatars.Clear();
		}

		private static IntPtr OnObjectInstantiatedPatch(IntPtr assetPtr, Vector3 pos, Quaternion rot, byte allowCustomShaders, byte isUI, byte validate, IntPtr nativeMethodPointer, ObjectInstantiateDelegate originalInstantiateDelegate)
		{
			if (WorldUtils.GetCurrentInstance() == null)
			{
				return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			}
			InstanceAccessType type = WorldUtils.GetCurrentInstance().type;
			if (1 == 0)
			{
			}
			bool flag = type switch
			{
				InstanceAccessType.Public => Configuration.GetAntiCrashConfig().AntiCrashEnablePublic, 
				InstanceAccessType.InviteOnly => Configuration.GetAntiCrashConfig().AntiCrashEnableInvite, 
				InstanceAccessType.InvitePlus => Configuration.GetAntiCrashConfig().AntiCrashEnableInvitePlus, 
				InstanceAccessType.FriendsOnly => Configuration.GetAntiCrashConfig().AntiCrashEnableFriends, 
				InstanceAccessType.FriendsOfGuests => Configuration.GetAntiCrashConfig().AntiCrashEnableFriendsPlus, 
				_ => false, 
			};
			if (1 == 0)
			{
			}
			if (!flag)
			{
				return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			}
			if (assetPtr == IntPtr.Zero)
			{
				return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			}
			GameObject gameObject = new UnityEngine.Object(assetPtr).TryCast<GameObject>();
			if (gameObject == null)
			{
				return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			}
			if (gameObject.name.StartsWith("UserUi") || gameObject.name.StartsWith("WorldUi") || gameObject.name.StartsWith("AvatarUi") || gameObject.name.StartsWith("Holoport"))
			{
				return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			}
			bool flag2 = gameObject.name.StartsWith("prefab");
			if (!gameObject.name.StartsWith("_CustomAvatar") && !gameObject.name.Equals("Avatar") && !gameObject.name.Equals("AvatarPrefab") && !flag2)
			{
				return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			}
			string newValue = "None";
			string text = "Unknown";
			string newValue2 = "Unknown";
			string newValue3 = "Unknown";
			if (currentlyLoadingAvatar != null)
			{
				newValue = currentlyLoadingAvatar.field_Private_VRCPlayer_0?.prop_Player_0?.prop_APIUser_0?.displayName ?? "None";
				text = currentlyLoadingAvatar.field_Private_ApiAvatar_0?.id ?? "Unknown";
				newValue2 = currentlyLoadingAvatar.field_Private_ApiAvatar_0?.name ?? "Unknown";
				newValue3 = currentlyLoadingAvatar.field_Private_ApiAvatar_0?.assetUrl ?? "Unknown";
				if (Configuration.GetAntiCrashConfig().WhitelistedAvatars.ContainsKey(text))
				{
					if (!previouslyCheckedAvatars.Contains(gameObject.Pointer))
					{
						previouslyCheckedAvatars.Add(gameObject.Pointer);
					}
					return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
				}
			}
			else if (flag2)
			{
				int num = gameObject.name.IndexOf('_') + 1;
				int num2 = gameObject.name.LastIndexOf('_');
				text = gameObject.name.Substring(num, num2 - num);
				if (Configuration.GetAntiCrashConfig().WhitelistedAvatars.ContainsKey(text))
				{
					if (!previouslyCheckedAvatars.Contains(gameObject.Pointer))
					{
						previouslyCheckedAvatars.Add(gameObject.Pointer);
					}
					return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
				}
				if (previouslyCheckedAvatars.Contains(gameObject.Pointer))
				{
					return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
				}
			}
			if (Configuration.GetAntiCrashConfig().WhitelistedAvatars.ContainsKey(text))
			{
				return originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			}
			bool activeSelf = gameObject.activeSelf;
			gameObject.SetActive(value: false);
			IntPtr intPtr = originalInstantiateDelegate(assetPtr, pos, rot, allowCustomShaders, isUI, validate, nativeMethodPointer);
			gameObject.SetActive(activeSelf);
			if (intPtr == IntPtr.Zero)
			{
				return intPtr;
			}
			GameObject gameObject2 = new GameObject(intPtr);
			if (gameObject2 == null)
			{
				return intPtr;
			}
			PerformanceProfiler.StartProfiling("OnObjectInstantiated");
			List<Component> list = MiscUtils.FindAllComponentsInGameObject<Component>(gameObject2);
			if (Configuration.GetAntiCrashConfig().MaxHeight > -1f && gameObject2.transform.lossyScale.y > Configuration.GetAntiCrashConfig().MaxHeight)
			{
				if (Configuration.GetAntiCrashConfig().VerboseLogging)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, "---------------------------------", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 241);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatar.Replace("{username}", newValue).Replace("{processingtime}", string.Format("{0:F2}", PerformanceProfiler.GetProfiling("OnObjectInstantiated"))), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 242);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarID.Replace("{id}", text), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 243);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarName.Replace("{name}", newValue2), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 244);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarDownload.Replace("{link}", newValue3), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 245);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Deleted for being too big ({gameObject2.transform.lossyScale.y})", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 246);
				}
				AntiCrashUtils.DisposeAvatar(gameObject2, list);
				list.Clear();
				return intPtr;
			}
			if (Configuration.GetAntiCrashConfig().MaxComponentsTotal > -1 && list.Count > Configuration.GetAntiCrashConfig().MaxComponentsTotal)
			{
				if (Configuration.GetAntiCrashConfig().VerboseLogging)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, "---------------------------------", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 259);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatar.Replace("{username}", newValue).Replace("{processingtime}", string.Format("{0:F2}", PerformanceProfiler.GetProfiling("OnObjectInstantiated"))), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 260);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarID.Replace("{id}", text), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 261);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarName.Replace("{name}", newValue2), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 262);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarDownload.Replace("{link}", newValue3), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 263);
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Deleted for having too many components ({list.Count})", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 264);
				}
				AntiCrashUtils.DisposeAvatar(gameObject2, list);
				list.Clear();
				return intPtr;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int nukedColliders = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int nukedConstraints = 0;
			int nukedRigidbodies = 0;
			int currentTransforms = 0;
			int currentColliders = 0;
			int currentSpringJoints = 0;
			int num9 = 0;
			int currentRigidbodies = 0;
			int num10 = 0;
			int currentConstraints = 0;
			int num11 = 0;
			AntiCrashClothPostProcess antiCrashClothPostProcess = new AntiCrashClothPostProcess();
			AntiCrashParticleSystemPostProcess post = new AntiCrashParticleSystemPostProcess();
			AntiCrashDynamicBoneColliderPostProcess antiCrashDynamicBoneColliderPostProcess = new AntiCrashDynamicBoneColliderPostProcess();
			AntiCrashDynamicBonePostProcess antiCrashDynamicBonePostProcess = new AntiCrashDynamicBonePostProcess();
			AntiCrashLightSourcePostProcess antiCrashLightSourcePostProcess = new AntiCrashLightSourcePostProcess();
			AntiCrashRendererPostProcess previousProcess = new AntiCrashRendererPostProcess();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == null)
				{
					continue;
				}
				if (Configuration.GetAntiCrashConfig().MaxDepth > -1 && MiscUtils.GetChildDepth(list[i].transform, gameObject2.transform) > Configuration.GetAntiCrashConfig().MaxDepth)
				{
					num5++;
					UnityEngine.Object.DestroyImmediate(list[i].gameObject, allowDestroyingAssets: true);
					continue;
				}
				if (Configuration.GetAntiCrashConfig().MaxChildren > -1)
				{
					for (int j = Configuration.GetAntiCrashConfig().MaxChildren; j < list[i].transform.childCount; j++)
					{
						num5++;
						UnityEngine.Object.DestroyImmediate(list[i].transform.GetChild(j).gameObject, allowDestroyingAssets: true);
					}
				}
				MonoBehaviour monoBehaviour = list[i].TryCast<MonoBehaviour>();
				if (Configuration.GetAntiCrashConfig().MaxMonobehaviours > -1 && monoBehaviour != null)
				{
					num11++;
					if (num11 > Configuration.GetAntiCrashConfig().MaxMonobehaviours)
					{
						if (Configuration.GetAntiCrashConfig().VerboseLogging)
						{
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, "---------------------------------", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 337);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatar.Replace("{username}", newValue).Replace("{processingtime}", string.Format("{0:F2}", PerformanceProfiler.GetProfiling("OnObjectInstantiated"))), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 338);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarID.Replace("{id}", text), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 339);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarName.Replace("{name}", newValue2), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 340);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarDownload.Replace("{link}", newValue3), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 341);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Deleted for having too many monobehaviours ({num11})", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 342);
						}
						AntiCrashUtils.DisposeAvatar(gameObject2, list);
						list.Clear();
						break;
					}
				}
				Transform transform = list[i].TryCast<Transform>();
				Rigidbody rigidbody = list[i].TryCast<Rigidbody>();
				Collider collider = list[i].TryCast<Collider>();
				Joint joint = list[i].TryCast<Joint>();
				AudioSource audioSource = list[i].TryCast<AudioSource>();
				Cloth cloth = list[i].TryCast<Cloth>();
				ParticleSystem particleSystem = list[i].TryCast<ParticleSystem>();
				DynamicBoneCollider dynamicBoneCollider = list[i].TryCast<DynamicBoneCollider>();
				DynamicBone dynamicBone = list[i].TryCast<DynamicBone>();
				Light light = list[i].TryCast<Light>();
				Renderer renderer = list[i].TryCast<Renderer>();
				Animator animator = list[i].TryCast<Animator>();
				ParentConstraint parentConstraint = list[i].TryCast<ParentConstraint>();
				RotationConstraint rotationConstraint = list[i].TryCast<RotationConstraint>();
				PositionConstraint positionConstraint = list[i].TryCast<PositionConstraint>();
				ScaleConstraint scaleConstraint = list[i].TryCast<ScaleConstraint>();
				LookAtConstraint lookAtConstraint = list[i].TryCast<LookAtConstraint>();
				AimConstraint aimConstraint = list[i].TryCast<AimConstraint>();
				VRCAvatarDescriptor vRCAvatarDescriptor = list[i].TryCast<VRCAvatarDescriptor>();
				VRC_AvatarDescriptor vRC_AvatarDescriptor = list[i].TryCast<VRC_AvatarDescriptor>();
				if (transform != null && Configuration.GetAntiCrashConfig().AntiPhysicsCrash)
				{
					if (AntiCrashUtils.ProcessTransform(transform, ref currentTransforms))
					{
						if (Configuration.GetAntiCrashConfig().VerboseLogging)
						{
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, "---------------------------------", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 382);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatar.Replace("{username}", newValue).Replace("{processingtime}", string.Format("{0:F2}", PerformanceProfiler.GetProfiling("OnObjectInstantiated"))), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 383);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarID.Replace("{id}", text), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 384);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarName.Replace("{name}", newValue2), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 385);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarDownload.Replace("{link}", newValue3), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 386);
							ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Deleted for being suspected as malicious ({currentTransforms})", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 387);
						}
						AntiCrashUtils.DisposeAvatar(gameObject2, list);
						list.Clear();
						break;
					}
				}
				else if (rigidbody != null && Configuration.GetAntiCrashConfig().AntiPhysicsCrash)
				{
					AntiCrashUtils.ProcessRigidbody(rigidbody, ref currentRigidbodies, ref nukedRigidbodies);
				}
				else if (collider != null && Configuration.GetAntiCrashConfig().AntiPhysicsCrash)
				{
					AntiCrashUtils.ProcessCollider(collider, ref currentColliders, ref nukedColliders);
				}
				else if (joint != null && Configuration.GetAntiCrashConfig().AntiPhysicsCrash)
				{
					if (AntiCrashUtils.ProcessJoint(joint, ref currentSpringJoints))
					{
						num6++;
					}
				}
				else if (audioSource != null && Configuration.GetAntiCrashConfig().AntiAudioCrash)
				{
					if (num9 >= Configuration.GetAntiCrashConfig().MaxAudioSources)
					{
						num7++;
						UnityEngine.Object.DestroyImmediate(audioSource.gameObject, allowDestroyingAssets: true);
					}
					else
					{
						num9++;
					}
				}
				else if (cloth != null && Configuration.GetAntiCrashConfig().AntiClothCrash)
				{
					antiCrashClothPostProcess = AntiCrashUtils.ProcessCloth(cloth, antiCrashClothPostProcess);
				}
				else if (particleSystem != null && Configuration.GetAntiCrashConfig().AntiParticleSystemCrash)
				{
					AntiCrashUtils.ProcessParticleSystem(particleSystem, ref post);
				}
				else if (dynamicBoneCollider != null && Configuration.GetAntiCrashConfig().AntiDynamicBoneCrash)
				{
					antiCrashDynamicBoneColliderPostProcess = AntiCrashUtils.ProcessDynamicBoneCollider(dynamicBoneCollider, antiCrashDynamicBoneColliderPostProcess.nukedDynamicBoneColliders, antiCrashDynamicBoneColliderPostProcess.dynamicBoneColiderCount);
				}
				else if (dynamicBone != null && Configuration.GetAntiCrashConfig().AntiDynamicBoneCrash)
				{
					antiCrashDynamicBonePostProcess = AntiCrashUtils.ProcessDynamicBone(dynamicBone, antiCrashDynamicBonePostProcess.nukedDynamicBones, antiCrashDynamicBonePostProcess.dynamicBoneCount);
				}
				else if (light != null && Configuration.GetAntiCrashConfig().AntiLightSourceCrash)
				{
					antiCrashLightSourcePostProcess = AntiCrashUtils.ProcessLight(light, antiCrashLightSourcePostProcess.nukedLightSources, antiCrashLightSourcePostProcess.lightSourceCount);
				}
				else if (renderer != null)
				{
					AntiCrashUtils.ProcessRenderer(renderer, ref previousProcess);
				}
				else if (animator != null && Configuration.GetAntiCrashConfig().MaxAnimators > -1)
				{
					if (num10 >= Configuration.GetAntiCrashConfig().MaxAnimators)
					{
						num8++;
						UnityEngine.Object.DestroyImmediate(animator, allowDestroyingAssets: true);
					}
					else
					{
						num10++;
					}
				}
				else if (parentConstraint != null)
				{
					AntiCrashUtils.ProcessConstraint(parentConstraint, ref currentConstraints, ref nukedConstraints);
				}
				else if (rotationConstraint != null)
				{
					AntiCrashUtils.ProcessConstraint(rotationConstraint, ref currentConstraints, ref nukedConstraints);
				}
				else if (positionConstraint != null)
				{
					AntiCrashUtils.ProcessConstraint(positionConstraint, ref currentConstraints, ref nukedConstraints);
				}
				else if (scaleConstraint != null)
				{
					AntiCrashUtils.ProcessConstraint(scaleConstraint, ref currentConstraints, ref nukedConstraints);
				}
				else if (lookAtConstraint != null)
				{
					AntiCrashUtils.ProcessConstraint(lookAtConstraint, ref currentConstraints, ref nukedConstraints);
				}
				else if (aimConstraint != null)
				{
					AntiCrashUtils.ProcessConstraint(aimConstraint, ref currentConstraints, ref nukedConstraints);
				}
				else if (vRCAvatarDescriptor != null && Configuration.GetAntiCrashConfig().AntiAvatarDescriptorCrash)
				{
					if (!(vRCAvatarDescriptor.expressionsMenu != null))
					{
						continue;
					}
					for (int k = 0; k < vRCAvatarDescriptor.expressionsMenu.controls.Count; k++)
					{
						VRCExpressionsMenu.Control control = vRCAvatarDescriptor.expressionsMenu.controls[k];
						if (control.name.Length > 200)
						{
							control.name = control.name.Substring(0, 200);
							num3++;
						}
						if (control.parameter.name.Length > 200)
						{
							control.parameter.name = control.parameter.name.Substring(0, 200);
							num3++;
						}
					}
				}
				else
				{
					if (!(vRC_AvatarDescriptor != null) || !Configuration.GetAntiCrashConfig().AntiAvatarDescriptorCrash || !(vRC_AvatarDescriptor.CustomStandingAnims != null))
					{
						continue;
					}
					for (int l = 0; l < vRC_AvatarDescriptor.CustomStandingAnims.animationClips.Count; l++)
					{
						AnimationClip animationClip = vRC_AvatarDescriptor.CustomStandingAnims.animationClips[l];
						if (animationClip.name.Length > 200)
						{
							animationClip.name = animationClip.name.Substring(0, 200);
							num4++;
						}
					}
				}
			}
			PerformanceProfiler.EndProfiling("OnObjectInstantiated");
			if (Configuration.GetAntiCrashConfig().VerboseLogging && (num5 > 0 || nukedColliders > 0 || num6 > 0 || num7 > 0 || num8 > 0 || nukedRigidbodies > 0 || nukedConstraints > 0 || antiCrashClothPostProcess.nukedCloths > 0 || post.nukedParticleSystems > 0 || antiCrashDynamicBonePostProcess.nukedDynamicBones > 0 || antiCrashDynamicBoneColliderPostProcess.nukedDynamicBoneColliders > 0 || antiCrashLightSourcePostProcess.nukedLightSources > 0 || num3 > 0 || num4 > 0 || previousProcess.nukedMeshes > 0 || previousProcess.nukedMaterials > 0 || previousProcess.nukedShaders > 0 || previousProcess.removedBlendshapeKeys))
			{
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, "---------------------------------", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 560);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatar.Replace("{username}", newValue).Replace("{processingtime}", string.Format("{0:F2}", PerformanceProfiler.GetProfiling("OnObjectInstantiated"))), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 561);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarID.Replace("{id}", text), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 562);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarName.Replace("{name}", newValue2), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 563);
				ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, LanguageManager.GetUsedLanguage().AntiCrashCheckedAvatarDownload.Replace("{link}", newValue3), ConsoleColor.Blue, "OnObjectInstantiatedPatch", 564);
				if (num5 > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Transforms: {num5} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 568);
				}
				if (nukedColliders > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Colliders: {nukedColliders} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 573);
				}
				if (num6 > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"SpringJoints: {num6} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 578);
				}
				if (num7 > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"AudioSources: {num7} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 583);
				}
				if (num8 > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Animators: {num8} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 588);
				}
				if (nukedConstraints > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Constraints: {nukedConstraints} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 593);
				}
				if (antiCrashClothPostProcess.nukedCloths > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Cloth: {antiCrashClothPostProcess.nukedCloths} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 598);
				}
				if (post.nukedParticleSystems > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"ParticleSystems: {post.nukedParticleSystems} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 603);
				}
				if (antiCrashDynamicBonePostProcess.nukedDynamicBones > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"DynamicBones: {antiCrashDynamicBonePostProcess.nukedDynamicBones} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 608);
				}
				if (antiCrashDynamicBoneColliderPostProcess.nukedDynamicBoneColliders > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"DynamicBoneColliders: {antiCrashDynamicBoneColliderPostProcess.nukedDynamicBoneColliders} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 613);
				}
				if (antiCrashLightSourcePostProcess.nukedLightSources > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"LightSources: {antiCrashLightSourcePostProcess.nukedLightSources} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 618);
				}
				if (num3 > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Expression Menus: {num3} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 623);
				}
				if (num4 > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Animation Clips: {num4} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 628);
				}
				if (previousProcess.nukedMeshes > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Meshes: {previousProcess.nukedMeshes} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 633);
				}
				if (previousProcess.nukedMaterials > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Materials: {previousProcess.nukedMaterials} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 638);
				}
				if (previousProcess.nukedShaders > 0)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, $"Shaders: {previousProcess.nukedShaders} total", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 643);
				}
				if (previousProcess.removedBlendshapeKeys)
				{
					ConsoleUtils.Info(LanguageManager.GetUsedLanguage().AntiCrashMenuName, "Invalid Blendshape Keys", ConsoleColor.Blue, "OnObjectInstantiatedPatch", 648);
				}
			}
			return intPtr;
		}
	}
}
