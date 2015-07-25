using System;

using Xamarin.Forms;
using System.Linq;

namespace Mais
{
    public class EnqueteMensagemView : ContentView
    {
        EnquetePublicaViewModel model;

        public EnqueteMensagemView(Enquete enquete)
        {
            this.BindingContext = enquete;
            this.model = new EnquetePublicaViewModel(this.Navigation);

//            var imgQuote = new Image
//            {
//                HorizontalOptions = LayoutOptions.StartAndExpand,
//                IsVisible = true,
//                Aspect = Aspect.Fill,
//                InputTransparent = true
//            };
//            imgQuote.SetBinding(Image.SourceProperty, "ImageSource");
//            
//            var imgSetaDireita = new Image
//            {
//                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("setaDireita.png")),
//                InputTransparent = true
//            };

            var lblTitulo = new Label
            {
                Style = Estilos._estiloFonteEnquete,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                YAlign = TextAlignment.Start,
                InputTransparent = true
            };
            lblTitulo.SetBinding(Label.TextProperty, "Titulo", BindingMode.Default,
                new TituloCompridoConverter(), null);

            var contanierLayout = new AbsoluteLayout
            {
                //WidthRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth - 50,
                InputTransparent = false,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            var overlay = new Image
            {
                /*BackgroundColor = Colors._defaultColorFromHex,*/ 
                InputTransparent = false,
                Aspect = Aspect.AspectFit
            };
            overlay.SetBinding(Image.SourceProperty, "ImageSource");

            var cView = new ContentView
            {
                Content = overlay,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth,
                InputTransparent = false
            };

//            AbsoluteLayout.SetLayoutFlags(cView, AbsoluteLayoutFlags.PositionProportional);
//            AbsoluteLayout.SetLayoutBounds(cView, new Rectangle(1, 0, 1, 1));
//
//
//            AbsoluteLayout.SetLayoutFlags(imgQuote, AbsoluteLayoutFlags.PositionProportional);
//            AbsoluteLayout.SetLayoutBounds(imgQuote, new Rectangle(0, 0.45, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
//
//            AbsoluteLayout.SetLayoutFlags(lblTitulo, AbsoluteLayoutFlags.PositionProportional);
//            AbsoluteLayout.SetLayoutBounds(lblTitulo, new Rectangle(0.50, 0.35, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
//
//            contanierLayout.Children.Add(cView);
//            contanierLayout.Children.Add(imgQuote);

            var frame_Click_Android = new TapGestureRecognizer();
            var frame_Click_iOS = new TapGestureRecognizer();

            if (Device.OS == TargetPlatform.iOS)
                frame_Click_iOS.Tapped += (sender, e) => MessagingCenter.Send<EnqueteMensagemView,int>(this, "CarregarMensagem", enquete.Id);
            else
                frame_Click_Android.Tapped += async (sender, e) => await this.model.CarregarMensagem(enquete.Id);

//            if (Device.OS == TargetPlatform.Android)
//            {
//                contanierLayout.GestureRecognizers.Add(frame_Click_Android);
//            }

//            var frame = new Frame
//            { 
//                OutlineColor = Color.Transparent,
//                BackgroundColor = Color.Red,
//                Content = cView,
//                //HeightRequest = 10,
//                HasShadow = false,
//                InputTransparent = true
//            };

//            if (Device.OS == TargetPlatform.Android)
//                frame.GestureRecognizers.Add(frame_Click_Android);
//            else
//                frame.GestureRecognizers.Add(frame_Click_iOS);

//            var frameSeta = new Frame
//            {
//                OutlineColor = Color.Transparent,
//                BackgroundColor = Color.FromHex("#1e3d63"),
//                HasShadow = false,
//                HeightRequest = 10,
//                WidthRequest = 0.01,
//                InputTransparent = true
//            };

//            if (Device.OS == TargetPlatform.Android)
//                frameSeta.GestureRecognizers.Add(frame_Click_Android);
//            else
//                frameSeta.GestureRecognizers.Add(frame_Click_iOS);

            var absLayout = new AbsoluteLayout();

            AbsoluteLayout.SetLayoutFlags(cView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(cView, new Rectangle(1, 0, 1, 1));

//            AbsoluteLayout.SetLayoutFlags(frameSeta, AbsoluteLayoutFlags.PositionProportional);
//            AbsoluteLayout.SetLayoutBounds(frameSeta, new Rectangle(1, 1, AbsoluteLayout.AutoSize, 1));

            AbsoluteLayout.SetLayoutFlags(lblTitulo, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lblTitulo, new Rectangle(0.50, 0.35, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

//            AbsoluteLayout.SetLayoutFlags(imgSetaDireita, AbsoluteLayoutFlags.PositionProportional);
//            AbsoluteLayout.SetLayoutBounds(imgSetaDireita, new Rectangle(0.97, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            absLayout.Children.Add(cView);
            absLayout.Children.Add(lblTitulo);
            //absLayout.Children.Add(frameSeta);
            //absLayout.Children.Add(imgSetaDireita);
            absLayout.InputTransparent = true;

            if (Device.OS == TargetPlatform.iOS)
                absLayout.GestureRecognizers.Add(frame_Click_iOS);
            else
                absLayout.GestureRecognizers.Add(frame_Click_Android);

            Content = absLayout;
        }
    }
}


