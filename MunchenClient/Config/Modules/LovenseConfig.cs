using System.Collections.Generic;
using MunchenClient.ModuleSystem.Modules;
using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class LovenseConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "LovenseConfig.json";

		public int ConfigVersion = 1;

		public Dictionary<string, PlayerLovensePermissions> PlayerPermissions = new Dictionary<string, PlayerLovensePermissions>();
	}
}
