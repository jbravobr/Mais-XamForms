using System;

using Xamarin.Forms;

namespace Mais
{
	public class IndexPage : ContentPage
	{
		public IndexPage()
		{
			this.BackgroundColor = Colors._defaultColorFromHex;

			var btnEnquentesPublicas = new Button();
			var btnEnquetesInteresse = new Button();
			var btnCriarEnquetes = new Button();
			var btnVisualizarDados = new Button();

			btnEnquentesPublicas.Text = "Conteúdo Público";
			btnEnquentesPublicas.Style = Estilos._estiloPadraoButtonBackgroundColorWhite;
			btnEnquentesPublicas.VerticalOptions = LayoutOptions.Center;
			btnEnquentesPublicas.HorizontalOptions = LayoutOptions.CenterAndExpand;
			btnEnquentesPublicas.Clicked += (sender, e) => this.Navigation.PushModalAsync(new MainPage(new EnquetePublica()));

			btnEnquetesInteresse.Text = "Conteúdo Pessoal";
			btnEnquetesInteresse.Style = Estilos._estiloPadraoButtonBackgroundColorWhite;
			btnEnquetesInteresse.VerticalOptions = LayoutOptions.Center;
			btnEnquetesInteresse.HorizontalOptions = LayoutOptions.CenterAndExpand;
			btnEnquetesInteresse.Clicked += (sender, e) => this.Navigation.PushModalAsync(new MainPage(new EnqueteInteresse()));

			btnCriarEnquetes.Text = "Nova Enquete";
			btnCriarEnquetes.Style = Estilos._estiloPadraoButtonBackgroundColorWhite;
			btnCriarEnquetes.VerticalOptions = LayoutOptions.Center;
			btnCriarEnquetes.HorizontalOptions = LayoutOptions.CenterAndExpand;
			btnCriarEnquetes.Clicked += (sender, e) => this.Navigation.PushModalAsync(new MainPage(new NovaEnquetePage()));

			btnVisualizarDados.Text = "Editar Dados Pessoais";
			btnVisualizarDados.Style = Estilos._estiloPadraoButtonBackgroundColorWhite;
			btnVisualizarDados.VerticalOptions = LayoutOptions.Center;
			btnVisualizarDados.HorizontalOptions = LayoutOptions.CenterAndExpand;
			btnVisualizarDados.Clicked += (sender, e) => this.Navigation.PushModalAsync(new MainPage(new CadastroEdicaoPage()));


			var mainLayout = new StackLayout
			{
				Padding = new Thickness(12, 12, 12, 12),
				Spacing = 4,
				Orientation = StackOrientation.Vertical,
				Children = { btnEnquentesPublicas, btnEnquetesInteresse, btnCriarEnquetes, btnVisualizarDados }
			};

			this.Content = mainLayout;
		}
	}
}


