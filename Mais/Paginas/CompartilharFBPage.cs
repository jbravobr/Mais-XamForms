using System;
using Xamarin.Forms;
using System.Linq;

namespace Mais
{
    public class CompartilharFBPage : ContentPage
    {
        Pergunta pergunta;
        string msg;
        string textoNoticia;
        Editor txtTexto;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = pergunta;

            var dbResposta = new Repositorio<Resposta>();
            var _resposta = await dbResposta.RetornarPorId(this.pergunta.Respostas.First(x => x.Respondida).Id);

            if (String.IsNullOrEmpty(this.textoNoticia))
                this.msg = String.Format("Eu votei na enquete {0} com {1}%... minha resposta foi {2}"
                , this.pergunta.TextoPergunta
                , _resposta.percentualResposta
                , _resposta.TextoResposta);
            else
                this.msg = this.textoNoticia;

            txtTexto.Text = this.msg;
        }

        public CompartilharFBPage(Pergunta pergunta, string textoNoticia)
        {
            this.textoNoticia = textoNoticia;
            this.pergunta = pergunta;

            var lblTexto = new Label
            {
                Text = "Digite abaixo o que você está pensando",
                FontSize = 18,
                YAlign = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            txtTexto = new Editor
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 70,
                WidthRequest = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth,
            };

            var btnCompartilhar = new Button
            {
                Text = "Concluir"
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

                var postou = await DependencyService.Get<IFacebook>().PostToWall(msg, userToken.First().access_token);

                if (postou)
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Postagem concluída com sucesso!");
                    await this.Navigation.PushModalAsync(new MainPage());
                }
                else
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro ao postar, tente novamente!");
            };

            var mainlayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 5,
                Children =
                {
                    lblTexto,
                    txtTexto,
                    btnCompartilhar
                }
            };

            this.Content = mainlayout;
        }
    }
}

