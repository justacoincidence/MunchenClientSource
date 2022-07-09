using System;
using MunchenClient.Menu;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC.SDKBase;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class PlayerTargetHandler : ModuleComponent
	{
		private static VRC_Pickup[] cachedItemPickups;

		private static GameObject centerItemOrbit;

		private static PlayerInformation selectedPlayerToOrbit;

		internal static bool attachToPlayer;

		private static PlayerInformation selectedPlayerToAttach;

		private static HumanBodyBones attachmentPoint;

		private static bool attachmentPointOffset;

		private static bool attachmentPointOrbit;

		private static GameObject attachmentPointOrbitPoint;

		protected override string moduleName => "Player Target Handler";

		internal override void OnUpdate()
		{
			if (GeneralUtils.itemOrbit)
			{
				if (selectedPlayerToOrbit != null)
				{
					centerItemOrbit.transform.position = selectedPlayerToOrbit.playerApi.GetBonePosition(HumanBodyBones.Chest);
					VRC_Pickup[] array = cachedItemPickups;
					foreach (VRC_Pickup vRC_Pickup in array)
					{
						if (Networking.GetOwner(vRC_Pickup.gameObject) != Networking.LocalPlayer)
						{
							Networking.SetOwner(Networking.LocalPlayer, vRC_Pickup.gameObject);
						}
						vRC_Pickup.transform.position = centerItemOrbit.transform.position + centerItemOrbit.transform.forward * 1f;
						centerItemOrbit.transform.Rotate(new Vector3(0f, 360 / cachedItemPickups.Length, 0f));
					}
					centerItemOrbit.transform.Rotate(Vector3.up, Time.deltaTime * 180f);
				}
				else
				{
					GeneralUtils.itemOrbit = false;
					FunMenu.itemOrbitButton.SetToggleState(state: false);
				}
			}
			if (!attachToPlayer)
			{
				return;
			}
			if (selectedPlayerToAttach != null)
			{
				if (!GeneralUtils.flight)
				{
					GeneralUtils.flight = true;
					GeneralUtils.ToggleFlight(state: true);
				}
				try
				{
					if (attachmentPointOrbit)
					{
						attachmentPointOrbitPoint.transform.position = selectedPlayerToAttach.playerApi.GetBonePosition(HumanBodyBones.Chest);
						attachmentPointOrbitPoint.transform.Rotate(Vector3.up, Time.deltaTime * 180f);
						attachmentPointOrbitPoint.transform.Translate(Vector3.forward);
						PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.transform.position = attachmentPointOrbitPoint.transform.position;
					}
					else
					{
						PlayerWrappers.GetLocalPlayerInformation().vrcPlayer.transform.position = selectedPlayerToAttach.playerApi.GetBonePosition(attachmentPoint) - (attachmentPointOffset ? (selectedPlayerToAttach.vrcPlayer.transform.forward / 4f) : Vector3.zero);
					}
				}
				catch (Exception)
				{
					attachToPlayer = false;
					PlayerMenu.attachToPlayerButton.SetToggleState(state: false);
					if (GeneralUtils.flight)
					{
						GeneralUtils.flight = false;
						GeneralUtils.ToggleFlight(state: false);
					}
				}
			}
			else
			{
				attachToPlayer = false;
				PlayerMenu.attachToPlayerButton.SetToggleState(state: false);
				if (GeneralUtils.flight)
				{
					GeneralUtils.flight = false;
					GeneralUtils.ToggleFlight(state: false);
				}
			}
			if (GeneralWrappers.IsInVR())
			{
				if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
				{
					attachToPlayer = false;
					PlayerMenu.attachToPlayerButton.SetToggleState(state: false);
					if (GeneralUtils.flight)
					{
						GeneralUtils.flight = false;
						GeneralUtils.ToggleFlight(state: false);
					}
				}
			}
			else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space))
			{
				attachToPlayer = false;
				PlayerMenu.attachToPlayerButton.SetToggleState(state: false);
				if (GeneralUtils.flight)
				{
					GeneralUtils.flight = false;
					GeneralUtils.ToggleFlight(state: false);
				}
			}
		}

		internal override void OnPlayerLeft(PlayerInformation playerInfo)
		{
			if (GeneralUtils.itemOrbit && playerInfo == selectedPlayerToOrbit)
			{
				GeneralUtils.itemOrbit = false;
				FunMenu.itemOrbitButton.SetToggleState(state: false);
			}
			if (attachToPlayer && playerInfo == selectedPlayerToOrbit)
			{
				attachToPlayer = false;
				PlayerMenu.attachToPlayerButton.SetToggleState(state: false);
				if (GeneralUtils.flight)
				{
					GeneralUtils.flight = false;
					GeneralUtils.ToggleFlight(state: false);
				}
			}
		}

		internal override void OnRoomLeft()
		{
			attachToPlayer = false;
			PlayerMenu.attachToPlayerButton.SetToggleState(state: false);
			GeneralUtils.itemOrbit = false;
			FunMenu.itemOrbitButton.SetToggleState(state: false);
		}

		internal static void SelectNewPlayerToOrbit(PlayerInformation player)
		{
			if (player != selectedPlayerToOrbit)
			{
				if (centerItemOrbit != null)
				{
					UnityEngine.Object.DestroyImmediate(centerItemOrbit, allowDestroyingAssets: true);
				}
				centerItemOrbit = new GameObject("Munchen Item Orbit");
				centerItemOrbit.transform.position = player.playerApi.GetBonePosition(HumanBodyBones.Chest);
			}
			cachedItemPickups = UnityEngine.Object.FindObjectsOfType<VRC_Pickup>();
			selectedPlayerToOrbit = player;
		}

		internal static PlayerInformation GetPlayerToOrbit()
		{
			return selectedPlayerToOrbit;
		}

		internal static void SelectNewPlayerToAttach(PlayerInformation player, HumanBodyBones attachment, bool offset, bool orbit)
		{
			attachmentPoint = attachment;
			selectedPlayerToAttach = player;
			attachmentPointOffset = offset;
			attachmentPointOrbit = orbit;
			if (attachmentPointOrbit)
			{
				if (attachmentPointOrbitPoint != null)
				{
					UnityEngine.Object.DestroyImmediate(attachmentPointOrbitPoint, allowDestroyingAssets: true);
				}
				attachmentPointOrbitPoint = new GameObject("Munchen Player Orbit");
				attachmentPointOrbitPoint.transform.position = player.playerApi.GetBonePosition(attachment);
			}
		}
	}
}
