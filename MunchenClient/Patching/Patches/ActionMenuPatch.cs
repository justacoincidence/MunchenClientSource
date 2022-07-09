using System.Linq;
using System.Reflection;
using ActionMenuAPI;
using UnhollowerRuntimeLib.XrefScans;

namespace MunchenClient.Patching.Patches
{
	internal class ActionMenuPatch : PatchComponent
	{
		protected override string patchName => "ActionMenuPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(ActionMenuPatch));
			PatchMethod(typeof(ActionMenu).GetMethods().FirstOrDefault((MethodInfo it) => XrefScanner.XrefScan(it).Any((XrefInstance jt) => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "Emojis")), null, GetLocalPatch("OpenMainPage"));
		}

		private static void OpenMainPage(ActionMenu __instance)
		{
			CustomActionMenu.OpenMainPage(__instance);
		}
	}
}
