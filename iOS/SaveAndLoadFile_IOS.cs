using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Mais.iOS;
using System.IO;
using Foundation;
using UIKit;
using System.Net;

[assembly: Dependency(typeof(SaveAndLoadFile_IOS))]
namespace Mais.iOS
{
	public class SaveAndLoadFile_IOS : ISaveAndLoadFile
	{
		UIImage image;

		#region ISaveAndLoadFile implementation

		public async Task<bool> SaveImage(ImageSource img, string imageName)
		{
			var render = new StreamImagesourceHandler();

			image = await render.LoadImageAsync(img);

			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var nomeImagem = Path.Combine(path, imageName);

			NSData imgData = image.AsJPEG();
			NSError erro = null;

			return imgData.Save(nomeImagem, false, out erro);
		}

		public string GetImage(string imageName)
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var nomeImagem = Path.Combine(path, imageName);

			return nomeImagem;
		}

		public byte[] GetImageArray(string imageName)
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var nomeImagem = Path.Combine(path, imageName);

			return File.ReadAllBytes(nomeImagem);
		}

		public async Task<bool> BaixaImagemSalvarEmDisco(string imagem, string url)
		{
			using (var client = new WebClient())
			{
				var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var nomeImagem = Path.Combine(path, imagem);

				try
				{
					var img = FromUrl(String.Format("{0}{1}", url, imagem));

					if (img != null)
					{
						NSData imgData = img.AsPNG();
						NSError erro = null;

						var rtn = imgData.Save(nomeImagem, false, out erro);

						return await Task.FromResult(rtn);
					}

					return await Task.FromResult(false);
				}
				catch (Exception ex)
				{
					return await Task.FromResult(false);
				}
			}
		}

		private static UIImage FromUrl(string uri)
		{
			using (var url = new NSUrl(uri))
			using (var data = NSData.FromUrl(url))
				return UIImage.LoadFromData(data);
		}

		public async Task<bool> BaixaThumbnailYoutubeSalvarEmDisco(string url, string imageName)
		{
			using (var client = new WebClient())
			{
				var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var nomeImagem = Path.Combine(path, imageName);

				try
				{
					var img = FromUrl(String.Format("{0}", url));

					if (img != null)
					{
						NSData imgData = img.AsPNG();
						NSError erro = null;

						var rtn = imgData.Save(nomeImagem, false, out erro);

						return await Task.FromResult(rtn);
					}

					return await Task.FromResult(false);
				}
				catch (Exception ex)
				{
					return await Task.FromResult(false);
				}
			}
		}

		#endregion
        
	}
}

