using System;

using Xamarin.Forms;
using Autofac;
using Acr.UserDialogs;

namespace Mais
{
    public class LoginPage : ContentPage
    {
        readonly LoginViewModel model;

        public LoginPage()
        {
            this.BindingContext = model = App.Container.Resolve<LoginViewModel>();
            model.ConfiguraNavegacao(this.Navigation);

            var imgLogoMini = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("logo.png"))
            };

            var stackLogo = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { imgLogoMini }
            };

            var entEmail = new Entry
            {
                Placeholder = AppResources.TextoCampoLoginFormLogin,
                Keyboard = Keyboard.Email
            };
            entEmail.SetBinding<LoginViewModel>(Entry.TextProperty, x => x.login);

            var entSenha = new Entry
            {
                Placeholder = AppResources.TextoCampoSenhaFormLogin,
                IsPassword = true
            };
            entSenha.SetBinding<LoginViewModel>(Entry.TextProperty, x => x.senha);

            var btnEntrar = new Button
            {
                Style = Estilos._estiloPadraoButton,
                Text = AppResources.TextoBotaoOKLogin
            };
            btnEntrar.SetBinding(Button.CommandProperty, "btnEntrar_Click");

            var entryStack = new StackLayout
            {
                Children = { entEmail, entSenha }
            };

            var controlStack = new StackLayout
            {
                Spacing = 140,
                Children = { stackLogo, entryStack }
            };

            var stackBtnEntrar = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = { btnEntrar }
            };

            var btnEsqueciMinhaSenha = new Button
            {
                Text = AppResources.TextoBotaoEsqueciMinhaSenha,
                VerticalOptions = LayoutOptions.Center
            };
            btnEsqueciMinhaSenha.Clicked += async (sender, e) =>
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

            var mainLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Padding = 50,
                Spacing = 5,
                Children =
                {
                    controlStack,
                    stackBtnEntrar,
                    btnEsqueciMinhaSenha
                }
            };

            this.Content = new ScrollView
            {
                Content = mainLayout
            };
        }
    }
}


