using ImageLab.Containers;
using ImageLab.Interfaces;
using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using Lumia.Imaging.Transforms;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ImageLab.ViewModels
{
	public class MainPageViewModel : ViewModel, IMainPageViewModel, IDisposable
	{
		private string _title = "test";
		private INavigationService _navigationService;
		private StorageFile _currentFile;
		private IImageProvider _source;
		private SoftwareBitmap _modified;

		private SoftwareBitmapRenderer _renderer;
		private SoftwareBitmapImageSource _imageSource;

		#region effects

		//brightness
		private double _brightnessLevel;
		private bool _brightnessEnabled = false;
		private BrightnessEffect _brightnessEffect = new BrightnessEffect();
		public double BrightnessLevel { get { return _brightnessLevel; } set { SetProperty(ref _brightnessLevel, value); } }
		public bool BrightnessEnabled { get { return _brightnessEnabled; } set { SetProperty(ref _brightnessEnabled, value); } }

		//contrast
		private double _contrastLevel;
		private bool _contrastEnabled = false;
		private ContrastEffect _contrastEffect = new ContrastEffect();
		public double ContrastLevel { get { return _contrastLevel; } set { SetProperty(ref _contrastLevel, value); } }
		public bool ContrastEnabled { get { return _contrastEnabled; } set { SetProperty(ref _contrastEnabled, value); } }

		//Negative
		private bool _negationEnabled = false;
		private NegativeEffect _negationEffect = new NegativeEffect();
		public bool NegationEnabled { get { return _negationEnabled; } set { SetProperty(ref _negationEnabled, value); } }

		//saturation
		private double _saturationLevel;
		private double _saturationHueLevel;
		private bool _saturationEnabled = false;
		private HueSaturationEffect _saturationEffect = new HueSaturationEffect();
		public double SaturationLevel { get { return _saturationLevel; } set { SetProperty(ref _saturationLevel, value); } }
		public double SaturationHueLevel { get { return _saturationHueLevel; } set { SetProperty(ref _saturationHueLevel, value); } }
		public bool SaturationEnabled { get { return _saturationEnabled; } set { SetProperty(ref _saturationEnabled, value); } }
		

		//warping
		private double _warpingLevel;
		private WarpMode _warpingMode;
		private bool _warpingEnabled = false;
		private WarpingEffect _warpingEffect = new WarpingEffect();
		public double WarpingLevel { get { return _warpingLevel; } set { SetProperty(ref _warpingLevel, value); } }
		public bool WarpingEnabled { get { return _warpingEnabled; } set { SetProperty(ref _warpingEnabled, value); } }
		public WarpMode WarpingMode { get { return _warpingMode; } set { SetProperty(ref _warpingMode, value); } }
		public List<WarpMode> WarpModes = Enum.GetValues(typeof(WarpMode)).Cast<WarpMode>().ToList();

		//rotation
		private double _rotationAngle;
		private bool _rotationEnabled = false;
		private RotationEffect _rotationEffect = new RotationEffect();
		public double RotationAngle { get { return _rotationAngle; } set { SetProperty(ref _rotationAngle, value); } }
		public bool RotationEnabled { get { return _rotationEnabled; } set { SetProperty(ref _rotationEnabled, value); } }

		//flip
		private FlipMode _flipMode;
		private bool _flipEnabled = false;
		private FlipEffect _flipEffect = new FlipEffect();
		public bool FlipEnabled { get { return _flipEnabled; } set { SetProperty(ref _flipEnabled, value); } }
		public FlipMode FlipMode { get { return _flipMode; } set { SetProperty(ref _flipMode, value); } }
		public List<FlipMode> FlipModes = Enum.GetValues(typeof(FlipMode)).Cast<FlipMode>().ToList();

		#endregion

		public string Title { get { return _title; } set { SetProperty(ref _title, value); } }
		public SoftwareBitmapSource CurrentImageSource { get; set; }
		public SoftwareBitmap CurrentImage { get; set; }
		


		public MainPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

			var container = navigationParameter as ImageContainer;
			CurrentImage = container.Image;
			_currentFile = container.File;

			_modified = new SoftwareBitmap(CurrentImage.BitmapPixelFormat, CurrentImage.PixelWidth, CurrentImage.PixelHeight, CurrentImage.BitmapAlphaMode);
			CurrentImage.CopyTo(_modified);

			_imageSource = new SoftwareBitmapImageSource(CurrentImage);

			_source = _imageSource;
			_renderer = new SoftwareBitmapRenderer();
			_renderer.SoftwareBitmap = _modified;
			
			CurrentImageSource = new SoftwareBitmapSource();

			Title = _currentFile.Name;

			SystemNavigationManager.GetForCurrentView().BackRequested += NavigateBackRequested;
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
				_navigationService.CanGoBack() ?
				AppViewBackButtonVisibility.Visible :
				AppViewBackButtonVisibility.Collapsed;
		}

		public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
		{
			base.OnNavigatedFrom(viewModelState, suspending);

			SystemNavigationManager.GetForCurrentView().BackRequested -= NavigateBackRequested;
		}


		private async void NavigateBackRequested(object sender, BackRequestedEventArgs e)
		{
			//MessageDialog dialog = new MessageDialog("test");
			//var t = await dialog.ShowAsync();
			if(_navigationService.CanGoBack())
				_navigationService.GoBack();
		}
		

		public async Task Save()
		{
			FileSavePicker picker = new FileSavePicker();
			picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			picker.SuggestedFileName = _currentFile.Name.Remove(_currentFile.Name.IndexOf("."));
			picker.FileTypeChoices.Add("Portable Network Graphics ", new string[] { ".png" });
			try
			{
				var file = await picker.PickSaveFileAsync();
				using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
				{

					BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
					var toSave = new SoftwareBitmap(CurrentImage.BitmapPixelFormat, CurrentImage.PixelWidth, CurrentImage.PixelHeight, CurrentImage.BitmapAlphaMode);
					_modified.CopyTo(toSave);
					encoder.SetSoftwareBitmap(toSave);
					await encoder.FlushAsync();
				}
			}
			catch
			{
			}

		}

		int lastChange = -1;
		public async Task EnqueueEffect()
		{
			lastChange++;
			if(lastChange == 0)
			{
				await Task.Delay(15);
			}
			else
			{
				return;
			}
			await ApplyEffect();
			lastChange = -1;
		}

		public async Task ApplyEffect()
		{
			_source = _imageSource;

			if (_warpingEnabled)
			{
				_warpingEffect.Level = _warpingLevel;
				_warpingEffect.Source = _source;
				_warpingEffect.WarpMode = WarpingMode;
				_source = _warpingEffect;
			}

			if (_brightnessEnabled)
			{
				_brightnessEffect.Level = _brightnessLevel;
				_brightnessEffect.Source = _source;
				_source = _brightnessEffect;
			}

			if (_saturationEnabled)
			{
				_saturationEffect.Saturation = _saturationLevel;
				_saturationEffect.Hue = _saturationHueLevel;
				_saturationEffect.Source = _source;
				_source = _saturationEffect;
			}

			if (_contrastEnabled)
			{
				_contrastEffect.Level = _contrastLevel;
				_contrastEffect.Source = _source;
				_source = _contrastEffect;
			}

			if (_negationEnabled)
			{
				_negationEffect.Source = _source;
				_source = _negationEffect;
			}

			if (_rotationEnabled)
			{
				_rotationEffect.RotationAngle = _rotationAngle;
				_rotationEffect.Source = _source;
				_source = _rotationEffect;
			}
			if (_flipEnabled)
			{
				_flipEffect.Source = _source;
				_flipEffect.FlipMode = _flipMode;
				_source = _flipEffect;
			}


			_renderer.Source = _source;
			_modified = await _renderer.RenderAsync();
			await CurrentImageSource.SetBitmapAsync(_modified);
		}

		public void Dispose()
		{
			_modified.Dispose();
			_renderer.Dispose();
			_imageSource.Dispose();
			_brightnessEffect.Dispose();
			_contrastEffect.Dispose();
			_negationEffect.Dispose();
			_saturationEffect.Dispose();
			_warpingEffect.Dispose();
			_rotationEffect.Dispose();
			_flipEffect.Dispose();
			CurrentImage.Dispose();
			CurrentImageSource.Dispose();
		}
	}
}
