using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MunchenClient.Config;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC.Core;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class GlobalDynamicBonesHandler : ModuleComponent
	{
		private static readonly Dictionary<string, AvatarParsingData> avatarsInScene = new Dictionary<string, AvatarParsingData>();

		private static readonly Dictionary<string, List<OriginalBoneSettings>> originalSettings = new Dictionary<string, List<OriginalBoneSettings>>();

		private static readonly Dictionary<string, AvatarRendererData> avatarRenderers = new Dictionary<string, AvatarRendererData>();

		private static GameObject localPlayer = null;

		private static readonly float nextUpdateVisibility = 0.25f;

		private static float nextUpdateVisibilityCurrent = 0f;

		protected override string moduleName => "Global Dynamic Bones Handler";

		internal override void OnUpdate()
		{
			if (!(nextUpdateVisibilityCurrent < Time.realtimeSinceStartup))
			{
				return;
			}
			foreach (KeyValuePair<string, AvatarRendererData> avatarRenderer in avatarRenderers)
			{
				if (avatarRenderer.Value.dynamicBonePermissions.VisibilityCheck && !(avatarRenderer.Value.avatarRenderer == null))
				{
					for (int i = 0; i < avatarRenderer.Value.dynamicBones.Count; i++)
					{
						avatarRenderer.Value.dynamicBones[i].enabled = avatarRenderer.Value.avatarRenderer.isVisible;
					}
				}
			}
			nextUpdateVisibilityCurrent = Time.realtimeSinceStartup + nextUpdateVisibility;
		}

		internal override void OnRoomJoined()
		{
			originalSettings.Clear();
			avatarsInScene.Clear();
			avatarRenderers.Clear();
			localPlayer = null;
		}

		internal override void OnPlayerLeft(PlayerInformation playerInfo)
		{
			if (avatarsInScene.ContainsKey(playerInfo.id))
			{
				RemoveBonesOfGameObjectInAllPlayers(avatarsInScene[playerInfo.id].dynamicBoneColliders);
			}
			DeleteOriginalColliders(playerInfo.id);
			RemovePlayerFromDict(playerInfo.id);
			RemoveDynamicBonesFromVisibilityList(playerInfo.id);
		}

		internal override void OnAvatarLoaded(string playerId, string playerName, ref GameObject avatar)
		{
			if (!Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBones)
			{
				return;
			}
			PlayerPrefs.SetInt("VRC_LIMIT_DYNAMIC_BONE_USAGE", 0);
			DynamicBonePermission dynamicBonePermission;
			if (APIUser.CurrentUser.id == playerId)
			{
				List<DynamicBone> list = MiscUtils.FindAllComponentsInGameObject<DynamicBone>(avatar);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].m_Root == null)
					{
						UnityEngine.Object.Destroy(list[i]);
					}
				}
				localPlayer = avatar;
				dynamicBonePermission = Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[PlayerRankStatus.Local];
			}
			else
			{
				PlayerInformation playerInformationByID = PlayerWrappers.GetPlayerInformationByID(playerId);
				if (playerInformationByID == null)
				{
					return;
				}
				if (Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides.ContainsKey(playerId))
				{
					DynamicBonePermissionOverride dynamicBonePermissionOverride = Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissionOverrides[playerId];
					dynamicBonePermission = ((!dynamicBonePermissionOverride.EnableOverride) ? Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[GeneralUtils.GetUserRank(playerInformationByID, ignoreLocal: false)] : new DynamicBonePermission
					{
						EnableDynamicBones = dynamicBonePermissionOverride.EnableDynamicBones,
						DistanceToDisable = dynamicBonePermissionOverride.DistanceToDisable,
						ColliderSizeLimit = dynamicBonePermissionOverride.ColliderSizeLimit,
						PerformanceMode = dynamicBonePermissionOverride.PerformanceMode,
						DistanceDisable = dynamicBonePermissionOverride.DistanceDisable,
						VisibilityCheck = dynamicBonePermissionOverride.VisibilityCheck,
						OptimizeOnly = dynamicBonePermissionOverride.OptimizeOnly
					});
				}
				else
				{
					dynamicBonePermission = Configuration.GetGlobalDynamicBonesConfig().GlobalDynamicBonesPermissions[GeneralUtils.GetUserRank(playerInformationByID, ignoreLocal: false)];
				}
			}
			if (dynamicBonePermission.EnableDynamicBones)
			{
				AddOrReplaceWithCleanup(playerId, new AvatarParsingData
				{
					avatarObject = avatar,
					dynamicBones = MiscUtils.FindAllComponentsInGameObject<DynamicBone>(avatar),
					dynamicBoneColliders = MiscUtils.FindAllComponentsInGameObject<DynamicBoneCollider>(avatar),
					dynamicBonePermissions = dynamicBonePermission
				});
			}
		}

		internal static void AddOrReplaceWithCleanup(string playerId, AvatarParsingData avatarData)
		{
			if (!avatarsInScene.ContainsKey(playerId ?? string.Empty))
			{
				SaveOriginalColliderList(playerId, avatarData.dynamicBones);
				AddToPlayerDict(playerId, avatarData);
			}
			else
			{
				DeleteOriginalColliders(playerId);
				SaveOriginalColliderList(playerId, avatarData.dynamicBones);
				RemovePlayerFromDict(playerId);
				AddToPlayerDict(playerId, avatarData);
				RemoveBonesOfGameObjectInAllPlayers(avatarsInScene[playerId].dynamicBoneColliders);
				RemoveDynamicBonesFromVisibilityList(playerId);
			}
			AddCollidersToAllPlayers(avatarData);
			if (avatarData.avatarObject != localPlayer)
			{
				AddDynamicBonesToVisibilityList(playerId, avatarData.dynamicBones, avatarData.avatarObject.GetComponentInChildren<SkinnedMeshRenderer>(), avatarData.dynamicBonePermissions);
			}
		}

		private static void AddCollidersToAllPlayers(AvatarParsingData avatarData)
		{
			for (int i = 0; i < avatarData.dynamicBones.Count; i++)
			{
				ApplyBoneSettings(avatarData.dynamicBones[i], avatarData.dynamicBonePermissions);
			}
			if (avatarData.dynamicBonePermissions.OptimizeOnly)
			{
				return;
			}
			foreach (AvatarParsingData value in avatarsInScene.Values)
			{
				if (value.avatarObject == avatarData.avatarObject)
				{
					continue;
				}
				foreach (DynamicBone dynamicBone in value.dynamicBones)
				{
					foreach (DynamicBoneCollider dynamicBoneCollider in avatarData.dynamicBoneColliders)
					{
						string text;
						Vector3 vector;
						try
						{
							text = dynamicBoneCollider.name.ToLower();
							vector = dynamicBoneCollider.transform.localScale;
						}
						catch (Exception)
						{
							text = string.Empty;
							vector = Vector3.zero;
						}
						if (avatarData.avatarObject == localPlayer || (avatarData.avatarObject != localPlayer && !text.Contains("floor") && vector.x < 2f && vector.y < 2f && vector.z < 2f && dynamicBoneCollider.m_Center.y >= -1f && dynamicBoneCollider.m_Radius <= 2f))
						{
							AddColliderToBone(dynamicBone, dynamicBoneCollider, value.dynamicBonePermissions);
						}
					}
				}
			}
			foreach (AvatarParsingData value2 in avatarsInScene.Values)
			{
				if (value2.avatarObject == avatarData.avatarObject)
				{
					continue;
				}
				foreach (DynamicBoneCollider dynamicBoneCollider2 in value2.dynamicBoneColliders)
				{
					foreach (DynamicBone dynamicBone2 in avatarData.dynamicBones)
					{
						string text2;
						Vector3 vector2;
						try
						{
							text2 = dynamicBoneCollider2.name.ToLower();
							vector2 = dynamicBoneCollider2.transform.localScale;
						}
						catch (Exception)
						{
							text2 = string.Empty;
							vector2 = Vector3.zero;
						}
						if (avatarData.avatarObject == localPlayer || (avatarData.avatarObject != localPlayer && !text2.Contains("floor") && vector2.x < 2f && vector2.y < 2f && vector2.z < 2f && dynamicBoneCollider2.m_Center.y >= -1f && dynamicBoneCollider2.m_Radius <= 2f))
						{
							AddColliderToBone(dynamicBone2, dynamicBoneCollider2, avatarData.dynamicBonePermissions);
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void ApplyBoneSettings(DynamicBone bone, DynamicBonePermission permissions)
		{
			float num3 = (bone.m_UpdateRate = (bone.field_Private_Single_4 = (permissions.PerformanceMode ? 30f : 60f)));
			bone.m_DistantDisable = permissions.DistanceDisable;
			bone.m_DistanceToObject = permissions.DistanceToDisable;
			if (localPlayer != null)
			{
				bone.m_ReferenceObject = localPlayer.transform;
			}
		}

		private static void AddColliderToBone(DynamicBone bone, DynamicBoneCollider collider, DynamicBonePermission permissions)
		{
			if (collider.m_Bound != DynamicBoneCollider.DynamicBoneColliderBound.Inside && !(collider.m_Radius > permissions.ColliderSizeLimit) && !(collider.m_Height > permissions.ColliderSizeLimit))
			{
				AddColliderToDynamicBone(bone, collider);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AddColliderToDynamicBone(DynamicBone bone, DynamicBoneCollider collider)
		{
			if (!(bone == null) && !(collider == null) && !bone.m_Colliders.Contains(collider))
			{
				bone.m_Colliders.Add(collider);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AddDynamicBonesToVisibilityList(string player, List<DynamicBone> dynamicBones, Renderer renderer, DynamicBonePermission permissions)
		{
			avatarRenderers.Add(player, new AvatarRendererData
			{
				avatarRenderer = renderer,
				dynamicBones = dynamicBones,
				dynamicBonePermissions = permissions
			});
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AddToPlayerDict(string name, AvatarParsingData value)
		{
			avatarsInScene.Add(name, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SaveOriginalColliderList(string name, List<DynamicBone> bones)
		{
			if (originalSettings.ContainsKey(name))
			{
				originalSettings.Remove(name);
			}
			List<OriginalBoneSettings> list = new List<OriginalBoneSettings>(bones.Count);
			for (int i = 0; i < bones.Count; i++)
			{
				List<DynamicBoneCollider> list2 = new List<DynamicBoneCollider>();
				for (int j = 0; j < bones[i].m_Colliders.Count; j++)
				{
					list2.Add(bones[i].m_Colliders[j]);
				}
				OriginalBoneSettings originalBoneSettings = default(OriginalBoneSettings);
				originalBoneSettings.distanceToDisable = bones[i].m_DistanceToObject;
				originalBoneSettings.updateRate = bones[i].m_UpdateRate;
				originalBoneSettings.distantDisable = bones[i].m_DistantDisable;
				originalBoneSettings.colliders = list2;
				originalBoneSettings.referenceToOriginal = bones[i];
				OriginalBoneSettings item = originalBoneSettings;
				list.Add(item);
			}
			originalSettings.Add(name, list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void RemoveBonesOfGameObjectInAllPlayers(List<DynamicBoneCollider> colliders)
		{
			for (int i = 0; i < avatarsInScene.Count; i++)
			{
				AvatarParsingData value = avatarsInScene.ElementAt(i).Value;
				for (int j = 0; j < value.dynamicBones.Count; j++)
				{
					for (int k = 0; k < colliders.Count; k++)
					{
						if (value.dynamicBones[j].m_Colliders.Contains(colliders[k]))
						{
							value.dynamicBones[j].m_Colliders.Remove(colliders[k]);
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void DeleteOriginalColliders(string name)
		{
			if (originalSettings.ContainsKey(name))
			{
				originalSettings.Remove(name);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void RemovePlayerFromDict(string name)
		{
			if (avatarsInScene.ContainsKey(name))
			{
				avatarsInScene.Remove(name);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void RemoveDynamicBonesFromVisibilityList(string player)
		{
			if (avatarRenderers.ContainsKey(player))
			{
				avatarRenderers.Remove(player);
			}
		}
	}
}
