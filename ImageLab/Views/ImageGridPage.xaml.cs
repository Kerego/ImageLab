using ImageLab.Controls;
using ImageLab.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace ImageLab.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ImageGridPage : PageBase
	{
		public ImageGridPageViewModel ViewModel { get; set; }
		public ImageGridPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			ViewModel = DataContext as ImageGridPageViewModel;
			base.OnNavigatedTo(e);
		}

		private void BorderRightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
		{
			FrameworkElement senderElement = sender as FrameworkElement;
			MenuFlyout menuFlyout = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;
			menuFlyout.ShowAt(senderElement, e.GetPosition(senderElement));
		}
	}
}
