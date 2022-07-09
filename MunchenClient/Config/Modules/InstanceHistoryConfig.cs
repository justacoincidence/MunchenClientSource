using System.Collections.Generic;
using MunchenClient.ModuleSystem.Modules;
using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class InstanceHistoryConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "InstanceHistory.json";

		public int ConfigVersion = 1;

		public List<SavedInstance> InstanceHistory = new List<SavedInstance>();
	}
}
