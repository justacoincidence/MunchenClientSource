using System;
using MunchenClient.ModuleSystem.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MunchenClient.Misc
{
	public class MunchenCustomRankFixer : MonoBehaviour
	{
		private Text textComponent;

		private bool initialized = false;

		public MunchenCustomRankFixer(IntPtr ptr)
			: base(ptr)
		{
		}

		public void OnEnable()
		{
			if (!initialized)
			{
				GameObject gameObject = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo/User Panel/TrustLevel/TrustText");
				textComponent = gameObject.GetComponent<Text>();
				initialized = true;
			}
			GeneralHandler.FixRank(textComponent);
		}
	}
}
