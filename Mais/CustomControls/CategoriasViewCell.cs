using System;

using Xamarin.Forms;

namespace Mais
{
	public class CategoriasViewCell : ViewCell
	{
		public CategoriasViewCell()
		{
			var imgOk = new Image
			{
				Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("ok.png")),
				HorizontalOptions = LayoutOptions.End,
			};
			imgOk.SetBinding(Image.ClassIdProperty, "Id");
			imgOk.SetBinding(Image.IsVisibleProperty, "Selecionada");

			var lblNomeCategoria = new Label
			{
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Style = Estilos._estiloFonteCategorias,
				YAlign = TextAlignment.Center
			};
			lblNomeCategoria.SetBinding(Label.TextProperty, "Nome");

			var layout = new StackLayout
			{
				Padding = new Thickness(5, 5, 5, 5),
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = { lblNomeCategoria, imgOk }
			};

			this.View = layout;
		}
	}
}


