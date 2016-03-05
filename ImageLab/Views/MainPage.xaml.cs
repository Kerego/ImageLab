using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using ImageLab.Controls;
using ImageLab.ViewModels;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using System.Collections.ObjectModel;

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

		private void AddEffect(object sender, RoutedEventArgs args)
		{
			FrameworkElement senderElement = sender as FrameworkElement;
			MenuFlyout flyout = new MenuFlyout();
			foreach(var effect in ViewModel.PossibleEffects)
			{
				flyout.Items.Add(new MenuFlyoutItem() { Text = effect.Key, Command = ViewModel.AddCommand, CommandParameter = effect.Value});
			}
			flyout.ShowAt(senderElement);
		}

		private void StackPanel_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
		{
			StackPanel senderElement = sender as StackPanel;
			MenuFlyout menuFlyout = new MenuFlyout();
			menuFlyout.Items.Add(new MenuFlyoutItem() { Text = "Remove", Command = ViewModel.RemoveCommand, CommandParameter = senderElement.DataContext });
			menuFlyout.ShowAt(senderElement, e.GetPosition(senderElement));
		}

	}
	
}
