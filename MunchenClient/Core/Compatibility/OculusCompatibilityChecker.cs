using System;

namespace MunchenClient.Core.Compatibility
{
	internal class OculusCompatibilityChecker
	{
		internal static void CheckOculusCompatibility()
		{
			Type typeFromHandle = typeof(VRCTrackingSteam);
			string name = typeFromHandle.Name;
			name.Clone();
		}
	}
}
