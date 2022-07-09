using System;
using MunchenClient.Core;
using MunchenClient.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class StressRelieverHandler : ModuleComponent
	{
		private static GameObject rainAudioObject;

		private static AudioSource rainAudioSource;

		protected override string moduleName => "Stress Reliever Handler";

		internal override void OnUIManagerLoaded()
		{
			rainAudioObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			rainAudioObject.name = "Munchen Rain";
			rainAudioObject.layer = 19;
			rainAudioObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			rainAudioObject.transform.parent = GameObject.Find("UserInterface").transform;
			rainAudioSource = rainAudioObject.AddComponent<AudioSource>();
			rainAudioSource.loop = true;
			rainAudioSource.volume = 0.3f;
			rainAudioSource.clip = AssetLoader.LoadAudio("PerfectRainLoop");
			rainAudioSource.outputAudioMixerGroup = new AudioMixerGroup();
			rainAudioSource.Play();
			ConsoleUtils.Info("ASMR", "Rain ASMR started", ConsoleColor.Gray, "OnUIManagerLoaded", 30);
		}
	}
}
