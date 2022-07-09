using System;
using System.Collections;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace MunchenClient.Utils
{
	internal class HttpClientWrapper
	{
		internal static void SendPostRequest(string url, Dictionary<string, string> parameters, bool encryptOnSend, bool decryptOnReceive, float sendDelay, Action<bool, string> onFinished)
		{
			MelonCoroutines.Start(SendPostRequestInternal(url, parameters, encryptOnSend, decryptOnReceive, sendDelay, onFinished));
		}

		private static IEnumerator SendPostRequestInternal(string url, Dictionary<string, string> parameters, bool encryptOnSend, bool decryptOnReceive, float sendDelay, Action<bool, string> onFinished)
		{
			if (sendDelay != 0f)
			{
				yield return new WaitForSeconds(sendDelay);
			}
			Dictionary<string, string> postValues = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				if (encryptOnSend)
				{
					postValues.Add(DataProtector.EncryptData(parameter.Key), DataProtector.EncryptData(parameter.Value));
				}
				else
				{
					postValues.Add(parameter.Key, parameter.Value);
				}
			}
			HttpClientCustom webRequest = HttpClientCustom.Post(url, postValues);
			webRequest.SendWebRequest();
			while (!webRequest.isDone)
			{
				yield return new WaitForEndOfFrame();
			}
			onFinished(arg2: (!decryptOnReceive) ? webRequest.responseText : DataProtector.DecryptData(webRequest.responseText), arg1: webRequest.isError);
		}

		internal static void SendGetRequest(string url, bool decryptOnReceive, float sendDelay, Action<bool, string> onFinished)
		{
			MelonCoroutines.Start(SendGetRequestInternal(url, decryptOnReceive, sendDelay, onFinished));
		}

		private static IEnumerator SendGetRequestInternal(string url, bool decryptOnReceive, float sendDelay, Action<bool, string> onFinished)
		{
			if (sendDelay != 0f)
			{
				yield return new WaitForSeconds(sendDelay);
			}
			HttpClientCustom webRequest = HttpClientCustom.Get(url);
			webRequest.SendWebRequest();
			while (!webRequest.isDone)
			{
				yield return new WaitForEndOfFrame();
			}
			onFinished(arg2: (!decryptOnReceive) ? webRequest.responseText : DataProtector.DecryptData(webRequest.responseText), arg1: webRequest.isError);
		}

		internal static string SendDownloadRequest(string url, float sendDelay, Action<string, bool, byte[]> onFinished)
		{
			string text = GeneralUtils.RandomString(GeneralUtils.fastRandom, 10);
			MelonCoroutines.Start(SendDownloadRequestInternal(text, url, sendDelay, onFinished));
			return text;
		}

		private static IEnumerator SendDownloadRequestInternal(string uniqueToken, string url, float sendDelay, Action<string, bool, byte[]> onFinished)
		{
			if (sendDelay != 0f)
			{
				yield return new WaitForSeconds(sendDelay);
			}
			HttpClientCustom webRequest = HttpClientCustom.Download(url);
			webRequest.SendWebRequest();
			while (!webRequest.isDone)
			{
				yield return new WaitForEndOfFrame();
			}
			onFinished(uniqueToken, webRequest.isError, webRequest.responseDownload);
		}
	}
}
