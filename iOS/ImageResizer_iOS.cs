using System;
using Mais.iOS;
using UIKit;
using System.Drawing;
using CoreGraphics;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageResizer_iOS))]
namespace Mais.iOS
{
	public class ImageResizer_iOS: IImageResizer
	{
		#region IImageResizer implementation

		public byte[] ResizeImage(byte[] imageData, float width, float height)
		{
			UIImage originalImage = ImageFromByteArray(imageData);

			using (var context = new CGBitmapContext(IntPtr.Zero,
				                     (int)width, (int)height, 8,
				                     (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
				                     CGImageAlphaInfo.PremultipliedFirst))
			{

				var imageRect = new RectangleF(0, 0, width, height);

				// draw the image
				context.DrawImage(imageRect, originalImage.CGImage);

				UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage());

				// save the image as a jpeg
				return resizedImage.AsJPEG().ToArray();
			}
		}

		public static UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			UIImage image;

			try
			{
				image = new UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Falha no carregamento da imagem: " + e.Message);
				return null;
			}
			return image;
		}

		#endregion
	}
}

