using System.Collections.Generic;
using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class AntiCrashConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "AntiCrashConfig.json";

		public int ConfigVersion = 1;

		public bool AntiCrashEnablePublic = false;

		public bool AntiCrashEnableInvite = false;

		public bool AntiCrashEnableInvitePlus = false;

		public bool AntiCrashEnableFriends = false;

		public bool AntiCrashEnableFriendsPlus = false;

		public bool AntiShaderCrash = false;

		public bool AntiAudioCrash = false;

		public bool AntiMeshCrash = false;

		public bool AntiMaterialCrash = false;

		public bool AntiFinalIKCrash = false;

		public bool AntiClothCrash = false;

		public bool AntiParticleSystemCrash = false;

		public bool AntiDynamicBoneCrash = false;

		public bool AntiLightSourceCrash = false;

		public bool AntiInvalidFloatsCrash = false;

		public bool AntiPhysicsCrash = false;

		public bool AntiBlendShapeCrash = false;

		public bool AntiAvatarDescriptorCrash = false;

		public bool AntiAvatarAudioMixerCrash = false;

		public bool AntiAvatarLoadingCrash = false;

		public bool AntiAssetBundleCrash = false;

		public bool GlobalAvatarBlacklist = false;

		public bool ExperimentalAvatarBlocker = false;

		public bool VerboseLogging = true;

		public uint MaxPolygons = 2500000u;

		public int MaxMeshes = 250;

		public int MaxMaterials = 300;

		public int MaxDynamicBones = 75;

		public int MaxDynamicBoneColliders = 50;

		public int MaxAudioSources = 150;

		public int MaxAnimators = 25;

		public int MaxConstraints = 200;

		public int MaxShaders = 100;

		public int MaxLightSources = 8;

		public int MaxColliders = 20;

		public int MaxSpringJoints = 15;

		public int MaxTransforms = 3500;

		public int MaxMonobehaviours = 1500;

		public int MaxComponentsTotal = 6500;

		public int MaxDepth = 125;

		public int MaxChildren = 125;

		public float MaxHeight = 10f;

		public int MaxCloth = 75;

		public int MaxClothVertices = 15000;

		public float MaxClothSolverFrequency = 180f;

		public int MaxRigidbodies = 25;

		public float MaxRigidbodyMass = 10000f;

		public float MaxRigidbodyAngularVelocity = 100f;

		public float MaxRigidbodyDepenetrationVelocity = 100f;

		public int MaxParticleLimit = 10000;

		public uint MaxParticleMeshVertices = 1000000u;

		public int MaxParticleCollisionShapes = 1024;

		public int MaxParticleRibbons = 10000;

		public float MaxParticleEmissionRate = 500f;

		public int MaxParticleEmissionBurstCount = 125;

		public int MaxParticleTrails = 64;

		public float MaxParticleSimulationSpeed = 5f;

		public int MaxShaderLoopLimit = 128;

		public int MaxShaderGeometryLimit = 25;

		public float MaxShaderTesselationPower = 2.5f;

		public Dictionary<string, bool> WhitelistedAvatars = new Dictionary<string, bool>();

		public Dictionary<string, bool> BlacklistedAvatars = new Dictionary<string, bool>();
	}
}
