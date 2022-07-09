namespace MunchenClient.Patching.Patches
{
	internal class VRCToolsPatch : PatchComponent
	{
		protected override string patchName => "VRCToolsPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(VRCToolsPatch));
		}
	}
}
