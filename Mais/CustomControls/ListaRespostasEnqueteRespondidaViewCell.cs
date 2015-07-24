using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mais
{
    public class ListaRespostasEnqueteRespondidaViewCell : ViewCell
    {
        Frame box;
        Label lblPercentualResposta;

        protected override void OnBindingContextChanged()
        {
            try
            {
                base.OnBindingContextChanged();
                
                dynamic resposta = BindingContext;
                
                if (resposta.percentualResposta == 0)
                    box.WidthRequest = -35;
                else if (resposta.percentualResposta > 0 && resposta.percentualResposta < 20)
                    box.WidthRequest = 20;
                else if (resposta.percentualResposta > 20 && resposta.percentualResposta < 40)
                    box.WidthRequest = 20;
                else if (resposta.percentualResposta > 40 && resposta.percentualResposta < 60)
                    box.WidthRequest = 30;
                else if (resposta.percentualResposta > 60 && resposta.percentualResposta < 80)
                    box.WidthRequest = 40;
                else if (resposta.percentualResposta > 80 && resposta.percentualResposta < 99)
                    box.WidthRequest = 50;
                else
                    box.WidthRequest = 55;
                
                lblPercentualResposta.SetBinding(Label.TextProperty, new Binding("percent"){ StringFormat = "{0:P}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListaRespostasEnqueteRespondidaViewCell()
        {
            try
            {
                box = new Frame
                {
                    HorizontalOptions = LayoutOptions.Start,
                    BackgroundColor = Colors._defaultColorFromHex,
                    HeightRequest = -40
                };

                if (Device.OS == TargetPlatform.iOS)
                    box.HasShadow = false;
                
                lblPercentualResposta = new Label
                {
                    VerticalOptions = LayoutOptions.Start,
                    FontSize = 13
                };
                
                var lblTituloResposta = new Label
                {
                    Style = Estilos._estiloFonteCategorias,
                };
                lblTituloResposta.SetBinding(Label.TextProperty, new Binding("TextoResposta"));

                var imgResposta = new Image();
                imgResposta.HeightRequest = 100;
                imgResposta.WidthRequest = 120;
                imgResposta.SetBinding(Image.SourceProperty, new Binding("ImgRespostaSource"));
                imgResposta.SetBinding(Image.ClassIdProperty, new Binding("Imagem"));
                               
                var imgResposta_Click = new TapGestureRecognizer();
                imgResposta_Click.Tapped += (sender, e) =>
                {
                    var fileName = ((Image)sender).ClassId;
                    var img = DependencyService.Get<ISaveAndLoadFile>().GetImage(fileName);
                    MessagingCenter.Send<ListaRespostasEnqueteRespondidaViewCell,string>(this, "MostraImagem", img);
                };
                imgResposta.GestureRecognizers.Add(imgResposta_Click);
                
                var gridPercentual = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Auto) },
                        new ColumnDefinition{ Width = new GridLength(20, GridUnitType.Auto) }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(25) },
                        new RowDefinition { Height = new GridLength(30) },

                    }
                };
                gridPercentual.Children.Add(lblTituloResposta, 0, 0);
                gridPercentual.Children.Add(box, 0, 1);
                gridPercentual.Children.Add(lblPercentualResposta, 1, 1);
                gridPercentual.Children.Add(imgResposta, 2, 1);

                
                this.View = gridPercentual;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

