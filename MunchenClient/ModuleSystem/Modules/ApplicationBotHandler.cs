using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ExitGames.Client.Photon;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Misc;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using Photon.Realtime;
using SharpNeatLib.Maths;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRC.Core;
using VRC.SDKBase;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class ApplicationBotHandler : ModuleComponent
	{
		private static readonly int botTargetFramerate = 30;

		private static readonly float botConnectionTimeout = 300f;

		internal static FastRandom botFastRandom = null;

		private static int botAmount = 0;

		private static Socket masterSocket = null;

		private static readonly System.Collections.Generic.List<Socket> botConnections = new System.Collections.Generic.List<Socket>();

		private static string botUsername = string.Empty;

		private static string botPassword = string.Empty;

		private static bool isAllFullyConnected = false;

		private static bool isBot = false;

		private static int botIndex = -1;

		private static bool firstSpawnDetected = false;

		private static string homeWorldID = string.Empty;

		private static bool serverStarting = false;

		private static System.Action<bool> serverStartingCallback = null;

		private static bool serverStartingFailed = false;

		private static float serverStartingTimeout = 0f;

		private static GameObject goButton = null;

		private static bool blockEvent7FromSending = false;

		internal static PlayerInformation userToImitate = null;

		internal static PlayerInformation userToFollow = null;

		protected override string moduleName => "Bot Handler";

		internal static bool IsBot()
		{
			return isBot;
		}

		internal static bool IsBotsFullyConnected()
		{
			return isAllFullyConnected;
		}

		internal static int GetBotAmount()
		{
			return botAmount;
		}

		internal static void PreCheckBotHandler()
		{
			if (!(MainUtils.GetLaunchParameter("--munchenBot") == string.Empty))
			{
				isBot = true;
				botIndex = int.Parse(MainUtils.GetLaunchParameter("--profile")) - 1;
				botUsername = MainUtils.GetLaunchParameter("--munchenBotUsername");
				botPassword = MainUtils.GetLaunchParameter("--munchenBotPassword");
				botFastRandom = new FastRandom(botIndex + UnityEngine.Random.Range(0, 999999995));
				Configuration.GetGeneralConfig().SpooferHWID = true;
				Configuration.GetGeneralConfig().SpooferSteamID = true;
				Configuration.GetGeneralConfig().SpooferPeripheral = true;
				Configuration.GetGeneralConfig().AntiFreezeExploit = true;
				Configuration.GetGeneralConfig().AntiInstanceLock = true;
				Application.targetFrameRate = botTargetFramerate;
				ConsoleUtils.Info("Bot Client", "Starting bot", System.ConsoleColor.Gray, "PreCheckBotHandler", 98);
			}
		}

		internal override void OnUIManagerLoaded()
		{
			if (isBot)
			{
				DisableAudioSystems();
				goButton = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/GoButton");
				MelonCoroutines.Start(CheckForVRChatLogin(10f));
				GameObject gameObject = GameObject.Find("_Application");
				gameObject.GetComponent<Screenshot>().enabled = false;
				gameObject.transform.Find("InputManager").gameObject.SetActive(value: false);
			}
		}

		internal override void OnUpdate()
		{
			if (!isBot)
			{
				if (serverStarting && Time.realtimeSinceStartup >= serverStartingTimeout)
				{
					serverStartingFailed = true;
					for (int i = 0; i < botConnections.Count; i++)
					{
						botConnections[i].Disconnect(reuseSocket: false);
						botConnections[i].Dispose();
					}
					botConnections.Clear();
					masterSocket.Close();
					masterSocket.Dispose();
					serverStarting = false;
					serverStartingCallback?.Invoke(obj: false);
					ConsoleUtils.Info("Bot Server", "Bots didn't start in time - aborting", System.ConsoleColor.Gray, "OnUpdate", 139);
				}
			}
			else if (goButton != null && goButton.activeInHierarchy)
			{
				goButton.GetComponent<Button>().Press();
			}
		}

		internal override void OnLevelWasInitialized(int levelIndex)
		{
			if (isBot)
			{
				MelonCoroutines.Start(CheckForCaptcha());
			}
		}

		internal override void OnPlayerJoin(PlayerInformation playerInfo)
		{
			if (!isBot || !playerInfo.isLocalPlayer)
			{
				return;
			}
			if (!firstSpawnDetected)
			{
				InitializeClientFully();
				firstSpawnDetected = true;
			}
			foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
			{
				if (rootGameObject.name != "VRCWorld" && rootGameObject.name != "PlayerManager" && rootGameObject.name != "SceneEventHandlerAndInstantiator" && rootGameObject.name != "VRC_OBJECTS" && !rootGameObject.name.Contains("VRCPlayer"))
				{
					UnityEngine.Object.DestroyImmediate(rootGameObject, allowDestroyingAssets: true);
				}
			}
			Physics.gravity = Vector3.zero;
			GeneralUtils.ToggleCollidersOnPlayer(toggle: false);
			playerInfo.vrcPlayer.transform.Find("AnimationController").gameObject.SetActive(value: false);
			VRCUiManager.field_Private_Static_VRCUiManager_0.enabled = false;
			CleanupCachedData();
		}

		private void InitializeClientFully()
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				StartClient();
			});
			thread.Start();
			GeneralWrappers.GetPlayerCamera().enabled = false;
			GeneralWrappers.GetUICamera().enabled = false;
			homeWorldID = WorldUtils.GetCurrentWorld().id + ":" + WorldUtils.GetCurrentInstance().instanceId;
			ConsoleUtils.Info("Bot Client", "Fully initialized as " + APIUser.CurrentUser.displayName, System.ConsoleColor.Gray, "InitializeClientFully", 214);
		}

		private void DisableAudioSystems()
		{
			foreach (AudioSource item in Resources.FindObjectsOfTypeAll<AudioSource>())
			{
				item.Stop();
				item.volume = 0f;
				item.enabled = false;
			}
			foreach (AudioListener item2 in Resources.FindObjectsOfTypeAll<AudioListener>())
			{
				item2.enabled = false;
			}
		}

		private static IEnumerator CheckForCaptcha()
		{
			yield return new WaitForSeconds(2f);
			GameObject captcha = GameObject.Find("UserInterface/MenuContent/Popups/InputCaptchaPopup");
			if (captcha.active)
			{
				ConsoleUtils.Info("Bot Client", "Input captcha required! Manual input required", System.ConsoleColor.Gray, "CheckForCaptcha", 242);
			}
		}

		private static IEnumerator CheckForVRChatLogin(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			GameObject loginPrompt = GameObject.Find("UserInterface/MenuContent/Screens/Authentication/StoreLoginPrompt");
			if (loginPrompt.activeInHierarchy)
			{
				ConsoleUtils.Info("Bot Client", "Login required! Starting bot will take longer than usual", System.ConsoleColor.Gray, "CheckForVRChatLogin", 256);
				GameObject vrchatLogin = loginPrompt.transform.Find("VRChatButtonLogin").gameObject;
				vrchatLogin.GetComponent<Button>().Press();
				yield return new WaitForSeconds(2f);
				UserInterface.PasteStringIntoInputField(botUsername);
				yield return new WaitForSeconds(2f);
				UserInterface.PasteStringIntoInputField(botPassword);
			}
		}

		private static void OnCommandReceived(string data)
		{
			string[] array = data.Split(':');
			if (array[0] == "JoinMe")
			{
				WorldUtils.GoToWorld(array[1] + ":" + array[2]);
			}
			else if (array[0] == "LeaveLobby")
			{
				WorldUtils.GoToWorld(homeWorldID);
			}
			else if (array[0] == "SetFPS")
			{
				if (!int.TryParse(array[1], out var result))
				{
					ConsoleUtils.Info("Bot Client", "FPS is invalid (" + array[1] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 287);
				}
				else
				{
					Application.targetFrameRate = result;
				}
			}
			else if (array[0] == "VoiceData")
			{
				PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
				if (localPlayerInformation != null)
				{
					byte[] array2 = System.Convert.FromBase64String(array[1]);
					byte[] bytes = System.BitConverter.GetBytes(localPlayerInformation.actorId);
					System.Buffer.BlockCopy(bytes, 0, array2, 0, 4);
					PhotonUtils.OpRaiseEvent(1, array2, new RaiseEventOptions
					{
						field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
						field_Public_EventCaching_0 = EventCaching.DoNotCache
					}, default(SendOptions));
				}
			}
			else if (array[0] == "SerializedData")
			{
				PlayerInformation localPlayerInformation2 = PlayerWrappers.GetLocalPlayerInformation();
				if (localPlayerInformation2 == null)
				{
					return;
				}
				PlayerInformation playerInformationByID = PlayerWrappers.GetPlayerInformationByID(array[1]);
				if (playerInformationByID != null)
				{
					byte[] unpackedData = System.Convert.FromBase64String(array[2]);
					Vector3 position = Event7Wrapper.GetPosition(ref unpackedData);
					Quaternion rotation = Event7Wrapper.GetRotation(ref unpackedData);
					Transform transform = playerInformationByID.vrcPlayer.transform;
					transform.rotation = rotation;
					switch (botIndex)
					{
					case 0:
						position += transform.right / 2f;
						break;
					case 1:
						position -= transform.right / 2f;
						break;
					case 2:
						position -= transform.forward / 2f;
						break;
					case 3:
						position -= transform.forward / 2f;
						position += transform.right / 2f;
						break;
					case 4:
						position -= transform.forward / 2f;
						position -= transform.right / 2f;
						break;
					}
					localPlayerInformation2.vrcPlayer.transform.position = position;
					localPlayerInformation2.vrcPlayer.transform.rotation = rotation;
					Event7Wrapper.SetActorID(ref unpackedData, localPlayerInformation2.actorIdData);
					Event7Wrapper.SetPosition(ref unpackedData, position);
					Event7Wrapper.SetPing(ref unpackedData, localPlayerInformation2.GetPing());
					Event7Wrapper.SetFPS(ref unpackedData, localPlayerInformation2.GetFPSRaw());
					blockEvent7FromSending = false;
					PhotonUtils.OpRaiseEvent(7, unpackedData, new RaiseEventOptions
					{
						field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
						field_Public_EventCaching_0 = EventCaching.DoNotCache
					}, default(SendOptions));
					blockEvent7FromSending = true;
				}
			}
			else if (array[0] == "FollowUser")
			{
				userToFollow = PlayerWrappers.GetPlayerInformationByID(array[1]);
				blockEvent7FromSending = userToImitate != null;
			}
			else if (array[0] == "ImitateUser")
			{
				userToImitate = PlayerWrappers.GetPlayerInformationByID(array[1]);
			}
			else if (array[0] == "ChangeAvatar")
			{
				PlayerInformation localPlayerInformation3 = PlayerWrappers.GetLocalPlayerInformation();
				if (localPlayerInformation3 != null)
				{
					PlayerUtils.ChangePlayerAvatar(array[1], logErrorOnHud: false);
				}
			}
			else if (array[0] == "Teleport")
			{
				PlayerInformation localPlayerInformation4 = PlayerWrappers.GetLocalPlayerInformation();
				if (localPlayerInformation4 != null)
				{
					float result3;
					float result4;
					if (!float.TryParse(array[1], out var result2))
					{
						ConsoleUtils.Info("Bot Client", "X position is invalid (" + array[1] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 393);
					}
					else if (!float.TryParse(array[2], out result3))
					{
						ConsoleUtils.Info("Bot Client", "Y position is invalid (" + array[2] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 399);
					}
					else if (!float.TryParse(array[3], out result4))
					{
						ConsoleUtils.Info("Bot Client", "Z position is invalid (" + array[3] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 405);
					}
					else
					{
						localPlayerInformation4.vrcPlayer.transform.position = new Vector3(result2, result3, result4);
					}
				}
			}
			else if (array[0] == "Rotate")
			{
				PlayerInformation localPlayerInformation5 = PlayerWrappers.GetLocalPlayerInformation();
				if (localPlayerInformation5 != null)
				{
					float result6;
					float result7;
					float result8;
					if (!float.TryParse(array[1], out var result5))
					{
						ConsoleUtils.Info("Bot Client", "X rotation is invalid (" + array[1] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 421);
					}
					else if (!float.TryParse(array[2], out result6))
					{
						ConsoleUtils.Info("Bot Client", "Y rotation is invalid (" + array[2] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 427);
					}
					else if (!float.TryParse(array[3], out result7))
					{
						ConsoleUtils.Info("Bot Client", "Z rotation is invalid (" + array[3] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 433);
					}
					else if (!float.TryParse(array[4], out result8))
					{
						ConsoleUtils.Info("Bot Client", "W rotation is invalid (" + array[4] + ")", System.ConsoleColor.Gray, "OnCommandReceived", 439);
					}
					else
					{
						localPlayerInformation5.vrcPlayer.transform.rotation = new Quaternion(result5, result6, result7, result8);
					}
				}
			}
			else if (array[0] == "StopBot")
			{
				Process.GetCurrentProcess().Kill();
			}
		}

		private void CleanupCachedData()
		{
			AssetBundleDownloadManager assetBundleDownloadManager = AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0;
			System.Collections.Generic.Dictionary<string, AssetBundleDownload> dictionary = new System.Collections.Generic.Dictionary<string, AssetBundleDownload>();
			Il2CppSystem.Collections.Generic.Dictionary<string, AssetBundleDownload>.KeyCollection.Enumerator enumerator = assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				dictionary.Add(current, assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0[current]);
			}
			foreach (string key in dictionary.Keys)
			{
				AssetBundleDownload assetBundleDownload = assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0[key];
				if (assetBundleDownload.field_Private_String_0.Contains("wrld_"))
				{
					assetBundleDownload.field_Private_AssetBundle_0.Unload(unloadAllLoadedObjects: true);
					assetBundleDownloadManager.field_Private_Dictionary_2_String_AssetBundleDownload_0.Remove(key);
				}
			}
			dictionary.Clear();
			GeneralUtils.ClearVRAM();
		}

		internal static void StartPhotonBots(System.Action<bool> onFinished, int requestedBotAmount)
		{
			ConsoleUtils.Info("Bot Server", "Starting servers", System.ConsoleColor.Gray, "StartPhotonBots", 481);
			string text = Configuration.GetClientFolderPath() + "Bots.json";
			if (!File.Exists(text))
			{
				File.Create(text);
				ConsoleUtils.Info("Bot Server", "Config file has been created at " + text, System.ConsoleColor.Gray, "StartPhotonBots", 488);
				onFinished(obj: false);
				return;
			}
			string[] array = File.ReadAllLines(text);
			if (array == null || array.Length == 0)
			{
				ConsoleUtils.Info("Bot Server", "No bots found inside of " + text, System.ConsoleColor.Gray, "StartPhotonBots", 497);
				onFinished(obj: false);
				return;
			}
			string text2 = Configuration.GetGameFolderPath() + "\\Mods\\";
			ConsoleUtils.Info("Mods Path", text2, System.ConsoleColor.Gray, "StartPhotonBots", 505);
			botAmount = 0;
			int num = MathUtils.Clamp(array.Length, 1, MathUtils.Clamp(requestedBotAmount, 1, 5));
			ConsoleUtils.Info("Bot Server", $"Bots Available: {num}", System.ConsoleColor.Gray, "StartPhotonBots", 509);
			string fileName = Directory.GetCurrentDirectory() + "\\VRChat.exe";
			for (int i = 0; i < num; i++)
			{
				if (string.IsNullOrEmpty(array[i]))
				{
					ConsoleUtils.Info("Bot Server", $"Invalid account found at line {i}", System.ConsoleColor.Gray, "StartPhotonBots", 516);
					continue;
				}
				string[] array2 = array[i].Split(':');
				if (string.IsNullOrEmpty(array2[0]) || string.IsNullOrEmpty(array2[1]))
				{
					ConsoleUtils.Info("Bot Server", $"Invalid account found at line {i}", System.ConsoleColor.Gray, "StartPhotonBots", 524);
					continue;
				}
				Process.Start(fileName, string.Format("-nolog -batchmode -nographics --profile={0} --no-vr --fps={1} --munchenBot --munchenBotUsername={2} --munchenBotPassword={3} {4}", i + 1, botTargetFramerate, array2[0], array2[1], GeneralUtils.IsBetaClient() ? "--betaClient" : ""));
				botAmount++;
			}
			serverStartingTimeout = Time.realtimeSinceStartup + botConnectionTimeout;
			serverStartingFailed = false;
			serverStarting = true;
			Thread thread = new Thread((ThreadStart)delegate
			{
				StartServer(onFinished);
			});
			thread.Start();
			ConsoleUtils.Info("Bot Server", "Server successfully initialized", System.ConsoleColor.Gray, "StartPhotonBots", 540);
		}

		internal static void StopPhotonBots(System.Action<bool> onFinished)
		{
			SendCommandToBots("StopBot");
			botConnections.Clear();
			masterSocket.Close();
			masterSocket.Dispose();
			isAllFullyConnected = false;
			onFinished?.Invoke(obj: true);
		}

		internal static void SendCommandToBots(string command)
		{
			for (int i = 0; i < botConnections.Count; i++)
			{
				SendCommandToBot(i, command);
			}
		}

		internal static void SendCommandToBot(int index, string command)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(command + ",");
			try
			{
				botConnections[index].Send(bytes);
			}
			catch (SocketException)
			{
			}
		}

		private static void StartServer(System.Action<bool> onFinished)
		{
			try
			{
				serverStartingCallback = onFinished;
				IPAddress iPAddress = Dns.GetHostEntry("localhost").AddressList[0];
				masterSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				masterSocket.Bind(new IPEndPoint(iPAddress, 1738));
				masterSocket.Listen(10);
				ConsoleUtils.Info("Bot Server", $"Server started. Waiting {botConnectionTimeout} seconds for {botAmount} bots to connect", System.ConsoleColor.Gray, "StartServer", 587);
				for (int i = 0; i < botAmount; i++)
				{
					Socket socket = masterSocket.Accept();
					if (!serverStartingFailed)
					{
						botConnections.Add(socket);
						ConsoleUtils.Info("Bot Server", $"Bot: {i} connected", System.ConsoleColor.Gray, "StartServer", 595);
					}
					else
					{
						socket.Close();
						socket.Dispose();
					}
				}
				if (serverStartingFailed)
				{
					isAllFullyConnected = false;
					ConsoleUtils.Info("Bot Server", "Bots failed to start", System.ConsoleColor.Gray, "StartServer", 607);
				}
				else
				{
					isAllFullyConnected = true;
					serverStarting = false;
					ConsoleUtils.Info("Bot Server", "Bots has been started", System.ConsoleColor.Gray, "StartServer", 613);
				}
				onFinished?.Invoke(!serverStartingFailed);
			}
			catch (System.Exception)
			{
				onFinished?.Invoke(obj: false);
			}
		}

		private static void StartClient()
		{
			try
			{
				IPAddress iPAddress = Dns.GetHostEntry("localhost").AddressList[0];
				masterSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				try
				{
					masterSocket.Connect(new IPEndPoint(iPAddress, 1738));
					ConsoleUtils.Info("Bot Client", "Connected to Server", System.ConsoleColor.Gray, "StartClient", 634);
					byte[] array = new byte[2048];
					while (GeneralUtils.GetMainProcess() != null)
					{
						int num = masterSocket.Receive(array);
						if (num == 0)
						{
							continue;
						}
						if (num > 1024)
						{
							ConsoleUtils.Info("Bot Client", $"Received oversized command: {num}", System.ConsoleColor.Gray, "StartClient", 653);
						}
						string @string = Encoding.ASCII.GetString(array, 0, num);
						if (string.IsNullOrEmpty(@string))
						{
							continue;
						}
						string[] array2 = @string.Split(',');
						string[] array3 = array2;
						foreach (string cmd in array3)
						{
							if (!string.IsNullOrEmpty(cmd))
							{
								MainUtils.mainThreadQueue.Enqueue(delegate
								{
									OnCommandReceived(cmd);
								});
							}
						}
					}
				}
				catch (System.ArgumentNullException e)
				{
					ConsoleUtils.Exception("Bot Client", "ArgumentNullException", e, "StartClient", 680);
				}
				catch (SocketException e2)
				{
					ConsoleUtils.Exception("Bot Client", "SocketException", e2, "StartClient", 684);
					Process.GetCurrentProcess().Kill();
				}
				catch (System.Exception e3)
				{
					ConsoleUtils.Exception("Bot Client", "Unknown", e3, "StartClient", 690);
				}
			}
			catch (System.Exception e4)
			{
				ConsoleUtils.Exception("Bot Client", "Start", e4, "StartClient", 695);
				Process.GetCurrentProcess().Kill();
			}
		}

		internal override bool OnEventReceived(ref EventData eventData)
		{
			if (!isBot || !GeneralUtils.isConnectedToInstance)
			{
				return true;
			}
			PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
			if (localPlayerInformation == null)
			{
				return true;
			}
			try
			{
				if (eventData.Code == 1)
				{
					PlayerInformation playerInformationByInstagatorID = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
					playerInformationByInstagatorID.lastNetworkedVoicePacket = Time.realtimeSinceStartup;
					if (userToImitate != null && userToImitate.id == playerInformationByInstagatorID.id)
					{
						byte[] unpackedData = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
						Event1Wrapper.SetActorID(ref unpackedData, localPlayerInformation.actorId);
						Event1Wrapper.SetServerTime(ref unpackedData, Networking.GetServerTimeInMilliseconds());
						PhotonUtils.OpRaiseEvent(1, unpackedData, new RaiseEventOptions
						{
							field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
							field_Public_EventCaching_0 = EventCaching.DoNotCache
						}, default(SendOptions));
					}
					return false;
				}
			}
			catch (System.Exception e)
			{
				ConsoleUtils.Exception("Bot", "VoiceData", e, "OnEventReceived", 743);
			}
			if (eventData.Code == 6)
			{
				return false;
			}
			try
			{
				if (eventData.Code == 7)
				{
					PlayerInformation playerInformationByInstagatorID2 = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
					if (userToFollow != null && userToFollow.id == playerInformationByInstagatorID2.id)
					{
						byte[] unpackedData2 = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
						if (Event7Wrapper.GetActorID(ref unpackedData2) == playerInformationByInstagatorID2.actorIdData)
						{
							Vector3 position = Event7Wrapper.GetPosition(ref unpackedData2);
							Quaternion rotation = Event7Wrapper.GetRotation(ref unpackedData2);
							Transform transform = userToFollow.vrcPlayer.transform;
							transform.rotation = rotation;
							switch (botIndex)
							{
							case 0:
								position += transform.right / 2f;
								break;
							case 1:
								position -= transform.right / 2f;
								break;
							case 2:
								position -= transform.forward / 2f;
								break;
							case 3:
								position -= transform.forward / 2f;
								position += transform.right / 2f;
								break;
							case 4:
								position -= transform.forward / 2f;
								position -= transform.right / 2f;
								break;
							}
							localPlayerInformation.vrcPlayer.transform.position = position;
							localPlayerInformation.vrcPlayer.transform.rotation = rotation;
							Event7Wrapper.SetActorID(ref unpackedData2, localPlayerInformation.actorIdData);
							Event7Wrapper.SetPosition(ref unpackedData2, position);
							Event7Wrapper.SetPing(ref unpackedData2, localPlayerInformation.GetPing());
							Event7Wrapper.SetFPS(ref unpackedData2, localPlayerInformation.GetFPSRaw());
							blockEvent7FromSending = false;
							PhotonUtils.OpRaiseEvent(7, unpackedData2, new RaiseEventOptions
							{
								field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
								field_Public_EventCaching_0 = EventCaching.DoNotCache
							}, default(SendOptions));
							blockEvent7FromSending = true;
						}
					}
					return false;
				}
			}
			catch (System.Exception e2)
			{
				ConsoleUtils.Exception("Bot", "SerializedData", e2, "OnEventReceived", 804);
			}
			try
			{
				if (eventData.Code == 9)
				{
					PlayerInformation playerInformationByInstagatorID3 = PlayerWrappers.GetPlayerInformationByInstagatorID(eventData.Sender);
					if (userToFollow != null && userToFollow.id == playerInformationByInstagatorID3.id)
					{
						byte[] buffer = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.CustomData.Pointer);
						int num = SerializationUtils.ReadInt(ref buffer, 0);
						if (num == playerInformationByInstagatorID3.actorIdDataOther)
						{
							System.Buffer.BlockCopy(System.BitConverter.GetBytes(playerInformationByInstagatorID3.actorIdDataOther), 0, buffer, 0, 4);
							PhotonUtils.OpRaiseEvent(9, buffer, new RaiseEventOptions
							{
								field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
								field_Public_EventCaching_0 = EventCaching.DoNotCache
							}, default(SendOptions));
						}
					}
					return false;
				}
			}
			catch (System.Exception e3)
			{
				ConsoleUtils.Exception("Bot", "SerializedDataReliable", e3, "OnEventReceived", 837);
			}
			return true;
		}

		internal override bool OnEventSent(byte eventCode, ref Il2CppSystem.Object eventData, ref RaiseEventOptions raiseEventOptions)
		{
			if (!isBot)
			{
				if (eventCode == 1 && userToImitate != null && userToImitate == PlayerWrappers.GetLocalPlayerInformation())
				{
					byte[] array = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.Pointer);
					byte[] bytes = System.BitConverter.GetBytes(Networking.GetServerTimeInMilliseconds());
					System.Buffer.BlockCopy(bytes, 0, array, 4, 4);
					SendCommandToBots("VoiceData:" + System.Convert.ToBase64String(array));
				}
				else if (eventCode == 7 && userToFollow != null && userToFollow == PlayerWrappers.GetLocalPlayerInformation())
				{
					byte[] unpackedData = Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(eventData.Pointer);
					if (Event7Wrapper.GetActorID(ref unpackedData) == PlayerWrappers.GetLocalPlayerInformation().actorIdData)
					{
						SendCommandToBots("SerializedData:" + PlayerWrappers.GetLocalPlayerInformation().id + ":" + System.Convert.ToBase64String(unpackedData));
					}
				}
				return true;
			}
			if (eventCode == 7)
			{
				return !blockEvent7FromSending;
			}
			return true;
		}
	}
}
