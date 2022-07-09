using System.Linq;
using System.Reflection;
using MelonLoader;
using MunchenClient.Config;
using RootMotion.FinalIK;

namespace MunchenClient.Patching.Patches
{
	internal class FinalIKPatch : PatchComponent
	{
		protected override string patchName => "FinalIKPatch";

		internal override void OnInitializeOnStart()
		{
			InitializeLocalPatchHandler(typeof(FinalIKPatch));
			PatchMethod((from m in typeof(IKSolverHeuristic).GetMethods()
				where m.Name.Equals("IsValid") && m.GetParameters().Length == 1
				select m).First(), GetLocalPatch("IsIKSolverValidPatch"), null);
			PatchMethod(typeof(IKSolverAim).GetMethod("GetClampedIKPosition"), GetLocalPatch("IKSolverAimGetClampedIKPositionPatch"), null);
			PatchMethod(typeof(IKSolverFullBody).GetMethod("Solve"), GetLocalPatch("IKSolverFullBodySolvePatch"), null);
			PatchMethod(typeof(IKSolverFABRIKRoot).GetMethod("OnUpdate"), GetLocalPatch("IKSolverFABRIKRootOnUpdatePatch"), null);
		}

		private static bool IsIKSolverValidPatch(ref IKSolverHeuristic __instance, ref bool __result, ref string message)
		{
			if (!Configuration.GetAntiCrashConfig().AntiFinalIKCrash)
			{
				return true;
			}
			if (__instance.maxIterations > 64)
			{
				__result = false;
				message = "Iteration limit reached";
				return false;
			}
			return true;
		}

		private static bool IKSolverAimGetClampedIKPositionPatch(ref IKSolverAim __instance)
		{
			if (!Configuration.GetAntiCrashConfig().AntiFinalIKCrash)
			{
				return true;
			}
			__instance.clampSmoothing = MelonUtils.Clamp(__instance.clampSmoothing, 0, 2);
			return true;
		}

		private static bool IKSolverFullBodySolvePatch(ref IKSolverFullBody __instance)
		{
			if (!Configuration.GetAntiCrashConfig().AntiFinalIKCrash)
			{
				return true;
			}
			__instance.iterations = MelonUtils.Clamp(__instance.iterations, 0, 10);
			return true;
		}

		private static bool IKSolverFABRIKRootOnUpdatePatch(ref IKSolverFABRIKRoot __instance)
		{
			if (!Configuration.GetAntiCrashConfig().AntiFinalIKCrash)
			{
				return true;
			}
			__instance.iterations = MelonUtils.Clamp(__instance.iterations, 0, 10);
			return true;
		}
	}
}
