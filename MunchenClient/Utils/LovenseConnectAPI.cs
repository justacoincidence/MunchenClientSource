using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MunchenClient.Utils
{
	internal class LovenseConnectAPI
	{
		private static LovenseDevice lovenseDevice = null;

		private static string protocolIp = string.Empty;

		private static LovenseConnectionState protocolConnectionState = LovenseConnectionState.Disconnected;

		private static Action<LovenseDevice> onLovenseConnected = null;

		private static Action<byte, bool, string> onVibrateChangeDone = null;

		private static byte vibrateChangeIntensity = 0;

		private static bool vibrateChanging = false;

		internal static bool IsLovenseConnected()
		{
			return protocolConnectionState == LovenseConnectionState.Connected;
		}

		internal static bool IsLovenseConnecting()
		{
			return protocolConnectionState == LovenseConnectionState.Connecting;
		}

		internal static bool DisconnectFromLovense()
		{
			if (!IsLovenseConnected() || IsLovenseConnecting())
			{
				return false;
			}
			protocolIp = string.Empty;
			protocolConnectionState = LovenseConnectionState.Disconnected;
			return true;
		}

		internal static void ConnectToLovense(string ip, Action<LovenseDevice> onConnected)
		{
			if (!IsLovenseConnected() && !IsLovenseConnecting())
			{
				protocolIp = ip;
				onLovenseConnected = onConnected;
				protocolConnectionState = LovenseConnectionState.Connecting;
				HttpClientWrapper.SendGetRequest("https://" + protocolIp + ":30010/GetToys", decryptOnReceive: false, 0f, OnLovenseFoundToys);
			}
		}

		internal static void OnLovenseFoundToys(bool error, string response)
		{
			if (error)
			{
				ConsoleUtils.Info("Lovense", "Failed connecting to Lovense (" + response + ")", ConsoleColor.Gray, "OnLovenseFoundToys", 98);
				protocolConnectionState = LovenseConnectionState.Disconnected;
				onLovenseConnected(null);
				return;
			}
			JObject jObject = JObject.Parse(response);
			string text = ((string?)jObject["type"])!.ToLower();
			int num = (int)jObject["code"];
			if (text != "ok" || num != 200)
			{
				ConsoleUtils.Info("Lovense", $"Got non-ok response from Lovense ({text} | {num})", ConsoleColor.Gray, "OnLovenseFoundToys", 111);
				protocolConnectionState = LovenseConnectionState.Disconnected;
				onLovenseConnected(null);
				return;
			}
			bool flag = false;
			foreach (KeyValuePair<string, JToken> item in (JObject)jObject["data"])
			{
				JObject jObject2 = JObject.Parse(item.Value.ToString());
				int num2 = (int)jObject2["status"];
				if (num2 != 1)
				{
					continue;
				}
				sbyte battery = (sbyte)((jObject2["battery"] != null) ? ((sbyte)jObject2["battery"]) : 0);
				string text2 = ((string?)jObject2["name"])!.ToLower();
				ConsoleUtils.Info("Lovense", "Name: " + text2, ConsoleColor.Gray, "OnLovenseFoundToys", 144);
				LovenseDeviceType lovenseDeviceType = (text2.Contains("max") ? LovenseDeviceType.AirPump : (text2.Contains("nora") ? LovenseDeviceType.Rotate : ((text2.Contains("edge") || text2.Contains("dolce")) ? LovenseDeviceType.DoubleVibrate : LovenseDeviceType.Generic)));
				ConsoleUtils.Info("Lovense", $"Device Type: {lovenseDeviceType}", ConsoleColor.Gray, "OnLovenseFoundToys", 163);
				lovenseDevice = new LovenseDevice
				{
					id = (string?)jObject2["id"],
					name = (string?)jObject2["name"],
					battery = battery,
					firmwareVersion = (string?)jObject2["fVersion"],
					nickname = (string?)jObject2["nickName"],
					mode = (string?)jObject2["mode"],
					version = (string?)jObject2["version"],
					type = lovenseDeviceType,
					intensity = 0
				};
				ConsoleUtils.Info("Lovense", "Found device named " + lovenseDevice.GetName(), ConsoleColor.Gray, "OnLovenseFoundToys", 179);
				flag = true;
				break;
			}
			if (!flag)
			{
				ConsoleUtils.Info("Lovense", "Found no valid device from Lovense", ConsoleColor.Gray, "OnLovenseFoundToys", 187);
				protocolConnectionState = LovenseConnectionState.Disconnected;
				onLovenseConnected(null);
			}
			else
			{
				ConsoleUtils.Info("Lovense", "Succesfully connected to Lovense", ConsoleColor.Gray, "OnLovenseFoundToys", 194);
				protocolConnectionState = LovenseConnectionState.Connected;
				onLovenseConnected(lovenseDevice);
			}
		}

		internal static void VibrateLovense(byte intensity, Action<byte, bool, string> onDone = null)
		{
			if (IsLovenseConnected() && !IsLovenseConnecting())
			{
				VibrateLovenseInternal(intensity, onDone);
			}
		}

		private static void VibrateLovenseInternal(byte intensity, Action<byte, bool, string> onDone = null)
		{
			if (vibrateChanging)
			{
				onDone?.Invoke(intensity, arg2: false, "Intensity already changing");
				return;
			}
			vibrateChanging = true;
			vibrateChangeIntensity = MathUtils.Clamp(intensity, (byte)0, (byte)20);
			onVibrateChangeDone = onDone;
			string text = lovenseDevice.type switch
			{
				LovenseDeviceType.AirPump => "Vibrate", 
				LovenseDeviceType.Rotate => "Vibrate", 
				LovenseDeviceType.DoubleVibrate => "Vibrate1", 
				_ => "Vibrate", 
			};
			ConsoleUtils.Info("Lovense", "First endpoint: " + text, ConsoleColor.Gray, "VibrateLovenseInternal", 264);
			string text2 = $"https://{protocolIp}:30010/{text}?v={vibrateChangeIntensity}";
			ConsoleUtils.Info("Lovense", "Sending first stage to: " + text2, ConsoleColor.Gray, "VibrateLovenseInternal", 268);
			HttpClientWrapper.SendGetRequest(text2, decryptOnReceive: false, 0.1f, VibrateLovenseInternalSecond);
		}

		private static void VibrateLovenseInternalSecond(bool error, string response)
		{
			ConsoleUtils.FlushToConsole("Lovense", $"Stage 1 vibrate: {error}", ConsoleColor.Gray, "VibrateLovenseInternalSecond", 275);
			if (error)
			{
				OnVibrateDone(error, response);
				return;
			}
			string text = lovenseDevice.type switch
			{
				LovenseDeviceType.AirPump => "AirAuto", 
				LovenseDeviceType.Rotate => "Rotate", 
				LovenseDeviceType.DoubleVibrate => "Vibrate2", 
				_ => string.Empty, 
			};
			ConsoleUtils.Info("Lovense", "Second endpoint: " + text, ConsoleColor.Gray, "VibrateLovenseInternalSecond", 316);
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = $"https://{protocolIp}:30010/{text}?v={vibrateChangeIntensity}";
				ConsoleUtils.Info("Lovense", "Sending second stage to: " + text2, ConsoleColor.Gray, "VibrateLovenseInternalSecond", 322);
				HttpClientWrapper.SendGetRequest(text2, decryptOnReceive: false, 0.1f, OnVibrateDone);
			}
			else
			{
				OnVibrateDone(error, response);
			}
		}

		private static void OnVibrateDone(bool error, string response)
		{
			ConsoleUtils.FlushToConsole("Lovense", $"Stage 2 vibrate: {error}", ConsoleColor.Gray, "OnVibrateDone", 334);
			if (error)
			{
				vibrateChanging = false;
				onVibrateChangeDone?.Invoke(vibrateChangeIntensity, arg2: false, response);
				return;
			}
			lovenseDevice.intensity = vibrateChangeIntensity;
			vibrateChanging = false;
			if (!IsLovenseConnected())
			{
				VibrateLovenseInternal(0);
			}
			else
			{
				onVibrateChangeDone?.Invoke(vibrateChangeIntensity, arg2: true, response);
			}
		}
	}
}
