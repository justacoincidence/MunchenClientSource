using System.Collections.Generic;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class GlobalDynamicBonesConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "GlobalDynamicBones.json";

		public int ConfigVersion = 1;

		public bool GlobalDynamicBones = false;

		public Dictionary<PlayerRankStatus, DynamicBonePermission> GlobalDynamicBonesPermissions = new Dictionary<PlayerRankStatus, DynamicBonePermission>();

		public Dictionary<string, DynamicBonePermissionOverride> GlobalDynamicBonesPermissionOverrides = new Dictionary<string, DynamicBonePermissionOverride>();
	}
}
