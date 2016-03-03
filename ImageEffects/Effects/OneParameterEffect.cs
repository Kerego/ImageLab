using ImageEffects.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEffects.Effects
{
	public class OneParameterEffect : BindableBase, IImageEffect
	{
		private string _name;
		public string Name { get { return _name; } set { SetProperty(ref _name, value); } }

		private bool _enabled;
		public bool Enabled { get { return _enabled; } set { SetProperty(ref _enabled, value); } }

		public virtual string Param1Label { get; }
		private double _param1;
		public double Param1 { get { return _param1; } set { SetProperty(ref _param1, value); } }

		public double Param1LimitL { get; }
		public double Param1LimitH { get; }
		public double Param1Step { get; }
		public OneParameterEffect(string name, string param1Label, double lowLimit, double highLimit, double step)
		{
			_name = name;
			Param1LimitL = lowLimit;
			Param1LimitH = highLimit;
			Param1Step = step;
			Param1Label = param1Label;
		}
	}
}
