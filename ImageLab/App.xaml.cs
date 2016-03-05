using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using ImageLab.Views;
using ImageLab.ViewModels;
using Windows.UI.Core;
using static ImageLab.Constants.Constants;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace ImageLab
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
			Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
				Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
				Microsoft.ApplicationInsights.WindowsCollectors.Session);
			this.InitializeComponent();
        }

		protected override Task OnInitializeAsync(IActivatedEventArgs args)
		{
			ViewModelLocationProvider.Register(typeof(MainPage).ToString(), () => new MainPageViewModel(NavigationService));
			ViewModelLocationProvider.Register(typeof(ImageGridPage).ToString(), () => new ImageGridPageViewModel(NavigationService));
			ViewModelLocationProvider.Register(typeof(SlideShowPage).ToString(), () => new SlideShowViewModel(NavigationService));
			var title = ApplicationView.GetForCurrentView().TitleBar;
			title.BackgroundColor = Colors.Black;
			title.ButtonBackgroundColor = Colors.Black;
			return base.OnInitializeAsync(args);
		}

		private void AppBackRequested(object sender, BackRequestedEventArgs e)
		{
			NavigationService.GoBack();
		}

		protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
		{
			NavigationService.Navigate(Experiences.ImageGrid.ToString(), null);
			return Task.FromResult<object>(null);
		}
    }
}
