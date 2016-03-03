using Lumia.Imaging.Artistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ImageLab.Converters
{
	public class ObjectToWarpingConverter : IValueConverter
	{
		public bool Reverse { get; set; }
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return Enum.Parse(value.GetType(), value.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return Enum.Parse(value.GetType(), value.ToString());
		}
	}
}
