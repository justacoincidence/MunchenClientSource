using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal class BatchModePatch : PatchComponent
	{
		protected override string patchName => "BatchModePatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(BatchModePatch));
			PatchMethod(typeof(Application).GetProperty("isBatchMode").GetGetMethod(), GetLocalPatch("IsBatchModePatch"), null);
		}

		private static bool IsBatchModePatch(ref bool __result)
		{
			__result = false;
			return false;
		}
	}
}
