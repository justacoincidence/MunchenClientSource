using Newtonsoft.Json;

namespace MunchenClient.Config.Modules
{
	public class LanguageConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "CustomLanguage.json";

		public int ConfigVersion = 1;

		public string ClientName = "MünchenClient";

		public string ClientDescription = "A powerful VRChat Client with many features";

		public string PCText = "PC";

		public string VRText = "VR";

		public string QuestText = "Quest";

		public string ConfirmText = "Confirm";

		public string CancelText = "Cancel";

		public string SuccessText = "Success";

		public string ErrorText = "Error";

		public string NoticeText = "Notice";

		public string ResetText = "Reset";

		public string UserAccountCategory = "User Account";

		public string IndicatorsCategory = "Indicators";

		public string FeaturesCategory = "Features";

		public string ResizeCategory = "Resize";

		public string AspectRatioCategory = "Aspect Ratio";

		public string NameplatesCategory = "Nameplates";

		public string MiscellaneousCategory = "Miscellaneous";

		public string ExperimentalCategory = "Experimental";

		public string SpecificCategory = "Specific";

		public string CrasherCategory = "Crasher";

		public string AvatarCategory = "Avatar";

		public string RanksCategory = "Ranks";

		public string RestartText = "Restart";

		public string RestartLaterText = "Restart Later";

		public string FeatureRequiresRestartText = "Changing this feature requires a restart. Restart now?";

		public string ClearVRAM = "Clear VRAM";

		public string ClearVRAMClicked = "VRAM Cleared (took: {TimeToClear} ms)";

		public string ClearVRAMDescription = "Clears your RAM/VRAM potentially increasing FPS";

		public string Discord = "Discord Server";

		public string DiscordDescription = "Join our official discord and be a part of the community";

		public string HideYourself = "Hide Yourself";

		public string HideYourselfDescription = "Hide/Unhide your own avatar (Local Only)";

		public string ClearHUDMessages = "Clear HUD Messages";

		public string ClearHUDMessagesClicked = "Cleared HUD messages";

		public string ClearHUDMessagesDescription = "Clears all currently queued messages and shown on screen";

		public string ProtectionsMenuName = "Protections";

		public string ProtectionsMenuDescription = "A menu full of protection options and safety related features";

		public string ProtectionsOtherMenuName = "Other Protections";

		public string AntiAvatarIntroMusic = "Anti Avatar Intro Music";

		public string AntiAvatarIntroMusicDescription = "Stops avatars from playing their annoying intro music";

		public string BlockAllRPCEvents = "Block RPC Events";

		public string BlockAllRPCEventsDescription = "Blocks all incoming RPC events (WARNING: THIS WILL WILL BREAK AVATAR & WORLD RELATED STUFF)";

		public string BlockAllUdonEvents = "Block Udon Events";

		public string BlockAllUdonEventsDescription = "Blocks all incoming Udon events (WARNING: THIS WILL BREAK UDON GAMEPLAY)";

		public string BlockAllPhotonEvents = "Block Photon Events";

		public string BlockAllPhotonEventsDescription = "Blocks all incoming Photon events (WARNING: THIS WILL BREAK SOME STUFF)";

		public string PortalsMenuName = "Portals";

		public string PortalsMenuDescription = "Safety features related to portals";

		public string RemoveAllPortals = "Remove All Portals";

		public string RemoveAllPortalsDescription = "Deletes all currently dropped portals in the lobby";

		public string PortalConfirmation = "Portal Confirmation";

		public string PortalConfirmationDescription = "Creates a popup asking if you really want to go through a portal you're about to enter";

		public string AutoDeleteOtherPortals = "Delete Others Portals";

		public string AutoDeleteOtherPortalsDescription = "Auto deletes portals dropped by non friends";

		public string AutoDeleteFriendsPortals = "Delete Friends Portals";

		public string AutoDeleteFriendsPortalsDescription = "Auto deletes portals dropped by friends";

		public string AutoDeleteDistantPlacedPortals = "Delete Distant Placed Portals";

		public string AutoDeleteDistantPlacedPortalsDescription = "Auto deletes portals that are dropped far from their owner";

		public string AllowFriendsDistantPlacedPortals = "Allow Friends Distant Placed Portals";

		public string AllowFriendsDistantPlacedPortalsDescription = "Allows friends to place distant portals from them";

		public string DeleteUnusedPortals = "Delete Unused Portals";

		public string DeleteUnusedPortalsDescription = "Portals that are unused and doesn't count down will be automatically deleted";

		public string DeleteBadlyConfiguredPortals = "Delete Badly Configured Portals";

		public string DeleteBadlyConfiguredPortalsDescription = "Portals that are maliciously configured will be automatically deleted";

		public string PortalConfirmationRandomInstance = "Random Instance";

		public string PortalConfirmationInstance = "Instance: {instanceId}";

		public string PortalConfirmationDropper = "Dropper: {username}";

		public string PortalConfirmationEnter = "Enter";

		public string PortalDroppedByFriendRemoved = "Deleted portal dropped by friend: {username}";

		public string PortalDroppedByStrangerRemoved = "Deleted portal dropped by stranger: {username}";

		public string PortalDroppedBadlyConfiguredRemoved = "Deleted portal from {username} for being badly configured";

		public string PortalDroppedUnused = "Deleted portal from {username} for being unused";

		public string PortalDroppedToofarRemoved = "Deleted portal from {username} for being dropped too far from them (Distance: {distance})";

		public string ModerationMenuName = "Moderation";

		public string ModerationMenuDescription = "Moderation features related to people joining, leaving, blocking you, etc";

		public string EnableModerationLogging = "Enable Moderation Logging";

		public string EnableModerationLoggingDescription = "Global toggle for all features in this tab";

		public string VRChatStaffWarning = "VRChat Staff Warning";

		public string VRChatStaffWarningDescription = "Alerts you if you encounter a VRChat Staff in any instance";

		public string ModerationSounds = "Moderation Sounds";

		public string ModerationSoundsDescription = "Plays a audible cue whenever a moderation happens";

		public string ModerationLogFriends = "Log Friends";

		public string ModerationLogFriendsDescription = "Logs friends";

		public string ModerationLogOthers = "Log Others";

		public string ModerationLogOthersDescription = "Logs strangers";

		public string ModerationLogToHud = "Log To Hud";

		public string ModerationLogToHudDescription = "Logs to hud";

		public string ModerationLogToConsole = "Log To Console";

		public string ModerationLogToConsoleDescription = "Logs to console";

		public string ModerationPlayerJoined = "<color=purple>{username} <color=white>has <color=green>joined";

		public string ModerationPlayerLeft = "<color=purple>{username} <color=white>has <color=red>left";

		public string ModerationPlayerMasterJoined = "Instance Master: <color=purple>{username}";

		public string ModerationPlayerMasterSwitched = "New Instance Master: <color=purple>{username}";

		public string ModerationFriendRequest = "<color=purple>{username} <color=white>sent you a friend request";

		public string ModerationInvitation = "<color=purple>{username} <color=white>sent you a invite to {world}";

		public string ModerationRequestedInvite = "<color=purple>{username} <color=white>requested invite to your world";

		public string ModerationInviteRequestDenied = "<color=purple>{username} <color=white>denied your invite request";

		public string ModerationVotekick = "Votekick started against <color=purple>{username}";

		public string ModerationPreventPlayerJoin = "Prevented <color=purple>{username} <color=white>from joining the instance";

