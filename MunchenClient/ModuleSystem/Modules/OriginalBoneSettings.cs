using System.Collections.Generic;

namespace MunchenClient.ModuleSystem.Modules
{
	internal struct OriginalBoneSettings
	{
		internal float updateRate;

		internal float distanceToDisable;

		internal List<DynamicBoneCollider> colliders;

		internal DynamicBone referenceToOriginal;

		internal bool distantDisable;
	}
}
