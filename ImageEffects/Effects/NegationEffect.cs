using ImageEffects.Interfaces;
using System;

namespace ImageEffects.Effects
{
	public class NegationEffect : OneParameterEffect, IPixelImageEffect
	{
		public NegationEffect() : base("Negation", "Level Of Negation", 0, 1, 0.01) { }

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
