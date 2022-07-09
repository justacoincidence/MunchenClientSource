using System.Collections.Generic;
using UnityEngine;

namespace MunchenClient.Core
{
	internal class PerformanceProfiler
	{
		private static readonly Dictionary<string, float> profiler = new Dictionary<string, float>();

		internal static void StartProfiling(string id)
		{
			if (profiler.ContainsKey(id))
			{
				profiler[id] = Time.realtimeSinceStartup;
			}
			else
			{
				profiler.Add(id, Time.realtimeSinceStartup);
			}
		}

		internal static float EndProfiling(string id)
		{
			if (profiler.ContainsKey(id))
			{
				float num = (Time.realtimeSinceStartup - profiler[id]) * 1000f;
				profiler[id] = num;
				return num;
			}
			return 0f;
		}

		internal static float GetProfiling(string id)
		{
			if (profiler.ContainsKey(id))
			{
				return profiler[id];
			}
			return -1f;
		}
	}
}
