using VRC.Core;

namespace MunchenClient.ModuleSystem.Modules
{
	public class FavoriteAvatar
	{
		public int AvatarSortIndex;

		public int AvatarVersionSystem;

		public string AvatarID;

		public string AvatarName;

		public string AvatarDescription;

		public int AvatarVersion;

		public int AvatarApiVersion;

		public string AvatarAssetUrl;

		public string AvatarImageUrl;

		public string AvatarReleaseStatus;

		public string AvatarAuthorID;

		public string AvatarAuthorName;

		public string AvatarPlatform;

		public ApiModel.SupportedPlatforms AvatarSupportedPlatforms;

		public FavoriteAvatar()
		{
		}

		public FavoriteAvatar(ApiAvatar avi)
		{
			AvatarVersionSystem = 2;
			AvatarName = avi.name;
			AvatarID = avi.id;
			AvatarDescription = avi.description;
			AvatarVersion = avi.version;
			AvatarApiVersion = avi.apiVersion;
			AvatarAssetUrl = avi.assetUrl;
			AvatarImageUrl = avi.thumbnailImageUrl;
			AvatarReleaseStatus = avi.releaseStatus;
			AvatarAuthorID = avi.authorId;
			AvatarAuthorName = avi.authorName;
			AvatarPlatform = avi.platform;
			AvatarSupportedPlatforms = avi.supportedPlatforms;
		}

		internal ApiAvatar ToApiAvatar()
		{
			return new ApiAvatar
			{
				id = AvatarID,
				name = AvatarName,
				description = AvatarDescription,
				version = AvatarVersion,
				apiVersion = AvatarApiVersion,
				assetUrl = AvatarAssetUrl,
				imageUrl = AvatarImageUrl,
				thumbnailImageUrl = AvatarImageUrl,
				releaseStatus = AvatarReleaseStatus,
				authorId = AvatarAuthorID,
				authorName = AvatarAuthorName,
				platform = AvatarPlatform,
				supportedPlatforms = AvatarSupportedPlatforms
			};
		}
	}
}
