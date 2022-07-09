using Newtonsoft.Json.Linq;

namespace ServerAPI.Utils
{
	internal class JsonParser
	{
		internal static int GetStatusCode(JObject json)
		{
			if (json == null)
			{
				return -1;
			}
			if (json.ContainsKey("StatusCode"))
			{
				return (int)json["StatusCode"];
			}
			return -1;
		}

		internal static string GetStatusError(JObject json)
		{
			if (json == null)
			{
				return string.Empty;
			}
			if (json.ContainsKey("Status"))
			{
				return (string?)json["Status"];
			}
			return string.Empty;
		}
	}
}
