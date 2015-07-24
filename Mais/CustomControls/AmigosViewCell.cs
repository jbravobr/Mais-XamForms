using System;
using Xamarin.Forms;

namespace Mais
{
	public class AmigosViewCell : ViewCell
	{
		public AmigosViewCell()
		{
			var imgOk = new Image
			{
				Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("ok.png")),
				HorizontalOptions = LayoutOptions.End,
			};
			imgOk.SetBinding(Image.ClassIdProperty, "Id");
			imgOk.SetBinding(Image.IsVisibleProperty, "Selecionado");

			var lblNomeAmigo = new Label
			{
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Style = Estilos._estiloFonteCategorias,
				YAlign = TextAlignment.Center
			};
			lblNomeAmigo.SetBinding(Label.TextProperty, "Nome");

			var layout = new StackLayout
			{
				Padding = new Thickness(5, 5, 5, 5),
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = { lblNomeAmigo, imgOk }
			};

			this.View = layout;
		}
	}
}

