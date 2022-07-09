using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using UnityEngine;
using VRC.Core;

namespace MunchenClient.Menu.PlayerOptions
{
	internal class PlayerOptionsMenu : QuickMenuNestedMenu
	{
		private readonly string iconDirectory = Configuration.GetClientFolderPath() + "/VRChat Icons";

		private readonly LovensePermissionsMenu lovensePermissionsMenu;

		private readonly QuickMenuToggleButton itemOrbitButton;

		private readonly QuickMenuToggleButton voiceImitationButton;

		private readonly QuickMenuToggleButton inverseKinematicMimicButton;

		private readonly QuickMenuToggleButton blockPhotonEventsButton;

		private readonly QuickMenuToggleButton blockUdonEventsButton;

		private readonly QuickMenuToggleButton blockRPCEventsButton;

		private readonly List<string> lewdList = new List<string>
		{
			"cloth", "shirt", "pant", "under", "undi", "jacket", "top", "bra", "bra ", "skirt",
			"jean", "trouser", "boxers", "hoodi", "bottom", "dress", "bandage", "bondage", "sweat", "cardig",
			"corset", "tiddy", "pastie", "suit", "stocking", "jewel", "frill", "gauze", "cover", "pubic",
			"sfw", "harn", "biki", "outfit", "panties", "short", "clothing", "shirt top", "pasties", "inv_swimsuit",
			"pants", "shoes", "underclothes", "shorts", "Hoodie", "plaster", "pussy cover", "radialswitch", "ribbon", "bottom1",
			"shorts nsfw", "top nsfw", "pastie+harness", "bralette harness", "bottom2", "robe", "rope", "ropes", "ropes", "lingerie toggle",
			"sandals", "shirt.001", "skrt", "sleeve", "sleeves", "snapdress", "socks", "tank", "stickers", "denimtop_b",
			"fish nets", "chest harness", "stockings", "straps", "strapsbottom", "body suit", "sweater", "swimsuit", "tank top", "tape",
			"shirt dress", "tearsweater", "thong", "toob", "toppants", "rf mask top", "longshirt", "asphalttop", "hood", "sweatshirt",
			"uppertop", "toggle top.001", "jacket.002", "underwear", "undies", "tokyohoodie", "wraps", "wrap", "outerwear", "wraps-top",
			"Одежка", "sticker", "dressy", "capeyyy", "bodysuity", "bodysuit", "верх", "низ", "パンティー", "ビキニ",
			"ブラジャー", "下着", "무녀복", "브라", "비키니", "속옷", "젖소", "gasmask", "팬티", "skirt.001",
			"huku_top", "other_glasses", "other_mask", "huku_pants", "huku_skirt", "huku_jacket", "clothes", "top_mesh", "kemono", "garterbelt",
			"langerie", "tap", "calça", "camisa", "beziercircle.001", "dress.001", "floof corset", "paisties", "string and gatter", "crop top",
			"panty", "sleeveless", "harness", "pantie", "bandaid", "mask", "chainsleeve", "hat", "hoodoff", "hoodon",
			"metal muzzle", "top2", "rush", "huku_bra", "huku_lace shirt", "huku_panties", "huku_shoes", "huku_shorts", "o_harness", "o_mask",
			"bottoms", "daddys slut", "bra.strapless", "butterfly dress", "chainnecklace", "denim shorts", "panties_berryvee", "tanktop", "waist jacket", "chocker_jhp",
			"brazbikini_bottoms", "brazbikini_top", "full harness", "glasses", "panty_misepan", "top1", "top3", "top4", "top5_bottom", "top5_top",
			"top6", "eraser", "bikini", "headset", "screen", "就是一个胡萝卜", "chain", "hesopi", "merino_scarf", "merino_bag",
			"bikini bottoms", "merino_panties", "tsg_buruma", "merino_cap", "kyoueimizugi", "kyoueimizugi_oppaiooki", "leotard", "hotpants", "hotpants_side_open", "merino_culottes",
			"merino_leggins", "merino_socks", "bikini", "merino_bra", "merino_jacket", "merino_inner", "tsg_shirt", "beer hat", "cuffs", "lace",
			"panties", "pasties", "shorts and shoes", "undergarments", "irukanicar", "ベルト", "wear", "tshirt", "waistbag", "nekomimicasquette",
			"dango", "penetrator", "comfy bottom", "comfy top", "hoodie", "strawberry panty", "strawberry top", "vest", "sleevedtop", "baggy top by cupkake",
			"harness by heyblake", "heart pasties by cupkake", "straps!", "crop strap hoodie flat", "harness & panties", "bunnycostume", "handwarmers", "belt", "cardigan", "turtle neck",
			"bandages", "holysuit", "nipplecovers", "panti", "Panti 2", "nipple covers", "maid outfit", "P&U", "nsfw", "heart pasties",
			"body lingerie", "sluttytop", "sports bra", "bear", "fishnets", "shirtone", "shirttwo", "bodymesh", "bikinitop"
		};