		public string ModerationSerializationDetected = "<color=purple>{username} <color=white>is using serialization";

		public string ModerationInvisibleDetected = "<color=purple>{username} <color=white>joined while being invisible";

		public string ModerationEventLagDetected = "<color=purple>{username} <color=white>is using <color=red>Event {event} Lagger";

		public string ModerationFreezeLagDetected = "<color=purple>{username} <color=white>is using a lag exploit";

		public string ModerationEarrapeDetected = "<color=purple>{username} <color=white>is using <color=red>earrape exploit";

		public string ModerationImageThumbnailUnsuitable = "Unsuitable image prevented: <color=purple>{link}";

		public string ModerationVideoPlayerUnsuitable = "Unsuitable video prevented: <color=purple>{link}";

		public string ModerationWarnDetected = "<color=purple>{username} <color=white>tried to warn you";

		public string ModerationMicOffDetected = "<color=purple>{username} <color=white>tried to turn your microphone off";

		public string ModerationMuteDetected = "<color=purple>{username} <color=white>muted you";

		public string ModerationUnmuteDetected = "<color=purple>{username} <color=white>unmuted you";

		public string ModerationBlockDetected = "<color=purple>{username} <color=white>blocked you";

		public string ModerationUnblockDetected = "<color=purple>{username} <color=white>unblocked you";

		public string ModerationJoinMenuName = "Join";

		public string ModerationJoinMenuDescription = "Features related to people joining your instance";

		public string ModerationLeaveMenuName = "Leave";

		public string ModerationLeaveMenuDescription = "Features related to people leaving your instance";

		public string ModerationBlockMenuName = "Block";

		public string ModerationBlockMenuDescription = "Features related to people blocking you";

		public string ModerationBlockPrevent = "Prevent Blocks";

		public string ModerationBlockPreventDescription = "People who block you will still be visible (They still can't see you, but you can see them)";

		public string ModerationBlockPreventAvatarChange = "Prevent Avatar Change";

		public string ModerationBlockPreventAvatarChangeDescription = "Prevents avatar changes on people who got you blocked (Useful for people with bypassers on target crash)";

		public string ModerationMicOffMenuName = "Mic Off";

		public string ModerationMicOffMenuNameDescription = "Features related to people turning your mic off";

		public string ModerationMicOffPrevent = "Prevent Mic Off";

		public string ModerationMicOffPreventDescription = "Prevents instance owners from turning your microphone off";

		public string ModerationMuteMenuName = "Mute";

		public string ModerationMuteMenuDescription = "Features related to people muting you";

		public string ModerationWarnMenuName = "Warn";

		public string ModerationWarnMenuDescription = "Features related to people warning you";

		public string ModerationWarnPrevent = "Prevent Warnings";

		public string ModerationWarnPreventDescription = "Prevents instance owners from giving you warnings";

		public string ModerationInstanceMasterMenuName = "Instance Master";

		public string ModerationInstanceMasterMenuDescription = "Tells you who is the new instance master";

		public string ModerationInviteMenuName = "Invites";

		public string ModerationInviteMenuDescription = "Lets you know when people are sending you invites";

		public string ModerationRequestInviteMenuName = "Request Invites";

		public string ModerationRequestInviteMenuDescription = "Lets you know when people are sending you request invites";

		public string ModerationDeniedRequestInviteMenuName = "Denied Request Invites";

		public string ModerationDeniedRequestInviteMenuDescription = "Lets you know when people are denying your request invites";

		public string ModerationVotekickMenuName = "Votekick";

		public string ModerationVotekickMenuDescription = "Lets you know who is getting votekicked";

		public string ModerationFriendRequestMenuName = "Friend Request";

		public string ModerationFriendRequestMenuDescription = "Lets you know when people are sending you friend requests";

		public string ModerationOnlineMenuName = "Online";

		public string ModerationOnlineMenuDescription = "Lets you know when people are going online";

		public string ModerationOfflineMenuName = "Offline";

		public string ModerationOfflineMenuDescription = "Lets you know when people are going offline";

		public string ModerationInstanceSwitchMenuName = "Instance Switch";

		public string ModerationInstanceSwitchMenuDescription = "Lets you know when people are switching instance to another";

		public string AntiIPLoggingVideoPlayerSafety = "Video Player Safety";

		public string AntiIPLoggingVideoPlayerSafetyDescription = "Protects you from certain urls people try play via video players";

		public string AntiIPLoggingImageThumbnailSafety = "Image Thumbnail Safety";

		public string AntiIPLoggingImageThumbnailSafetyDescription = "Protects you from certain urls when trying to see avatar/world thumbnails";

		public string SerializationDetection = "Serialization Detection";

		public string SerializationDetectionDescription = "Tries to correctly identify people who use serialization and notify about it";

		public string InvisibleDetection = "Invisible Detection";

		public string InvisibleDetectionDescription = "Tries to correctly identify people who join with invisibility on";

		public string AntiCrashMenuName = "Anti Crash";

		public string AntiCrashMenuDescription = "Features related to preventing people from crashing you";

		public string WhitelistAvatarByID = "Whitelist Avatar By ID";

		public string WhitelistAvatarByIDDescription = "Allows you to whitelist an avatar and prevent it from being cleaned by the anti crash system";

		public string AntiCrashEnablePublic = "Enable In Public";

		public string AntiCrashEnablePublicDescription = "Enables the AntiCrash in public lobbies";

		public string AntiCrashEnableInvite = "Enable In Invite";

		public string AntiCrashEnableInviteDescription = "Enables the AntiCrash in invite lobbies";

		public string AntiCrashEnableInvitePlus = "Enable In InvitePlus";

		public string AntiCrashEnableInvitePlusDescription = "Enables the AntiCrash in invite plus lobbies";

		public string AntiCrashEnableFriends = "Enable In Friends";

		public string AntiCrashEnableFriendsDescription = "Enables the AntiCrash in friends lobbies";

		public string AntiCrashEnableFriendsPlus = "Enable In FriendsPlus";

		public string AntiCrashEnableFriendsPlusDescription = "Enables the AntiCrash in friends plus lobbies";

		public string AntiShaderCrash = "Anti Shader Crash";

		public string AntiShaderCrashDescription = "Prevents maliciously crafted shaders from crashing you";

		public string AntiAudioCrash = "Anti Audio Crash";

		public string AntiAudioCrashDescription = "Prevents avatars with a high number of audio sources from crashing you";

		public string AntiMeshCrash = "Anti Mesh Crash";

		public string AntiMeshCrashDescription = "Tries to prevent maliciously crafted meshes from crashing you";

		public string AntiMaterialCrash = "Anti Material Crash";

		public string AntiMaterialCrashDescription = "Prevents an avatar with too many materials from crashing you";

		public string AntiFinalIKCrash = "Anti FinalIK Crash";

		public string AntiFinalIKCrashDescription = "Prevents a all known exploits regarding FinalIK from crashing you";

		public string AntiClothCrash = "Anti Cloth Crash";

		public string AntiClothCrashDescription = "Prevents maliciously crafted cloth from crashing you";

		public string AntiParticleSystemCrash = "Anti ParticleSystem Crash";

		public string AntiParticleSystemCrashDescription = "Prevents particle systems from crashing you";

		public string AntiDynamicBoneCrash = "Anti DynamicBone Crash";

		public string AntiDynamicBoneCrashDescription = "Prevents dynamic bones from crashing you";

