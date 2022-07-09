using System.Collections.Generic;
using UnityEngine;

namespace MunchenClient.ModuleSystem.Modules
{
	internal struct AvatarRendererData
	{
		internal Renderer avatarRenderer;

		internal List<DynamicBone> dynamicBones;

		internal DynamicBonePermission dynamicBonePermissions;
	}
}
