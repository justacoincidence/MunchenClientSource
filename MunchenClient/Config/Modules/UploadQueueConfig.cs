using System.Collections.Generic;
using Newtonsoft.Json;
using ServerAPI.Core;

namespace MunchenClient.Config.Modules
{
	public class UploadQueueConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "UploadQueue.json";

		public int ConfigVersion = 1;

		public Queue<TempUploadContainer> UploadQueue = new Queue<TempUploadContainer>();
	}
}
