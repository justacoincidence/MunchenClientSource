using System.Collections.Generic;
using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class TestedAssetBundlesConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "TestedAssetBundlesConfig.json";

		public int ConfigVersion = 1;

		public Dictionary<string, List<int>> TestedBundles = new Dictionary<string, List<int>>();
	}
}
