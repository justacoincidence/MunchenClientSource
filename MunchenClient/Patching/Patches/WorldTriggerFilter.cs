using System.Collections.Generic;

namespace MunchenClient.Patching.Patches
{
	internal struct WorldTriggerFilter
	{
		internal WorldTriggerType worldType;

		internal Dictionary<string, ObjectTriggerType> objectType;
	}
}
