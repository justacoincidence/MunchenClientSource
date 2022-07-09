using System;

namespace MunchenClient.Core
{
	internal struct DependencyDownload
	{
		public string name;

		public Action onFinished;
	}
}
