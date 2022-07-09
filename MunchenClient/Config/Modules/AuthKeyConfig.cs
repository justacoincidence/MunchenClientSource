using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class AuthKeyConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetClientFolderPath() + "AuthToken.json";

		public int ConfigVersion = 1;

		public string Token = string.Empty;
	}
}
