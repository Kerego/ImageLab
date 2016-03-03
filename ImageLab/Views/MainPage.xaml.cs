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
		const string Brightness = "Brightness Modifier";
		const string Saturation = "Saturation Modifier";
		const string Contrast = "Contrast Modifier";
		const string Rotation = "Rotation Modifier";
		const string Negation = "Negation Modifier";
		const string Warping = "Warping Modifier";
		const string Flip = "Flip Modifier";

		Dictionary<string, ICommand> _effects = new Dictionary<string, ICommand>();
		ObservableCollection<string> _activeEffects = new ObservableCollection<string>();

		public MainPageViewModel ViewModel { get; set; }
		public DelegateCommand<string> RemoveCommand;
		public DelegateCommand<object> AddCommand;

		public MainPage()
		{
			InitializeComponent();
		}
		

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			ViewModel = DataContext as MainPageViewModel;
			_effects.Add(Brightness,
					new DelegateCommand(() =>
					{
						if (_activeEffects.Contains(Brightness))
						{
							ViewModel.BrightnessEnabled = false;
							_activeEffects.Remove(Brightness);
							return;
						}
						ViewModel.BrightnessEnabled = true;
						_activeEffects.Add(Brightness);
					}));

			_effects.Add(Saturation,
					new DelegateCommand(() =>
					{
						if (_activeEffects.Contains(Saturation))
						{
							ViewModel.SaturationEnabled = false;
							_activeEffects.Remove(Saturation);
							return;
						}
						ViewModel.SaturationEnabled = true;
						_activeEffects.Add(Saturation);
					}));

			_effects.Add(Warping,
					new DelegateCommand(() =>
					{
						if (_activeEffects.Contains(Warping))
						{
							ViewModel.WarpingEnabled = false;
							_activeEffects.Remove(Warping);
							return;
						}
						ViewModel.WarpingEnabled = true;
						_activeEffects.Add(Warping);
					}));

			_effects.Add(Contrast,
					new DelegateCommand(() =>
					{
						if (_activeEffects.Contains(Contrast))
						{
							ViewModel.ContrastEnabled = false;
							_activeEffects.Remove(Contrast);
							return;
						}
						ViewModel.ContrastEnabled = true;
						_activeEffects.Add(Contrast);
					}));

			_effects.Add(Rotation,
					new DelegateCommand(() =>
					{
						if (_activeEffects.Contains(Rotation))
						{
							ViewModel.RotationEnabled = false;
							_activeEffects.Remove(Rotation);
							return;
						}
						ViewModel.RotationEnabled = true;
						_activeEffects.Add(Rotation);
					}));

			_effects.Add(Negation,
					new DelegateCommand(() =>
					{
						if (_activeEffects.Contains(Negation))
						{
							ViewModel.NegationEnabled = false;
							_activeEffects.Remove(Negation);
							return;
						}
						ViewModel.NegationEnabled = true;
						_activeEffects.Add(Negation);
					}));
			_effects.Add(Flip,
					new DelegateCommand(() =>
					{
						if (_activeEffects.Contains(Flip))
						{
							ViewModel.FlipEnabled = false;
							_activeEffects.Remove(Flip);
							return;
						}
						ViewModel.FlipEnabled = true;
						_activeEffects.Add(Flip);
					}));


			RemoveCommand = new DelegateCommand<string>(eff => _effects[eff].Execute(null));
			AddCommand = new DelegateCommand<object>(o => AddEffect(o), o =>_effects.Count != _activeEffects.Count);
			_activeEffects.CollectionChanged += async (s, args) => { await ViewModel.ApplyEffect(); AddCommand.RaiseCanExecuteChanged(); };
			base.OnNavigatedTo(e);
		}

		private void AddEffect(object sender)
		{
			FrameworkElement senderElement = sender as FrameworkElement;
			MenuFlyout flyout = new MenuFlyout();
			foreach(var effect in _effects.Where(x=>!_activeEffects.Contains(x.Key)))
			{
				flyout.Items.Add(new MenuFlyoutItem() { Text = effect.Key, Command = effect.Value});
			}
			flyout.ShowAt(senderElement);
		}

		private void StackPanel_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
		{
			FrameworkElement senderElement = sender as FrameworkElement;
			MenuFlyout menuFlyout = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;
			menuFlyout.ShowAt(senderElement, e.GetPosition(senderElement));
		}

	}
	
}
