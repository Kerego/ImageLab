using ImageLab.Containers;
using ImageLab.ViewModels;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageLab.Interfaces
{
	public interface IImageGridPageViewModel
	{
		ObservableCollection<ImageContainer> ContainerList { get; set; }
	}
}
