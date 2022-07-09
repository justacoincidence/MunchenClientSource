using System.Collections.Generic;
using MunchenClient.ModuleSystem.Modules;
using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class AvatarHistoryConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "AvatarHistory.json";

		public int ConfigVersion = 1;

		public List<SavedAvatar> AvatarHistory = new List<SavedAvatar>();
	}
}
