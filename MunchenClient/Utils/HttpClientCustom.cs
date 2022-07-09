using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MunchenClient.Utils
{
	internal class HttpClientCustom
	{
		private static HttpClient httpClient;

		internal bool isDone = false;

		internal bool isError = false;

		internal string responseText = string.Empty;

		internal byte[] responseDownload = null;

		internal string url = string.Empty;

		internal Dictionary<string, string> parameters;

		internal bool isPost = false;

		internal bool isDownload = false;

		internal static void SetHttpClient(HttpClient client)
		{
			httpClient = client;
			httpClient.Timeout = TimeSpan.FromMinutes(3.0);
		}

		internal static HttpClient GetHttpClient()
		{
			return httpClient;
		}

		internal HttpClientCustom(HttpClient client)
		{
			httpClient = client;
			httpClient.Timeout = TimeSpan.FromMinutes(3.0);
		}

		internal HttpClientCustom()
		{
			if (httpClient == null)
			{
				httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
				httpClient.DefaultRequestHeaders.Add("Client-Agent", "MunchenClient/1.0 (VRChatMansionGang)");
				httpClient.Timeout = TimeSpan.FromMinutes(3.0);
			}
		}

		internal bool OnServerCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		internal static HttpClientCustom Post(string url, Dictionary<string, string> parameters)
		{
			return new HttpClientCustom
			{
				url = url,
				parameters = parameters,
				isPost = true,
				isDownload = false
			};
		}

		internal static HttpClientCustom Get(string url)
		{
			return new HttpClientCustom
			{
				url = url,
				isPost = false,
				isDownload = false
			};
		}

		internal static HttpClientCustom Download(string url)
		{
			return new HttpClientCustom
			{
				url = url,
				isPost = false,
				isDownload = true
			};
		}

		internal async void SendWebRequest()
		{
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(OnServerCertificateValidate));
			HttpResponseMessage response = null;
			try
			{
				if (isPost)
				{
					response = await httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));
					responseText = await response.Content.ReadAsStringAsync();
				}
				else if (isDownload)
				{
					response = await httpClient.GetAsync(url);
					using Stream stream = await response.Content.ReadAsStreamAsync();
					responseDownload = ReadToEnd(stream);
				}
				else
				{
					response = await httpClient.GetAsync(url);
					responseText = await response.Content.ReadAsStringAsync();
				}
				isError = !response.IsSuccessStatusCode;
			}
			catch (HttpRequestException ex)
			{
				HttpRequestException e2 = ex;
				Exception innerException = e2.InnerException;
				if (innerException is WebException webException)
				{
					if (webException.Status == WebExceptionStatus.ConnectFailure)
					{
						responseText = "No internet or Server is down";
						isError = true;
					}
					else if (webException.Status == WebExceptionStatus.ReceiveFailure)
					{
						responseText = "Server sent no response";
						isError = true;
					}
					else if (webException.Status == WebExceptionStatus.SecureChannelFailure)
					{
						responseText = "Secure connection couldn't be established";
						isError = true;
					}
					else if (webException.Status == WebExceptionStatus.NameResolutionFailure)
					{
						responseText = "DNS server issues";
						isError = true;
					}
					else
					{
						ConsoleUtils.Info("HttpClientCustom", $"ExceptionCode: {webException.Status}", ConsoleColor.Gray, "SendWebRequest", 147);
						ConsoleUtils.Exception("HttpClientCustom", "SendWebRequest - HttpRequestException", e2, "SendWebRequest", 148);
						responseText = e2.InnerException.Message;
						isError = true;
					}
				}
				else
				{
					ConsoleUtils.Exception("HttpClientCustom", "SendWebRequest - HttpRequestException", e2, "SendWebRequest", 156);
					responseText = e2.InnerException.Message;
					isError = true;
				}
			}
			catch (Exception ex2)
			{
				Exception e = ex2;
				ConsoleUtils.Exception("HttpClientCustom", "SendWebRequest - Exception", e, "SendWebRequest", 164);
				responseText = e.Message;
				isError = true;
			}
			response?.Dispose();
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Remove(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(OnServerCertificateValidate));
			isDone = true;
		}

		private byte[] ReadToEnd(Stream stream)
		{
			long position = 0L;
			if (stream.CanSeek)
			{
				position = stream.Position;
				stream.Position = 0L;
			}
			try
			{
				byte[] array = new byte[4096];
				int num = 0;
				int num2;
				while ((num2 = stream.Read(array, num, array.Length - num)) > 0)
				{
					num += num2;
					if (num == array.Length)
					{
						int num3 = stream.ReadByte();
						if (num3 != -1)
						{
							byte[] array2 = new byte[array.Length * 2];
							Buffer.BlockCopy(array, 0, array2, 0, array.Length);
							Buffer.SetByte(array2, num, (byte)num3);
							array = array2;
							num++;
						}
					}
				}
				byte[] array3 = array;
				if (array.Length != num)
				{
					array3 = new byte[num];
					Buffer.BlockCopy(array, 0, array3, 0, num);
				}
				return array3;
			}
			finally
			{
				if (stream.CanSeek)
				{
					stream.Position = position;
				}
			}
		}
	}
}