		public string AntiBlendShapeCrash = "Anti BlendShape Crash";

		public string AntiBlendShapeCrashDescription = "Tries to prevent BlendShape crashes from happening from maliciously crafted avatars";

		public string AntiLightSourceCrash = "Anti LightSource Crash";

		public string AntiLightSourceCrashDescription = "Prevents too many lightsources from lagging you out";

		public string AntiAvatarLoadingCrash = "Anti Avatar Loading Crash";

		public string AntiAvatarLoadingCrashDescription = "Prevents broken assetbundles from crashing you";

		public string AntiAvatarAudioMixerCrash = "Anti Avatar AudioMixer Crash";

		public string AntiAvatarAudioMixerCrashDescription = "Prevents avatars with maliciously crafted audio mixers from RAM crashing you";

		public string AntiPhysicsCrash = "Anti Physics Crash";

		public string AntiPhysicsCrashDescription = "Makes every physics related component uncrashable";

		public string AntiInvalidFloatsCrash = "Anti Invalid Floats";

		public string AntiInvalidFloatsCrashDescription = "Prevents invalid float numbers from crashing your game";

		public string AntiAvatarDescriptorCrash = "Anti AvatarDescriptor Crash";

		public string AntiAvatarDescriptorCrashDescription = "Prevents a few exploits crashing your game on the expression wheel";

		public string AntiAssetBundleCrash = "Anti Asset Bundle";

		public string AntiAssetBundleCrashDescription = "Prevents corrupted asset bundles from crashing you (WARNING: Might increase loading time from 2x - 3x)";

		public string ExperimentalAvatarBlocker = "Experimental Avatar Blocker";

		public string ExperimentalAvatarBlockerDescription = "If an avatar fails to load and crashes your game, then it won't load next time you see it";

		public string GlobalAvatarBlacklist = "Global Avatar Blacklist";

		public string GlobalAvatarBlacklistDescription = "Prevents downloading a known crashing avatar if it's in our predefined database";

		public string AntiCrashCheckedAvatar = "Checked {username}'s avatar ({processingtime} ms)";

		public string AntiCrashCheckedAvatarID = "Avatar ID: {id}";

		public string AntiCrashCheckedAvatarName = "Avatar Name: {name}";

		public string AntiCrashCheckedAvatarDownload = "Avatar Download: {link}";

		public string AntiCrashProcessedAvatar = "Processing avatar took {processingtime} ms";

		public string AntiExploitMenuName = "Anti Exploit";

		public string AntiExploitMenuDescription = "Features related to preventing people from using exploits";

		public string AntiFreezeExploit = "Anti Freeze Exploit";

		public string AntiFreezeExploitDescription = "Prevents malicious events from 'freezing' your game for an extended period of time";

		public string AntiInstanceLock = "Anti Instance Lock";

		public string AntiInstanceLockDescription = "Prevents Instance Lock from affecting you so you can force join instances";

		public string AntiEarrapeExploit = "Anti Earrape Exploit";

		public string AntiEarrapeExploitDescription = "Tries to prevent the insanely loud earrape exploit";

		public string AntiWorldTriggers = "Anti World Triggers";

		public string AntiWorldTriggersDescription = "Tries to prevent world triggers from getting exploited which shouldn't be networked";

		public string SpooferMenuName = "Spoofers";

		public string SpooferMenuDescription = "Helping features that spoofs several things thus increases your privacy";

		public string SpooferRealisticMode = "Realistic Spoofer Mode";

		public string SpooferRealisticModeDescription = "Realistically imitates what a real world ping/fps would look like to make you less detectable";

		public string SetSpoofedFPS = "Set Spoofed FPS";

		public string SetSpoofedFPSDescription = "Sets your spoofed FPS to something of your own liking";

		public string SetSpoofedPing = "Set Spoofed Ping";

		public string SetSpoofedPingDescription = "Sets your spoofed Ping to something of your own liking";

		public string HWIDSpoofer = "HWID Spoofer";

		public string HWIDSpooferDescription = "This feature enables/disables the hardware id spoofer, saves you from getting hwid banned";

		public string PeripheralSpoofer = "Peripheral Spoofer";

		public string PeripheralSpooferDescription = "Spoofs several devices on your computer to make you even more undetectable";

		public string SteamIDSpoofer = "SteamID Spoofer";

		public string SteamIDSpooferDescription = "This feature completely disables steam integration - saves you from getting steam banned and tracked by people";

		public string PingSpoofer = "Ping Spoofer";

		public string PingSpooferDescription = "This feature enables/disables the ping spoofer, so all the skids can't see your real ping and determine where you're from";

		public string FPSSpoofer = "FPS Spoofer";

		public string FPSSpooferDescription = "This feature enables/disables the fps spoofer, saves you from skids trying to call you out for having a bad pc";

		public string AutoRegenerate = "Auto Regenerate Spoofers";

		public string AutoRegenerateDescription = "Auto regenerates the several hardware ids each time you start the game";

		public string RegenerateHWID = "Regenerate HWID";

		public string RegenerateHWIDDescription = "Regenerates your hardware id";

		public string RegenerateHWIDNotice = "Are you sure you want to regenerate HWID?";

		public string RegenerateHWIDText = "Regenerate";

		public string SettingsMenuName = "Settings";

		public string SettingsMenuDescription = "Configure the client's settings and make it more comfortable for yourself";

		public string RestartGame = "Restart Game";

		public string RestartGameDescription = "Restarts your game immediately";

		public string ReloadAvatars = "Reload Avatars";

		public string ReloadAvatarsReloaded = "Reloaded all avatars";

		public string ReloadAvatarsDescription = "Reloads all avatars in the current instance";

		public string CopyAvatarData = "Copy Avatar Data";

		public string CopyAvatarDataCopied = "Copied Avatar Asset to clipboard";

		public string CopyAvatarDataDescription = "Copies the local player's current avatar asset url";

		public string ForceClone = "Force Clone";

		public string ForceCloneDescription = "Allows you to force clone any avatar except private ones";

		public string ForceClonePerformed = "Force cloned avatar from: {username}";

		public string MinimumCameraClippingDistance = "Minimum Clipping Distance";

		public string MinimumCameraClippingDistanceDescription = "Allows you to go even closer to people without them disappearing";

		public string LocalizedClient = "Localized Client";

		public string LocalizedClientDescription = "Translates the whole client into your own language (If supported)";

		public string LocalizedClientCustom = "Custom Language";

		public string LocalizedClientCustomDescription = "Uses your own customized language of choice (Requires Localized Client to be enabled)";

		public string AutoClearCache = "Auto Cache Clear";

		public string AutoClearCacheDescription = "Upon launching the game and closing the game, it will auto clear your cache, potentially freeing up space";

		public string SetCustomCrasher = "Set Custom Crashing Avatars";

		public string SetCustomCrasherDescription = "Lets you use a custom avatar to crash people with (Leave blank to reset)";

		public string SetCustomCrasherQuestion = "Which crasher do you want to set?";

		public string SetCustomCrasherPlaceholderText = "Insert avatar id(leave blank to reset)";

		public string SetCustomCrasherPC = "Change PC Crasher";

		public string SetCustomCrasherSuccessResetPC = "Successfully reset crasher avatar for PC";

		public string SetCustomCrasherSuccessSetPC = "Successfully set crasher avatar for PC";

		public string SetCustomCrasherFailedPC = "Failed setting PC crasher ({error})";

		public string SetCustomCrasherQuest = "Change Quest Crasher";

