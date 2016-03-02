using ImageEffects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEffects.Effects
{
	public class NegationEffect : IPixelImageEffect
	{
		public string Name { get; set; }
		public Dictionary<string, double> Parameters { get; set; }

		public void ApplyPixelEffect(byte ri, byte gi, byte bi, out byte ro, out byte go, out byte bo)
		{
			ro = (byte)(255 - ri);
			go = (byte)(255 - gi);
			bo = (byte)(255 - bi);
		}
	}
}
