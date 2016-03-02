using ImageEffects.Interfaces;
using System;

namespace ImageEffects.Effects
{
	public class NegationEffect : IPixelImageEffect
	{
		public string Name { get; set; }
		public string Param1Label { get; set; }
		public double Param1 { get; set; }

		public byte Lerp(int start, int end)
		{
			return (byte)(start + Param1 * (end - start));
		}

		public void ApplyPixelEffect(byte ri, byte gi, byte bi, out byte ro, out byte go, out byte bo)
		{
			ro = Lerp(ri, 255 - ri);
			go = Lerp(gi, 255 - gi);
			bo = Lerp(bi, 255 - bi);
		}
	}
}
