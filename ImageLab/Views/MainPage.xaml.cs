using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Lumia.Imaging.Artistic;
using Lumia.Imaging.Compositing;
using Lumia.Imaging.Transforms;
using ImageLab.Controls;
using ImageLab.ViewModels;
using ImageLab.Interfaces;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageLab.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : PageBase
	{
		public MainPageViewModel ViewModel { get; set; }
		
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			ViewModel = DataContext as MainPageViewModel;
			base.OnNavigatedTo(e);
		}
		
	}
}
