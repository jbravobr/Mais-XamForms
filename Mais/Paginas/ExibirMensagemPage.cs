using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Autofac;
using System.Linq;

namespace Mais
{
    public class ExibirMensagemPage : ContentPage
    {
        PerguntaViewModel model;
        readonly int enqueteID;
        Enquete enquete;
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
            try
            {
                base.OnAppearing();
                this.model = App.Container.Resolve<PerguntaViewModel>();
                this.BindingContext = model;
				
                model.AdicionaMensagem(await model.GetMensagem(enqueteID));
                this.enquete = model.Mensagem;
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
                lblTitulo.SetBinding<PerguntaViewModel>(Label.TextProperty, x => x.Mensagem.Titulo);
				
                var lblDescricao = new Label
                {
                    FontSize = 22,
                    FontFamily = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    ),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    YAlign = TextAlignment.Center,
                    XAlign = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.WordWrap
                };
                lblDescricao.SetBinding<PerguntaViewModel>(Label.TextProperty, x => x.Mensagem.Descricao);
				
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
                        webView.HeightRequest = 320;
                        webView.WidthRequest = 270;
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
                        imgThumbVideo_Click.Tapped += (object sender, EventArgs e) => this.Navigation.PushModalAsync(new VideoPage());
                        imgThumbVideo.GestureRecognizers.Add(imgThumbVideo_Click);
                    }
                }
				
                var btnCompartilhar = new Button
                {
                    Text = "Compartilhar",
                    Style = Estilos._estiloPadraoButton
                };
                btnCompartilhar.Clicked += async (sender, e) =>
                {

                    var dbFacebook = new Repositorio<FacebookInfos>();
                    var userToken = await dbFacebook.RetornarTodos();

                    if (userToken == null)
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro na autenticação com o Facebook, tente novamente!");
                        return;
                    }

                    var postou = await DependencyService.Get<IFacebook>().PostToWall(this.enquete.Titulo, userToken.First().access_token);

                    if (postou)
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Postagem concluída com sucesso!");
                        await this.Navigation.PushModalAsync(new MainPage());
                    }
                    else
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro ao postar, tente novamente!");
                };

                if (App.Current.Properties.ContainsKey("UsuarioLogado"))
                {
                    var u = App.Current.Properties["UsuarioLogado"] as Usuario;
                    var dbFacebook = new Repositorio<FacebookInfos>();
                    var dadosFacebook = await dbFacebook.ExisteRegistroFacebook();

                    if (dadosFacebook == null || String.IsNullOrEmpty(dadosFacebook.access_token))
                        btnCompartilhar.IsVisible = false;
                }

                StackLayout perguntaLayout;
				
                if (!String.IsNullOrEmpty(this.urlVideo))
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        perguntaLayout = new StackLayout
                        {
                            HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                            HorizontalOptions = LayoutOptions.Center,
                            Children = { imgThumbVideo, btnCompartilhar },
                            Padding = 20
                        };
                    }
                    else
                        perguntaLayout = new StackLayout
                        {
                            HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                            HorizontalOptions = LayoutOptions.Center,
                            Children = { webView, btnCompartilhar },
                            Padding = 20
                        };
						
                }
                else if (!String.IsNullOrEmpty(this.imagem))
                {
                    perguntaLayout = new StackLayout
                    {
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = { Imagem, btnCompartilhar },
                        Padding = 20
                    };
                }
                else
                {
                    perguntaLayout = new StackLayout
                    {
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = { btnCompartilhar },
                        Padding = 20
                    };
                }
				
                btnResponder.SetBinding(Button.CommandProperty, "btnGravar_Click");
                                				
                this.mainLayout.Children.Add(lblTitulo);
                this.mainLayout.Children.Add(lblDescricao);
                this.mainLayout.Children.Add(perguntaLayout);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ExibirMensagemPage(int enqueteId, string imagemNome, string urlvideo, bool temVoucher)
        {
            this.enqueteID = enqueteId;	
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
                HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth,
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


