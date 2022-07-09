using System.Collections.Generic;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnchainedButtonAPI;
using VRC.SDKBase;

namespace MunchenClient.Menu.Fun
{
	internal class UdonManipulatorOptionsMenu : QuickMenuNestedMenu
	{
		private readonly System.Collections.Generic.List<QuickMenuButtonRow> buttonRows = new System.Collections.Generic.List<QuickMenuButtonRow>();

		internal int udonBehaviourIndex = 0;

		internal UdonManipulatorOptionsMenu(QuickMenuButtonRow parent)
			: base(parent, "Udon Options", string.Empty, createButtonOnParent: false)
		{
			QuickMenuUtils.ChangeScrollState(this, state: true);
		}

		private void UdonEventClicked(QuickMenuSingleButton button)
		{
			Object[] array = new Object[1] { (String)(string)button.customData };
			Networking.RPC(RPC.Destination.All, UdonManipulatorMenu.udonBehaviours[udonBehaviourIndex].gameObject, "UdonSyncRunProgramAsRPC", array);
		}

		internal override void OnMenuShownCallback()
		{
			for (int i = 0; i < buttonRows.Count; i++)
			{
				buttonRows[i].Dispose();
			}
			buttonRows.Clear();
			int num = 0;
			QuickMenuButtonRow quickMenuButtonRow = new QuickMenuButtonRow(this);
			buttonRows.Add(quickMenuButtonRow);
			Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Collections.Generic.List<uint>>.Enumerator enumerator = UdonManipulatorMenu.udonBehaviours[udonBehaviourIndex]._eventTable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Il2CppSystem.Collections.Generic.KeyValuePair<string, Il2CppSystem.Collections.Generic.List<uint>> current = enumerator.Current;
				QuickMenuSingleButton button = new QuickMenuSingleButton(quickMenuButtonRow, current.Key, null, string.Empty)
				{
					customData = current.Key
				};
				button.SetAction(delegate
				{
					UdonEventClicked(button);
				});
				num++;
				if (num >= 4)
				{
					num = 0;
					quickMenuButtonRow = new QuickMenuButtonRow(this);
					buttonRows.Add(quickMenuButtonRow);
				}
			}
		}
	}
}
