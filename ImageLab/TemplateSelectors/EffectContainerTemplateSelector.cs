using ImageLab.Containers;
using Lumia.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ImageLab.TemplateSelectors
{
	public class EffectContainerTemplateSelector : DataTemplateSelector
	{
		public DataTemplate OneParameterTemplate { get; set; }
		public DataTemplate NoParameterTemplate { get; set; }
		public DataTemplate EnumParameterTemplate { get; set; }
		public DataTemplate TwoParameterTemplate { get; set; }
		public DataTemplate ThreeParameterTemplate { get; set; }
		public DataTemplate EnumOneParameterTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			if (item is IThreeParam)
				return ThreeParameterTemplate;

			if (item is ITwoParam)
				return TwoParameterTemplate;

			if (item is IEnumOneParam)
				return EnumOneParameterTemplate;

			if (item is IEnumParam)
				return EnumParameterTemplate;

			if (item is IOneParam)
				return OneParameterTemplate;

			if (item is INoParam)
				return NoParameterTemplate;

			return base.SelectTemplateCore(item, container);
		}

	}
}
