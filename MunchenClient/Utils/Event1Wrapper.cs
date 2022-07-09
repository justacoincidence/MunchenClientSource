using System;

namespace MunchenClient.Utils
{
	internal class Event1Wrapper
	{
		internal static void SetActorID(ref byte[] unpackedData, int actorId)
		{
			Buffer.BlockCopy(SerializationUtils.IntToBytes(actorId), 0, unpackedData, 0, 4);
		}

		internal static int GetActorID(ref byte[] unpackedData)
		{
			return SerializationUtils.ReadInt(ref unpackedData, 0);
		}

		internal static void SetServerTime(ref byte[] unpackedData, int serverTime)
		{
			Buffer.BlockCopy(SerializationUtils.IntToBytes(serverTime), 0, unpackedData, 4, 4);
		}

		internal static int GetServerTime(ref byte[] unpackedData)
		{
			return SerializationUtils.ReadInt(ref unpackedData, 4);
		}
	}
}
