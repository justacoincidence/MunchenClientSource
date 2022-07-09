using System;
using System.Collections.Generic;
using UnityEngine;

namespace MunchenClient.Utils
{
	internal class AudioUtils
	{
		private static readonly Dictionary<int, List<LoggedRelativeSum>> potentialMaliciousVoicePackets = new Dictionary<int, List<LoggedRelativeSum>>();

		internal static void ClearLoggedVoicePackets()
		{
			potentialMaliciousVoicePackets.Clear();
		}

		internal static bool IsVoiceDataBad(int actorId, byte[] voiceData)
		{
			if (voiceData.Length <= 8)
			{
				return true;
			}
			int num = BitConverter.ToInt32(voiceData, 0);
			if (num != actorId)
			{
				return true;
			}
			int num2 = 0;
			for (int i = 8; i < voiceData.Length; i++)
			{
				num2 += voiceData[i];
			}
			if (potentialMaliciousVoicePackets.ContainsKey(actorId))
			{
				bool flag = false;
				bool flag2 = false;
				for (int j = 0; j < potentialMaliciousVoicePackets[actorId].Count; j++)
				{
					int num3 = potentialMaliciousVoicePackets[actorId][j].sum - 64;
					int num4 = potentialMaliciousVoicePackets[actorId][j].sum + 64;
					if (num2 < num3 || num2 > num4)
					{
						continue;
					}
					flag = true;
					if (Time.realtimeSinceStartup - potentialMaliciousVoicePackets[actorId][j].lastSeen < 1f)
					{
						if (potentialMaliciousVoicePackets[actorId][j].totalDetections >= 3)
						{
							flag2 = true;
							if (j == potentialMaliciousVoicePackets[actorId].Count - 1)
							{
								potentialMaliciousVoicePackets[actorId].Add(new LoggedRelativeSum
								{
									sum = potentialMaliciousVoicePackets[actorId][j].sum + 64,
									lastSeen = Time.realtimeSinceStartup,
									totalDetections = potentialMaliciousVoicePackets[actorId][j].totalDetections
								});
							}
							int totalDetections = potentialMaliciousVoicePackets[actorId][j].totalDetections;
							for (int k = 0; k < potentialMaliciousVoicePackets[actorId].Count; k++)
							{
								potentialMaliciousVoicePackets[actorId][k].lastSeen = Time.realtimeSinceStartup;
								potentialMaliciousVoicePackets[actorId][k].totalDetections = totalDetections;
							}
						}
						else
						{
							potentialMaliciousVoicePackets[actorId][j].lastSeen = Time.realtimeSinceStartup;
							potentialMaliciousVoicePackets[actorId][j].totalDetections++;
						}
					}
					else
					{
						potentialMaliciousVoicePackets[actorId][j].lastSeen = Time.realtimeSinceStartup;
						potentialMaliciousVoicePackets[actorId][j].totalDetections--;
					}
					break;
				}
				if (!flag)
				{
					potentialMaliciousVoicePackets[actorId].Add(new LoggedRelativeSum
					{
						sum = num2,
						lastSeen = Time.realtimeSinceStartup,
						totalDetections = 0
					});
				}
				if (flag2)
				{
					return true;
				}
			}
			else
			{
				potentialMaliciousVoicePackets.Add(actorId, new List<LoggedRelativeSum>
				{
					new LoggedRelativeSum
					{
						sum = num2,
						lastSeen = Time.realtimeSinceStartup,
						totalDetections = 0
					}
				});
			}
			return false;
		}

		internal static void PlayAudioClip(AudioClip audioClip)
		{
			GameObject gameObject = new GameObject(audioClip.name);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = audioClip;
			audioSource.volume = 0.2f;
			audioSource.dopplerLevel = 0f;
			audioSource.spatialBlend = 0f;
			audioSource.Play();
			UnityEngine.Object.Destroy(gameObject, audioSource.clip.length);
		}

		internal static void PlayAudioClip(AudioClip audioClip, Vector3 position)
		{
			GameObject gameObject = new GameObject(audioClip.name);
			gameObject.transform.position = position;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = audioClip;
			audioSource.volume = 0.2f;
			audioSource.dopplerLevel = 0f;
			audioSource.spatialBlend = 1f;
			audioSource.Play();
			UnityEngine.Object.Destroy(gameObject, audioSource.clip.length);
		}
	}
}
