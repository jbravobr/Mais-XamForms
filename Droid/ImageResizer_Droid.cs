using System;
using Xamarin.Forms;
using Mais.Droid;
using Android.Graphics;
using System.IO;

[assembly: Dependency(typeof(ImageResizer_Droid))]
namespace Mais.Droid
{
	public class ImageResizer_Droid : IImageResizer
	{
		#region IImageResizer implementation

		public byte[] ResizeImage(byte[] imageData, float width, float height)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

			using (var ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
				return ms.ToArray();
			}
		}

		#endregion
	}
}

