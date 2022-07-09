using UnityEngine;

namespace MunchenClient.Config
{
	public class ColorConfig
	{
		public float ColorRed = 1f;

		public float ColorGreen = 1f;

		public float ColorBlue = 1f;

		public float ColorAlpha = 1f;

		public ColorConfig()
		{
			ColorRed = 1f;
			ColorGreen = 1f;
			ColorBlue = 1f;
			ColorAlpha = 1f;
		}

		public ColorConfig(float red, float green, float blue)
		{
			ColorRed = red;
			ColorGreen = green;
			ColorBlue = blue;
			ColorAlpha = 1f;
		}

		public ColorConfig(float red, float green, float blue, float alpha)
		{
			ColorRed = red;
			ColorGreen = green;
			ColorBlue = blue;
			ColorAlpha = alpha;
		}

		public ColorConfig(Color color)
		{
			ColorRed = color.r;
			ColorGreen = color.g;
			ColorBlue = color.b;
			ColorAlpha = color.a;
		}

		internal Color GetColor()
		{
			return new Color(ColorRed, ColorGreen, ColorBlue, ColorAlpha);
		}

		internal void SetColor(float red, float green, float blue)
		{
			ColorRed = red;
			ColorGreen = green;
			ColorBlue = blue;
		}

		internal void SetColor(float red, float green, float blue, float alpha)
		{
			ColorRed = red;
			ColorGreen = green;
			ColorBlue = blue;
			ColorAlpha = alpha;
		}

		internal void SetColor(Color color)
		{
			ColorRed = color.r;
			ColorGreen = color.g;
			ColorBlue = color.b;
			ColorAlpha = color.a;
		}
	}
}
