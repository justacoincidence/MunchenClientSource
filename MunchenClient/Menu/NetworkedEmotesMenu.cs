using MunchenClient.Config;
using MunchenClient.Core;
using MunchenClient.Utils;
using MunchenClient.Wrappers;
using UnchainedButtonAPI;
using VRC.SDKBase;

namespace MunchenClient.Menu
{
	internal class NetworkedEmotesMenu : QuickMenuNestedMenu
	{
		private ulong totalQuacks = 0uL;

		private ulong totalSizzs = 0uL;

		private ulong totalQgs = 0uL;

		private ulong totalCowboys = 0uL;

		private ulong totalAllahs = 0uL;

		private ulong totalGays = 0uL;

		internal NetworkedEmotesMenu(QuickMenuButtonRow parent)
			: base(parent, "NASA Technology", "TOP SECRET SHIT IN HERE (GONE WRONG) (GONE WILD) (IS HE GONNA SURVIVE)")
		{
			QuickMenuButtonRow parentRow = new QuickMenuButtonRow(this);
			new QuickMenuSpacers(this);
			QuickMenuButtonRow parentRow2 = new QuickMenuButtonRow(this);
			new QuickMenuToggleButton(parentRow, LanguageManager.GetUsedLanguage().NetworkedEmotes, Configuration.GetGeneralConfig().NetworkedEmotes, delegate
			{
				Configuration.GetGeneralConfig().NetworkedEmotes = true;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().NetworkedEmotesDescription, delegate
			{
				Configuration.GetGeneralConfig().NetworkedEmotes = false;
				Configuration.SaveGeneralConfig();
			}, LanguageManager.GetUsedLanguage().NetworkedEmotesDescription);
			new QuickMenuSingleButton(parentRow, "Quack!", delegate
			{
				totalQuacks++;
				QuickMenuUtils.ShowAlert($"Quack! ({totalQuacks})");
				GeneralUtils.InformHudText("Lmao", $"Quack! ({totalQuacks})");
				AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Quack"));
				if (Configuration.GetGeneralConfig().NetworkedEmotes)
				{
					PlayerInformation localPlayerInformation6 = PlayerWrappers.GetLocalPlayerInformation();
					Networking.RPC(RPC.Destination.Others, localPlayerInformation6.vrcPlayer.gameObject, "NetworkedQuack", GeneralUtils.rpcParameters);
				}
			}, "I wonder what this does?", MainUtils.CreateSprite(AssetLoader.LoadTexture("Quack")));
			new QuickMenuSingleButton(parentRow, "Exception", delegate
			{
				totalSizzs++;
				QuickMenuUtils.ShowAlert($"Exception ({totalSizzs})");
				GeneralUtils.InformHudText("Lmao", $"Exception ({totalSizzs})");
				AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Sizzukie"));
				if (Configuration.GetGeneralConfig().NetworkedEmotes)
				{
					PlayerInformation localPlayerInformation5 = PlayerWrappers.GetLocalPlayerInformation();
					Networking.RPC(RPC.Destination.Others, localPlayerInformation5.vrcPlayer.gameObject, "NetworkedException", GeneralUtils.rpcParameters);
				}
			}, "When you interpolate 600 divided by 600, so your game lags because you raise an exception", MainUtils.CreateSprite(AssetLoader.LoadTexture("Sizzukie")));
			new QuickMenuSingleButton(parentRow, "Qg Is Gay", delegate
			{
				totalQgs++;
				QuickMenuUtils.ShowAlert($"Qg Is Gay ({totalQgs})");
				GeneralUtils.InformHudText("Lmao", $"Qg Is Gay ({totalQgs})");
				AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("QgIsGay"));
				if (Configuration.GetGeneralConfig().NetworkedEmotes)
				{
					PlayerInformation localPlayerInformation4 = PlayerWrappers.GetLocalPlayerInformation();
					Networking.RPC(RPC.Destination.Others, localPlayerInformation4.vrcPlayer.gameObject, "NetworkedQg", GeneralUtils.rpcParameters);
				}
			}, "Is he really? Let's find out!", MainUtils.CreateSprite(AssetLoader.LoadTexture("QgIsGay")));
			new QuickMenuSingleButton(parentRow2, "Cowboy KenKen", delegate
			{
				totalCowboys++;
				QuickMenuUtils.ShowAlert($"Cowboy KenKen ({totalCowboys})");
				GeneralUtils.InformHudText("Lmao", $"Cowboy KenKen ({totalCowboys})");
				AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Niggers"));
				if (Configuration.GetGeneralConfig().NetworkedEmotes)
				{
					PlayerInformation localPlayerInformation3 = PlayerWrappers.GetLocalPlayerInformation();
					Networking.RPC(RPC.Destination.Others, localPlayerInformation3.vrcPlayer.gameObject, "NetworkedCowboy", GeneralUtils.rpcParameters);
				}
			}, "Shiver me niggers!", MainUtils.CreateSprite(AssetLoader.LoadTexture("Niggers")));
			new QuickMenuSingleButton(parentRow2, "Allat Cat", delegate
			{
				totalAllahs++;
				QuickMenuUtils.ShowAlert($"Allah Cat ({totalAllahs})");
				GeneralUtils.InformHudText("Lmao", $"Allah Cat ({totalAllahs})");
				AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("Allah"));
				if (Configuration.GetGeneralConfig().NetworkedEmotes)
				{
					PlayerInformation localPlayerInformation2 = PlayerWrappers.GetLocalPlayerInformation();
					Networking.RPC(RPC.Destination.Others, localPlayerInformation2.vrcPlayer.gameObject, "NetworkedAllah", GeneralUtils.rpcParameters);
				}
			}, "ALLAAAAAAAAAAAAAAAH!", MainUtils.CreateSprite(AssetLoader.LoadTexture("Allah")));
			new QuickMenuSingleButton(parentRow2, "Gay", delegate
			{
				totalGays++;
				QuickMenuUtils.ShowAlert($"Gay ({totalGays})");
				GeneralUtils.InformHudText("Lmao", $"Gay ({totalGays})");
				AudioUtils.PlayAudioClip(AssetLoader.LoadAudio("GayEcho"));
				if (Configuration.GetGeneralConfig().NetworkedEmotes)
				{
					PlayerInformation localPlayerInformation = PlayerWrappers.GetLocalPlayerInformation();
					Networking.RPC(RPC.Destination.Others, localPlayerInformation.vrcPlayer.gameObject, "NetworkedGay", GeneralUtils.rpcParameters);
				}
			}, "Only use if someone is extremely gay", MainUtils.CreateSprite(AssetLoader.LoadTexture("OkayChamp")));
		}
	}
}