		public string SetCustomCrasherSuccessResetQuest = "Successfully reset crasher avatar for Quest";

		public string SetCustomCrasherSuccessSetQuest = "Successfully set crasher avatar for Quest";

		public string SetCustomCrasherFailedQuest = "Failed setting Quest crasher ({error})";

		public string ForceServerSync = "Force Server Sync";

		public string ForceServerSyncClicked = "Successfully synced data";

		public string ForceServerSyncDescription = "Forces the server to sync all data to you so you're up to date in case something didn't automatically sync";

		public string AvatarDownloadLogging = "Avatar Download Logging";

		public string AvatarDownloadLoggingDescription = "Logs in your console for each avatar that is being downloaded (useful for identifying crashers)";

		public string ComfyVRMenu = "Comfy VR Menu";

		public string ComfyVRMenuDescription = "Spawns your big menu in front of you when laying down instead underneath you";

		public string PersistentQuickMenu = "Persistent QuickMenu";

		public string PersistentQuickMenuDescription = "Opens the last quickmenu you've opened";

		public string NetworkedEmotes = "Networked Emotes";

		public string NetworkedEmotesDescription = "Makes your emote's sound hearable by other people who has this setting enabled too";

		public string PerformanceMenuName = "Performance";

		public string PerformanceMenuDescription = "Features that increases your fps";

		public string PerformanceLimitFPSToMonitor = "Limit FPS To Monitor";

		public string PerformanceLimitFPSToMonitorDescription = "Limits your fps to your monitor's refresh rate which is {RefreshRate} Hz (Desktop Only)";

		public string PerformanceHighPriority = "High Game Priority";

		public string PerformanceHighPriorityDescription = "When enabled it will let Windows prioritize VRChat over certain other programs";

		public string PerformanceImageCache = "Image Cache";

		public string PerformanceImageCacheDescription = "Caches all images, reducing both network load, and increasing performance";

		public string PerformanceLowGraphicsMode = "Low Graphics Mode";

		public string PerformanceLowGraphicsModeDescription = "Caches every image reducing both network load and increasing performance";

		public string PerformanceNoAA = "No AntiAliasing";

		public string PerformanceNoAADescription = "Disables AntiAliasing which is typically a very demanding feature";

		public string PerformanceNoShadows = "No Shadows";

		public string PerformanceNoShadowsDescription = "Disables Realtime Shadows which is typically a very demanding feature";

		public string PerformanceNoHT = "No HT";

		public string PerformanceNoHTDescription = "When enabled it will disable HyperThreading, which will possibly cause higher fps";

		public string PerformanceNoPerfStats = "No Avatar Performance Calculation";

		public string PerformanceNoPerfStatsDescription = "Disables the performance calculation completely thus reducing lagspikes when loading avatars";

		public string PerformanceSmartFPS = "Smart FPS";

		public string PerformanceSmartFPSDescription = "Reduces FPS to a minimum when tabbed out and restores to normal once tabbed in again";

		public string PerformanceInputDelayReducer = "Input Delay Reducer";

		public string PerformanceInputDelayReducerDescription = "Reduces maximum amount of queued frames allowed which could potentially lead to a 16.6 ms or more input delay reduction, but may also decrease your fps";

		public string ColorChangerMenuName = "Color Changer";

		public string ColorChangerMenuDescription = "Recolor various stuff to your own liking";

		public string ColorChangerMenuOptionsHeader = "Menu Options";

		public string ColorChangerMenuColorsHeader = "Menu Colors";

		public string ColorChangerPlayerWallhackHeader = "Player Wallhack";

		public string ColorChanger = "Enable Color Changer";

		public string ColorChangerDescription = "Enables the whole recoloring system";

		public string ColorChangerRainbowMode = "RGB Mode";

		public string ColorChangerRainbowModeDescription = "Cycles through all the colors";

		public string ColorChangerHyperMode = "Hyper Mode";

		public string ColorChangerHyperModeDescription = "Rainbow mode with LSD (Requires Rainbow Mode)";

		public string ColorChangerCustom = "Custom Color";

		public string ColorChangerCustomDescription = "Changes the current color to your own choice";

		public string ColorChangerMagenta = "Magenta";

		public string ColorChangerMagentaDescription = "Changes the current color to magenta";

		public string ColorChangerCyan = "Cyan";

		public string ColorChangerCyanDescription = "Changes the current color to cyan";

		public string ColorChangerBlue = "Blue";

		public string ColorChangerBlueDescription = "Changes the current color to blue";

		public string ColorChangerRed = "Red";

		public string ColorChangerRedDescription = "Changes the current color to red";

		public string ColorChangerGreen = "Green";

		public string ColorChangerGreenDescription = "Changes the current color to green";

		public string ColorChangerYellow = "Yellow";

		public string ColorChangerYellowDescription = "Changes the current color to yellow";

		public string ColorChangerOrange = "Orange";

		public string ColorChangerOrangeDescription = "Changes the current color to orange";

		public string ColorChangerPink = "Pink";

		public string ColorChangerPinkDescription = "Changes the current color to pink";

		public string ColorChangerRGBFriendsWallhack = "RGB Friends Wallhack";

		public string ColorChangerRGBFriendsWallhackDescription = "Fuck RGB in hardware. We're doing it on friends now!";

		public string ColorChangerRGBStrangersWallhack = "RGB Strangers Wallhack";

		public string ColorChangerRGBStrangersWallhackDescription = "Fuck RGB in hardware. We're doing it on strangers now!";

		public string ColorChangerFailedParsing = "Failed parsing color";

		public string DisableFeaturesMenuName = "Disable Features";

		public string DisableFeaturesMenuDescription = "Toggles related to disabling certain parts of the client";

		public string DisableAllKeybinds = "Disable All Keybinds";

		public string DisableAllKeybindsDescription = "Disables every keybind currently active in the client";

		public string DisableFBTSavingCompletely = "Disable FBT Saving Completely";

		public string DisableFBTSavingCompletelyDescription = "Disables the patches for calibration saving completely (Makes client compatible with clients such as VRTools)";

		public string DisableMenuPlayerButtons = "Disable Menu Player Buttons";

		public string DisableMenuPlayerButtonsDescription = "Disables the client buttons on the social page to interact with a player";

		public string DisableKeyboardImprovements = "Disable Keyboard Improvements";

		public string DisableKeyboardImprovementsDescription = "Disables certain buttons on the keyboard that was put there for QoL changes";

		public string DisableAvatarFavoritesSearchButton = "Disable Favorites Search Button";

		public string DisableAvatarFavoritesSearchButtonDescription = "Disables the search button on the client avatar favorites";

		public string DisableAvatarFavorites = "Disable Avatar Favorites";

		public string DisableAvatarFavoritesDescription = "Disables the client avatar favorites";

		public string DisableAvatarDatabase = "Disable Global Avatar Database";

		public string DisableAvatarDatabaseDescription = "Disables the client global avatar database";

		public string MediaControlsMenuName = "Media Controls";

		public string MediaControlsMenuDescription = "Allows control over the media currently playing systemwide";

		public string MediaControlsPauseUnpause = "(Un)Pause";

		public string MediaControlsPauseUnpauseClicked = "Paused/Unpaused current media";

		public string MediaControlsPauseUnpauseDescription = "Pause or Unpauses your current media that is playing";

		public string MediaControlsNext = "Next";

		public string MediaControlsNextClicked = "Skipped current media";

