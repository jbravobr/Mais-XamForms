using System;

using Xamarin.Forms;

namespace Mais
{
	public class TestPage : ContentPage
	{
		const string url = "http://revistaapolice.com.br/wp-content/uploads/2013/01/";
		const string imagem = "game-icatu-300x164.jpg";
		String imgSource = String.Empty;

		public TestPage()
		{
			var img = new Image();

			var btnCarregarImagem = new Button
			{
				Text = "Carregar Imagem"
			};
			btnCarregarImagem.Clicked += async (sender, e) =>
			{
				await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(imagem, url);
				imgSource = DependencyService.Get<ISaveAndLoadFile>().GetImage(imagem);
				img.Source = ImageSource.FromFile(imgSource);
			};

			Content = new StackLayout
			{ 
				Children =
				{
					img,
					btnCarregarImagem
				}
			};
		}
	}
}


