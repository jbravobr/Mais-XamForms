using System;

using Xamarin.Forms;

namespace Mais
{
    public class CategoriasViewCell : ViewCell
    {
        Image imgCategoria;
        StackLayout layout;
        Label lblNomeCategoria;
        Image imgOk;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            dynamic b = this.BindingContext;

            if (!String.IsNullOrEmpty(b.Imagem))
            {
                var pathImg = DependencyService.Get<ISaveAndLoadFile>().GetImage(b.Imagem);
                imgCategoria.Source = ImageSource.FromFile(pathImg);

                layout = new StackLayout
                {
                    Padding = new Thickness(5, 5, 5, 5),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Children = { imgCategoria, lblNomeCategoria, imgOk }
                };
            }
            else
            {
                layout = new StackLayout
                {
                    Padding = new Thickness(5, 5, 5, 5),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Children = { lblNomeCategoria, imgOk }
                };
            }

            this.View = layout;

        }

        public CategoriasViewCell()
        {
            imgOk = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("ok.png")),
                HorizontalOptions = LayoutOptions.End,
            };
            imgOk.SetBinding(Image.ClassIdProperty, "Id");
            imgOk.SetBinding(Image.IsVisibleProperty, "Selecionada");

            imgCategoria = new Image
                { HorizontalOptions = LayoutOptions.Start };

            lblNomeCategoria = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Style = Estilos._estiloFonteCategorias,
                YAlign = TextAlignment.Center
            };
            lblNomeCategoria.SetBinding(Label.TextProperty, "Nome");
        }
    }
}


