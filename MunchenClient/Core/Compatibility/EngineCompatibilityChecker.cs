using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using MunchenClient.Utils;
using UnityEngine;

namespace MunchenClient.Core.Compatibility
{
	internal class EngineCompatibilityChecker
	{
		internal static ProcessModule unityPlayerModule = null;

		internal const string unityPlayerModuleName = "UnityPlayer.dll";

		internal static string unityPlayerHash = string.Empty;

		internal static readonly Dictionary<string, UnityEngineOffsets> engineOffsets = new Dictionary<string, UnityEngineOffsets>
		{
			{
				"Unknown",
				new UnityEngineOffsets
				{
					assertLoadAssetBundle = 0,
					loadAudioMixer = 0,
					readFloatValue = 0,
					loadPlugin = 0,
					countNodes = 0,
					debugAssert = 0,
					outOfBoundsCheck = 0,
					reallocateString = 0,
					calculateBoneBindPoseBounds = 0,
					readObject = 0,
					assetBundleDownloadHandlerVTable = 0,
					assetBundleDownloadHandlerCreateCached = 0
				}
			},
			{
				"aCEmIwSIcjYriBQDFjQlpTNNW1/kA8Wlbkqelmt1USOMB09cnKwK7QWyOulz9d7DEYJh4+vO0Ldv8gdH+dZCrg==",
				new UnityEngineOffsets
				{
					assertLoadAssetBundle = 6087252,
					loadAudioMixer = 4822976,
					readFloatValue = 881440,
					loadPlugin = 8491312,
					countNodes = 0,
					debugAssert = 0,
					outOfBoundsCheck = 0,
					reallocateString = 0,
					calculateBoneBindPoseBounds = 0,
					readObject = 0,
					assetBundleDownloadHandlerVTable = 0,
					assetBundleDownloadHandlerCreateCached = 0
				}
			},
			{
				"5dkhl/dWeTREXhHCIkZK17mzZkbjhTKlxb+IUSk+YaWzZrrV+G+M0ekTOEGjZ4dJuB4O3nU/oE3dycXWeJq9uA==",
				new UnityEngineOffsets
				{
					assertLoadAssetBundle = 0,
					loadAudioMixer = 11011680,
					readFloatValue = 817984,
					loadPlugin = 7975920,
					countNodes = 0,
					debugAssert = 0,
					outOfBoundsCheck = 0,
					reallocateString = 0,
					calculateBoneBindPoseBounds = 0,
					readObject = 0,
					assetBundleDownloadHandlerVTable = 0,
					assetBundleDownloadHandlerCreateCached = 0
				}
			},
			{
				"MV6xP7theydao4ENbGi6BbiBxdZsgGOBo/WrPSeIqh6A/E00NImjUNZn+gL+ZxzpVbJms7nUb6zluLL3+aIcfg==",
				new UnityEngineOffsets
				{
					assertLoadAssetBundle = 14513915,
					loadAudioMixer = 11019088,
					readFloatValue = 818768,
					loadPlugin = 7979104,
					countNodes = 0,
					debugAssert = 0,
					outOfBoundsCheck = 0,
					reallocateString = 0,
					calculateBoneBindPoseBounds = 0,
					readObject = 0,
					assetBundleDownloadHandlerVTable = 0,
					assetBundleDownloadHandlerCreateCached = 0
				}
			},
			{
				"ccZ4F7iE7a78kWdXdMekJzP7/ktzS5jOOS8IOITxa1C5Jg2TKxC0/ywY8F0o9I1vZHsxAO4eh7G2sOGzsR/+uQ==",
				new UnityEngineOffsets
				{
					assertLoadAssetBundle = 14521675,
					loadAudioMixer = 11023376,
					readFloatValue = 818816,
					loadPlugin = 7982816,
					countNodes = 0,
					debugAssert = 0,
					outOfBoundsCheck = 0,
					reallocateString = 0,
					calculateBoneBindPoseBounds = 0,
					readObject = 0,
					assetBundleDownloadHandlerVTable = 0,
					assetBundleDownloadHandlerCreateCached = 0
				}
			},
			{
				"sgZUlX3+LSHKnTiTC+nXNcdtLOTrAB1fNjBLOwDdKzCyndlFLAdL0udR4S1szTC/q5pnFhG3Kdspsj5jvwLY1A==",
				new UnityEngineOffsets
				{
					assertLoadAssetBundle = 14533227,
					loadAudioMixer = 11035248,
					readFloatValue = 819760,
					loadPlugin = 7991408,
					countNodes = 14625264,
					debugAssert = 14532032,
					outOfBoundsCheck = 8101552,
					reallocateString = 813552,
					calculateBoneBindPoseBounds = 7390240,
					readObject = 9486016,
					assetBundleDownloadHandlerVTable = 21492056,
					assetBundleDownloadHandlerCreateCached = 3403056
				}
			}
		};

		internal static void PrepareUnityModules()
		{
			bool flag = false;
			for (int i = 0; i < GeneralUtils.GetMainProcess().Modules.Count; i++)
			{
				string text = GeneralUtils.GetMainProcess().Modules[i].FileName.Substring(GeneralUtils.GetMainProcess().Modules[i].FileName.LastIndexOf('\\') + 1);
				if (text == "UnityPlayer.dll")
				{
					unityPlayerModule = GeneralUtils.GetMainProcess().Modules[i];
					flag = true;
					break;
				}
			}
			if (flag)
			{
				using (SHA512 sHA = SHA512.Create())
				{
					using FileStream inputStream = File.OpenRead("UnityPlayer.dll");
					unityPlayerHash = Convert.ToBase64String(sHA.ComputeHash(inputStream));
					ConsoleUtils.Info("CompatibilityLayer", "Engine Hash: " + unityPlayerHash, ConsoleColor.Gray, "PrepareUnityModules", 170);
					if (engineOffsets.ContainsKey(unityPlayerHash))
					{
						ConsoleUtils.Info("CompatibilityLayer", "Found offsets for " + Application.unityVersion, ConsoleColor.Gray, "PrepareUnityModules", 174);
					}
					else
					{
						ConsoleUtils.Info("CompatibilityLayer", "No offsets found for " + Application.unityVersion, ConsoleColor.Gray, "PrepareUnityModules", 178);
					}
					return;
				}
			}
			ConsoleUtils.Info("CompatibilityLayer", "Failed to find UnityPlayer module", ConsoleColor.Gray, "PrepareUnityModules", 183);
		}
	}
}
