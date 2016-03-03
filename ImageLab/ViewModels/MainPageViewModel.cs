using ImageEffects;
using ImageEffects.Effects;
using ImageEffects.Interfaces;
using ImageLab.Containers;
using ImageLab.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System;

namespace ImageLab.ViewModels
{
	public class MainPageViewModel : ViewModel, IMainPageViewModel
	{
		private string _title = "test";
		private INavigationService _navigationService;
		private StorageFile _currentFile;
		private ImageEditor _effectEditor;

		public string Title { get { return _title; } set { SetProperty(ref _title, value); } }
		public SoftwareBitmapSource CurrentImageSource { get; set; }
		public SoftwareBitmap CurrentImage { get; set; }
		public ObservableCollection<IImageEffect> Effects = new ObservableCollection<IImageEffect>();


		public MainPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

			var container = navigationParameter as ImageContainer;
			CurrentImage = container.Image;
			_currentFile = container.File;

			_effectEditor = new ImageEditor(CurrentImage);

			CurrentImageSource = new SoftwareBitmapSource();

			Title = _currentFile.Name;

			for (int i = 0; i < 3; i++)
			{
				NegationEffect negation = new NegationEffect();
				Effects.Add(negation);
				//negation.PropertyChanged += async (s, e) => await EnqueueEffect();
			}
			NegationEffect negation1 = new NegationEffect();
			Effects.Add(negation1);
			negation1.PropertyChanged += async (s, e) => await EnqueueEffect();
			await ApplyEffect();




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
			MessageDialog dialog = new MessageDialog("test");
			var t = await dialog.ShowAsync();
			if(t != null)
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
					CurrentImage.CopyTo(toSave);
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
			_effectEditor.ApplyPixelEffects(Effects);
			await CurrentImageSource.SetBitmapAsync(CurrentImage);
		}
	}
}
