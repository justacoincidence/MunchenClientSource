using System;
using UnityEngine;
using VRC.UI.Elements;

namespace UnchainedButtonAPI
{
	public class QuickMenuMunchenPage : MonoBehaviour
	{
		public UIPage uiPage;

		public QuickMenuMunchenPage(IntPtr ptr)
			: base(ptr)
		{
		}

		public void OnDisable()
		{
			QuickMenuUtils.OnUIPageDisabled(uiPage);
		}
	}
}
