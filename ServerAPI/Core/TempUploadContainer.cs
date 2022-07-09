using MunchenClient.ModuleSystem.Modules;

namespace ServerAPI.Core
{
	public struct TempUploadContainer
	{
		internal int uploadType;

		internal FavoriteAvatar saved_avatar;

		internal string player_id;

		internal string player_name;

		internal string player_custom_name;

		internal string player_custom_color;

		internal string discordLink;

		internal string avatar_id;

		internal bool avatar_quest;
	}
}
