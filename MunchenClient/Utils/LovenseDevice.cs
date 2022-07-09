namespace MunchenClient.Utils
{
	internal class LovenseDevice
	{
		internal string id;

		internal string name;

		internal sbyte battery;

		internal string firmwareVersion;

		internal string nickname;

		internal string mode;

		internal string version;

		internal LovenseDeviceType type;

		internal byte intensity;

		internal string GetName()
		{
			return string.IsNullOrEmpty(nickname) ? (name + " " + version) : nickname;
		}
	}
}
