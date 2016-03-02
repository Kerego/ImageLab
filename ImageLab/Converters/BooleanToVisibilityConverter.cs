using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageLab.Converters
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public bool Reverse { get; set; }
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var val = System.Convert.ToBoolean(value);
			return val ^ Reverse ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
