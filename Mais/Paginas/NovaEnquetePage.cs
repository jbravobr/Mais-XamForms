using System;
using System.Linq;

using Xamarin.Forms;
using System.Collections.Generic;
using Autofac;

namespace Mais
{
	public class NovaEnquetePage : ContentPage
	{
		CriaNovaEnqueteView enqueteContent;
		NovaEnqueteViewModel model;

		public NovaEnquetePage()
		{
			this.BindingContext = model = App.Container.Resolve<NovaEnqueteViewModel>();
			MessagingCenter.Subscribe<SelecaoAmigosEnquetePage,ICollection<Amigo>>(this, "gravarAmigos", (sender, arg) => this.model.AdicionarAmigoAoEnvio(arg));
			model.ConfiguraNavegacao(this.Navigation);
			this.Title = "Nova Enquete";

			var logo = new Image
			{
				Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("logo_mini.png")),
			};

			var btnSelecionarAmigos = new Button
			{
				Text = AppResources.TextoBotaoSelecionarAmigos,
				Style = Estilos._estiloPadraoButtonFonteMenor,
				HorizontalOptions = LayoutOptions.End
			};
			btnSelecionarAmigos.Clicked += async (sender, e) =>
			{
				await this.Navigation.PushModalAsync(new SelecaoAmigosEnquetePage());
			};

			enqueteContent = new CriaNovaEnqueteView();

			var stackResposta = new StackLayout
			{
				HeightRequest = HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenHeight * 2, 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				Children = { enqueteContent } 
			};

			var enqueteScroll = new ScrollView
			{
				Orientation = ScrollOrientation.Vertical,
				Content = stackResposta
			};
			
			var btnAdicionarResposta = new Button
			{
				Style = Estilos._estiloPadraoButtonFonteMenor,
				Text = AppResources.TextoBotaoAdicionarResposta,
				VerticalOptions = LayoutOptions.End
			};
			btnAdicionarResposta.Clicked += (sender, e) =>
			{
				var entResposta = new Entry
				{
					Placeholder = AppResources.TextoPlaceholderTituloNaRespostaEnquete,
					WidthRequest = 100
				};
				var apagar = new Image();
				apagar.Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("Cancel-32.png"));
				apagar.HorizontalOptions = LayoutOptions.End;
				apagar.ClassId = Guid.NewGuid().ToString();

				var grid = new Grid
				{
					ColumnDefinitions =
					{
						new ColumnDefinition{ Width = new GridLength(2, GridUnitType.Star) },
						new ColumnDefinition{ Width = GridLength.Auto }
					}
				};
				grid.Children.Add(entResposta, 0, 0);
				grid.Children.Add(apagar, 1, 0);
				grid.ClassId = apagar.ClassId;

				var apagar_click = new TapGestureRecognizer();
				apagar_click.Tapped += (s, elemnt) =>
				{
					foreach (var item in this.enqueteContent.RespostasStack.Children)
					{
						if ((item).ClassId == ((Image)s).ClassId)
						{
							this.enqueteContent.RespostasStack.Children.Remove(item);
							break;
						}
					}
				};
				apagar.GestureRecognizers.Add(apagar_click);

				this.enqueteContent.RespostasStack.Children.Add(grid);
			};

			var btnFinalizarEnquete = new Button
			{
				Style = Estilos._estiloPadraoButtonFonteMenor,
				Text = AppResources.TextoBotaoFinalizarEnquete,
				VerticalOptions = LayoutOptions.End
			};
			btnFinalizarEnquete.Clicked += async (sender, e) =>
			{
				var listaRespostas = new List<Resposta>();

				foreach (var item in enqueteContent.RespostasStack.Children)
				{
					if (item.GetType() == typeof(Entry))
					{
						var textoResposta = ((Entry)item).Text;
						listaRespostas.Add(new Resposta{ Respondida = false, TextoResposta = textoResposta });
					}
					else if (item.GetType() == typeof(Grid))
					{
						foreach (var row in ((Grid)item).Children.Where(g=>g.GetType() == typeof(Entry)))
						{
							var texto = ((Entry)row).Text;
							listaRespostas.Add(new Resposta{ Respondida = false, TextoResposta = texto });
						}
					}
				}
				model.AdicionaRespostaParaColecao(listaRespostas);
				await model.SalvaEnquete();
			};
			
			var gridFooter = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) }
				}
			};
			gridFooter.Children.Add(btnAdicionarResposta, 0, 0);
			gridFooter.Children.Add(btnFinalizarEnquete, 1, 0);

			var gridHeader = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition{ Height = GridLength.Auto }			
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(0.20, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(0.15, GridUnitType.Star) }
				}
			};
			gridHeader.Children.Add(logo, 1, 0);
			gridHeader.Children.Add(btnSelecionarAmigos, 2, 0);

			var mainLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = Device.OnPlatform(15, 10, 0),
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(5, Device.OnPlatform(20, 10, 0), 5, 5),
				Children = 
						{ gridHeader, enqueteScroll, gridFooter }
			};

			this.Content = mainLayout;
		}
	}
}


