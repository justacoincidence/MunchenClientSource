using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class ModerationsConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "ModerationsConfig.json";

		public int ConfigVersion = 1;

		public bool LogModerationsWarnAboutModerators = false;

		public bool ModerationSounds = false;

		public bool LogModerationsLeftFriends = false;

		public bool LogModerationsLeftOthers = false;

		public bool LogModerationsLeftHUD = false;

		public bool LogModerationsJoinFriends = false;

		public bool LogModerationsJoinOthers = false;

		public bool LogModerationsJoinHUD = false;

		public bool LogModerationsWarningsPrevent = false;

		public bool LogModerationsWarningsLog = false;

		public bool LogModerationsWarningsHUD = false;

		public bool LogModerationsMicOffPrevent = false;

		public bool LogModerationsMicOffLog = false;

		public bool LogModerationsMicOffHUD = false;

		public bool LogModerationsBlockPrevent = false;

		public bool ModerationBlockPreventAvatarChange = false;

		public bool LogModerationsBlockLog = false;

		public bool LogModerationsBlockHUD = false;

		public bool LogModerationsMuteLog = false;

		public bool LogModerationsMuteHUD = false;

		public bool LogModerationsInstanceMasterChangeFriends = false;

		public bool LogModerationsInstanceMasterChangeOthers = false;

		public bool LogModerationsInstanceMasterChangeHUD = false;

		public bool LogModerationsInvite = false;

		public bool LogModerationsInviteHUD = false;

		public bool LogModerationsRequestInvite = false;

		public bool LogModerationsRequestInviteHUD = false;

		public bool LogModerationsDeniedRequestInvite = false;

		public bool LogModerationsDeniedRequestInviteHUD = false;

		public bool LogModerationsVotekickFriends = false;

		public bool LogModerationsVotekickOthers = false;

		public bool LogModerationsVotekickHUD = false;

		public bool LogModerationsFriendRequest = false;

		public bool LogModerationsFriendRequestHUD = false;

		public bool LogModerationsOnline = false;

		public bool LogModerationsOnlineHUD = false;

		public bool LogModerationsOffline = false;

		public bool LogModerationsOfflineHUD = false;

		public bool LogModerationsInstanceSwitch = false;

		public bool LogModerationsInstanceSwitchHUD = false;
	}
}
