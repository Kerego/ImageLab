using ImageLab.Containers;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ImageLab.ViewModels
{
	public class SlideShowViewModel : ViewModel
	{
		private INavigationService _navigationService;
		public SlideShowViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		List<ImageContainer> _images;
		private bool _flag;

		public SoftwareBitmapSource CurrentImage { get; set; } = new SoftwareBitmapSource();

		public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

			_images = navigationParameter as List<ImageContainer>;

			SystemNavigationManager.GetForCurrentView().BackRequested += NavigateBackRequested;
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
				_navigationService.CanGoBack() ?
				AppViewBackButtonVisibility.Visible :
				AppViewBackButtonVisibility.Collapsed;
			ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
			await SlideShow();


		}

		private async Task SlideShow()
		{
			foreach (var image in _images)
			{
				CurrentImage.SetBitmapAsync(image.Image);
				await Task.Delay(1500);
			}
			CurrentImage.SetBitmapAsync(null);
		}

		public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
		{
			base.OnNavigatedFrom(viewModelState, suspending);

			ApplicationView.GetForCurrentView().ExitFullScreenMode();
			SystemNavigationManager.GetForCurrentView().BackRequested -= NavigateBackRequested;
		}


		private void NavigateBackRequested(object sender, BackRequestedEventArgs e)
		{
			_flag = false;
			if (_navigationService.CanGoBack())
				_navigationService.GoBack();
		}
	}
}