		public string MediaControlsNextDescription = "Skips the current song and goes on to the next one";

		public string MediaControlsPrevious = "Previous";

		public string MediaControlsPreviousClicked = "Went back to previous media";

		public string MediaControlsPreviousDescription = "Goes a song back to play the previous one";

		public string MediaControlsMuteUnmute = "(Un)Mute";

		public string MediaControlsMuteUnmuteClicked = "Mute/Unmuted currently playing media";

		public string MediaControlsMuteUnmuteDescription = "Mutes or Unmutes your current media that is playing";

		public string MediaControlsVolumeDown = "Volume Down";

		public string MediaControlsVolumeDownClicked = "Lowered volume";

		public string MediaControlsVolumeDownDescription = "Lowers the volume of your current media that is playing";

		public string MediaControlsVolumeUp = "Volume Up";

		public string MediaControlsVolumeUpClicked = "Increased volume";

		public string MediaControlsVolumeUpDescription = "Increases the volume of your current media that is playing";

		public string MicrophoneMenuName = "Microphone";

		public string MicrophoneMenuDescription = "Allows you to change several features regarding your microphone";

		public string MicrophoneIncreaseVolume = "Increase Volume";

		public string MicrophoneIncreaseVolumeClicked = "Increased Volume";

		public string MicrophoneIncreaseVolumeDescription = "Increase your microphone's volume";

		public string MicrophoneDecreaseVolume = "Decrease Volume";

		public string MicrophoneDecreaseVolumeClicked = "Decreased Volume";

		public string MicrophoneDecreaseVolumeDescription = "Decrease your microphone's volume";

		public string MicrophoneIncreaseVolume10x = "Increase Volume 10x";

		public string MicrophoneIncreaseVolume10xClicked = "Increased Volume 10x";

		public string MicrophoneIncreaseVolume10xDescription = "Increase your microphone's volume 10x";

		public string MicrophoneDecreaseVolume10x = "Decrease Volume 10x";

		public string MicrophoneDecreaseVolume10xClicked = "Decreased Volume 10x";

		public string MicrophoneDecreaseVolume10xDescription = "Decrease your microphone's volume 10x";

		public string MicrophoneEarrapeVolume = "Earrape Volume";

		public string MicrophoneEarrapeVolumeClicked = "Earraped Volume";

		public string MicrophoneEarrapeVolumeDescription = "Make your microphone sound like earrape";

		public string MicrophoneResetVolume = "Reset Volume";

		public string MicrophoneResetVolumeClicked = "Reset Volume";

		public string MicrophoneResetVolumeDescription = "Reset your microphone back to default sound level";

		public string InstanceHistoryMenuName = "Instance History";

		public string InstanceHistoryMenuDescription = "Allows you to join back on the previous instances you've been in";

		public string InstanceHistoryButtonDescription = "This instance is hosted in {WorldRegion}";

		public string InstanceHistoryOptionsMenuName = "Instance Options";

		public string InstanceHistoryOptionsJoin = "Join Instance";

		public string InstanceHistoryOptionsJoinDescription = "Joins the instance directly";

		public string InstanceHistoryOptionsDropPortal = "Drop Portal";

		public string InstanceHistoryOptionsDropPortalDescription = "Drops a portal in front of you to the instance";

		public string InstanceHistoryOptionsCopyID = "Copy Instance ID";

		public string InstanceHistoryOptionsCopyIDDescription = "Copies the instance ID to your clipboard";

		public string InstanceHistoryOptionsRemove = "Remove Instance From List";

		public string InstanceHistoryOptionsRemoveDescription = "Removes the instance from your history";

		public string AvatarHistoryMenuName = "Avatar History";

		public string AvatarHistoryMenuDescription = "Shows you a history of your previously used avatars";

		public string AvatarHistoryOptionsSwitch = "Switch Into";

		public string AvatarHistoryOptionsSwitchDescription = "Switches into the avatar";

		public string AvatarHistoryOptionsBots = "Use On Bots";

		public string AvatarHistoryOptionsBotsDescription = "Switches all your bots into the avatar";

		public string AvatarHistoryOptionsRemove = "Remove Avatar From List";

		public string AvatarHistoryOptionsRemoveDescription = "Removes the avatar from your history";

		public string PhotonExploitsMenuName = "Photon Exploits";

		public string PhotonExploitsMenuDescription = "Features related to exploiting vrchat's underlying network infrastructure";

		public string Serialization = "Serialization";

		public string SerializationDescription = "Prevents position updates from getting sent (makes you stand still for others while you can move around yourself)";

		public string OnKeySerialization = "On Key Serialization";

		public string OnKeySerializationDescription = "Only enables serialization when holding CTRL on Desktop or Trigger (left or right) in VR";

		public string Fakelag = "Fakelag";

		public string FakelagDescription = "Fakelags your position making you feel laggy to others";

		public string GhostJoin = "Ghost Join";

		public string GhostJoinDescription = "Prevents you from being seen by people when joining making you a 'ghost' (Instance Master can still see you)";

		public string LockInstance = "Lock Instance";

		public string LockInstanceDescription = "Prevents people from joining the lobby (Requires being Instance Master)";

		public string VoiceImitation = "Voice Imitation";

		public string VoiceImitationDescription = "Mimics a persons voice";

		public string InverseKinematicMimic = "IK Mimic";

		public string InverseKinematicMimicDescription = "Mimics a persons movement";

		public string LagInstance = "Lag Instance";

		public string LagInstanceDescription = "Starts lagging the whole instance using a flaw in VRChat's network infrastructure";

		public string WorldTweaksMenuName = "World Tweaks";

		public string WorldTweaksMenuDescription = "A wide selection of features targetted at improving your world experience";

		public string TheGreatPugRemoveUnnecessaryDoors = "Great Pug Door Remover";

		public string TheGreatPugRemoveUnnecessaryDoorsDescription = "Completely removes those annoying ass doors at The Great Pug which is completely unnecessary";

		public string JustBClubDoorBypass = "Just B Club Door Bypass";

		public string JustBClubDoorBypassDescription = "Allows you to bypass locked doors to private rooms in Just B Club";

		public string JustBClubIntroFix = "Just B Club Intro Remover";

		public string JustBClubIntroFixDescription = "Removes the annoying text whenever you join Just B Club";

		public string JustBClubVIPBypass = "Just B Club VIP Bypass";

		public string JustBClubVIPBypassDescription = "Allows you to enter the VIP room in Just B Club";

		public string FBTHeavenDoorBypass = "FBT Heaven Door Bypass";

		public string FBTHeavenDoorBypassDescription = "Prevents doors from locking in FBT Heaven";

		public string VoidClubAnnoyingEntryDoorFix = "Void Club Entrace Remover";

		public string VoidClubAnnoyingEntryDoorFixDescription = "Completely removes that annoying entry door at void club";

		public string WorldMenuName = "World";

		public string WorldMenuDescription = "All features related to world stuff";

		public string JoinByID = "Join By ID";

		public string JoinByIDDescription = "Join an instance you'd like by it's world id and instance id";

		public string CopyInstanceID = "Copy Instance ID";

		public string CopyInstanceIDClicked = "Copied instance id to clipboard";

		public string CopyInstanceIDDescription = "Copies the current instance id";

		public string ItemWallhack = "Item ESP";

		public string ItemWallhackDescription = "Creates an outline around items which can be seen through walls";

		public string TriggerWallhack = "Trigger ESP";

