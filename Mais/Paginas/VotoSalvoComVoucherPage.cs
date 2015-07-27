using System;

using Xamarin.Forms;
using Share.Forms.Plugin.Abstractions;
using System.Linq;

namespace Mais
{
    public class VotoSalvoComVoucherPage  : ContentPage
    {
        Pergunta pergunta;
        Button btnCompartilhar;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.Current.Properties.ContainsKey("UsuarioLogado"))
            {
                var u = App.Current.Properties["UsuarioLogado"] as Usuario;
                var dbFacebook = new Repositorio<FacebookInfos>();
                var dadosFacebook = await dbFacebook.ExisteRegistroFacebook();

                if (dadosFacebook == null || String.IsNullOrEmpty(dadosFacebook.access_token))
                    btnCompartilhar.IsVisible = false;
            }
        }

        public VotoSalvoComVoucherPage(Pergunta p)
        {
            this.pergunta = p;

            var voltarStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(3, Device.OnPlatform(20, 0, 0), 0, 0),
                BackgroundColor = Color.White,
                HeightRequest = 40,
                Children =
                {
                    new Image
                    {
                        Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("voltar.png")),
                        VerticalOptions = LayoutOptions.Start
                    },
                    new Label
                    { 
                        Text = AppResources.TextoVoltar, 
                        TextColor = Color.Black, 
                        YAlign = TextAlignment.Center,
                        FontSize = 18,
                        VerticalOptions = LayoutOptions.StartAndExpand
                    }
                }
            };

            var voltarStack_Click = new TapGestureRecognizer();
            voltarStack_Click.Tapped += async (sender, e) => await this.Navigation.PushModalAsync(new MainPage());
            voltarStack.GestureRecognizers.Add(voltarStack_Click);

            var aviso = new Label
            {
                Style = Estilos._estiloFonteSucessoResposta,
                Text = AppResources.TextoParaTelaDeVotoSalvoComSucesso
            };

            var sucesso = new Label
            {
                Style = Estilos._estiloFonteSucessoRespostaSucessoVoucher,
                Text = AppResources.TextoSucessoComVoucher
            };

            var imagem = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("enqueteresp.png"))
            };

            var imagem_Click = new TapGestureRecognizer();
            imagem_Click.Tapped += async (sender, e) => await this.Navigation.PushModalAsync(new MainPage());
            imagem.GestureRecognizers.Add(imagem_Click);

            var overlayUP = new StackLayout { BackgroundColor = Colors._defaultColorFromHex, /*Children = { voltarStack }*/  };

            btnCompartilhar = new Button
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

                if (userToken == null)
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro na autenticação com o Facebook, tente novamente!");
                    return;
                }

                var postou = await DependencyService.Get<IFacebook>().PostToWall(msg, userToken.First().access_token);

                if (postou)
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Postagem concluída com sucesso!");
                    await this.Navigation.PushModalAsync(new MainPage());
                }
                else
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro ao postar, tente novamente!");
            };



            var absLayout = new AbsoluteLayout { HeightRequest = 400 };

            AbsoluteLayout.SetLayoutFlags(overlayUP, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(overlayUP, new Rectangle(1, 0, 1, 0.55));

            AbsoluteLayout.SetLayoutFlags(sucesso, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(sucesso, new Rectangle(0.5, 0.30, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(aviso, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(aviso, new Rectangle(0.5, 0.15, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(imagem, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imagem, new Rectangle(0.5, 0.55, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(btnCompartilhar, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(btnCompartilhar, new Rectangle(0.5, 0.9, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            absLayout.Children.Add(overlayUP);
            absLayout.Children.Add(sucesso);
            absLayout.Children.Add(aviso);
            absLayout.Children.Add(imagem);
            absLayout.Children.Add(btnCompartilhar);

            this.Content = absLayout;
        }
    }
}


