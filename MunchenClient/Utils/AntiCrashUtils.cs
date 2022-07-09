using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Config;
using MunchenClient.Wrappers;
using UnhollowerBaseLib;
using UnityEngine;
using VRC.Core;

namespace MunchenClient.Utils
{
	internal class AntiCrashUtils
	{
		private static readonly Regex duplicatedMeshNameRegex = new Regex("[a-zA-Z0-9]+(\\s|\\.\\d+)+(\\(\\d+\\)|\\d+|\\.\\d+)");

		private static readonly Regex numberRegex = new Regex("^\\d{5,20}");

		private static readonly Regex isNewPoyomiShader = new Regex("hidden\\/(locked\\/|)(\\.|)poiyomi\\/(\\s|•|★|\\?|)+poiyomi (pro|toon|cutout|transparent)(\\s|•|★|\\?|)+\\/[a-z0-9\\s\\.\\d-_\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)=\\]\\[]+");

		private static readonly int maximumRenderQueue = 85899;

		private static readonly Il2CppSystem.Collections.Generic.List<Material> antiCrashTempMaterialsList = new Il2CppSystem.Collections.Generic.List<Material>();

		internal static void ProcessRenderer(Renderer renderer, ref AntiCrashRendererPostProcess previousProcess)
		{
			if (!Configuration.GetAntiCrashConfig().AntiMeshCrash || !ProcessMeshPolygons(renderer, ref previousProcess.meshCount, ref previousProcess.nukedMeshes, ref previousProcess.polygonCount, ref previousProcess.removedBlendshapeKeys))
			{
				if (Configuration.GetAntiCrashConfig().AntiMaterialCrash)
				{
					AntiCrashMaterialPostProcess antiCrashMaterialPostProcess = ProcessMaterials(renderer, previousProcess.nukedMaterials, previousProcess.materialCount);
					previousProcess.nukedMaterials = antiCrashMaterialPostProcess.nukedMaterials;
					previousProcess.materialCount = antiCrashMaterialPostProcess.materialCount;
				}
				if (Configuration.GetAntiCrashConfig().AntiShaderCrash)
				{
					AntiCrashShaderPostProcess antiCrashShaderPostProcess = ProcessShaders(renderer, previousProcess.nukedShaders, previousProcess.shaderCount);
					previousProcess.nukedShaders = antiCrashShaderPostProcess.nukedShaders;
					previousProcess.shaderCount = antiCrashShaderPostProcess.shaderCount;
				}
			}
		}

