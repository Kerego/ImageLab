using ImageLab.Containers;
using ImageLab.Interfaces;
using Lumia.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ImageLab.Helpers;

namespace ImageLab.ViewModels
{
	public class MainPageViewModel : ViewModel, IMainPageViewModel, IDisposable
	{
		private string _title = "";
		private INavigationService _navigationService;
		private StorageFile _currentFile;
		private IImageProvider _source;
		private SoftwareBitmap _modified;

		private SoftwareBitmapRenderer _renderer;
		private SoftwareBitmapImageSource _imageSource;
		
		public string Title { get { return _title; } set { SetProperty(ref _title, value); } }
		public SoftwareBitmapSource CurrentImageSource { get; set; }
		public SoftwareBitmap CurrentImage { get; set; }

		public Dictionary<string, Type> PossibleEffects = new Dictionary<string,Type>
		{
			["Saturation Modifier"] = typeof(SaturationEffectContainer),
			["Contrast Modifier"] = typeof(ContrastEffectContainer),
			["Negation Modifier"] = typeof(NegationEffectContainer),
			["Warping Modifier"] = typeof(WarpingEffectContainer),
			["Brightness Modifier"] = typeof(BrightnessEffectContainer),
			["Flip Modifier"] = typeof(FlipEffectContainer),
			["Rotation Modifier"] = typeof(RotationEffectContainer),
			["Mirror Modifier"] = typeof(MirrorEffectContainer),
			["Grayscale Modifier"] = typeof(GrayscaleEffectContainer),
			["Colorization Modifier"] = typeof(ColorAdjustEffectContainer)
		};

		public ObservableCollection<EffectContainer> ContainerList { get; set; } = new ObservableCollection<EffectContainer>();

		public DelegateCommand<Type> AddCommand { get; set; }
		public DelegateCommand<EffectContainer> RemoveCommand { get; set; }


		public MainPageViewModel(INavigationService navigationService, ApplicationState appState)
		{
			_navigationService = navigationService;
			RemoveCommand = new DelegateCommand<EffectContainer>(async x =>
			{
				ContainerList.Remove(x);
				await EnqueueEffect();
			});

			AddCommand = new DelegateCommand<Type>(async x =>
			{
				var effect = Activator.CreateInstance(x) as EffectContainer;
				ContainerList.Insert(0, effect);
				effect.PropertyChanged += EffectPropertyChanged;
				await EnqueueEffect();
			});

			this._appState = appState;
		}

		public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

			var containerIndex = (int)navigationParameter;
			var container = _appState.Containers[containerIndex];
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
			await EnqueueEffect();


			SystemNavigationManager.GetForCurrentView().BackRequested += NavigateBackRequested;
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
				_navigationService.CanGoBack() ?
				AppViewBackButtonVisibility.Visible :
				AppViewBackButtonVisibility.Collapsed;
		}

		private async void EffectPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			await EnqueueEffect();
		}

		public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
		{
			base.OnNavigatedFrom(viewModelState, suspending);
			foreach (var effect in ContainerList)
				effect.PropertyChanged -= EffectPropertyChanged;

			SystemNavigationManager.GetForCurrentView().BackRequested -= NavigateBackRequested;
		}


		private void NavigateBackRequested(object sender, BackRequestedEventArgs e)
		{
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
		private ApplicationState _appState;

		public async Task EnqueueEffect()
		{
			lastChange++;
			if(lastChange == 0)
			{
				await Task.Delay(100);
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
			
			foreach (var t in ContainerList)
				t.ApplyEffect(ref _source);

			_renderer.Source = _source;
			_modified = await _renderer.RenderAsync();
			await CurrentImageSource.SetBitmapAsync(_modified);
		}




		public void Dispose()
		{
			_modified.Dispose();
			_renderer.Dispose();
			_imageSource.Dispose();
			CurrentImage.Dispose();
			CurrentImageSource.Dispose();
		}
	}
}
