using System;
using Xamarin.Forms;

namespace Mais
{
    public class ListaRespostasViewCell : ViewCell
    {
        Image imgResposta2;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            dynamic b = this.BindingContext;

            if (String.IsNullOrEmpty(b.Imagem))
                imgResposta2.IsVisible = false;
        }

        public ListaRespostasViewCell()
        {
            var imgResposta = new Image
            {
                HorizontalOptions = LayoutOptions.Start,
            };
            imgResposta.SetBinding<Resposta>(Image.SourceProperty, x => x.ImgSource);

            imgResposta2 = new Image();
            imgResposta2.HeightRequest = 100;
            imgResposta2.WidthRequest = 120;
            imgResposta2.SetBinding(Image.SourceProperty, new Binding("ImgRespostaSource"));
            imgResposta2.SetBinding(Image.ClassIdProperty, new Binding("Imagem"));

            var imgResposta_Click = new TapGestureRecognizer();
            imgResposta_Click.Tapped += async (sender, e) =>
            {
                var fileName = ((Image)sender).ClassId;
                var img = DependencyService.Get<ISaveAndLoadFile>().GetImage(fileName);
                MessagingCenter.Send<ListaRespostasViewCell,string>(this, "MostraImagem", img);
            };
            imgResposta2.GestureRecognizers.Add(imgResposta_Click);

            var lblTituloResposta = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Style = Estilos._estiloFonteCategorias,
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Start
            };
            lblTituloResposta.SetBinding<Resposta>(Label.TextProperty, x => x.TextoResposta);

            var layout = new StackLayout
            {
                Padding = new Thickness(5, 15, 5, 0),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { imgResposta, imgResposta2, lblTituloResposta }
            };
			
            this.View = layout;
        }
    }
}

