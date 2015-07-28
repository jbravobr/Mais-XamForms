using System;
using System.Linq;

using Xamarin.Forms;
using Acr.UserDialogs;
using Autofac;

namespace Mais
{
    public class RootPage : ContentPage
    {
        RootViewModel model { get; set; }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var dbSession = new Repositorio<ControleSession>();
            var isLogado = await dbSession.RetornarTodos();

            if (isLogado != null && isLogado.Any(c => c.Logado))
                await this.Navigation.PushModalAsync(new MainPage());
        }

        public RootPage()
        {
            this.BackgroundColor = Colors._loginBackgroundColorFromHex;
            this.BindingContext = model = App.Container.Resolve<RootViewModel>();
            model.ConfiguraNavigation(this.Navigation);

            var btnEntrar = new Button();
            btnEntrar.Style = Estilos._estiloPadraoButton;
            btnEntrar.Text = AppResources.TextoBotaoOKLogin;
            btnEntrar.SetBinding(Button.CommandProperty, "btnEntrar_Click");

            var imgFacebook = new Image();
            imgFacebook.Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("LoginFacebook.png"));
            imgFacebook.VerticalOptions = LayoutOptions.Center;
            imgFacebook.HorizontalOptions = LayoutOptions.CenterAndExpand;
            imgFacebook.HeightRequest = 100;
            imgFacebook.WidthRequest = 100;

            var img_Click = new TapGestureRecognizer();
            img_Click.Tapped += async (sender, e) =>
            {
                if (Device.OS == TargetPlatform.iOS)
                    await this.Navigation.PushModalAsync(new CadastroComFacebookPage());
                else
                    await this.Navigation.PushModalAsync(new CadastroComFacebookPage());
            };

            imgFacebook.GestureRecognizers.Add(img_Click);

            var btnCadastrar = new Button();
            btnCadastrar.Style = Estilos._estiloPadraoButton;
            btnCadastrar.Text = AppResources.TextoBotaoCadastrar;
            btnCadastrar.SetBinding(Button.CommandProperty, "btnCadastrar_Click");

            var imgLogo = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("logo.png"))
            };

            var imgStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { imgLogo }
            };

            var layoutPrincipal = new StackLayout
            {
                Padding = 100,
                Spacing = 15,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { imgStack, btnEntrar, btnCadastrar, imgFacebook }
            };

            this.Content = layoutPrincipal;
        }
    }
}


