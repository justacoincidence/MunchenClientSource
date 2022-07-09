using System;
using UnityEngine;

namespace MunchenClient.Utils
{
	internal class Event7Wrapper
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

		internal static void SetPosition(ref byte[] unpackedData, Vector3 position)
		{
			Buffer.BlockCopy(SerializationUtils.Vector3ToBytes(position), 0, unpackedData, 48, 12);
		}

		internal static Vector3 GetPosition(ref byte[] unpackedData)
		{
			return SerializationUtils.ReadVector3(ref unpackedData, 48);
		}

		internal static void SetRotation(ref byte[] unpackedData, Quaternion rotation)
		{
			byte[] src = ObjectPublicAbstractSealedSiBySiDiBy2SiObByObUnique.Method_Public_Static_ArrayOf_Byte_Quaternion_EnumNPublicSealedvaNoHaZe6vZeZeUnique_0(rotation, ObjectPublicAbstractSealedSiBySiDiBy2SiObByObUnique.EnumNPublicSealedvaNoHaZe6vZeZeUnique.ZeroToOne10Bit);
			Buffer.BlockCopy(src, 0, unpackedData, 60, 5);
		}

		internal static Quaternion GetRotation(ref byte[] unpackedData)
		{
			byte[] array = new byte[5];
			Buffer.BlockCopy(unpackedData, 60, array, 0, 5);
			Quaternion param_ = Quaternion.identity;
			ObjectPublicAbstractSealedSiBySiDiBy2SiObByObUnique.Method_Private_Static_Void_ArrayOf_Byte_byref_Quaternion_PDM_2(array, ref param_);
			return param_;
		}

		internal static void SetPing(ref byte[] unpackedData, short ping)
		{
			Buffer.BlockCopy(SerializationUtils.ShortToBytes(ping), 0, unpackedData, 68, 2);
		}

		internal static short GetPing(ref byte[] unpackedData)
		{
			return SerializationUtils.ReadShort(ref unpackedData, 68);
		}

		internal static void SetFPS(ref byte[] unpackedData, byte fps)
		{
			unpackedData[72] = fps;
		}

		internal static byte GetFPS(ref byte[] unpackedData)
		{
			return SerializationUtils.ReadByte(ref unpackedData, 72);
		}
	}
}
