using System;

using Xamarin.Forms;
using System.Linq;

namespace Mais
{
    public class LogoffPage : ContentPage
    {
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Saindo...");
            var dbControlSession = new Repositorio<ControleSession>();
            var isLogado = (await dbControlSession.RetornarTodos()).FirstOrDefault();

            if (isLogado != null)
            {
                isLogado.Logado = false;
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                this.Navigation.PushModalAsync(new Login_v2Page());
            }

        }

        public LogoffPage()
        {
            Content = new StackLayout
            { 
                Children =
                {
                    new Label { }
                }
            };
        }
    }
}


