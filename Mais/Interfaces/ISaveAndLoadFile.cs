using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Mais
{
	public interface ISaveAndLoadFile
	{
		Task<bool> SaveImage(ImageSource img, string imageName);

		string GetImage(string imageName);

		byte[] GetImageArray(string imageName);

		Task<bool> BaixaImagemSalvarEmDisco(string imagem, string url);

		Task<bool> BaixaThumbnailYoutubeSalvarEmDisco(string url, string imageName);
	}
}

