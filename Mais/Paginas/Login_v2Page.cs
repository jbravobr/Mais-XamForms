using System;
using Xamarin.Forms;
using Autofac;

namespace Mais
{
    public class Login_v2Page : ContentPage
    {
        Image logo;
        Entry txtLogin;
        Entry txtSenha;
        Button btnentrar;
        Label esqueciSenha;
        Label textoCadastrar;
        Label novoCadastro;
        Image linhaVerticalHum;
        Image linhaVerticalDois;
        Image circulo;
        Image facebook;
        Label textoEntrarComFacebook;
        readonly LoginViewModel model;

        public Login_v2Page()
        {
            this.BindingContext = model = App.Container.Resolve<LoginViewModel>();
            model.ConfiguraNavegacao(this.Navigation);

            logo = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("logo.png"))
            };

            txtLogin = new CustomEntry
            {
                Placeholder = AppResources.TextoCampoLoginFormLogin,
                Keyboard = Keyboard.Email,
            };
            txtLogin.SetBinding<LoginViewModel>(Entry.TextProperty, x => x.login);

            txtSenha = new CustomEntry
            {
                Placeholder = AppResources.TextoCampoSenhaFormLogin,
                IsPassword = true,
            };
            txtSenha.SetBinding<LoginViewModel>(Entry.TextProperty, x => x.senha);

            var camposLoginStack = new StackLayout
            {
                Children = { txtLogin, txtSenha }
            };

            btnentrar = new Button
            {
                Style = Estilos._estiloPadraoButton,
                Text = AppResources.TextoBotaoOKLogin
            };
            btnentrar.SetBinding(Button.CommandProperty, "btnEntrar_Click");

            esqueciSenha = new Label
            {
                Text = "Esqueci minha senha",
                FontFamily = Device.OnPlatform(
                    iOS: "Helvetica",
                    Android: "Roboto",
                    WinPhone: "Segoe"
                ),
                FontSize = 13,
                TextColor = Color.FromHex("##BBEEFF"),
                HorizontalOptions = LayoutOptions.Center,
                YAlign = TextAlignment.Center
            };
            var esqueciSenha_Click = new TapGestureRecognizer();
            esqueciSenha_Click.Tapped += async (sender, e) =>
            {
                var confirmConfigs = new Acr.UserDialogs.PromptConfig
                {
                    CancelText = AppResources.TextoBotaoCancelarLogin,
                    OkText = AppResources.TextoResetarSenha,
                    Message = AppResources.TextoEsqueciMinhaSenha,
                };

                var result = await Acr.UserDialogs.UserDialogs.Instance.PromptAsync(confirmConfigs);

                if (result.Ok)
                {
                    await model.EsqueciMinhaSenha(result.Text);
                }
            };
            
            var entrarStack = new StackLayout
            {
                Children = { btnentrar, esqueciSenha },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            textoCadastrar = new Label
            {
                Text = "Não possui uma conta Mais ?",
                FontFamily = Device.OnPlatform(
                    iOS: "Helvetica",
                    Android: "Roboto",
                    WinPhone: "Segoe"
                ),
                FontSize = 13,
                TextColor = Color.White
            };

            novoCadastro = new Label
            {
                Text = "Cadastre-se aqui !",
                FontFamily = Device.OnPlatform(
                    iOS: "Helvetica",
                    Android: "Roboto",
                    WinPhone: "Segoe"
                ),
                FontSize = 13,
                TextColor = Color.FromHex("##BBEEFF"),
                HorizontalOptions = LayoutOptions.Center,
                YAlign = TextAlignment.Center
            };
            var novoCadastro_Click = new TapGestureRecognizer();
            novoCadastro_Click.Tapped += async (sender, e) =>
            {
                var paginaCadastro = Activator.CreateInstance<CadastroPage>();
                await this.Navigation.PushModalAsync(paginaCadastro);
            };

            var cadastroStack = new StackLayout
            {
                Children = { textoCadastrar, novoCadastro },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 2, 0, 2)
            };

            linhaVerticalHum = new Image
            {
                BackgroundColor = Color.Gray,
                HeightRequest = 1,
                WidthRequest = 20,
            };

            facebook = new Image();
            facebook.Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("LoginFacebook.png"));
            facebook.VerticalOptions = LayoutOptions.Center;
            facebook.HorizontalOptions = LayoutOptions.CenterAndExpand;
            facebook.HeightRequest = 60;
            facebook.WidthRequest = 63;

            var facebook_Click = new TapGestureRecognizer();
            facebook_Click.Tapped += async (sender, e) =>
            {
                if (Device.OS == TargetPlatform.iOS)
                    await this.Navigation.PushModalAsync(new CadastroComFacebookPage());
                else
                    await this.Navigation.PushModalAsync(new CadastroComFacebookPage());
            };
            facebook.GestureRecognizers.Add(facebook_Click);

            textoEntrarComFacebook = new Label
            {
                Text = "Entre com a sua rede social",
                FontFamily = Device.OnPlatform(
                    iOS: "Helvetica",
                    Android: "Roboto",
                    WinPhone: "Segoe"
                ),
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center,
                YAlign = TextAlignment.Center,
                TextColor = Color.White
            };

            var facebookStack = new StackLayout
            {
                Children = { facebook, textoEntrarComFacebook },
                VerticalOptions = LayoutOptions.Center,
            };

            var logoStack = new StackLayout
            {
                Children = { logo },
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(20, Device.OnPlatform(40, 40, 0), 5, 30)
            };

            var mainLayout = new StackLayout
            {
                Padding = new Thickness(5, Device.OnPlatform(40, 40, 0), 5, 5),
                Children = { logoStack, camposLoginStack, entrarStack, cadastroStack, linhaVerticalHum, facebookStack }
            };
            
            this.BackgroundColor = Colors._loginBackgroundColorFromHex;
            this.Content = new ScrollView
            {
                Content = mainLayout
            };
        }
    }
}

