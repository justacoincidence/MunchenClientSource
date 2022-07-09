using System;
using System.Reflection;
using HarmonyLib;
using MunchenClient.Utils;

namespace MunchenClient.Patching
{
	internal class PatchComponent
	{
		private Type patchType = null;

		protected virtual string patchName => "Undefined Patch";

		internal string GetPatchName()
		{
			return patchName;
		}

		protected void InitializeLocalPatchHandler(Type type)
		{
			patchType = type;
		}

		protected void PatchMethod(MethodBase targetMethod, HarmonyMethod preMethod, HarmonyMethod postMethod)
		{
			if (targetMethod == null)
			{
				ConsoleUtils.Info(patchName, "Cannot patch null method", ConsoleColor.Gray, "PatchMethod", 28);
			}
			else if (preMethod == null && postMethod == null)
			{
				ConsoleUtils.Info(patchName, "Cannot patch " + targetMethod.Name + " since no valid Pre/Post method was found", ConsoleColor.Gray, "PatchMethod", 35);
			}
			else
			{
				Patcher.PatchMethod(targetMethod, preMethod, postMethod);
			}
		}

		protected bool CheckUsing(MethodInfo method, string match, Type type)
		{
			return PatchManager.CheckUsing(method, match, type);
		}

		protected bool CheckUsed(MethodBase methodBase, string methodName)
		{
			return PatchManager.CheckUsed(methodBase, methodName);
		}

		protected bool CheckMethod(MethodBase methodBase, string match)
		{
			return PatchManager.CheckMethod(methodBase, match);
		}

		protected bool CheckNonGlobalMethod(MethodBase methodBase, string match)
		{
			return PatchManager.CheckNonGlobalMethod(methodBase, match);
		}

		protected HarmonyMethod GetLocalPatch(string name)
		{
			return PatchManager.GetLocalPatch(patchType, name);
		}

		internal virtual void OnInitializeOnStart()
		{
		}

		internal virtual void OnInitializeOnUIInit()
		{
		}
	}
}
