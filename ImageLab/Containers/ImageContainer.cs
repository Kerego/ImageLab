using Microsoft.Practices.Prism.Mvvm;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageLab.Containers
{
	public class ImageContainer : BindableBase
	{
		private double _width;
		private double _height;
		public double Width { get { return _width; } set { SetProperty(ref _width, value); } }
		public double Height { get { return _height; } set { SetProperty(ref _height, value); } }

		public SoftwareBitmapSource ImageSource { get; set; }
		public SoftwareBitmap Image { get; set; }
		public StorageFile File { get; set; }
	}
}
