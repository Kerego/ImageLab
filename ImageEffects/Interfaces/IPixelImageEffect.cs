using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEffects.Interfaces
{
	public interface IPixelImageEffect : IImageEffect
	{
		void ApplyPixelEffect(byte ri, byte gi, byte bi, out byte ro, out byte go, out byte bo);
	}
}
