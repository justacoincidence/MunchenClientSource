using System.Collections;
using System.Linq;
using MelonLoader;
using MunchenClient.Config;
using MunchenClient.Utils;
using UnityEngine;
using VRC.Core;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class InstanceHistoryHandler : ModuleComponent
	{
		protected override string moduleName => "Instance History";

		internal override void OnLevelWasInitialized(int level)
		{
			if (level == -1)
			{
				MelonCoroutines.Start(OnEnteredWorldEnumerator());
			}
		}

		private static IEnumerator OnEnteredWorldEnumerator()
		{
			ApiWorld currentWorld = WorldUtils.GetCurrentWorld();
			ApiWorldInstance currentInstance = WorldUtils.GetCurrentInstance();
			while (currentWorld == null || currentInstance == null)
			{
				currentWorld = WorldUtils.GetCurrentWorld();
				currentInstance = WorldUtils.GetCurrentInstance();
				yield return new WaitForEndOfFrame();
			}
			SavedInstance serializedWorld = new SavedInstance
			{
				ID = currentWorld.id,
				Name = currentWorld.name,
				Tags = currentInstance.instanceId,
				Owner = currentInstance.ownerId,
				AccessType = currentInstance.type,
				Region = currentInstance.region,
				Nonce = currentInstance.nonce,
				ClientVersion = currentInstance.clientVersion,
				Capacity = currentWorld.capacity,
				ThumbnailURL = currentWorld.thumbnailImageUrl
			};
			string worldNameSanitized = currentWorld.name.ToLower();
			WorldUtils.isAmongUsGame = worldNameSanitized.Contains("among us");
			WorldUtils.isJustBClub = worldNameSanitized.Contains("just b club");
			WorldUtils.isFBTHeaven = worldNameSanitized.Contains("fbt heaven");
			Configuration.GetInstanceHistoryConfig().InstanceHistory.Add(serializedWorld);
			while (Configuration.GetInstanceHistoryConfig().InstanceHistory.Count > 33)
			{
				Configuration.GetInstanceHistoryConfig().InstanceHistory.RemoveAt(0);
			}
			Configuration.SaveInstanceHistoryConfig();
		}

		internal static string GetInstanceID(string baseID)
		{
			if (baseID.Contains('~'))
			{
				return baseID.Substring(0, baseID.IndexOf('~'));
			}
			return baseID;
		}
	}
}
