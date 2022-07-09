using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MunchenClient.Utils
{
	internal sealed class Crc32 : HashAlgorithm
	{
		internal const uint DefaultPolynomial = 3988292384u;

		internal const uint DefaultSeed = uint.MaxValue;

		private static uint[] defaultTable;

		private readonly uint seed;

		private readonly uint[] table;

		private uint hash;

		public override int HashSize => 32;

		internal Crc32()
			: this(3988292384u, uint.MaxValue)
		{
		}

		internal Crc32(uint polynomial, uint seed)
		{
			if (!BitConverter.IsLittleEndian)
			{
				throw new PlatformNotSupportedException("Not supported on Big Endian processors");
			}
			table = InitializeTable(polynomial);
			this.seed = (hash = seed);
		}

		public override void Initialize()
		{
			hash = seed;
		}

		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			hash = CalculateHash(table, hash, array, ibStart, cbSize);
		}

		protected override byte[] HashFinal()
		{
			return HashValue = UInt32ToBigEndianBytes(~hash);
		}

		internal static string CalculateCRC(byte[] bytes)
		{
			try
			{
				Crc32 crc = new Crc32();
				string text = string.Empty;
				byte[] array = crc.ComputeHash(bytes);
				for (int i = 0; i < array.Length; i++)
				{
					text += array[i].ToString("x2").ToLower();
				}
				return text;
			}
			catch (Exception e)
			{
				ConsoleUtils.Exception("CRCHasher", "CalculateCRC", e, "CalculateCRC", 78);
				return string.Empty;
			}
		}

		internal static uint Compute(byte[] buffer)
		{
			return Compute(uint.MaxValue, buffer);
		}

		internal static uint Compute(uint seed, byte[] buffer)
		{
			return Compute(3988292384u, seed, buffer);
		}

		internal static uint Compute(uint polynomial, uint seed, byte[] buffer)
		{
			return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
		}

		internal static uint[] InitializeTable(uint polynomial)
		{
			if (polynomial == 3988292384u && defaultTable != null)
			{
				return defaultTable;
			}
			uint[] array = new uint[256];
			for (int i = 0; i < 256; i++)
			{
				uint num = (uint)i;
				for (int j = 0; j < 8; j++)
				{
					num = (((num & 1) != 1) ? (num >> 1) : ((num >> 1) ^ polynomial));
				}
				array[i] = num;
			}
			if (polynomial == 3988292384u)
			{
				defaultTable = array;
			}
			return array;
		}

		internal static uint CalculateHash(uint[] table, uint seed, IList<byte> buffer, int start, int size)
		{
			uint num = seed;
			for (int i = start; i < start + size; i++)
			{
				num = (num >> 8) ^ table[buffer[i] ^ (num & 0xFF)];
			}
			return num;
		}

		internal static byte[] UInt32ToBigEndianBytes(uint uint32)
		{
			byte[] bytes = BitConverter.GetBytes(uint32);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}
	}
}