		public string TriggerWallhackDescription = "Creates an outline around triggers which can be seen through walls";

		public string MovementMenuName = "Movement";

		public string MovementMenuDescription = "Controls movement related variables on your own player";

		public string RunSpeedIndicator = "Run Speed";

		public string RunSpeedIndicatorDescription = "Your current run speed";

		public string WalkSpeedIndicator = "Walk Speed";

		public string WalkSpeedIndicatorDescription = "Your current walk speed";

		public string GravityIndicator = "Gravity";

		public string GravityIndicatorDescription = "Your current gravitational force";

		public string JumpPowerIndicator = "Jump Power";

		public string JumpPowerIndicatorDescription = "Your current jump power";

		public string FlightSpeedIndicator = "Flight Speed";

		public string FlightSpeedIndicatorDescription = "Your current flight speed";

		public string Flight = "Flight";

		public string FlightDescription = "Allows you to fly around (even through walls)";

		public string AutoBhop = "Auto Bhop";

		public string AutoBhopDescription = "Upon landing, it will make you instantly jump again";

		public string InfiniteJump = "Infinite Jump";

		public string InfiniteJumpDescription = "Allows you to continously jump in air";

		public string Jetpack = "Jetpack";

		public string JetpackDescription = "While holding down spacebar, you'll be propelled upwards depending on how long you hold down the key";

		public string FlightDoubleJump = "Flight Double Jump";

		public string FlightDoubleJumpDescription = "Doubletap the spacebar to go into flight mode";

		public string IncreaseSpeed = "Increase Speed";

		public string IncreaseSpeedClicked = "Increased Speed";

		public string IncreaseSpeedDescription = "Increases your speed";

		public string DecreaseSpeed = "Decrease Speed";

		public string DecreaseSpeedClicked = "Decreased Speed";

		public string DecreaseSpeedDescription = "Decreases your speed";

		public string IncreaseSpeed4x = "Increase Speed 4x";

		public string IncreaseSpeed4xClicked = "Increased Speed by 4x";

		public string IncreaseSpeed4xDescription = "Increases your speed 4x";

		public string DecreaseSpeed4x = "Decrease Speed 4x";

		public string DecreaseSpeed4xClicked = "Decreased Speed by 4x";

		public string DecreaseSpeed4xDescription = "Decreases your speed 4x";

		public string IncreaseGravity = "Increase Gravity";

		public string IncreaseGravityClicked = "Increased Gravity";

		public string IncreaseGravityDescription = "Makes gravity more effective, making your jumps last shorter";

		public string DecreaseGravity = "Decrease Gravity";

		public string DecreaseGravityClicked = "Decreased Gravity";

		public string DecreaseGravityDescription = "Makes gravity less effective, making your jumps last longer";

		public string IncreaseJumpPower = "Increase Jump Power";

		public string IncreaseJumpPowerClicked = "Increased Jump Power";

		public string IncreaseJumpPowerDescription = "Makes you able to jump higher";

		public string DecreaseJumpPower = "Decrease Jump Power";

		public string DecreaseJumpPowerClicked = "Decreased Jump Power";

		public string DecreaseJumpPowerDescription = "Makes your jump less effective";

		public string ResetSpeeds = "Reset Speeds";

		public string ResetSpeedsClicked = "Reset all player speeds back to default";

		public string ResetSpeedsDescription = "Resets all player related speeds back to default";

		public string ResetGravity = "Reset Gravity";

		public string ResetGravityClicked = "Reset gravity back to default";

		public string ResetGravityDescription = "Resets gravity back to default";

		public string ResetJumpPower = "Reset Jump Power";

		public string ResetJumpPowerClicked = "Reset jump speed back to default";

		public string ResetJumpPowerDescription = "Resets jump power back to default";

		public string IncreaseFlightSpeed = "Increase Flight Speed";

		public string IncreaseFlightSpeedClicked = "Increased Flight Speed";

		public string IncreaseFlightSpeedDescription = "Makes you fly faster";

		public string DecreaseFlightSpeed = "Decrease Flight Speed";

		public string DecreaseFlightSpeedClicked = "Decreased Flight Speed";

		public string DecreaseFlightSpeedDescription = "Makes you fly slower";

		public string VideoPlayerMenuName = "Video Player";

		public string VideoPlayerMenuDescription = "A menu with features controlling the video players (Only SDK2 worlds for now)";

		public string VideoPlayerNoPlayer = "No video player found";

		public string QueueVideo = "Queue Video";

		public string QueueVideoDescription = "Adds a video to the queue on the video player";

		public string QueueVideosFromFile = "Queue Videos From File";

		public string QueueVideosFromFileDescription = "Adds everything from Videos.txt to the queue on the video player";

		public string ExportVideosToFile = "Export Videos To File";

		public string ExportVideosToFileClicked = "Exported {videoAmount} videos to ExportedVideos.txt";

		public string ExportVideosToFileDescription = "Exports every video from the video player to ExportedVideos.txt";

		public string EarrapeVideo = "Earrape Exploit";

		public string EarrapeVideoDescription = "Attempts to earrape people in the instance with a certain video";

		public string PlayVideo = "Play Video";

		public string PlayVideoClicked = "Playing current video";

		public string PlayVideoDescription = "Plays the current video on the video player";

		public string PauseUnpauseVideo = "(Un)Pause Video";

		public string PauseUnpauseClicked = "Paused/Unpaused current video";

		public string PauseUnpauseDescription = "Pauses/starts the current video on the video player";

		public string PreviousVideo = "Previous Video";

		public string PreviousClicked = "Playing previous video";

		public string PreviousDescription = "Rewinds to the previous video on the video player";

		public string NextVideo = "Next Video";

		public string NextClicked = "Playing next video";

		public string NextDescription = "Skips the current video on the video player";

		public string StopVideo = "Stop Video";

		public string StopClicked = "Stopped current video";

		public string StopDescription = "Stops the current video on the video player";

		public string ClearVideo = "Clear Video";

		public string ClearClicked = "Cleared current video playlist";

		public string ClearDescription = "Clears the video in queue on the video player";

		public string SpeedUpVideo = "Speed Up Video";

		public string SpeedUpClicked = "Sped up video player";

		public string SpeedUpDescription = "Speeds up the current video on the video player";

		public string SpeedDownVideo = "Speed Down Video";

		public string SpeedDownClicked = "Sped down video player";

		public string SpeedDownDescription = "Slows down the current video on the video player";

		public string PlayerMenuName = "Player";

		public string PlayerMenuDescription = "All features related to player stuff";

		public string ChangeAvatarByID = "Change Avatar By ID";

		public string ChangeAvatarByIDDescription = "Changes your current avatar using an avatar ID";

		public string FBTCalibrationSaving = "FBT Calibration Saving";

		public string FBTCalibrationSavingError = "Unable to enable feature due to either IKTweaks or FBT Saver being installed";

		public string FBTCalibrationSavingDescription = "Caches your fbt calibration if you've done it previously, saving you time since you don't have to calibrate again";

		public string PlayerWallhack = "Player Wallhack";

		public string PlayerWallhackDescription = "Creates a capsule around players you can see through walls";

		public string PlayerWallhackSetColor = "Set Wallhack Color";

		public string PlayerWallhackSetColorQuestion = "Which color do you want to change?";

		public string PlayerWallhackSetColorFriends = "Friends";

		public string PlayerWallhackSetColorStrangers = "Strangers";

