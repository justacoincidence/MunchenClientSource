using VRC.Core;

namespace MunchenClient.ModuleSystem.Modules
{
	public struct SavedInstance
	{
		public string ID;

		public string Name;

		public string Tags;

		public string Owner;

		public InstanceAccessType AccessType;

		public NetworkRegion Region;

		public string Nonce;

		public string ClientVersion;

		public int Capacity;

		public string ThumbnailURL;
	}
}
