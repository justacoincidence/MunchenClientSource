using System.Collections.Generic;
using MunchenClient.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace MunchenClient.Config.Modules
{
	public class GeneralConfig
	{
		[JsonIgnore]
		public static readonly string ConfigLocation = Configuration.GetConfigFolderPath() + "GeneralConfig.json";

		[JsonIgnore]
		public static readonly int ClientAssetsVersionCurrent = 5;

		public int ConfigVersion = 1;

		public int ClientAssetsVersion = 1;

		public bool ColorChangerEnable = false;

		public bool ColorChangerRainbowMode = false;

		public bool ColorChangerRainbowHyperMode = false;

		public ColorConfig ColorChangerColor = new ColorConfig(Color.magenta);

		public bool LocalizedClient = false;

		public bool LocalizedClientCustom = false;

		public bool AutoClearCache = false;

		public bool OnKeySerialization = false;

		public bool AvatarDownloadLogging = false;

		public bool WorldTriggers = false;

		public bool UdonGodmode = false;

		public bool AutoBhop = false;

		public bool InfiniteJump = false;

		public bool Jetpack = false;

		public bool FlightDoubleJump = false;

		public bool ComfyVRMenu = false;

		public bool PersistentQuickMenu = false;

		public bool NetworkedEmotes = true;

		public bool AdvancedAvatarHider = false;

		public float FlightSpeed = 10f;

		public bool ItemWallhack = false;

		public bool TriggerWallhack = false;

		public bool NameplateWallhack = false;

		public bool PlayerWallhack = false;

		public bool PlayerWallhackRGBFriends = false;

		public bool PlayerWallhackRGBStrangers = false;

		public ColorConfig PlayerWallhackFriendsColor = new ColorConfig(Color.green);

		public ColorConfig PlayerWallhackStrangersColor = new ColorConfig(Color.red);

		public bool FlashlightActive = false;

		public HumanBodyBones FlashlightAttachedBone = HumanBodyBones.Head;

		public int FlashlightSpotAngle = 40;

		public float FlashlightRange = 30f;

		public float FlashlightIntensity = 1f;

		public bool DisableAllKeybinds = false;

		public bool DisableFBTSavingCompletely = false;

		public bool DisableMenuPlayerButtons = false;

		public bool DisableAvatarFavorites = false;

		public bool DisableAvatarFavoritesSearchButton = false;

		public bool DisableAvatarDatabase = false;

		public bool DisableKeyboardImprovements = false;

		public bool DisableActionMenuIntegration = false;

		public bool MinimumCameraClippingDistance = false;

		public bool HiddenAvatarScaler = false;

		public bool PortalsProtectionsAutoDeleteNonFriends = false;

		public bool PortalsProtectionsAutoDeleteFriends = false;

		public bool PortalsProtectionsDeleteUnused = false;

		public bool PortalsProtectionsDeleteBadlyConfigured = false;

		public bool PortalsProtectionsDeleteDistantPlaced = false;

		public bool PortalsProtectionsDeleteDistantPlacedAllowFriends = false;

		public bool PortalsProtectionsConfirmation = false;

		public bool AntiAvatarIntroMusic = false;

		public bool AntiFreezeExploit = false;

		public bool AntiInstanceLock = false;

		public bool AntiEarrapeExploit = false;

		public bool AntiWorldTriggers = false;

		public bool SerializationDetection = false;

		public bool InvisibleDetection = false;

		public string CrasherPC = string.Empty;

		public string CrasherQuest = string.Empty;

		public bool RanksCustomRanks = false;

		public bool NameplateRankColor = false;

		public bool NameplateMoreInfo = false;

		public bool ShowInstanceOwner = false;

		public bool ShowActorID = false;

		public bool ShowAvatarStatus = false;

		public bool DetectLagOrCrash = false;

		public bool DetectClientUser = false;

		public bool PerformanceInputDelayReducer = false;

		public bool PerformanceSmartFPS = false;

		public bool PerformanceUnlimitedFPS = false;

		public bool PerformanceHighPriority = false;

		public bool PerformanceImageCache = false;

		public bool PerformanceLowGraphics = false;

		public bool PerformanceNoHyperThreading = false;

		public bool PerformanceNoPerformanceStats = false;

		public bool PerformanceNoAntiAliasing = false;

		public bool PerformanceNoShadows = false;

		public bool SpooferAutoRegenerate = false;

		public bool SpooferHWID = true;

		public bool SpooferSteamID = false;

		public bool SpooferRealisticMode = false;

		public bool SpooferPing = false;

		public int SpooferPingCustom = -1;

		public bool SpooferFPS = false;

		public int SpooferFPSCustom = -1;

		public bool SpooferPeripheral = false;

		public string SpooferGeneratedHWID = string.Empty;

		public string SpooferGeneratedDeviceModel = string.Empty;

		public string SpooferGeneratedDeviceName = string.Empty;

		public string SpooferGeneratedGraphicsDeviceName = string.Empty;

		public string SpooferGeneratedGraphicsDeviceVersion = string.Empty;

		public int SpooferGeneratedGraphicsDeviceIdentifier = -1;

		public string SpooferGeneratedProcessorType = string.Empty;

		public string SpooferGeneratedOperatingSystem = string.Empty;

		public bool AntiIPLoggingVideoPlayerSafety = false;

		public bool AntiIPLoggingImageThumbnailSafety = false;

		public bool PortableMirrorPickupable = false;

		public float PortableMirrorSizeX = 1f;

		public float PortableMirrorSizeY = 1f;

		public bool MurderAntiKillscreen = false;

		public bool MurderForceWeaponPickupable = false;

		public bool MurderAnnounceKiller = false;

		public bool VoidClubAnnoyingEntryDoorFix = false;

		public bool TheGreatPugRemoveUnnecessaryDoors = false;

		public bool JustBClubIntroFix = false;

		public bool JustBClubVIPBypass = false;

		public bool JustBClubDoorBypass = false;

		public bool FBTHeavenDoorBypass = false;

		public bool BlockAllPhotonEvents = false;

		public bool BlockAllUdonEvents = false;

		public bool BlockAllRPCEvents = false;

		public Dictionary<string, PlayerBlockedEvents> BlockedPlayerEvents = new Dictionary<string, PlayerBlockedEvents>();
	}
}
