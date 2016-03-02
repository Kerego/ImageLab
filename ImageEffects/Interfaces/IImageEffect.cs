using System.Collections.Generic;

namespace ImageEffects.Interfaces
{
	public interface IImageEffect
	{
		string Name { get; set; }
		Dictionary<string, double> Parameters { get; set; }
	}
}
