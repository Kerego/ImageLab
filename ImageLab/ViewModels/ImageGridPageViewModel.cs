using ImageLab.Interfaces;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Practices.Prism.Mvvm;
using Windows.Storage.Pickers;
using System;
using Windows.Graphics.Imaging;
using Windows.Storage;
using System.Linq;
using System.Threading.Tasks;
using ImageLab.Containers;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using Windows.UI.Core;
using static ImageLab.Constants.Constants;

namespace ImageLab.ViewModels
{
	public class ImageGridPageViewModel : ViewModel, IImageGridPageViewModel
	{
		private INavigationService _navigationService;

		public ObservableCollection<ImageContainer> ContainerList { get; set; } = new ObservableCollection<ImageContainer>();
		public DelegateCommand<ImageContainer> EditCommand { get; set; }
		public DelegateCommand<ImageContainer> RemoveCommand { get; set; }

		public bool IsEmpty { get { return !ContainerList.Any(); } }

		private FileOpenPicker _picker = new FileOpenPicker();
	
		public ImageGridPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			RemoveCommand = new DelegateCommand<ImageContainer>(x =>
			{
				ContainerList.Remove(x);
				OnPropertyChanged(() => IsEmpty);
			});
			EditCommand = new DelegateCommand<ImageContainer>(x =>
			{
				_navigationService.Navigate(Experiences.Main.ToString(), x);
			});

			_picker.FileTypeFilter.Add(".png");
			_picker.FileTypeFilter.Add(".bmp");
			_picker.FileTypeFilter.Add(".jpg");
			_picker.FileTypeFilter.Add(".jpeg");
		}

		public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

			if (viewModelState.ContainsKey("containers"))
			{
				var items = viewModelState["containers"] as IEnumerable<ImageContainer>;
				//if(items.Any())
				//	await ProcessFiles(items.Select(x => x.File));
				foreach(var item in items)
				{
					ContainerList.Add(item);
				}
			}

			SystemNavigationManager.GetForCurrentView().BackRequested += NavigateBackRequested; ;
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
				_navigationService.CanGoBack() ?
				AppViewBackButtonVisibility.Visible :
				AppViewBackButtonVisibility.Collapsed;
		}

		public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
		{
			base.OnNavigatedFrom(viewModelState, suspending);
			viewModelState["containers"] = ContainerList;
			SystemNavigationManager.GetForCurrentView().BackRequested -= NavigateBackRequested;
		}

		private void NavigateBackRequested(object sender, BackRequestedEventArgs e)
		{
			if(_navigationService.CanGoBack())
				_navigationService.GoBack();
		}

		public void SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var min = 200;
			var offset = 5;
			var sizeX = (int)e.NewSize.Width / min;
			var sizeY = (int)e.NewSize.Height / min;
			sizeY += (sizeY == 0) ? 1 : 0;

			foreach (var container in ContainerList)
			{
				container.Height = (int)(e.NewSize.Height / sizeY) - offset;
				container.Width = (int)(e.NewSize.Width / sizeX) - offset;
			}
		} 

		public async Task AddFiles()
		{
			var files = await _picker.PickMultipleFilesAsync();
			await ProcessFiles(files);
		}

		public async Task StartSlideShow()
		{
			_navigationService.Navigate(Experiences.SlideShow.ToString(), ContainerList.ToList());
		}

		public async Task ProcessFiles(IEnumerable<StorageFile> files)
		{
			foreach(var file in files.Where(x=>ContainerList.Count(y=>y.File.Path == x.Path) == 0))
			{
				try
				{
					using (var stream = await file.OpenAsync(FileAccessMode.Read))
					{
						BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
						var sb = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
						var sbs = new SoftwareBitmapSource();
						await sbs.SetBitmapAsync(sb);
						var container = new ImageContainer
						{
							ImageSource = sbs,
							Image = sb,
							File = file,
							Width = ContainerList.FirstOrDefault()?.Width ?? 250,
							Height = ContainerList.FirstOrDefault()?.Height ?? 250
						};
						ContainerList.Add(container);
					}
				}
				catch
				{
				}
			}
			OnPropertyChanged(() => IsEmpty);
		}
	}
}
