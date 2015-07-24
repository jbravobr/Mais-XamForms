using System;
using Xamarin.Forms;

namespace Mais
{
	public class EnqueteViewCell : ViewCell
	{
		public EnqueteViewCell()
		{
			var imgQuote = new Image
			{
				HorizontalOptions = LayoutOptions.StartAndExpand
			};
			imgQuote.SetBinding(Image.SourceProperty, "ImageSource");

			var lblTitulo = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				YAlign = TextAlignment.Center,
				Style = Estilos._estiloFonteEnquete
			};
			lblTitulo.SetBinding(Label.TextProperty, "Titulo");

			var imgSetaDireita = new Image
			{
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("setaDireita2.png")),
				Aspect = Aspect.AspectFit
			};

			var mainLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.FromHex("#002347"),
				Children = { imgQuote, lblTitulo, imgSetaDireita },
				Padding = new Thickness(8, 0, 0, 0)
			};
			
			this.View = mainLayout;
		}
	}
}