		internal static bool ProcessMeshPolygons(Renderer renderer, ref int currentMeshes, ref int currentNukedMeshes, ref uint currentPolygonCount, ref bool removedBlendshapeKeys)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = renderer.TryCast<SkinnedMeshRenderer>();
			MeshFilter component = renderer.GetComponent<MeshFilter>();
			Mesh mesh = skinnedMeshRenderer?.sharedMesh ?? component?.sharedMesh;
			if (mesh == null)
			{
				return false;
			}
			if (currentMeshes >= Configuration.GetAntiCrashConfig().MaxMeshes)
			{
				currentNukedMeshes++;
				UnityEngine.Object.DestroyImmediate(renderer.gameObject, allowDestroyingAssets: true);
				return true;
			}
			if (Configuration.GetAntiCrashConfig().AntiBlendShapeCrash && skinnedMeshRenderer != null)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				for (int i = 0; i < mesh.blendShapeCount; i++)
				{
					string text = mesh.GetBlendShapeName(i).ToLower();
					if (text.Contains("reverted"))
					{
						flag = true;
					}
					if (text.Contains("posetorest"))
					{
						flag2 = true;
					}
					else if (text.Contains("key 22"))
					{
						flag3 = true;
					}
					else if (text.Contains("key 56"))
					{
						flag4 = true;
					}
					else if (text.Contains("slant"))
					{
						flag5 = true;
					}
				}
				if (flag && flag2 && flag3 && flag4 && flag5)
				{
					removedBlendshapeKeys = true;
					mesh.ClearBlendShapes();
				}
			}
			int num;
			try
			{
				num = mesh.subMeshCount;
			}
			catch (Exception)
			{
				num = 0;
			}
			try
			{
				renderer.GetSharedMaterials(antiCrashTempMaterialsList);
				int num2 = ProcessMesh(mesh, num, ref currentNukedMeshes, ref currentPolygonCount);
				if (num2 != -1)
				{
					antiCrashTempMaterialsList.RemoveRange(num2, antiCrashTempMaterialsList.Count - num2);
					renderer.SetMaterialArray((Il2CppReferenceArray<Material>)antiCrashTempMaterialsList.ToArray());
				}
				if (num + 2 < renderer.GetMaterialCount())
				{
					UnityEngine.Object.Destroy(renderer.gameObject);
					return true;
				}
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("AntiCrash", "ProcessMesh", e, "ProcessMeshPolygons", 202);
			}
			currentMeshes++;
			return false;
		}

		internal static AntiCrashMaterialPostProcess ProcessMaterials(Renderer renderer, int currentNukedMaterials, int currentMaterialCount)
		{
			int materialCount = renderer.GetMaterialCount();
			int num = currentMaterialCount + materialCount;
			if (num > Configuration.GetAntiCrashConfig().MaxMaterials)
			{
				int num2 = ((currentMaterialCount < Configuration.GetAntiCrashConfig().MaxMaterials) ? Configuration.GetAntiCrashConfig().MaxMaterials : 0);
				int num3 = ((num2 == 0) ? materialCount : (num - Configuration.GetAntiCrashConfig().MaxMaterials));
				if (num2 > materialCount)
				{
					num2 = materialCount;
				}
				int num4 = materialCount - num2;
				if (num3 > num4)
				{
					num3 = num4;
				}
				currentNukedMaterials += num3;
				num -= num3;
				if (materialCount == num3)
				{
					UnityEngine.Object.DestroyImmediate(renderer.gameObject, allowDestroyingAssets: true);
				}
				else
				{
					Il2CppSystem.Collections.Generic.List<Material> list = new Il2CppSystem.Collections.Generic.List<Material>();
					renderer.GetSharedMaterials(list);
					list.RemoveRange(num2, num3);
					renderer.materials = (Il2CppReferenceArray<Material>)list.ToArray();
				}
			}
			currentMaterialCount = num;
			return new AntiCrashMaterialPostProcess
			{
				nukedMaterials = currentNukedMaterials,
				materialCount = currentMaterialCount
			};
		}

		internal static AntiCrashShaderPostProcess ProcessShaders(Renderer renderer, int currentNukedShaders, int currentShaderCount)
		{
			if (renderer == null)
			{
				return new AntiCrashShaderPostProcess
				{
					nukedShaders = currentNukedShaders,
					shaderCount = currentShaderCount
				};
			}
			for (int i = 0; i < renderer.materials.Length; i++)
			{
				if (!(renderer.materials[i] == null))
				{
					currentShaderCount++;
					if (ProcessShader(renderer.materials[i]))
					{
						currentNukedShaders++;
					}
				}
			}
			return new AntiCrashShaderPostProcess
			{
				nukedShaders = currentNukedShaders,
				shaderCount = currentShaderCount
			};
		}

		internal static AntiCrashClothPostProcess ProcessCloth(Cloth cloth, AntiCrashClothPostProcess previousReport)
		{
			if (previousReport.clothCount >= Configuration.GetAntiCrashConfig().MaxCloth)
			{
				previousReport.nukedCloths++;
				UnityEngine.Object.DestroyImmediate(cloth.gameObject, allowDestroyingAssets: true);
				return new AntiCrashClothPostProcess
				{
					nukedCloths = previousReport.nukedCloths,
					clothCount = previousReport.clothCount,
					currentVertexCount = previousReport.currentVertexCount
				};
			}
			Mesh mesh = cloth.GetComponent<SkinnedMeshRenderer>()?.sharedMesh;
			if (mesh == null)
			{
				previousReport.nukedCloths++;
				UnityEngine.Object.DestroyImmediate(cloth.gameObject, allowDestroyingAssets: true);
				return new AntiCrashClothPostProcess
				{
					nukedCloths = previousReport.nukedCloths,
					clothCount = previousReport.clothCount,
					currentVertexCount = previousReport.currentVertexCount
				};
			}
			int num = previousReport.currentVertexCount + mesh.vertexCount;
			if (num >= Configuration.GetAntiCrashConfig().MaxClothVertices)
			{
				previousReport.nukedCloths++;
				UnityEngine.Object.DestroyImmediate(cloth.gameObject, allowDestroyingAssets: true);
				return new AntiCrashClothPostProcess
				{
					nukedCloths = previousReport.nukedCloths,
					clothCount = previousReport.clothCount,
					currentVertexCount = previousReport.currentVertexCount
				};
			}
			cloth.clothSolverFrequency = MathUtils.Clamp(cloth.clothSolverFrequency, 0f, Configuration.GetAntiCrashConfig().MaxClothSolverFrequency);
			previousReport.currentVertexCount = num;
			previousReport.clothCount++;
			return new AntiCrashClothPostProcess
			{
				nukedCloths = previousReport.nukedCloths,
				clothCount = previousReport.clothCount,
				currentVertexCount = previousReport.currentVertexCount
			};
		}

		internal static void ProcessParticleSystem(ParticleSystem particleSystem, ref AntiCrashParticleSystemPostProcess post)
		{
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			if (component == null)
			{
				post.nukedParticleSystems++;
				UnityEngine.Object.DestroyImmediate(particleSystem, allowDestroyingAssets: true);
				return;
			}
			particleSystem.main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
			particleSystem.main.simulationSpeed = MathUtils.Clamp(particleSystem.main.simulationSpeed, 0f, Configuration.GetAntiCrashConfig().MaxParticleSimulationSpeed);
			particleSystem.collision.maxCollisionShapes = MathUtils.Clamp(particleSystem.collision.maxCollisionShapes, 0, Configuration.GetAntiCrashConfig().MaxParticleCollisionShapes);
			particleSystem.trails.ribbonCount = MathUtils.Clamp(particleSystem.trails.ribbonCount, 0, Configuration.GetAntiCrashConfig().MaxParticleTrails);
			particleSystem.emissionRate = MathUtils.Clamp(particleSystem.emissionRate, 0f, Configuration.GetAntiCrashConfig().MaxParticleEmissionRate);
			for (int i = 0; i < particleSystem.emission.burstCount; i++)
			{
				ParticleSystem.Burst burst = particleSystem.emission.GetBurst(i);
				burst.maxCount = MathUtils.Clamp(burst.maxCount, (short)0, (short)Configuration.GetAntiCrashConfig().MaxParticleEmissionBurstCount);
				burst.cycleCount = MathUtils.Clamp(burst.cycleCount, 0, Configuration.GetAntiCrashConfig().MaxParticleEmissionBurstCount);
				particleSystem.emission.SetBurst(i, burst);
			}
			int num = Configuration.GetAntiCrashConfig().MaxParticleLimit - post.currentParticleCount;
			if (num <= 0 && particleSystem.maxParticles > 100)
			{
				particleSystem.maxParticles = 100;
			}
			else if (particleSystem.maxParticles > num)
			{
				particleSystem.maxParticles = num;
			}
			if (component.renderMode == ParticleSystemRenderMode.Mesh)
			{
				Il2CppReferenceArray<Mesh> il2CppReferenceArray = new Il2CppReferenceArray<Mesh>(component.meshCount);
				component.GetMeshes(il2CppReferenceArray);
				uint currentPolygonCount = 0u;
				int currentNukedMeshes = 0;
				if (component.mesh != null && duplicatedMeshNameRegex.IsMatch(component.mesh.name))
				{
					component.enabled = false;
					particleSystem.playOnAwake = false;
					if (particleSystem.isPlaying)
					{
						particleSystem.Stop();
					}
				}
				foreach (Mesh item in il2CppReferenceArray)
				{
					int subMeshCount;
					try
					{
						subMeshCount = item.subMeshCount;
					}
					catch (Exception)
					{
						subMeshCount = 0;
					}
					component.GetSharedMaterials(antiCrashTempMaterialsList);
					int num2 = ProcessMesh(item, subMeshCount, ref currentNukedMeshes, ref currentPolygonCount);
					if (num2 != -1)
					{
						antiCrashTempMaterialsList.RemoveRange(num2, antiCrashTempMaterialsList.Count - num2);
					}
					Il2CppSystem.Collections.Generic.List<Material>.Enumerator enumerator2 = antiCrashTempMaterialsList.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						Material current2 = enumerator2.Current;
						if (!(current2 == null))
						{
							ProcessShader(current2);
						}
					}
					if (num2 != -1)
					{
						component.SetMaterialArray((Il2CppReferenceArray<Material>)antiCrashTempMaterialsList.ToArray());
					}
				}
				if (currentPolygonCount * particleSystem.maxParticles > Configuration.GetAntiCrashConfig().MaxParticleMeshVertices)
				{
					int num4 = (particleSystem.maxParticles = (int)(Configuration.GetAntiCrashConfig().MaxParticleMeshVertices / currentPolygonCount));
				}
			}
			if (particleSystem.maxParticles == 0)
			{
				post.nukedParticleSystems++;
				UnityEngine.Object.DestroyImmediate(particleSystem, allowDestroyingAssets: true);
			}
			post.currentParticleCount += particleSystem.maxParticles;
		}

		internal static int ProcessMesh(Mesh mesh, int subMeshCount, ref int currentNukedMeshes, ref uint currentPolygonCount)
		{
			int num = -1;
			for (int i = 0; i < subMeshCount; i++)
			{
				try
				{
					uint num2 = mesh.GetIndexCount(i);
					switch (mesh.GetTopology(i))
					{
					case MeshTopology.Triangles:
						num2 /= 3u;
						break;
					case MeshTopology.Quads:
						num2 /= 4u;
						break;
					case MeshTopology.Lines:
						num2 /= 2u;
						break;
					}
					if (currentPolygonCount + num2 > Configuration.GetAntiCrashConfig().MaxPolygons)
					{
						currentPolygonCount += num2;
						currentNukedMeshes++;
						if (num == -1)
						{
							num = i;
						}
						UnityEngine.Object.DestroyImmediate(mesh, allowDestroyingAssets: true);
						break;
					}
					currentPolygonCount += num2;
					continue;
				}
				catch (Exception e)
				{
					ConsoleUtils.Exception("AntiCrash", "SubMesh Processor", e, "ProcessMesh", 532);
					continue;
				}
			}
			if (mesh == null)
			{
				return num;
			}
			if (MathUtils.IsBeyondLimit(mesh.bounds.extents, -100f, 100f))
			{
				UnityEngine.Object.DestroyImmediate(mesh, allowDestroyingAssets: true);
				return num;
			}
			if (MathUtils.IsBeyondLimit(mesh.bounds.size, -100f, 100f))
			{
				UnityEngine.Object.DestroyImmediate(mesh, allowDestroyingAssets: true);
				return num;
			}
			if (MathUtils.IsBeyondLimit(mesh.bounds.center, -100f, 100f))
			{
				UnityEngine.Object.DestroyImmediate(mesh, allowDestroyingAssets: true);
				return num;
			}
			if (MathUtils.IsBeyondLimit(mesh.bounds.min, -100f, 100f))
			{
				UnityEngine.Object.DestroyImmediate(mesh, allowDestroyingAssets: true);
				return num;
			}
			if (MathUtils.IsBeyondLimit(mesh.bounds.max, -100f, 100f))
			{
				UnityEngine.Object.DestroyImmediate(mesh, allowDestroyingAssets: true);
				return num;
			}
			return num;
		}

		internal static AntiCrashDynamicBonePostProcess ProcessDynamicBone(DynamicBone dynamicBone, int currentNukedDynamicBones, int currentDynamicBones)
		{
			if (currentDynamicBones >= Configuration.GetAntiCrashConfig().MaxDynamicBones)
			{
				currentNukedDynamicBones++;
				UnityEngine.Object.DestroyImmediate(dynamicBone, allowDestroyingAssets: true);
				return new AntiCrashDynamicBonePostProcess
				{
					nukedDynamicBones = currentNukedDynamicBones,
					dynamicBoneCount = currentDynamicBones
				};
			}
			currentDynamicBones++;
			dynamicBone.m_UpdateRate = MathUtils.Clamp(dynamicBone.m_UpdateRate, 0f, 60f);
			dynamicBone.m_Radius = MathUtils.Clamp(dynamicBone.m_Radius, 0f, 2f);
			dynamicBone.m_EndLength = MathUtils.Clamp(dynamicBone.m_EndLength, 0f, 10f);
			dynamicBone.m_DistanceToObject = MathUtils.Clamp(dynamicBone.m_DistanceToObject, 0f, 1f);
			Vector3 endOffset = dynamicBone.m_EndOffset;
			endOffset.x = MathUtils.Clamp(endOffset.x, -5f, 5f);
			endOffset.y = MathUtils.Clamp(endOffset.y, -5f, 5f);
			endOffset.z = MathUtils.Clamp(endOffset.z, -5f, 5f);
			dynamicBone.m_EndOffset = endOffset;
			Vector3 gravity = dynamicBone.m_Gravity;
			gravity.x = MathUtils.Clamp(gravity.x, -5f, 5f);
			gravity.y = MathUtils.Clamp(gravity.y, -5f, 5f);
			gravity.z = MathUtils.Clamp(gravity.z, -5f, 5f);
			dynamicBone.m_Gravity = gravity;
			Vector3 force = dynamicBone.m_Force;
			force.x = MathUtils.Clamp(force.x, -5f, 5f);
			force.y = MathUtils.Clamp(force.y, -5f, 5f);
			force.z = MathUtils.Clamp(force.z, -5f, 5f);
			dynamicBone.m_Force = force;
			Il2CppSystem.Collections.Generic.List<DynamicBoneCollider> list = new Il2CppSystem.Collections.Generic.List<DynamicBoneCollider>();
			foreach (DynamicBoneCollider item in dynamicBone.m_Colliders.ToArray())
			{
				if (item != null && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			dynamicBone.m_Colliders = list;
			return new AntiCrashDynamicBonePostProcess
			{
				nukedDynamicBones = currentNukedDynamicBones,
				dynamicBoneCount = currentDynamicBones
			};
		}

		internal static AntiCrashDynamicBoneColliderPostProcess ProcessDynamicBoneCollider(DynamicBoneCollider dynamicBoneCollider, int currentNukedDynamicBoneColliders, int currentDynamicBoneColliders)
		{
			if (currentDynamicBoneColliders >= Configuration.GetAntiCrashConfig().MaxDynamicBoneColliders)
			{
				currentNukedDynamicBoneColliders++;
				UnityEngine.Object.DestroyImmediate(dynamicBoneCollider, allowDestroyingAssets: true);
				return new AntiCrashDynamicBoneColliderPostProcess
				{
					nukedDynamicBoneColliders = currentNukedDynamicBoneColliders,
					dynamicBoneColiderCount = currentDynamicBoneColliders
				};
			}
			currentDynamicBoneColliders++;
			dynamicBoneCollider.m_Radius = MathUtils.Clamp(dynamicBoneCollider.m_Radius, 0f, 50f);
			dynamicBoneCollider.m_Height = MathUtils.Clamp(dynamicBoneCollider.m_Height, 0f, 50f);
			Vector3 center = dynamicBoneCollider.m_Center;
			MathUtils.Clamp(center.x, -50f, 50f);
			MathUtils.Clamp(center.y, -50f, 50f);
			MathUtils.Clamp(center.z, -50f, 50f);
			dynamicBoneCollider.m_Center = center;
			return new AntiCrashDynamicBoneColliderPostProcess
			{
				nukedDynamicBoneColliders = currentNukedDynamicBoneColliders,
				dynamicBoneColiderCount = currentDynamicBoneColliders
			};
		}

		internal static AntiCrashLightSourcePostProcess ProcessLight(Light light, int currentNukedLights, int currentLights)
		{
			if (currentLights >= Configuration.GetAntiCrashConfig().MaxLightSources)
			{
				currentNukedLights++;
				UnityEngine.Object.DestroyImmediate(light, allowDestroyingAssets: true);
			}
			currentLights++;
			return new AntiCrashLightSourcePostProcess
			{
				nukedLightSources = currentNukedLights,
				lightSourceCount = currentLights
			};
		}

		internal static bool ProcessTransform(Transform _, ref int currentTransforms)
		{
			if (currentTransforms >= Configuration.GetAntiCrashConfig().MaxTransforms)
			{
				return true;
			}
			currentTransforms++;
			return false;
		}

		internal static bool ProcessConstraint(Behaviour constraint, ref int currentConstraints, ref int nukedConstraints)
		{
			if (Configuration.GetAntiCrashConfig().MaxConstraints > -1 && currentConstraints >= Configuration.GetAntiCrashConfig().MaxConstraints)
			{
				nukedConstraints++;
				UnityEngine.Object.DestroyImmediate(constraint, allowDestroyingAssets: true);
				return true;
			}
			currentConstraints++;
			return false;
		}

		internal static bool ProcessRigidbody(Rigidbody rigidbody, ref int currentRigidbodies, ref int nukedRigidbodies)
		{
			if (currentRigidbodies >= Configuration.GetAntiCrashConfig().MaxRigidbodies)
			{
				nukedRigidbodies++;
				UnityEngine.Object.DestroyImmediate(rigidbody.gameObject, allowDestroyingAssets: true);
				return true;
			}
			rigidbody.mass = MathUtils.Clamp(rigidbody.mass, 0f - Configuration.GetAntiCrashConfig().MaxRigidbodyMass, Configuration.GetAntiCrashConfig().MaxRigidbodyMass);
			rigidbody.maxAngularVelocity = MathUtils.Clamp(rigidbody.maxAngularVelocity, 0f - Configuration.GetAntiCrashConfig().MaxRigidbodyAngularVelocity, Configuration.GetAntiCrashConfig().MaxRigidbodyAngularVelocity);
			rigidbody.maxDepenetrationVelocity = MathUtils.Clamp(rigidbody.maxDepenetrationVelocity, 0f - Configuration.GetAntiCrashConfig().MaxRigidbodyDepenetrationVelocity, Configuration.GetAntiCrashConfig().MaxRigidbodyDepenetrationVelocity);
			return false;
		}

		internal static bool ProcessCollider(Collider collider, ref int currentColliders, ref int nukedColliders)
		{
			if (currentColliders >= Configuration.GetAntiCrashConfig().MaxColliders)
			{
				nukedColliders++;
				UnityEngine.Object.DestroyImmediate(collider, allowDestroyingAssets: true);
				return true;
			}
			if ((collider.bounds.center.x < -100f && collider.bounds.center.x > 100f) || (collider.bounds.center.y < -100f && collider.bounds.center.y > 100f) || (collider.bounds.center.z < -100f && collider.bounds.center.z > 100f) || (collider.bounds.extents.x < -100f && collider.bounds.extents.x > 100f) || (collider.bounds.extents.y < -100f && collider.bounds.extents.y > 100f) || (collider.bounds.extents.z < -100f && collider.bounds.extents.z > 100f))
			{
				nukedColliders++;
				UnityEngine.Object.DestroyImmediate(collider, allowDestroyingAssets: true);
				return true;
			}
			currentColliders++;
			return false;
		}

		internal static bool ProcessJoint(Joint joint, ref int currentSpringJoints)
		{
			if (currentSpringJoints >= Configuration.GetAntiCrashConfig().MaxSpringJoints)
			{
				UnityEngine.Object.DestroyImmediate(joint.gameObject, allowDestroyingAssets: true);
				return true;
			}
			currentSpringJoints++;
			joint.connectedMassScale = MathUtils.Clamp(joint.connectedMassScale, -25f, 25f);
			joint.massScale = MathUtils.Clamp(joint.massScale, -25f, 25f);
			joint.breakTorque = MathUtils.Clamp(joint.breakTorque, -100f, 100f);
			joint.breakForce = MathUtils.Clamp(joint.massScale, -100f, 100f);
			if (joint is SpringJoint springJoint)
			{
				springJoint.damper = MathUtils.Clamp(springJoint.damper, -100f, 100f);
				springJoint.maxDistance = MathUtils.Clamp(springJoint.maxDistance, -100f, 100f);
				springJoint.minDistance = MathUtils.Clamp(springJoint.minDistance, -100f, 100f);
				springJoint.spring = MathUtils.Clamp(springJoint.spring, -100f, 100f);
				springJoint.tolerance = MathUtils.Clamp(springJoint.tolerance, -100f, 100f);
			}
			return false;
		}

		internal static bool ProcessShader(Material material)
		{
			string text = material.shader.name.ToLower();
			if (!material.shader.isSupported)
			{
				SanitizeShader(material);
				return true;
			}
			if (ShaderUtils.IsFakeEngineShader(material))
			{
				SanitizeShader(material);
				return true;
			}
			if (isNewPoyomiShader.IsMatch(text))
			{
				return false;
			}
			if (ShaderUtils.blacklistedShaders.Contains(text))
			{
				SanitizeShader(material);
				return true;
			}
			int num = (Encoding.UTF8.GetByteCount(text) - text.Length) / 4;
			if (string.IsNullOrEmpty(text) || text.Length > 100 || material.shader.renderQueue > maximumRenderQueue || num > 10 || numberRegex.IsMatch(text))
			{
				SanitizeShader(material);
				return true;
			}
			return false;
		}

		internal static void SanitizeShader(Material material)
		{
			material.shader = ShaderUtils.GetStandardShader();
		}

		internal static void DisposeAvatar(GameObject avatar, System.Collections.Generic.List<Component> components)
		{
			for (int i = 0; i < components.Count; i++)
			{
				UnityEngine.Object.DestroyImmediate(components[i], allowDestroyingAssets: true);
			}
			for (int j = 0; j < avatar.transform.childCount; j++)
			{
				UnityEngine.Object.DestroyImmediate(avatar.transform.GetChild(j).gameObject, allowDestroyingAssets: true);
			}
		}

		internal static void ProcessAvatarWhitelist(ApiAvatar avatar)
		{
			if (!Configuration.GetAntiCrashConfig().WhitelistedAvatars.ContainsKey(avatar.id))
			{
				GeneralWrappers.AlertAction("Whitelist", "Are you sure you want to add " + avatar.name + " to your whitelist?", "Add", delegate
				{
					GeneralWrappers.ClosePopup();
					Configuration.GetAntiCrashConfig().WhitelistedAvatars.Add(avatar.id, value: true);
					Configuration.SaveAntiCrashConfig();
					GeneralUtils.RemoveAvatarFromCache(avatar.id);
					PlayerUtils.ReloadAvatarByID(avatar.id);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}
			else if (Configuration.GetAntiCrashConfig().WhitelistedAvatars[avatar.id])
			{
				GeneralWrappers.AlertAction("Whitelist", "Are you sure you want to remove " + avatar.name + " from your whitelist?", "Remove", delegate
				{
					GeneralWrappers.ClosePopup();
					Configuration.GetAntiCrashConfig().WhitelistedAvatars.Remove(avatar.id);
					Configuration.SaveAntiCrashConfig();
					GeneralUtils.RemoveAvatarFromCache(avatar.id);
					PlayerUtils.ReloadAvatarByID(avatar.id);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}
			else
			{
				GeneralWrappers.AlertAction("Whitelist", "Are you sure you want to add " + avatar.name + " to your whitelist?", "Add", delegate
				{
					GeneralWrappers.ClosePopup();
					Configuration.GetAntiCrashConfig().WhitelistedAvatars[avatar.id] = true;
					Configuration.SaveAntiCrashConfig();
					GeneralUtils.RemoveAvatarFromCache(avatar.id);
					PlayerUtils.ReloadAvatarByID(avatar.id);
				}, "Cancel", delegate
				{
					GeneralWrappers.ClosePopup();
				});
			}
		}
	}
}
