using Il2CppSystem;
using UnityEngine;

namespace MunchenClient.Patching.Patches
{
	internal struct ImageDownloadContainer
	{
		internal string imageUrl;

		internal int imageSize;

		internal Action<Texture2D> onImageDownload;

		internal Action onImageDownloadFailed;

		internal string fallbackImageUrl;

		internal bool isRetry;
	}
}
