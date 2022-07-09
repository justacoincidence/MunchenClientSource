using System.Collections.Generic;
using MunchenClient.Utils;
using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class AvatarCalibrationsConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "AvatarCalibrations.json";

		public int ConfigVersion = 1;

		public bool SaveCalibrations = false;

		public bool CalibrationMirror = false;

		public Dictionary<string, CachedCalibration> SavedCalibrations = new Dictionary<string, CachedCalibration>();
	}
}