		internal override void OnMenuShownCallback()
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				itemOrbitButton.SetToggleState(GeneralUtils.itemOrbit && PlayerTargetHandler.GetPlayerToOrbit()?.id == selectedPlayer.id);
				voiceImitationButton.SetToggleState(GeneralUtils.voiceImitation && selectedPlayer.networkBehaviour.prop_Int32_0 == GeneralUtils.voiceImitationPlayerKey);
				inverseKinematicMimicButton.SetToggleState(GeneralUtils.inverseKinematicMimic && selectedPlayer.networkBehaviour.prop_Int32_0 == GeneralUtils.inverseKinematicMimicPlayerKey);
				if (Configuration.GetGeneralConfig().BlockedPlayerEvents.ContainsKey(selectedPlayer.id))
				{
					blockPhotonEventsButton.SetToggleState(Configuration.GetGeneralConfig().BlockedPlayerEvents[selectedPlayer.id].BlockedPhoton);
					blockUdonEventsButton.SetToggleState(Configuration.GetGeneralConfig().BlockedPlayerEvents[selectedPlayer.id].BlockedUdon);
					blockRPCEventsButton.SetToggleState(Configuration.GetGeneralConfig().BlockedPlayerEvents[selectedPlayer.id].BlockedRPC);
				}
				else
				{
					blockPhotonEventsButton.SetToggleState(state: false);
					blockUdonEventsButton.SetToggleState(state: false);
					blockRPCEventsButton.SetToggleState(state: false);
				}
			}
		}

		private void PrepareUserInList(PlayerInformation playerInfo)
		{
			if (!Configuration.GetGeneralConfig().BlockedPlayerEvents.ContainsKey(playerInfo.id))
			{
				Configuration.GetGeneralConfig().BlockedPlayerEvents.Add(playerInfo.id, new PlayerBlockedEvents
				{
					BlockedRPC = false,
					BlockedUdon = false,
					BlockedPhoton = false
				});
			}
		}

		private void ChangePhotonBlockStatus(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList(selectedPlayer);
				Configuration.GetGeneralConfig().BlockedPlayerEvents[selectedPlayer.id].BlockedPhoton = state;
				Configuration.SaveGeneralConfig();
			}
		}

		private void ChangeUdonBlockStatus(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList(selectedPlayer);
				Configuration.GetGeneralConfig().BlockedPlayerEvents[selectedPlayer.id].BlockedUdon = state;
				Configuration.SaveGeneralConfig();
			}
		}

		private void ChangeRPCBlockStatus(bool state)
		{
			PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
			if (selectedPlayer != null)
			{
				PrepareUserInList(selectedPlayer);
				Configuration.GetGeneralConfig().BlockedPlayerEvents[selectedPlayer.id].BlockedRPC = state;
				Configuration.SaveGeneralConfig();
			}
		}

		internal PlayerOptionsMenu(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().PlayerOptionsMenuName, LanguageManager.GetUsedLanguage().PlayerOptionsMenuDescription)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().ProtectionsMenuName);
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().AvatarCategory);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			new QuickMenuHeader(this, LanguageManager.GetUsedLanguage().FeaturesCategory);
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow3 = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow quickMenuButtonRow2 = new QuickMenuButtonRow(this);
			blockPhotonEventsButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().BlockPhotonEvents, state: false, delegate
			{
				ChangePhotonBlockStatus(state: true);
			}, LanguageManager.GetUsedLanguage().BlockPhotonEventsDescription, delegate
			{
				ChangePhotonBlockStatus(state: false);
			}, LanguageManager.GetUsedLanguage().BlockPhotonEventsDescription);
			blockUdonEventsButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().BlockUdonEvents, state: false, delegate
			{
				ChangeUdonBlockStatus(state: true);
			}, LanguageManager.GetUsedLanguage().BlockUdonEventsDescription, delegate
			{
				ChangeUdonBlockStatus(state: false);
			}, LanguageManager.GetUsedLanguage().BlockUdonEventsDescription);
			blockRPCEventsButton = new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().BlockRPCEvents, state: false, delegate
			{
				ChangeRPCBlockStatus(state: true);
			}, LanguageManager.GetUsedLanguage().BlockRPCEventsDescription, delegate
			{
				ChangeRPCBlockStatus(state: false);
			}, LanguageManager.GetUsedLanguage().BlockRPCEventsDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().AntiCrashWhitelistAvatar, delegate
			{
				PlayerInformation selectedPlayer12 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer12 != null)
				{
					AntiCrashUtils.ProcessAvatarWhitelist(selectedPlayer12.vrcPlayer.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0);
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 430);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().AntiCrashWhitelistAvatarDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().AddToFavorites, delegate
			{
				PlayerInformation selectedPlayer11 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer11 != null)
				{
					if (selectedPlayer11.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2 != null)
					{
						if (selectedPlayer11.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.releaseStatus != "private")
						{
							AvatarFavoritesHandler.AddAvatarByID(selectedPlayer11.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2.id, null, null);
						}
						else
						{
							ConsoleUtils.Info("Player", "Can't add avatar since it's private", ConsoleColor.Gray, ".ctor", 452);
							GeneralWrappers.AlertPopup("Warning", "Can't add avatar since it's private");
						}
					}
					else
					{
						ConsoleUtils.Info("Player", "Can't add avatar since player is either switching or is a robot", ConsoleColor.Gray, ".ctor", 458);
						GeneralWrappers.AlertPopup("Warning", "Can't add avatar since player is either switching or is a robot");
					}
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 464);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().AddToFavoritesDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().CopyAvatarDataUser, delegate
			{
				PlayerInformation selectedPlayer10 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer10 != null)
				{
					ApiAvatar apiAvatar2 = selectedPlayer10.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2;
					Clipboard.SetText("ID: " + apiAvatar2.id + "\nName: " + apiAvatar2.name + "\nStatus: " + apiAvatar2.releaseStatus + "\nAsset Url: " + apiAvatar2.assetUrl + "\nAuthor Name: " + apiAvatar2.authorName + "\nAuthor ID: " + apiAvatar2.authorId + "\nDescription: " + apiAvatar2.description + "\nPreview: " + apiAvatar2.imageUrl);
					ConsoleUtils.Info("Player", "Copied avatar data found from " + selectedPlayer10.displayName + " to clipboard", ConsoleColor.Gray, ".ctor", 480);
					GeneralWrappers.AlertPopup("Warning", "Copied avatar data from " + selectedPlayer10.displayName + " to clipboard");
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 485);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().CopyAvatarDataUserDescription);
			new QuickMenuSingleButton(parentRow2, LanguageManager.GetUsedLanguage().ReloadAvatar, delegate
			{
				PlayerInformation selectedPlayer9 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer9 != null)
				{
					PlayerUtils.ReloadAvatar(selectedPlayer9.apiUser);
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 501);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().ReloadAvatarDescription);
			new QuickMenuSingleButton(parentRow2, "Download Avatar File", delegate
			{
				PlayerInformation selectedPlayer8 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer8 != null)
				{
					ApiAvatar apiAvatar = selectedPlayer8.vrcPlayer.prop_VRCAvatarManager_0.prop_ApiAvatar_2;
					GeneralUtils.DownloadFileToPath(apiAvatar.assetUrl, "Avatars", apiAvatar.name + "-" + apiAvatar.id, "vrca", OnAvatarDownloadFinished);
					QuickMenuUtils.ShowAlert("Started downloading avatar");
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 521);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, "Downloads the avatar file onto your disk");
			lovensePermissionsMenu = new LovensePermissionsMenu(quickMenuButtonRow);
			lovensePermissionsMenu.SetMenuAccessibility(state: false, "Can't access menu - Under construction (coming soon)");
			new GlobalDynamicBonesPlayerPermissionsMenu(quickMenuButtonRow);
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().CopyUserID, delegate
			{
				PlayerInformation selectedPlayer7 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer7 != null)
				{
					Clipboard.SetText(selectedPlayer7.id);
					ConsoleUtils.Info("Player", "Copied UserID from " + selectedPlayer7.displayName + " to clipboard", ConsoleColor.Gray, ".ctor", 542);
					GeneralWrappers.AlertPopup("Success", "Copied UserID from " + selectedPlayer7.displayName + " to clipboard");
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 547);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().CopyUserIDDescription);
			new QuickMenuSingleButton(quickMenuButtonRow, LanguageManager.GetUsedLanguage().TeleportToUser, delegate
			{
				PlayerInformation selectedPlayer6 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer6 != null)
				{
					PlayerWrappers.GetCurrentPlayer().transform.position = selectedPlayer6.vrcPlayer.transform.position;
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 563);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().TeleportToUserDescription);
			itemOrbitButton = new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().ItemOrbitUser, GeneralUtils.itemOrbit, delegate
			{
				PlayerInformation selectedPlayer5 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer5 != null)
				{
					PlayerTargetHandler.SelectNewPlayerToOrbit(selectedPlayer5);
					GeneralUtils.itemOrbit = true;
					FunMenu.itemOrbitButton.SetToggleState(state: true);
				}
				else
				{
					itemOrbitButton.SetToggleState(state: false);
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 586);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().ItemOrbitUserDescription, delegate
			{
				GeneralUtils.itemOrbit = false;
				FunMenu.itemOrbitButton.SetToggleState(state: false);
			}, LanguageManager.GetUsedLanguage().ItemOrbitUserDescription);
			voiceImitationButton = new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().VoiceImitationUser, GeneralUtils.voiceImitation, delegate
			{
				PlayerInformation selectedPlayer4 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer4 != null)
				{
					voiceImitationButton.SetToggleState(state: true);
					PhotonExploitsMenu.voiceImitationButton.SetToggleState(state: true);
					GeneralUtils.voiceImitation = true;
					GeneralUtils.voiceImitationPlayerKey = selectedPlayer4.networkBehaviour.prop_Int32_0;
				}
				else
				{
					voiceImitationButton.SetToggleState(state: false);
					PhotonExploitsMenu.voiceImitationButton.SetToggleState(state: false);
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 613);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().VoiceImitationUserDescription, delegate
			{
				voiceImitationButton.SetToggleState(state: false);
				PhotonExploitsMenu.voiceImitationButton.SetToggleState(state: false);
				GeneralUtils.voiceImitation = false;
				GeneralUtils.voiceImitationPlayerKey = -1;
			}, LanguageManager.GetUsedLanguage().VoiceImitationUserDescription);
			inverseKinematicMimicButton = new QuickMenuToggleButton(parentRow3, LanguageManager.GetUsedLanguage().InverseKinematicMimicUser, GeneralUtils.inverseKinematicMimic, delegate
			{
				PlayerInformation selectedPlayer3 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer3 != null)
				{
					inverseKinematicMimicButton.SetToggleState(state: true);
					PhotonExploitsMenu.inverseKinematicMimicButton.SetToggleState(state: true);
					GeneralUtils.inverseKinematicMimic = true;
					GeneralUtils.inverseKinematicMimicPlayerKey = selectedPlayer3.networkBehaviour.prop_Int32_0;
				}
				else
				{
					inverseKinematicMimicButton.SetToggleState(state: false);
					PhotonExploitsMenu.inverseKinematicMimicButton.SetToggleState(state: false);
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 643);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().InverseKinematicMimicUserDescription, delegate
			{
				inverseKinematicMimicButton.SetToggleState(state: false);
				PhotonExploitsMenu.inverseKinematicMimicButton.SetToggleState(state: false);
				GeneralUtils.inverseKinematicMimic = false;
				GeneralUtils.inverseKinematicMimicPlayerKey = -1;
			}, LanguageManager.GetUsedLanguage().InverseKinematicMimicUserDescription);
			new QuickMenuSingleButton(parentRow3, LanguageManager.GetUsedLanguage().SaveIconToDisk, delegate
			{
				PlayerInformation selectedPlayer2 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer2 != null)
				{
					if (selectedPlayer2.vrcPlayer.field_Public_PlayerNameplate_0.field_Public_GameObject_3.active)
					{
						if (GeneralUtils.SaveTextureToDisk(selectedPlayer2.vrcPlayer.field_Public_PlayerNameplate_0.field_Public_RawImage_0.texture, iconDirectory, selectedPlayer2.displayName, includeCRC32InFileName: true))
						{
							ConsoleUtils.Info("Player", selectedPlayer2.displayName + "'s icon saved to " + iconDirectory + "/" + selectedPlayer2.displayName + ".png", ConsoleColor.Gray, ".ctor", 667);
							GeneralWrappers.AlertPopup("Success", selectedPlayer2.displayName + "'s icon saved to " + iconDirectory + "/" + selectedPlayer2.displayName + ".png");
						}
						else
						{
							ConsoleUtils.Info("Player", "Failed to save " + selectedPlayer2.displayName + "'s icon", ConsoleColor.Gray, ".ctor", 672);
							GeneralWrappers.AlertPopup("Warning", "Failed to save " + selectedPlayer2.displayName + "'s icon");
						}
					}
					else
					{
						ConsoleUtils.Info("Player", selectedPlayer2.displayName + " doesn't have an icon", ConsoleColor.Gray, ".ctor", 678);
						GeneralWrappers.AlertPopup("Warning", selectedPlayer2.displayName + " doesn't have an icon");
					}
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 684);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().SaveIconToDiskDescription);
			new QuickMenuSingleButton(quickMenuButtonRow2, LanguageManager.GetUsedLanguage().CrashUser, delegate
			{
				PlayerInformation playerInfo = PlayerWrappers.GetSelectedPlayer();
				if (playerInfo != null)
				{
					GeneralWrappers.AlertAction("Notice", "Are you sure you want to crash this user?", "Crash", delegate
					{
						if (!playerInfo.isLocalPlayer)
						{
							GeneralUtils.RunGameCloseExploit(playerInfo.isQuestUser, playerInfo.apiUser);
							GeneralWrappers.ClosePopup();
						}
						else
						{
							GeneralWrappers.AlertPopup("Warning", "You can't crash yourself");
						}
					}, "Cancel", delegate
					{
						GeneralWrappers.ClosePopup();
					});
				}
				else
				{
					ConsoleUtils.Info("Crasher", "Player not found", ConsoleColor.Gray, ".ctor", 717);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, LanguageManager.GetUsedLanguage().CrashUserDescription);
			new PlayerAttachMenu(quickMenuButtonRow2);
			new QuickMenuSingleButton(quickMenuButtonRow2, "Force Lewd", delegate
			{
				PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer != null)
				{
					List<Transform> source = MiscUtils.FindAllComponentsInGameObject<Transform>(selectedPlayer.GetAvatar(), includeInactive: true, searchParent: false);
					{
						foreach (Transform item in from component in source
							where lewdList.Contains(component.gameObject.name.ToLower())
							where (bool)component.GetComponent<MeshRenderer>() || (bool)component.GetComponent<SkinnedMeshRenderer>()
							select component)
						{
							UnityEngine.Object.Destroy(item.gameObject);
						}
						return;
					}
				}
				ConsoleUtils.Info("Lewd", "Player not found", ConsoleColor.Gray, ".ctor", 740);
				GeneralWrappers.AlertPopup("Warning", "Player not found");
			}, "ERP?");
			if (GeneralUtils.HasSpecialBenefits())
			{
				new AdminUserModerationMenu(quickMenuButtonRow2);
			}
		}

		private static void OnAvatarDownloadFinished(bool success)
		{
			if (success)
			{
				ConsoleUtils.Info("Avatars", "Success downloading avatar", ConsoleColor.Gray, "OnAvatarDownloadFinished", 755);
				QuickMenuUtils.ShowAlert("Success downloading avatar");
			}
			else
			{
				ConsoleUtils.Info("Avatars", "Failed downloading avatar", ConsoleColor.Gray, "OnAvatarDownloadFinished", 760);
				QuickMenuUtils.ShowAlert("Failed downloading avatar");
			}
		}
	}
}
