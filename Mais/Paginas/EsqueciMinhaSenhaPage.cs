using System;

using Xamarin.Forms;

namespace Mais
{
	public class EsqueciMinhaSenhaPage : ContentPage
	{
		public EsqueciMinhaSenhaPage()
		{
			var lblTexto = new Label
			{
				Style = Estilos._estiloFonteCategorias,
				YAlign = TextAlignment.Center,
				Text = AppResources.TextoEsqueciMinhaSenha
			};

			var entEmail = new Entry
			{
				Keyboard = Keyboard.Email
			};

			var btnEnviar = new Button
			{
				Text = AppResources.TextoResetarSenha,
				Style = Estilos._estiloPadraoButton,
				HorizontalOptions = LayoutOptions.Center
			};

			var mainLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(0, 20, 0, 0),
				Spacing = 10,
				Children = { lblTexto, entEmail, btnEnviar }
			};

			this.Content = mainLayout;
		}
	}
}


