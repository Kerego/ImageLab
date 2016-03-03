using ImageEffects.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;

namespace ImageEffects
{
	public class ImageEditor
	{
		const int BYTES_PER_PIXEL = 4;
		protected SoftwareBitmap InputImage { get; }
		public SoftwareBitmap OutputImage { get; set; }


		public ImageEditor(SoftwareBitmap image)
		{
			OutputImage = image;
			InputImage = new SoftwareBitmap(image.BitmapPixelFormat, image.PixelWidth, image.PixelHeight, image.BitmapAlphaMode);
			OutputImage.CopyTo(InputImage);
			Debug.Write("bla");
		}


		public unsafe void ApplyKernelEffects(IEnumerable<IKernelImageEffect> effects)
		{

		}



		public unsafe void ApplyPixelEffects(IEnumerable<IImageEffect> effects)
		{
			if (InputImage.BitmapPixelFormat != BitmapPixelFormat.Bgra8)
			{
				return;
			}
			using (var inputBuffer = InputImage.LockBuffer(BitmapBufferAccessMode.Read))
			using (var outputBuffer = OutputImage.LockBuffer(BitmapBufferAccessMode.Write))
			using (var inputReference = inputBuffer.CreateReference())
			using (var outputReference = outputBuffer.CreateReference())
			{
				if (inputReference is IMemoryBufferByteAccess && outputReference is IMemoryBufferByteAccess)
				{Stopwatch sw = Stopwatch.StartNew();
					// Get a pointer to the pixel buffer
					byte* inputData;
					byte* outputData;
					uint capacity;
					((IMemoryBufferByteAccess)inputReference).GetBuffer(out inputData, out capacity);
					((IMemoryBufferByteAccess)outputReference).GetBuffer(out outputData, out capacity);

					// Get information about the BitmapBuffer
					var desc = inputBuffer.GetPlaneDescription(0);
					
					Parallel.For(0, desc.Height, (row, state) =>
					{
						for (int col = 0; col < desc.Width; col++)
						{
							var currPixel = desc.StartIndex + desc.Stride * row + BYTES_PER_PIXEL * col;
							var b = inputData[currPixel + 0];
							var g = inputData[currPixel + 1];
							var r = inputData[currPixel + 2];

							foreach (var t in effects.Where(x=>x.Enabled && x is IPixelImageEffect).Cast<IPixelImageEffect>())
							{
								t.ApplyPixelEffect(r, g, b, out r, out g, out b);
							}
								
							outputData[currPixel + 0] = b;
							outputData[currPixel + 1] = g;
							outputData[currPixel + 2] = r;
						}
					});
					sw.Stop();

				}

			}

		}





	}

}