		public string PlayerWallhackSetColorDescription = "Allows you to change the colors of the player wallhack";

		public string AttachToPlayer = "Attach To Player";

		public string AttachToPlayerDescription = "Attaches yourself to someones bodypart, following them around everywhere";

		public string LocalAvatarClone = "Local Avatar Clone";

		public string LocalAvatarCloneDescription = "Creates a local version of your avatar near you";

		public string AdvancedAvatarHider = "Advanced Avatar Hider";

		public string AdvancedAvatarHiderDescription = "Uses complex algorithms to determine if an avatar should be shown or not, effectively boosting your performance overall";

		public string NameplateMenuName = "Nameplate";

		public string NameplateMenuDescription = "Features related to manipulating the nameplates of people";

		public string CustomRanks = "Custom Ranks";

		public string CustomRanksDescription = "Ever wanted something else than your 'Trusted' rank? We got you! With this you can set it to pretty much whatever you want";

		public string NameplateWallhack = "Nameplate Wallhack";

		public string NameplateWallhackDescription = "Makes nameplates render through walls";

		public string NameplateMoreInfo = "Nameplate More Info";

		public string NameplateMoreInfoDescription = "Shows various information above players nameplates";

		public string NameplateRankColor = "Nameplate Rank Color";

		public string NameplateRankColorDescription = "Brings back nameplates colors based on ranks";

		public string ShowInstanceOwner = "Instance Master Indicator";

		public string ShowInstanceOwnerDescription = "Shows [Host] in people's nameplate if they're the instance owner (Requires 'Nameplate More Info' to be activated)";

		public string ShowActorID = "Actor ID";

		public string ShowActorIDDescription = "Shows player's actor id on the nameplate (Requires 'Nameplate More Info' to be activated)";

		public string ShowAvatarStatus = "Avatar Release Status";

		public string ShowAvatarStatusDescription = "Shows [Public] or [Private] in people's nameplate based on their avatars release status (Requires 'Nameplate More Info' to be activated)";

		public string DetectLagOrCrash = "Lag/Crash Detection";

		public string DetectLagOrCrashDescription = "Shows [Lagging] or [Crashed] in people's nameplate depending on what they are (Requires 'Nameplate More' Info to be activated)";

		public string DetectClientUser = "Client User Detection";

		public string DetectClientUserDescription = "Shows [Client User] on people who got detected with clients (Requires 'Nameplate More' Info to be activated)";

		public string GlobalDynamicBonesMenuName = "Global Dynamic Bones";

		public string GlobalDynamicBonesMenuDescription = "Changes settings related to touching other players dynamic bones or optimizing them";

		public string GlobalDynamicBonesRankDescription = "Configures dynamic bones for {rankName}";

		public string GlobalDynamicBonesRankLocalPlayer = "Local Player";

		public string GlobalDynamicBonesRankFriends = "Friends";

		public string GlobalDynamicBonesRankTrusted = "Trusted";

		public string GlobalDynamicBonesRankKnown = "Known";

		public string GlobalDynamicBonesRankUser = "User";

		public string GlobalDynamicBonesRankNewUser = "New User";

		public string GlobalDynamicBonesRankVisitor = "Visitor";

		public string GlobalDynamicBonesEnable = "Enable Global Dynamic Bones";

		public string GlobalDynamicBonesEnableDescription = "Enables overall our dynamic bones system on configured ranks";

		public string GlobalDynamicBonesReset = "Reset Overrides";

		public string GlobalDynamicBonesResetConfirmation = "Are you sure you want to reset all overrides?";

		public string GlobalDynamicBonesResetDescription = "Resets all user saved overrides";

		public string EnableDynamicBones = "Enable Dynamic Bones";

		public string EnableDynamicBonesDescription = "Enables our dynamic bones system";

		public string PerformanceMode = "Performance Mode";

		public string PerformanceModeDescription = "Reduces the amount of updates dynamic bones does";

		public string DisableAtDistance = "Disable At Distance";

		public string DisableAtDistanceDescription = "Disables dynamic bones after a certain distance";

		public string VisibilityCheck = "Visibility Check";

		public string VisibilityCheckDescription = "Enable dynamic bones only if they're actually visible";

		public string OptimizeOnly = "Optimize Only";

		public string OptimizeOnlyDescription = "Disabled all features listed here and only goes for optimization of the bones";

		public string FunMenuName = "Fun";

		public string FunMenuDescription = "A menu full of fun stuff";

		public string InteractWithAllPickups = "Interact With All Pickups";

		public string InteractWithAllPickupsDescription = "Interact with all mirrors in the world";

		public string WorldTriggers = "World Triggers";

		public string WorldTriggersDescription = "Makes local triggers show for everyone";

		public string UdonGodmode = "Udon Godmode";

		public string UdonGodmodeDescription = "Enables godmode in specific udon worlds (e.g. Murder 4 & Among Us)";

		public string CapsuleHider = "Capsule Hider";

		public string CapsuleHiderDescription = "Hides your capsule from your actual avatar making you unable to be targetted by other players (fly mode gets enabled) - Thanks Skiddai <3";

		public string ItemOrbit = "Item Orbit";

		public string ItemOrbitDescription = "Circles all items around a selected player";

		public string PCCrasher = "PC Crasher";

		public string PCCrasherDescription = "Tries to crash all PC users in the instance with an avatar";

		public string QuestCrasher = "Quest Crasher";

		public string QuestCrasherDescription = "Tries to crash all Quest users in the instance with an avatar";

		public string MurderAmongUsMenuName = "Murder 4";

		public string MurderAmongUsDescription = "Certain fun stuff to give you an advantage in Murder 4";

		public string AntiBystanderKillscreen = "Anti Bystander Killscreen";

		public string AntiBystanderKillscreenDescription = "Removes that annoying screen where you're blinded for a certain amount of time after killing a bystander";

		public string MurderForceWeaponPickupable = "Weapon Force Pickupable";

		public string MurderForceWeaponPickupableDescription = "Allows you to pickup weapons again after killing a bystander";

		public string AnnounceKiller = "Announce Murder Imposter";

		public string AnnounceKillerDescription = "Shows you who the murder/imposter is when the game is starting";

		public string TeleportShotgun = "Teleport Shotgun";

		public string TeleportShotgunDescription = "Teleports the shotgun to you";

		public string TeleportPistol = "Teleport Pistol";

		public string TeleportPistolDescription = "Teleports a pistol to you";

		public string TeleportKnife = "Teleport Knife";

		public string TeleportKnifeDescription = "Teleports a knife to you";

		public string TeleportRevolver = "Teleport Revolver";

		public string TeleportRevolverDescription = "Teleports a revolver to you";

		public string TeleportGrenade = "Teleport Grenade";

		public string TeleportGrenadeDescription = "Teleports a grenade to you";

		public string TeleportBearTrap = "Teleport Bear Trap";

		public string TeleportBearTrapDescription = "Teleports a bear trap to you";

		public string TeleportAllWeapons = "Teleport All Weapons";

		public string TeleportAllWeaponsDescription = "Teleports all weapons to you";

		public string MakeAllWeaponsPickupable = "Make All Weapons Pickupable";

		public string MakeAllWeaponsPickupableDescription = "Makes every weapon pickupable (including knifes when you're not the murderer)";

		public string MassMurder = "Mass Murder";

		public string MassMurderDescription = "Kills all players in the game that are currently alive";

		public string ForceStartGame = "Force Start Game";

