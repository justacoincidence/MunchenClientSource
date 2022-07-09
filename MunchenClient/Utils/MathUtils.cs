using UnityEngine;

namespace MunchenClient.Utils
{
	internal class MathUtils
	{
		internal static bool IsInvalid(Vector2 vector)
		{
			return float.IsNaN(vector.x) || float.IsInfinity(vector.x) || float.IsNaN(vector.y) || float.IsInfinity(vector.y);
		}

		internal static bool IsInvalid(Vector3 vector)
		{
			return float.IsNaN(vector.x) || float.IsInfinity(vector.x) || float.IsNaN(vector.y) || float.IsInfinity(vector.y) || float.IsNaN(vector.z) || float.IsInfinity(vector.z);
		}

		internal static bool IsInvalid(Quaternion quaternion)
		{
			return float.IsNaN(quaternion.x) || float.IsInfinity(quaternion.x) || float.IsNaN(quaternion.y) || float.IsInfinity(quaternion.y) || float.IsNaN(quaternion.z) || float.IsInfinity(quaternion.z) || float.IsNaN(quaternion.w) || float.IsInfinity(quaternion.w);
		}

		internal static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		internal static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		internal static short Clamp(short value, short min, short max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		internal static byte Clamp(byte value, byte min, byte max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		internal static sbyte Clamp(sbyte value, sbyte min, sbyte max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		internal static bool IsBeyondLimit(Vector3 vector, float lowerLimit, float higherLimit)
		{
			if (vector.x < lowerLimit || vector.x > higherLimit || vector.y < lowerLimit || vector.y > higherLimit || vector.z < lowerLimit || vector.z > higherLimit)
			{
				return true;
			}
			return false;
		}

		internal static byte NormalizeValue(byte value)
		{
			if (value < 0)
			{
				return (byte)(-value);
			}
			return value;
		}
	}
}
