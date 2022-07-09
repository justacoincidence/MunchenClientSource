using System.Windows.Forms;
using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.ModuleSystem.Modules;
using MunchenClient.Utils;
using UnchainedButtonAPI;

namespace MunchenClient.Menu.World
{
	internal class InstanceHistoryWorldOptions : QuickMenuNestedMenu
	{
		internal int instanceIndex = 0;

		internal void JoinWorld()
		{
			SavedInstance savedInstance = Configuration.GetInstanceHistoryConfig().InstanceHistory[instanceIndex];
			string worldId = savedInstance.ID + ":" + savedInstance.Tags;
			WorldUtils.GoToWorld(worldId);
		}

		internal void DropPortal()
		{
			SavedInstance savedInstance = Configuration.GetInstanceHistoryConfig().InstanceHistory[instanceIndex];
			WorldUtils.DropPortal(savedInstance.ID, savedInstance.Tags);
		}

		internal void CopyInstanceID()
		{
			SavedInstance savedInstance = Configuration.GetInstanceHistoryConfig().InstanceHistory[instanceIndex];
			string text = savedInstance.ID + ":" + savedInstance.Tags;
			Clipboard.SetText(text);
		}

		internal void RemoveInstanceFromList()
		{
			Configuration.GetInstanceHistoryConfig().InstanceHistory.RemoveAt(instanceIndex);
			Configuration.SaveInstanceHistoryConfig();
			buttonHandler.Press();
		}

		internal InstanceHistoryWorldOptions(QuickMenuButtonRow parent)
			: base(parent, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsMenuName, string.Empty, createButtonOnParent: false)
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsJoin, delegate
			{
				JoinWorld();
			}, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsJoinDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsDropPortal, delegate
			{
				DropPortal();
			}, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsDropPortalDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsCopyID, delegate
			{
				CopyInstanceID();
			}, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsCopyIDDescription);
			new QuickMenuSingleButton(parentRow, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsRemove, delegate
			{
				RemoveInstanceFromList();
			}, LanguageManager.GetUsedLanguage().InstanceHistoryOptionsRemoveDescription);
		}
	}
}
