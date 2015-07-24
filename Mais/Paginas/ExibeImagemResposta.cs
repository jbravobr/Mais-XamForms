using System;

using Xamarin.Forms;

namespace Mais
{
    public class ExibeImagemResposta : ContentPage
    {
        public ExibeImagemResposta(string Imagem)
        {
            Content = new StackLayout
            { 
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Black,
                Children =
                {
                    new Image { Source = ImageSource.FromFile(Imagem), Aspect = Aspect.AspectFill }
                },
                Padding = new Thickness(5, 35, 5, 0)
            };
        }
    }
}


