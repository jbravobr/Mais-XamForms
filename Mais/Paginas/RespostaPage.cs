using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Autofac;

namespace Mais
{
	public class RespostaPage : ContentPage
	{
		PerguntaViewModel model;
		Pergunta pergunta;
		readonly StackLayout mainLayout;
		readonly Button btnResponder;
		string imagem;
		string urlVideo;
		bool temVoucher;
		string[] str;
		Image imgThumbVideo;
		WebView webView;

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			this.model = App.Container.Resolve<PerguntaViewModel>();
			this.BindingContext = model;

			this.pergunta = model.Pergunta;
			this.model.ConfigurarNavigation(this.Navigation);

			var lblTitulo = new Label
			{
				FontSize = 28,
				FontFamily = Device.OnPlatform(
					iOS: "Helvetica",
					Android: "Roboto",
					WinPhone: "Segoe"
				),
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				YAlign = TextAlignment.Center,
				XAlign = TextAlignment.Center,
				LineBreakMode = LineBreakMode.WordWrap
			};
			lblTitulo.SetBinding<PerguntaViewModel>(Label.TextProperty, x => x.Pergunta.TextoPergunta);

			var Imagem = new Image
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Aspect = Aspect.AspectFit,
			};

			if (!String.IsNullOrEmpty(this.imagem))
				Imagem.Source = ImageSource.FromFile(DependencyService.Get<ISaveAndLoadFile>().GetImage(this.imagem));

			if (!String.IsNullOrEmpty(this.urlVideo))
			{
				if (Device.OS == TargetPlatform.iOS)
				{
					webView = new WebView();
					var htmlSource = new HtmlWebViewSource();
					htmlSource.Html = BuildEmbedUrl(this.urlVideo);
					webView.Source = htmlSource;
					webView.HeightRequest = 200;
					webView.WidthRequest = 210;
					webView.VerticalOptions = LayoutOptions.Center;
					webView.HorizontalOptions = LayoutOptions.Center;
				}
				else
				{
					str = new Uri(this.urlVideo).Segments;
					App.UrlVideo = this.urlVideo;

					imgThumbVideo = new Image
					{
						Source = ImageSource.FromFile(DependencyService.Get<ISaveAndLoadFile>().GetImage(String.Concat(str[2], ".jpg"))),
						HeightRequest = 200,
						WidthRequest = 100
					};

					var imgThumbVideo_Click = new TapGestureRecognizer();
					imgThumbVideo_Click.Tapped += (object sender, EventArgs e) => DependencyService.Get<IYoutubePlayer>().PlayVideo(this.urlVideo);
					imgThumbVideo.GestureRecognizers.Add(imgThumbVideo_Click);
				}
			}

			var tituloNoticia = new Label();

			MessagingCenter.Subscribe<ListaRespostasView,List<Resposta>>(this, "Respondido", async (sender, respostas) =>
				{
					this.model.AdicionaRespostasRespondidas(respostas, temVoucher);
				});

			StackLayout perguntaLayout;

			if (!String.IsNullOrEmpty(this.urlVideo))
			{
				if (Device.OS == TargetPlatform.Android)
				{
					perguntaLayout = new StackLayout
					{
						HeightRequest = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth * 2,
						HorizontalOptions = LayoutOptions.Start,
						Children = { imgThumbVideo, tituloNoticia },
						Padding = 20
					};
				}
				else
					perguntaLayout = new StackLayout
					{
						HeightRequest = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth * 2,
						HorizontalOptions = LayoutOptions.Start,
						Children = { webView, tituloNoticia },
						Padding = 20
					};

			}
			else if (!String.IsNullOrEmpty(this.imagem))
			{
				perguntaLayout = new StackLayout
				{
					HeightRequest = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth * 2,
					HorizontalOptions = LayoutOptions.Start,
					Children = { Imagem, tituloNoticia },
					Padding = 20
				};
			}
			else
			{
				perguntaLayout = new StackLayout
				{
					HeightRequest = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth * 2,
					HorizontalOptions = LayoutOptions.Start,
					Children = { tituloNoticia },
					Padding = 20
				};
			}

			btnResponder.SetBinding(Button.CommandProperty, "btnGravar_Click");

			this.mainLayout.Children.Add(lblTitulo);
			this.mainLayout.Children.Add(perguntaLayout);
			this.mainLayout.Children.Add(btnResponder);
		}

		public RespostaPage(string imagemNome, string urlvideo, bool temVoucher)
		{
			this.imagem = imagemNome;
			this.urlVideo = urlvideo;
			this.temVoucher = temVoucher;

			btnResponder = new Button
			{
				Style = Estilos._estiloPadraoButton,
				Text = AppResources.TextoParaBotaoResponderEnquete,
				VerticalOptions = LayoutOptions.End
			};
			btnResponder.SetBinding(Button.CommandProperty, "btnGravar_Click");
			btnResponder.SetBinding(Button.CommandParameterProperty, "Respostas");

			this.mainLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				HeightRequest = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth,
				Padding = new Thickness(0, 20, 0, 10)
			};

			this.Content = mainLayout;
		}

		private static string BuildEmbedUrl(string videoSource)
		{
			var iframeURL = string.Format("<html><body><iframe width=\"240\" height=\"110\" src=\"{0}\" frameborder=\"0\" allowfullscreen></iframe></body></html>", videoSource);
			return iframeURL;
		}
	}
}


