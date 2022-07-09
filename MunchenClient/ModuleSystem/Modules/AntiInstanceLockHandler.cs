using System.Collections;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Wrappers;
using UnityEngine;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class AntiInstanceLockHandler : ModuleComponent
	{
		protected override string moduleName => "Anti Instance Lock Handler";

		internal override void OnLevelWasLoaded(int levelIndex)
		{
			if (levelIndex == -1 && Configuration.GetGeneralConfig().AntiInstanceLock)
			{
				MelonCoroutines.Start(UnlockInstance());
			}
		}

		private IEnumerator UnlockInstance()
		{
			while (PlayerWrappers.GetCurrentPlayer() == null)
			{
				yield return new WaitForEndOfFrame();
			}
			VRC_EventLog.field_Internal_Static_VRC_EventLog_0.field_Internal_EventReplicator_0.field_Private_Boolean_0 = true;
		}
	}
}
