
using Windows.Graphics.Imaging;

namespace ImageLab.Interfaces
{
	public interface IMainPageViewModel
	{
		string Title { get; set; }
		SoftwareBitmap CurrentImage { get; set; }
	}
}
