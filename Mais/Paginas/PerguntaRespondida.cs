﻿using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Autofac;
using System.Linq;

namespace Mais
{
    public class PerguntaRespondidaPage : ContentPage
    {
        PerguntaViewModel model;
        readonly int perguntaID;
        Pergunta pergunta;
        readonly StackLayout mainLayout;
        readonly Button btnResponder;
        string imagem;
        string urlVideo;
        bool? temVoucher;
        ListaRespostasView listaRespostas;
        string[] str;
        Image imgThumbVideo;
        WebView webView;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this.model = App.Container.Resolve<PerguntaViewModel>();
            this.BindingContext = model;

            model.AdicionarPergunta(await model.GetPerguntaPorId(perguntaID));
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
                    imgThumbVideo_Click.Tapped += (object sender, EventArgs e) => this.Navigation.PushModalAsync(new VideoPage()); //DependencyService.Get<IYoutubePlayer> ().PlayVideo (this.urlVideo);
                    imgThumbVideo.GestureRecognizers.Add(imgThumbVideo_Click);
                }
            }

            if (this.pergunta.Respostas != null && this.pergunta.Respostas.Any())
                listaRespostas = new ListaRespostasView(this.pergunta.Respostas, 1);

            var btnCompartilhar = new Button
            {
                Text = "Compartilhar",
                Style = Estilos._estiloPadraoButton
            };
            btnCompartilhar.Clicked += async (sender, e) =>
            {
                var dbResposta = new Repositorio<Resposta>();
                var _resposta = await dbResposta.RetornarPorId(this.pergunta.Respostas.First(x => x.Respondida).Id);

                var msg = String.Format("Eu votei na enquete {0} com {1}%... minha resposta foi {2}"
						, this.pergunta.TextoPergunta
						, _resposta.percentualResposta
						, _resposta.TextoResposta);

                var dbFacebook = new Repositorio<FacebookInfos>();
                var userToken = await dbFacebook.RetornarTodos();

                var postou = await DependencyService.Get<IFacebook>().PostToWall(msg, userToken.First().access_token);

                if (postou)
                    await this.Navigation.PushModalAsync(new EnquetePage());
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
                if (listaRespostas != null)
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        perguntaLayout = new StackLayout
                        {
                            HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                            HorizontalOptions = LayoutOptions.Start,
                            Children = { imgThumbVideo, listaRespostas, btnCompartilhar },
                            Padding = 20
                        };
                    }
                    else
                    {
                        perguntaLayout = new StackLayout
                        {
                            HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                            HorizontalOptions = LayoutOptions.Start,
                            Children = { webView, listaRespostas, btnCompartilhar },
                            Padding = 20
                        };
                    }
                }
                else
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        perguntaLayout = new StackLayout
                        {
                            HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                            HorizontalOptions = LayoutOptions.Start,
                            Children = { imgThumbVideo, btnCompartilhar },
                            Padding = 20
                        };
                    }
                    else
                    {
                        perguntaLayout = new StackLayout
                        {
                            HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                            HorizontalOptions = LayoutOptions.Start,
                            Children = { webView, btnCompartilhar },
                            Padding = 20
                        };   
                    }
                }

            }
            else if (!String.IsNullOrEmpty(this.imagem))
            {
                if (listaRespostas != null)
                {
                    perguntaLayout = new StackLayout
                    {
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                        HorizontalOptions = LayoutOptions.Start,
                        Children = { Imagem, listaRespostas, btnCompartilhar },
                        Padding = 20
                    };
                }
                else
                {
                    perguntaLayout = new StackLayout
                    {
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                        HorizontalOptions = LayoutOptions.Start,
                        Children = { Imagem, btnCompartilhar },
                        Padding = 20
                    };
                }

            }
            else
            {
                if (listaRespostas != null)
                {
                    perguntaLayout = new StackLayout
                    {
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                        HorizontalOptions = LayoutOptions.Start,
                        Children = { listaRespostas, btnCompartilhar },
                        Padding = 20
                    };
                }
                else
                {
                    perguntaLayout = new StackLayout
                    {
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth * 2,
                        HorizontalOptions = LayoutOptions.Start,
                        Children = { btnCompartilhar },
                        Padding = 20
                    };
                }
            }

            btnResponder.SetBinding(Button.CommandProperty, "btnGravar_Click");

            this.mainLayout.Children.Add(lblTitulo);
            this.mainLayout.Children.Add(perguntaLayout);
        }

        public PerguntaRespondidaPage(int perguntaId, string imagemNome, string urlvideo, bool? temVoucher = null)
        {
            this.perguntaID = perguntaId;	
            this.imagem = imagemNome;
            this.urlVideo = urlvideo;
            this.temVoucher = temVoucher;

            btnResponder = new Button
            {
                Style = Estilos._estiloPadraoButton,
                Text = AppResources.TextoParaBotaoResponderEnquete,
                VerticalOptions = LayoutOptions.End,
            };
            btnResponder.SetBinding(Button.CommandProperty, "btnGravar_Click");
            btnResponder.SetBinding(Button.CommandParameterProperty, "Respostas");

            MessagingCenter.Subscribe<ListaRespostasEnqueteRespondidaViewCell,string>(this, "MostraImagem", async (sender, img) =>
                {
                    this.Navigation.PushModalAsync(new ExibeImagemResposta(img));
                });

            MessagingCenter.Subscribe<ListaRespostasViewCell,string>(this, "MostraImagem", async (sender, img) =>
                {
                    this.Navigation.PushModalAsync(new ExibeImagemResposta(img));
                });

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

