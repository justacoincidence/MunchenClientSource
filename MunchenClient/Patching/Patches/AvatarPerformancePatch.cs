using MunchenClient.Config;
using VRC.SDKBase.Validation.Performance;
using VRC.SDKBase.Validation.Performance.Stats;

namespace MunchenClient.Patching.Patches
{
	internal class AvatarPerformancePatch : PatchComponent
	{
		protected override string patchName => "AvatarPerformancePatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(AvatarPerformancePatch));
			PatchMethod(typeof(PerformanceScannerSet).GetMethod("RunPerformanceScan"), GetLocalPatch("CalculateAvatarPerformancePatch"), null);
			PatchMethod(typeof(PerformanceScannerSet).GetMethod("RunPerformanceScanEnumerator"), GetLocalPatch("CalculateAvatarPerformancePatch"), null);
		}

		private static bool CalculateAvatarPerformancePatch(AvatarPerformanceStats __1)
		{
			if (Configuration.GetGeneralConfig().PerformanceNoPerformanceStats)
			{
				for (int i = 0; i < __1._performanceRatingCache.Count; i++)
				{
					__1._performanceRatingCache[i] = PerformanceRating.Good;
				}
				return false;
			}
			return true;
		}
	}
}
