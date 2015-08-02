using System;

using Xamarin.Forms;
using System.Linq;

namespace Mais
{
    public class TutorialPage_Android : ContentPage
    {

        protected async override void OnAppearing()
        {
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

        public TutorialPage_Android()
        {

            var img = new Image
            { 
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("tutorial_android.PNG")), 
                Aspect = Aspect.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            var img_Click = new TapGestureRecognizer();
            img_Click.Tapped += async (sender, e) => await this.Navigation.PushModalAsync(new NavigationPage(new MenuPrincipalPage()));

            img.GestureRecognizers.Add(img_Click);

            Content = new StackLayout
            { 
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { img }
            };
        }
    }
}


