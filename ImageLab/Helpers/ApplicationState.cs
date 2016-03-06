using ImageLab.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLab.Helpers
{

	public interface IApplicationState
	{
		List<ImageContainer> Containers { get; set; }
		
	}
	public class ApplicationState : IApplicationState
	{
		public List<ImageContainer> Containers { get; set; }
	}
}
