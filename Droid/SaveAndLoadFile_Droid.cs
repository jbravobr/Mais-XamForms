using System;
using Xamarin.Forms;
using Mais.Droid;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System.Net;
using System.IO;

[assembly: Dependency (typeof(SaveAndLoadFile_Droid))]
namespace Mais.Droid
{
	public class SaveAndLoadFile_Droid : ISaveAndLoadFile
	{
		Bitmap imagem;

		#region ISaveAndLoadFile implementation

		public async Task<bool> SaveImage (ImageSource img, string imageName)
		{
			try {
				var renderer = new StreamImagesourceHandler ();
				imagem = await renderer.LoadImageAsync (img, Forms.Context);

				var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				string pngPath = System.IO.Path.Combine (documentsDirectory, imageName + ".png");

				using (System.IO.FileStream fs = new System.IO.FileStream (pngPath, System.IO.FileMode.OpenOrCreate)) {
					imagem.Compress (Bitmap.CompressFormat.Png, 100, fs);
					return await Task.FromResult<bool> (true);
				}
			} catch (Exception) {
				return await Task.FromResult<bool> (false);
			}
		}

		public string GetImage (string imageName)
		{
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var nomeImagem = System.IO.Path.Combine (path, imageName);

			return nomeImagem;
		}

		public byte[] GetImageArray (string imageName)
		{
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var nomeImagem = System.IO.Path.Combine (path, imageName);

			return System.IO.File.ReadAllBytes (nomeImagem);
		}

		public async Task<bool> BaixaImagemSalvarEmDisco (string imagem, string url)
		{
			using (var client = new WebClient ()) {
				var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				var nomeImagem = System.IO.Path.Combine (path, imagem);

				var imageBytes = await client.DownloadDataTaskAsync (new Uri (String.Format ("{0}{1}", url, imagem)));
				Bitmap imagemBitmap = null;

				using (var ms = new MemoryStream (imageBytes)) {
					if (imageBytes != null && imageBytes.Length > 0) {
						var options = new BitmapFactory.Options { InSampleSize = 2 };
						imagemBitmap = await BitmapFactory.DecodeStreamAsync (ms, null, options);
						await imagemBitmap.CompressAsync (Bitmap.CompressFormat.Png, 70, ms);
					}

					try {
						if (imagemBitmap != null) {
							System.IO.File.WriteAllBytes (nomeImagem, ms.ToArray ());
							return await Task.FromResult (true);
						}

						return await Task.FromResult (false);
					} catch (Exception ex) {
						return await Task.FromResult (false);
					}
				}
			}
		}

		public async Task<bool> BaixaThumbnailYoutubeSalvarEmDisco (string url, string imageName)
		{
			using (var client = new WebClient ()) {
				var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				var nomeImagem = System.IO.Path.Combine (path, imageName);

				var imageBytes = await client.DownloadDataTaskAsync (new Uri (url));
				Bitmap imagemBitmap = null;

				using (var ms = new MemoryStream (imageBytes)) {
					if (imageBytes != null && imageBytes.Length > 0) {
						var options = new BitmapFactory.Options { InSampleSize = 2 };
						imagemBitmap = await BitmapFactory.DecodeStreamAsync (ms, null, options);
						await imagemBitmap.CompressAsync (Bitmap.CompressFormat.Png, 70, ms);
					}

					try {
						if (imagemBitmap != null) {
							System.IO.File.WriteAllBytes (nomeImagem, ms.ToArray ());
							return await Task.FromResult (true);
						}

						return await Task.FromResult (false);
					} catch (Exception ex) {
						return await Task.FromResult (false);
					}
				}
			}
		}

		#endregion
	}
}

