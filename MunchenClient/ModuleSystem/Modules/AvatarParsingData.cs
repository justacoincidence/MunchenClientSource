using System.Collections.Generic;
using UnityEngine;

namespace MunchenClient.ModuleSystem.Modules
{
	internal struct AvatarParsingData
	{
		internal GameObject avatarObject;

		internal List<DynamicBone> dynamicBones;

		internal List<DynamicBoneCollider> dynamicBoneColliders;

		internal DynamicBonePermission dynamicBonePermissions;
	}
}