		public string ForceStartGameDescription = "Forces the game to start";

		public string ForceEndGame = "Force End Game";

		public string ForceEndGameDescription = "Forces the game to end";

		public string PortableMirrorMenuName = "Portable Mirror";

		public string PortableMirrorMenuDescription = "Want to look at yourself? Here you go chief";

		public string UdonManipulatorMenuName = "Udon Manipulator";

		public string UdonManipulatorMenuDescription = "Want to annoy people using Udon? Welp, with this you can turn on and off mirrors and much more";

		public string PlayerOptionsMenuName = "Player Options";

		public string PlayerOptionsMenuDescription = "Various features targetted towards your selected player";

		public string BlockPhotonEvents = "Block Photon Events";

		public string BlockPhotonEventsDescription = "Block incoming Photon events from this user (WARNING: THIS WILL BREAK TOGGLES AND OTHER STUFF)";

		public string BlockUdonEvents = "Block Udon Events";

		public string BlockUdonEventsDescription = "Block incoming Udon events from this user (WARNING: THIS WILL BREAK UDON GAMEPLAY)";

		public string BlockRPCEvents = "Block RPC Events";

		public string BlockRPCEventsDescription = "Block incoming RPC events from this user (WARNING: THIS WILL WILL BREAK AVATAR & WORLD RELATED STUFF)";

		public string AntiCrashWhitelistAvatar = "AntiCrash Whitelist Avatar";

		public string AntiCrashWhitelistAvatarDescription = "Whitelist an avatar to not get sanitized by the AntiCrash";

		public string ItemOrbitUser = "Item Orbit";

		public string ItemOrbitUserDescription = "Orbits all items in the world around this player";

		public string VoiceImitationUser = "Voice Imitation";

		public string VoiceImitationUserDescription = "Mimics a persons voice";

		public string InverseKinematicMimicUser = "IK Mimic";

		public string InverseKinematicMimicUserDescription = "Mimics a persons movement";

		public string TeleportToUser = "Teleport";

		public string TeleportToUserDescription = "Teleports you to the selected user";

		public string AddToFavorites = "Add To Favorites";

		public string AddToFavoritesDescription = "Add selected player's avatar to your favorites";

		public string CopyAvatarDataUser = "Copy Avatar Data";

		public string CopyAvatarDataUserDescription = "Copies selected player's current avatar asset url";

		public string CopyUserID = "Copy UserID";

		public string CopyUserIDDescription = "Copies selected player's userid";

		public string ReloadAvatar = "Reload Avatar";

		public string ReloadAvatarDescription = "Reloads selected player's avatar";

		public string SaveIconToDisk = "Save Icon To Disk";

		public string SaveIconToDiskDescription = "Saves selected player's current icon to disk";

		public string CrashUser = "Crash User";

		public string CrashUserDescription = "Tries to crash user with an avatar";

		public string PlayerAttachMenuName = "Player Attach";

		public string PlayerAttachMenuDescription = "Selects a bodypart on a player to attach yourself to";

		public string AttachHead = "Head";

		public string AttachHeadDescription = "Attaches you to the selected player's head";

		public string AttachChest = "Chest";

		public string AttachChestDescription = "Attaches you to the selected player's chest";

		public string AttachRightHand = "Right Hand";

		public string AttachRightHandDescription = "Attaches you to the selected player's right hand";

		public string AttachLeftHand = "Left Hand";

		public string AttachLeftHandDescription = "Attaches you to the selected player's left hand";

		public string AttachHips = "Hips";

		public string AttachHipsDescription = "Attaches you to the selected player's hips";

		public string AttachBack = "Back";

		public string AttachBackDescription = "Attaches you to the selected player's back";

		public string AttachRightFoot = "Right Foot";

		public string AttachRightFootDescription = "Attaches you to the selected player's right foot";

		public string AttachLeftFoot = "Left Foot";

		public string AttachLeftFootDescription = "Attaches you to the selected player's left foot";

		public string AttachOrbit = "Orbit";

		public string AttachOrbitDescription = "Attaches you to the selected player and orbits around them";

		public string GlobalDynamicBonesPlayerMenuName = "Global Dynamic Bones Override";

		public string GlobalDynamicBonesPlayerMenuDescription = "Change dynamic bones related stuff on selected player's avatar";

		public string EnableOverride = "Enable Override";

		public string EnableOverrideDescription = "When enabled it will override the default settings set for this users rank";

		public string LovenseMenuName = "Lovense";

		public string LovenseMenuDescription = "Take full control over your lovense device in-game with our immersive integration";

		public string ConnectToLovense = "Connect To Device";

		public string ConnectingToLovense = "Connecting...";

		public string DisconnectFromLovense = "Disonnect From Device";

		public string ConnectToLovenseDescription = "Connect to your Lovense device";

		public string GlobalAvatarDatabase = "München Database";

		public string PersonalAvatarDatabase = "München Favorites";

		public string FavoriteAvatar = "Favorite Avatar";

		public string UnfavoriteAvatar = "Unfavorite Avatar";

		public string LeaveBlankToReset = "Leave blank to reset";

		public string SearchAfterAvatar = "Search After Avatar";

		public string AddAvatarByID = "Add By ID";

		public string AvatarNotFound = "Avatar not found";

		public string AvatarPrivate = "Avatar is private";

		public string AvatarAlreadyFavorited = "Avatar already favorited";

		public string AvatarFavorited = "Avatar favorited: {name}";

		public string AvatarUnfavorited = "Avatar unfavorited: {name}";

		public string ErrorSearchForAvatar = "Error occured while searching for avatars";

		public string ErrorNoAvatarsFound = "No avatars were found";

		public string FlashlightMenuName = "Flashlight";

		public string FlashlightMenuDescription = "Illuminate the path ahead of you";

		public string FlashlightBoneSelectorMenuName = "Select Attached Bone";

		public string FlashlightBoneSelectorMenuDescription = "Selects the attached bone";

		public string FlashlightIndicatorAttachedBone = "Attached Bone";

		public string FlashlightIndicatorAttachedBoneDescription = "The bone your flashlight is currently attached to";

		public string FlashlightIndicatorSpotAngle = "Spot Angle";

		public string FlashlightIndicatorSpotAngleDescription = "The wideness of the beam";

		public string FlashlightIndicatorRange = "Range";

		public string FlashlightIndicatorRangeDescription = "The range the flashlight affects ahead of you";

		public string FlashlightIndicatorIntensity = "Intensity";

		public string FlashlightIndicatorIntensityDescription = "The intensity of the flashlight";

		public string FlashlightIncreaseSpotAngle = "Increase Spot Angle";

		public string FlashlightIncreaseSpotAngleDescription = "Increases the lights spot angle";

		public string FlashlightDecreaseSpotAngle = "Decrease Spot Angle";

		public string FlashlightDecreaseSpotAngleDescription = "Decreases the lights spot angle";

		public string FlashlightIncreaseRange = "Increase Range";

		public string FlashlightIncreaseRangeDescription = "Increases the lights range";

		public string FlashlightDecreaseRange = "Decrease Range";

		public string FlashlightDecreaseRangeDescription = "Decreases the lights range";

		public string FlashlightIncreaseIntensity = "Increase Intensity";

		public string FlashlightIncreaseIntensityDescription = "Increases the lights intensity";

		public string FlashlightDecreaseIntensity = "Decrease Intensity";

		public string FlashlightDecreaseIntensityDescription = "Decreases the lights intensity";
	}
}
