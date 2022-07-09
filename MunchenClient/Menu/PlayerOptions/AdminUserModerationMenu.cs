using System;
using Il2CppSystem.Collections.Generic;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using ServerAPI.Core;
using UnchainedButtonAPI;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.Menu.PlayerOptions
{
	internal class AdminUserModerationMenu : QuickMenuNestedMenu
	{
		private string currentChangingPlayerCustomRankId;

		internal AdminUserModerationMenu(QuickMenuButtonRow parent)
			: base(parent, "Admin Moderation", "Several moderation features for targeted player")
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, "Change Rank Name", delegate
			{
				PlayerInformation selectedPlayer4 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer4 != null)
				{
					currentChangingPlayerCustomRankId = selectedPlayer4.id;
					GeneralWrappers.ShowInputPopup("Change Custom Rank Name", "", InputField.InputType.Standard, isNumeric: false, "Confirm", ChangePlayerCustomRankName, null, "Enter rank...", closeAfterInput: true, null, unknownBool: false, 5000);
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 34);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, "Changes the selected player's custom rank");
			new QuickMenuSingleButton(parentRow, "Remove Rank Name", delegate
			{
				PlayerInformation selectedPlayer3 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer3 != null)
				{
					currentChangingPlayerCustomRankId = selectedPlayer3.id;
					GeneralWrappers.AlertAction("Custom Rank Changer", "Are you sure you want to remove " + selectedPlayer3.displayName + "'s custom rank name?", "Remove", delegate
					{
						GeneralWrappers.ClosePopup();
						RemovePlayerCustomRankName();
					}, "Cancel", delegate
					{
						GeneralWrappers.ClosePopup();
					});
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 60);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, "Remove the selected player's custom rank");
			new QuickMenuSingleButton(parentRow, "Change Rank Color", delegate
			{
				PlayerInformation selectedPlayer2 = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer2 != null)
				{
					currentChangingPlayerCustomRankId = selectedPlayer2.id;
					GeneralWrappers.ShowInputPopup("Change Custom Rank Color", "", InputField.InputType.Standard, isNumeric: false, "Confirm", ChangePlayerCustomRankColor, null, "Enter rank color...", closeAfterInput: true, null, unknownBool: false, 7);
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 78);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, "Changes the selected player's custom rank color");
			new QuickMenuSingleButton(parentRow, "Remove Rank Color", delegate
			{
				PlayerInformation selectedPlayer = PlayerWrappers.GetSelectedPlayer();
				if (selectedPlayer != null)
				{
					currentChangingPlayerCustomRankId = selectedPlayer.id;
					GeneralWrappers.AlertAction("Custom Rank Changer", "Are you sure you want to remove " + selectedPlayer.displayName + "'s custom rank color?", "Remove", delegate
					{
						GeneralWrappers.ClosePopup();
						RemovePlayerCustomRankColor();
					}, "Cancel", delegate
					{
						GeneralWrappers.ClosePopup();
					});
				}
				else
				{
					ConsoleUtils.Info("Player", "Player not found", ConsoleColor.Gray, ".ctor", 104);
					GeneralWrappers.AlertPopup("Warning", "Player not found");
				}
			}, "Removes the selected player's custom rank color");
		}

		private void ChangePlayerCustomRankName(string customRankName, List<KeyCode> pressedKeys, Text text)
		{
			if (customRankName.Length > 5000)
			{
				ConsoleUtils.Info("Player", "Custom rank too long", ConsoleColor.Gray, "ChangePlayerCustomRankName", 114);
				GeneralWrappers.AlertPopup("Warning", "Custom rank too long");
				return;
			}
			ServerAPICore.GetInstance().ChangePlayerCustomRankName(currentChangingPlayerCustomRankId, customRankName.Trim());
			if (PlayerUtils.playerCustomRank.ContainsKey(currentChangingPlayerCustomRankId))
			{
				CustomRankInfo customRankInfo = PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId];
				PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId] = new CustomRankInfo
				{
					customRankEnabled = true,
					customRank = customRankName.Trim(),
					customRankColorEnabled = customRankInfo.customRankColorEnabled,
					customRankColor = customRankInfo.customRankColor
				};
			}
			else
			{
				PlayerUtils.playerCustomRank.Add(currentChangingPlayerCustomRankId, new CustomRankInfo
				{
					customRankEnabled = true,
					customRank = customRankName.Trim(),
					customRankColorEnabled = false,
					customRankColor = default(Color)
				});
			}
			GeneralWrappers.AlertPopup("Warning", "Player rank succesfully set");
		}

		private void ChangePlayerCustomRankColor(string customRankColor, List<KeyCode> pressedKeys, Text text)
		{
			if (customRankColor.Length > 7)
			{
				ConsoleUtils.Info("Player", "Custom rank color too long", ConsoleColor.Gray, "ChangePlayerCustomRankColor", 154);
				GeneralWrappers.AlertPopup("Warning", "Custom rank color too long");
				return;
			}
			if (!ColorUtility.TryParseHtmlString(customRankColor.Trim(), out var color))
			{
				ConsoleUtils.Info("Player", "Color is invalid", ConsoleColor.Gray, "ChangePlayerCustomRankColor", 162);
				GeneralWrappers.AlertPopup("Error", "Color is invalid");
				return;
			}
			ServerAPICore.GetInstance().ChangePlayerCustomRankColor(currentChangingPlayerCustomRankId, customRankColor.Trim());
			if (PlayerUtils.playerCustomRank.ContainsKey(currentChangingPlayerCustomRankId))
			{
				CustomRankInfo customRankInfo = PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId];
				PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId] = new CustomRankInfo
				{
					customRankEnabled = customRankInfo.customRankEnabled,
					customRank = customRankInfo.customRank,
					customRankColorEnabled = true,
					customRankColor = color
				};
			}
			else
			{
				PlayerUtils.playerCustomRank.Add(currentChangingPlayerCustomRankId, new CustomRankInfo
				{
					customRankEnabled = false,
					customRank = string.Empty,
					customRankColorEnabled = true,
					customRankColor = color
				});
			}
			GeneralWrappers.AlertPopup("Warning", "Player rank color succesfully set");
		}

		private void RemovePlayerCustomRankName()
		{
			ServerAPICore.GetInstance().RemovePlayerCustomRankName(currentChangingPlayerCustomRankId);
			if (PlayerUtils.playerCustomRank.ContainsKey(currentChangingPlayerCustomRankId))
			{
				CustomRankInfo customRankInfo = PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId];
				PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId] = new CustomRankInfo
				{
					customRankEnabled = false,
					customRank = string.Empty,
					customRankColorEnabled = customRankInfo.customRankColorEnabled,
					customRankColor = customRankInfo.customRankColor
				};
				GeneralWrappers.AlertPopup("Warning", "Player rank succesfully removed");
			}
			else
			{
				GeneralWrappers.AlertPopup("Warning", "Player doesn't have custom rank");
			}
		}

		private void RemovePlayerCustomRankColor()
		{
			ServerAPICore.GetInstance().RemovePlayerCustomRankColor(currentChangingPlayerCustomRankId);
			if (PlayerUtils.playerCustomRank.ContainsKey(currentChangingPlayerCustomRankId))
			{
				CustomRankInfo customRankInfo = PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId];
				PlayerUtils.playerCustomRank[currentChangingPlayerCustomRankId] = new CustomRankInfo
				{
					customRankEnabled = customRankInfo.customRankEnabled,
					customRank = customRankInfo.customRank,
					customRankColorEnabled = false,
					customRankColor = default(Color)
				};
				GeneralWrappers.AlertPopup("Warning", "Player rank color succesfully removed");
			}
			else
			{
				GeneralWrappers.AlertPopup("Warning", "Player doesn't have custom rank color");
			}
		}
	}
}
