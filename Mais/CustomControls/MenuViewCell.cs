using System;
using Xamarin.Forms;

namespace Mais
{
	public class MenuViewCell : ViewCell
	{
		public MenuViewCell()
		{
			var img = new Image
			{
				VerticalOptions = LayoutOptions.Start
			};
			img.SetBinding(Image.SourceProperty, "Icone");

			var lblMenu = new Label
			{
				Style = Estilos._estiloFonteMenu
			};
			lblMenu.SetBinding(Label.TextProperty, "Titulo");

			var mainLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = { img, lblMenu },
				Padding = 10
			};

			this.View = mainLayout;
		}
	}
}

