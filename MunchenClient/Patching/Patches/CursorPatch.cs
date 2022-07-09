using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal class CursorPatch : PatchComponent
	{
		protected override string patchName => "CursorPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(CursorPatch));
			PatchMethod(typeof(Cursor).GetProperty("lockState").GetSetMethod(), GetLocalPatch("CursorSetLockStatePatch"), null);
			PatchMethod(typeof(Cursor).GetProperty("visible").GetSetMethod(), GetLocalPatch("CursorSetVisiblePatch"), null);
		}

		private static bool CursorSetLockStatePatch()
		{
			return false;
		}

		private static bool CursorSetVisiblePatch()
		{
			return false;
		}
	}
}
