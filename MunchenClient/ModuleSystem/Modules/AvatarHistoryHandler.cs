using MunchenClient.Config;
using MunchenClient.Wrappers;
using UnityEngine;
using VRC;
using VRC.Core;

namespace MunchenClient.ModuleSystem.Modules
{
	internal class AvatarHistoryHandler : ModuleComponent
	{
		protected override string moduleName => "Avatar History";

		internal override void OnAvatarLoaded(string playerId, string playerName, ref GameObject avatar)
		{
			if (playerId != APIUser.CurrentUser?.id)
			{
				return;
			}
			Player playerByName = PlayerWrappers.GetPlayerByName(playerName);
			if (playerByName == null)
			{
				return;
			}
			ApiAvatar field_Private_ApiAvatar_ = playerByName.prop_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0;
			if (Configuration.GetAvatarHistoryConfig().AvatarHistory.Count <= 0 || !(field_Private_ApiAvatar_.id == Configuration.GetAvatarHistoryConfig().AvatarHistory[Configuration.GetAvatarHistoryConfig().AvatarHistory.Count - 1].ID))
			{
				Configuration.GetAvatarHistoryConfig().AvatarHistory.Add(new SavedAvatar
				{
					ID = field_Private_ApiAvatar_.id,
					Name = field_Private_ApiAvatar_.name
				});
				while (Configuration.GetAvatarHistoryConfig().AvatarHistory.Count > 33)
				{
					Configuration.GetAvatarHistoryConfig().AvatarHistory.RemoveAt(0);
				}
				Configuration.SaveAvatarHistoryConfig();
			}
		}
	}
}
