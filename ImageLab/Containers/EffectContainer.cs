using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using Lumia.Imaging.Transforms;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLab.Containers
{
	//interfaces for runtime detection of effect type
	#region interfaces
	public interface INoParam
	{
		string Name { get; set; }
	}

	public interface IOneParam
	{
		string Name { get; set; }
		double Param1 { get; set; }
		string Param1Label { get; }
		double Param1L { get; }
		double Param1H { get; }
		double Param1Step { get; }
	}
	public interface IEnumParam
	{
		string Name { get; set; }
		Enum Mode { get; set; }
		List<Enum> Modes { get; }
	}
	public interface IEnumOneParam 
	{
		string Name { get; set; }
		double Param1 { get; set; }
		string Param1Label { get; }
		double Param1L { get; }
		double Param1H { get; }
		double Param1Step { get; }
		Enum Mode { get; set; }
		List<Enum> Modes { get; }
		
	}
	public interface ITwoParam
	{
		string Name { get; set; }
		double Param1 { get; set; }
		string Param1Label { get; }
		double Param1L { get; }
		double Param1H { get; }
		double Param1Step { get; }
		double Param2 { get; set; }
		string Param2Label { get; }
		double Param2L { get; }
		double Param2H { get; }
		double Param2Step { get; }
	}

	public interface IThreeParam
	{
		string Name { get; set; }
		double Param1 { get; set; }
		string Param1Label { get; }
		double Param1L { get; }
		double Param1H { get; }
		double Param1Step { get; }
		double Param2 { get; set; }
		string Param2Label { get; }
		double Param2L { get; }
		double Param2H { get; }
		double Param2Step { get; }
		double Param3 { get; set; }
		string Param3Label { get; }
		double Param3L { get; }
		double Param3H { get; }
		double Param3Step { get; }
	}

	#endregion

	#region abstract
	public abstract class EffectContainer : BindableBase
	{
		public string Name { get; set; }
		public bool _isEnabled;
		protected IImageProvider _effect;

		public abstract void ApplyEffect(ref IImageProvider source);
	}

	public abstract class EffectContainer<T> : EffectContainer, INoParam
	{
		public EffectContainer(string name)
		{
			Name = name;
			_effect = Activator.CreateInstance<T>() as IImageProvider;
		}
	}

	public abstract class OneParameterEffectContainer<T> : EffectContainer<T>, IOneParam
	{
		private double _param1;
		public double Param1 { get { return _param1; } set { SetProperty(ref _param1, value); } }
		public double Param1L { get; }
		public double Param1H { get; }
		public double Param1Step { get; }
		public string Param1Label { get; }

		public OneParameterEffectContainer(string name, string param1label, double param1l = -1, double param1h = 1, double param1s = 0.01) : base(name)
		{
			Param1H = param1h;
			Param1L = param1l;
			Param1Step = param1s;
			Param1Label = param1label;
		}
	}

	public abstract class EnumParameterEffectContainer<T,K> : EffectContainer<T>, IEnumParam
	{
		private Enum _mode;
		public Enum Mode { get { return _mode; } set { SetProperty(ref _mode, value); } }
		public List<Enum> Modes { get; }
		public EnumParameterEffectContainer(string name) : base(name)
		{
			Modes = Enum.GetValues(typeof(K)).Cast<Enum>().ToList();
			Mode = Modes[0];
		}
	}

	public abstract class EnumOneParameterEffectContainer<T, K> : OneParameterEffectContainer<T>, IEnumOneParam
	{
		private Enum _mode;
		public Enum Mode { get { return _mode; } set { SetProperty(ref _mode, value); } }
		public List<Enum> Modes { get; }
		public EnumOneParameterEffectContainer(string name, string param1label, double param1l = -1, double param1h = 1, double param1s = 0.01) 
			: base(name, param1label, param1l, param1h, param1s)
		{
			Modes = Enum.GetValues(typeof(K)).Cast<Enum>().ToList();
			Mode = Modes[0];
		}
	}


	public abstract class TwoParameterEffectContainer<T> : OneParameterEffectContainer<T>, ITwoParam
	{
		private double _param2;
		public double Param2 { get { return _param2; } set { SetProperty(ref _param2, value); } }
		public string Param2Label { get; }
		public double Param2L { get; }
		public double Param2H { get; }
		public double Param2Step { get; }

		public TwoParameterEffectContainer(
			string name, 
			string param1label, 
			string param2label, 
			double param1l = -1, 
			double param1h = 1, 
			double param1s = 0.01, 
			double param2l = -1, 
			double param2h = 1, 
			double param2s = 0.01)

			: base(name, 
				  param1label, 
				  param1l,
				  param1h, 
				  param1s) 
		{
			Param2Label = param2label;
			Param2L = param2l;
			Param2H = param2h;
			Param2Step = param2s;
		}
	}

	public abstract class ThreeParameterEffectContainer<T> : TwoParameterEffectContainer<T>, IThreeParam
	{
		private double _param3;
		public double Param3 { get { return _param3; } set { SetProperty(ref _param3, value); } }
		public string Param3Label { get; }
		public double Param3L { get; }
		public double Param3H { get; }
		public double Param3Step { get; }

		public ThreeParameterEffectContainer(
			string name,
			string param1label,
			string param2label,
			string param3label,
			double param1l = -1,
			double param1h = 1,
			double param1s = 0.01,
			double param2l = -1,
			double param2h = 1,
			double param2s = 0.01,
			double param3l = -1,
			double param3h = 1,
			double param3s = 0.01)

			: base(name,
				  param1label,
				  param2label,
				  param1l,
				  param1h,
				  param1s,
				  param2l,
				  param2h,
				  param2s)
		{
			Param3Label = param3label;
			Param3L = param3l;
			Param3H = param3h;
			Param3Step = param3s;
		}
	}

	#endregion

	#region EffectContainers

	public class NegationEffectContainer : EffectContainer<NegativeEffect>
	{
		public NegationEffectContainer() : base("Negation") { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as NegativeEffect;
			eff.Source = source;
			source = eff;
		}
	}

	public class FlipEffectContainer : EnumParameterEffectContainer<FlipEffect, FlipMode>
	{
		public FlipEffectContainer() : base("Flip") { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as FlipEffect;
			eff.Source = source;
			eff.FlipMode = (FlipMode)Mode;
			source = eff;
		}
	}

	public class WarpingEffectContainer : EnumOneParameterEffectContainer<WarpingEffect, WarpMode>
	{
		public WarpingEffectContainer() : base("Warping","Warping level", 0) { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as WarpingEffect;
			eff.Source = source;
			eff.WarpMode = (WarpMode)Mode;
			eff.Level = Param1;
			source = eff;
		}
	}

	public class ContrastEffectContainer : OneParameterEffectContainer<ContrastEffect>
	{
		public ContrastEffectContainer() : base("Contrast", "Contrast") { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as ContrastEffect;
			eff.Source = source;
			eff.Level = Param1;
			source = eff;
		}
	}

	public class RotationEffectContainer : OneParameterEffectContainer<RotationEffect>
	{
		public RotationEffectContainer() : base("Rotation", "Angle", 0, 270, 90) { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as RotationEffect;
			eff.Source = source;
			eff.RotationAngle = Param1;
			source = eff;
		}
	}

	public class BrightnessEffectContainer : OneParameterEffectContainer<BrightnessEffect>
	{
		public BrightnessEffectContainer() : base("Brightness", "Brightness") { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as BrightnessEffect;
			eff.Source = source;
			eff.Level = Param1;
			source = eff;
		}
	}

	public class SaturationEffectContainer : TwoParameterEffectContainer<HueSaturationEffect>
	{
		public SaturationEffectContainer() : base("Saturation", "Saturation", "Hue") { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as HueSaturationEffect;
			eff.Source = source;
			eff.Saturation = Param1;
			eff.Hue = Param2;
			source = eff;
		}
	}
	
	public class MirrorEffectContainer : EffectContainer<MirrorEffect>
	{
		public MirrorEffectContainer() : base("Mirror") { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as MirrorEffect;
			eff.Source = source;
			source = eff;
		}
	}

	public class GrayscaleEffectContainer : EffectContainer<GrayscaleEffect>
	{
		public GrayscaleEffectContainer() : base("Grayscale") { }
		public override void ApplyEffect(ref IImageProvider source)
		{
			var eff = _effect as GrayscaleEffect;
			eff.Source = source;
			source = eff;
		}
	}
	public class ColorAdjustEffectContainer : ThreeParameterEffectContainer<ColorAdjustEffect>
	{
		public ColorAdjustEffectContainer() : base("Colorization", "Red", "Green", "Blue", 0, 1, 0.01,   0, 1, 0.01,   0, 1, 0.01) { }
		private GrayscaleEffect _gray = new GrayscaleEffect();
		public override void ApplyEffect(ref IImageProvider source)
		{
			_gray.Source = source;
			source = _gray;

			var eff = _effect as ColorAdjustEffect;
			eff.Red = Param1;
			eff.Green = Param2;
			eff.Blue = Param3;
			eff.Source = source;
			source = eff;
		}
	}
	#endregion
}
