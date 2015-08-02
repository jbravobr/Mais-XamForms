using System;

using Xamarin.Forms;
using System.Linq;

namespace Mais
{
    public class TutorialPage_iOS : ContentPage
    {
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            base.OnAppearing();

            var dbControlaSession = new Repositorio<ControleSession>();
            var _control = (await dbControlaSession.RetornarTodos()).First();
            _control.ViuTutorial = true;

            await dbControlaSession.Atualizar(_control);

            Device.StartTimer(TimeSpan.FromSeconds(10), (() =>
                {                    
                    this.Navigation.PushModalAsync(new NavigationPage(new MenuPrincipalPage()));
                    return false;
                }

                ));
        }

        public TutorialPage_iOS()
        {
            var img = new Image
            { 
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("tutorial_ios.PNG")), 
                Aspect = Aspect.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            var img_Click = new TapGestureRecognizer();
            img_Click.Tapped += async (sender, e) => await this.Navigation.PushModalAsync(new NavigationPage(new MenuPrincipalPage()));

            Content = new StackLayout
            { 
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { img }
            };
        }
    }
}


