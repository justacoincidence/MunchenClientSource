using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Menu;
using MunchenClient.Menu.Fun;
using MunchenClient.Menu.Others;
using MunchenClient.Misc;
using MunchenClient.Patching.Patches;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using ServerAPI.Core;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.SDKBase;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class GeneralHandler : ModuleComponent
	{
		private bool previousModerationButtonState = true;

		private float nextCloneChange = 0f;

		private bool lastPositionRevealed = false;

		internal static readonly Il2CppSystem.Collections.Generic.List<Renderer> pickupRenderersToHighlight = new Il2CppSystem.Collections.Generic.List<Renderer>();

		private string currentAvatarId = string.Empty;

		protected override string moduleName => "General Handler";

		internal override void OnLateUpdate()
		{
			for (int i = 0; i < pickupRenderersToHighlight.Count; i++)
			{
				HighlightsFX.Method_Public_Static_Void_Renderer_Boolean_PDM_0(pickupRenderersToHighlight[i], param_1: true);
			}
		}

		internal override void OnUpdate()
		{
			try
			{
				UserInterface.OnUpdate();
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("GeneralHandler", "InterfaceUpdate", e, "OnUpdate", 48);
			}
			if (GeneralUtils.hideSelf)
			{
				PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
				if (currentAvatarId != localPlayerInformation.vrcPlayer.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id)
				{
					GeneralUtils.RemoveAvatarFromCache(currentAvatarId);
					currentAvatarId = localPlayerInformation.vrcPlayer.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id;
				}
			}
			if (GeneralUtils.isConnectedToInstance)
			{
				try
				{
					if (GeneralUtils.flight)
					{
						HandleFlight();
					}
				}
				catch (Exception e2)
				{
					ConsoleUtils.Exception("GeneralHandler", "FlightHandler", e2, "OnUpdate", 74);
				}
				try
				{
					GameObject socialMenuModerationButton = GeneralWrappers.GetSocialMenuModerationButton();
					if (socialMenuModerationButton != null && UserInfoMenu.teleportButton != null && previousModerationButtonState != socialMenuModerationButton.activeInHierarchy)
					{
						UserInfoMenu.teleportButton.SetActive(socialMenuModerationButton.activeInHierarchy);
						UserInfoMenu.favoriteAvatarButton.SetActive(socialMenuModerationButton.activeInHierarchy);
						UserInfoMenu.stealAvatarButton.SetActive(socialMenuModerationButton.activeInHierarchy);
						previousModerationButtonState = GeneralWrappers.GetSocialMenuModerationButton().activeInHierarchy;
					}
				}
				catch (Exception e3)
				{
					ConsoleUtils.Exception("GeneralHandler", "MenuModerationHandler", e3, "OnUpdate", 96);
				}
				try
				{
					SerializeFakelagFix();
				}
				catch (Exception e4)
				{
					ConsoleUtils.Exception("GeneralHandler", "SerializeFakelagFix", e4, "OnUpdate", 105);
				}
			}
			try
			{
				if (ImageDownloaderPatch.imageDownloadQueue.Count > 0 && ImageDownloaderPatch.imageDownloadQueue.TryDequeue(out var imageContainer))
				{
					ImageDownloaderPatch.DownloadImage(imageContainer.imageUrl, imageContainer.imageSize, (Action<Texture2D>)delegate(Texture2D tex)
					{
						CacheUtils.AddCachedImage(imageContainer.imageUrl, tex);
						imageContainer.onImageDownload.Invoke(tex);
					}, (Action)delegate
					{
						imageContainer.onImageDownloadFailed.Invoke();
					}, imageContainer.fallbackImageUrl, imageContainer.isRetry);
				}
			}
			catch (Exception e5)
			{
				ConsoleUtils.Exception("GeneralHandler", "ImageDownloaderHandler", e5, "OnUpdate", 127);
			}
		}

		internal override void OnUIManagerLoaded()
		{
			ClassInjector.RegisterTypeInIl2Cpp<MunchenCustomRankFixer>();
			GameObject gameObject = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo");
			gameObject.AddComponent<MunchenCustomRankFixer>();
		}

		internal override void OnLevelWasInitialized(int levelIndex)
		{
			if (levelIndex != -1)
			{
				return;
			}
			if (Configuration.GetGeneralConfig().ItemWallhack)
			{
				pickupRenderersToHighlight.Clear();
				VRC_Pickup[] array = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].gameObject.name == "ViewFinder")
					{
						continue;
					}
					System.Collections.Generic.List<Renderer> list = MiscUtils.FindAllComponentsInGameObject<Renderer>(array[i].gameObject, includeInactive: false, searchParent: false);
					for (int j = 0; j < list.Count; j++)
					{
						if (!(list[j] == null))
						{
							GeneralWrappers.EnableOutline(list[j], state: true);
							pickupRenderersToHighlight.Add(list[j]);
						}
					}
				}
			}
			if (Configuration.GetGeneralConfig().TriggerWallhack)
			{
				VRC_Trigger[] array2 = Resources.FindObjectsOfTypeAll<VRC_Trigger>();
				for (int k = 0; k < array2.Length; k++)
				{
					System.Collections.Generic.List<Renderer> list2 = MiscUtils.FindAllComponentsInGameObject<Renderer>(array2[k].gameObject, includeInactive: false, searchParent: false);
					if (list2.Count != 0 && !(list2[0] == null))
					{
						GeneralWrappers.EnableOutline(list2[0], state: true);
					}
				}
			}
			WorldUtils.CacheAnnoyingGameObjects();
			WorldUtils.FixVoidClubAnnoyingEntryDoor(Configuration.GetGeneralConfig().VoidClubAnnoyingEntryDoorFix);
			WorldUtils.FixMurderAntiKillscreen(Configuration.GetGeneralConfig().MurderAntiKillscreen);
			WorldUtils.FixUnnecessaryDoorsInTheGreatPug(Configuration.GetGeneralConfig().TheGreatPugRemoveUnnecessaryDoors);
			WorldUtils.FixAnnoyingIntroInJustBClub(Configuration.GetGeneralConfig().JustBClubIntroFix);
		}

		internal override void OnRoomJoined()
		{
			GeneralUtils.isConnectedToInstance = true;
			if (!ApplicationBotHandler.IsBot())
			{
				AntiCrashPatch.OnRoomJoined();
				ServerAPICore.GetInstance().LinkVRChatAccountToAuthKey(APIUser.CurrentUser.id, APIUser.CurrentUser.displayName);
			}
		}

		internal override void OnRoomLeft()
		{
			if (!ApplicationBotHandler.IsBot())
			{
				AntiCrashPatch.OnRoomLeft();
				if (GeneralUtils.localAvatarClone)
				{
					GeneralUtils.localAvatarClone = false;
					PlayerMenu.localAvatarCloneButton.SetToggleState(state: false);
					UnityEngine.Object.DestroyImmediate(PlayerMenu.localAvatarClone);
				}
				GeneralUtils.ToggleFlight(state: false);
				MovementMenu.flightButton.SetToggleState(state: false);
				if (GeneralUtils.capsuleHider)
				{
					GeneralUtils.capsuleHider = false;
					PlayerUtils.ChangeCapsuleState(state: false);
					FunMenu.capsuleHiderButton.SetToggleState(state: false);
				}
				PhotonExploitsMenu.earrapeExploitButton.SetToggleState(state: false);
				PhotonExploitsMenu.StopEarrapeExploit();
				PortableMirror.RemoveAllMirrors();
				PortableMirrorMenu.portableMirrorButton.SetToggleState(state: false);
				GeneralUtils.portableMirror = false;
				PlayerHandler.ResetPlayerESPStates();
				if (GeneralUtils.hideSelf)
				{
					GeneralWrappers.GetAvatarPreviewBase().SetActive(value: true);
					PlayerWrappers.GetCurrentPlayer().prop_VRCAvatarManager_0.gameObject.SetActive(value: true);
					AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0.gameObject.SetActive(value: true);
					GeneralUtils.hideSelf = false;
				}
			}
			PlayerUtils.ClearAllClones();
			PlayerUtils.playerCachingList.Clear();
			PlayerUtils.localPlayerInfo = null;
			GeneralUtils.notificationTracker.Clear();
			pickupRenderersToHighlight.Clear();
			GeneralUtils.isConnectedToInstance = false;
		}

		internal override void OnRoomMasterChanged(PlayerInformation newMaster)
		{
			newMaster.isInstanceMaster = true;
		}

		private void HandleFlight()
		{
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			if (localPlayerInformation == null)
			{
				return;
			}
			float num = (Input.GetKey(KeyCode.LeftShift) ? (Configuration.GetGeneralConfig().FlightSpeed * 2f) : ((!Input.GetKey(KeyCode.LeftControl)) ? Configuration.GetGeneralConfig().FlightSpeed : (Configuration.GetGeneralConfig().FlightSpeed / 2f)));
			if (GeneralWrappers.IsInVR())
			{
				if (Input.GetAxis("Vertical") != 0f)
				{
					localPlayerInformation.vrcPlayer.transform.position -= GeneralWrappers.GetPlayerCamera().transform.forward * num * Time.deltaTime * (Input.GetAxis("Vertical") * -1f);
				}
				if (Input.GetAxis("Horizontal") != 0f)
				{
					localPlayerInformation.vrcPlayer.transform.position += GeneralWrappers.GetPlayerCamera().transform.right * num * Time.deltaTime * Input.GetAxis("Horizontal");
				}
				if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") != 0f)
				{
					localPlayerInformation.vrcPlayer.transform.position += localPlayerInformation.vrcPlayer.transform.up * num * Time.deltaTime * Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical");
				}
				return;
			}
			if (Input.GetKey(KeyCode.W))
			{
				localPlayerInformation.vrcPlayer.transform.position += GeneralWrappers.GetPlayerCamera().transform.forward * num * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.S))
			{
				localPlayerInformation.vrcPlayer.transform.position -= GeneralWrappers.GetPlayerCamera().transform.forward * num * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.A))
			{
				localPlayerInformation.vrcPlayer.transform.position -= localPlayerInformation.vrcPlayer.transform.right * num * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.D))
			{
				localPlayerInformation.vrcPlayer.transform.position += localPlayerInformation.vrcPlayer.transform.right * num * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				localPlayerInformation.vrcPlayer.transform.position -= localPlayerInformation.vrcPlayer.transform.up * num * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				localPlayerInformation.vrcPlayer.transform.position += localPlayerInformation.vrcPlayer.transform.up * num * Time.deltaTime;
			}
		}

		private void SerializeFakelagFix()
		{
			if (PlayerUtils.playerCachingList.Count > 1)
			{
				return;
			}
			if (GeneralUtils.serialization)
			{
				if (!Configuration.GetGeneralConfig().OnKeySerialization)
				{
					if (!lastPositionRevealed)
					{
						lastPositionRevealed = true;
						PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
					}
				}
				else if (GeneralWrappers.IsInVR())
				{
					if (Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.5f)
					{
						if (!lastPositionRevealed)
						{
							lastPositionRevealed = true;
							PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
						}
					}
					else if (lastPositionRevealed)
					{
						lastPositionRevealed = false;
						PlayerUtils.ClearClone(PlayerWrappers.GetLocalPlayerInformation());
					}
				}
				else if (Input.GetKey(KeyCode.LeftControl))
				{
					if (!lastPositionRevealed)
					{
						lastPositionRevealed = true;
						PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
					}
				}
				else if (lastPositionRevealed)
				{
					lastPositionRevealed = false;
					PlayerUtils.ClearClone(PlayerWrappers.GetLocalPlayerInformation());
				}
			}
			else if (GeneralUtils.fakelag)
			{
				if (Time.realtimeSinceStartup >= nextCloneChange)
				{
					nextCloneChange = Time.realtimeSinceStartup + 1.5f;
					PlayerUtils.GenerateAvatarClone(PlayerWrappers.GetLocalPlayerInformation());
				}
			}
			else if (lastPositionRevealed)
			{
				lastPositionRevealed = false;
				PlayerUtils.ClearClone(PlayerWrappers.GetLocalPlayerInformation());
			}
		}

		internal static void FixRank(Text textComponent)
		{
			MelonCoroutines.Start(FixRankEnumerator(textComponent));
		}

		private static IEnumerator FixRankEnumerator(Text textComponent)
		{
			yield return new WaitForEndOfFrame();
			textComponent.text = RemoveRichStyleFromText(textComponent.text);
		}

		internal static string RemoveRichStyleFromText(string text)
		{
			if (text == null)
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			while (true)
			{
				int num = text.IndexOf('<');
				if (num == -1)
				{
					break;
				}
				int num2 = text.IndexOf('>');
				if (num2 == -1 || num >= num2)
				{
					break;
				}
				string oldValue = text.Substring(num, num2 - num + 1);
				text = text.Replace(oldValue, "");
			}
			return text;
		}
	}
}
