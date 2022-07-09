using System.Collections.Generic;
using MunchenClient.Core;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace MunchenClient.Misc
{
	internal class PortableMirror
	{
		private static readonly Dictionary<int, Mirror> portableMirrors = new Dictionary<int, Mirror>();

		internal static Mirror SpawnMirror(int identifier, float sizeX, float sizeY, bool pickupable)
		{
			if (portableMirrors.ContainsKey(identifier))
			{
				return portableMirrors[identifier];
			}
			Camera playerCamera = GeneralWrappers.GetPlayerCamera();
			GameObject gameObject = Object.Instantiate(AssetLoader.LoadGameObject("Mirror"));
			gameObject.transform.localScale = new Vector3(sizeX, sizeY, 1f);
			gameObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
			gameObject.transform.LookAt(playerCamera.transform);
			gameObject.transform.Rotate(0f, 180f, 0f, Space.Self);
			gameObject.GetComponent<Collider>().isTrigger = true;
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			VRCPickup vRCPickup = gameObject.AddComponent<VRCPickup>();
			vRCPickup.proximity = 4f;
			vRCPickup.pickupable = pickupable;
			vRCPickup.orientation = VRC_Pickup.PickupOrientation.Grip;
			vRCPickup.AutoHold = VRC_Pickup.AutoHoldMode.No;
			vRCPickup.pickupDropEventBroadcastType = VRC_EventHandler.VrcBroadcastType.Local;
			vRCPickup.useEventBroadcastType = VRC_EventHandler.VrcBroadcastType.Local;
			portableMirrors[identifier] = new Mirror
			{
				portableMirror = gameObject,
				portableMirrorRigidbody = rigidbody,
				portableMirrorPickup = vRCPickup
			};
			return portableMirrors[identifier];
		}

		internal static void RemoveMirror(int identifier)
		{
			if (portableMirrors.ContainsKey(identifier))
			{
				Object.Destroy(portableMirrors[identifier].portableMirror);
				portableMirrors.Remove(identifier);
			}
		}

		internal static void RemoveAllMirrors()
		{
			for (int i = 0; i < portableMirrors.Count; i++)
			{
				Object.Destroy(portableMirrors[i].portableMirror);
			}
			portableMirrors.Clear();
		}

		internal static Mirror GetMirror(int identifier)
		{
			if (!portableMirrors.ContainsKey(identifier))
			{
				return null;
			}
			return portableMirrors[identifier];
		}
	}
}
