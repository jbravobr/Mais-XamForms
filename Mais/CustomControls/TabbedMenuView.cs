using System;

using Xamarin.Forms;

namespace Mais
{
    public class TabbedMenuView : ContentView
    {
        private INavigation nav { get; set; }

        public TabbedMenuView(INavigation navigation)
        {
            this.nav = navigation;

            #region botão Enquetes de Públicas
            var irParaEnquetesPublicas = new TapGestureRecognizer();
            irParaEnquetesPublicas.Tapped += (sender, e) => MessagingCenter.Send<TabbedMenuView>(this, "CarregaEnquetesPublicas");

            var buttonLeft = new StackLayout
            {
                WidthRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth / 2,
                HeightRequest = 70,
                BackgroundColor = Colors._defaultColorFromHex,
                Children =
                { 
                    new Label
                    {
                        Text = AppResources.TituloTabEnquetePublica, 
                        TextColor = Color.White,
                        FontSize = 18,
                        FontFamily = Device.OnPlatform(
                            iOS: "Helvetica",
                            Android: "Roboto",
                            WinPhone: "Segoe"),
                        FontAttributes = FontAttributes.Bold,
                        LineBreakMode = LineBreakMode.WordWrap,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        XAlign = TextAlignment.Center,
                        YAlign = TextAlignment.Center,
                    }
                }
            };
            buttonLeft.GestureRecognizers.Add(irParaEnquetesPublicas);
            #endregion

            #region Enquetes de Interesse
            var irParaEnquetesDeSeuInteresse = new TapGestureRecognizer();
            irParaEnquetesDeSeuInteresse.Tapped += (sender, e) => MessagingCenter.Send<TabbedMenuView>(this, "CarregaEnquetesDeSeuInteresse");

            var buttonRight = new StackLayout
            {
                WidthRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth / 2,
                HeightRequest = 70,
                BackgroundColor = Colors._defaultColorDarkerFromHex,
                Children =
                { 
                    new Label
                    {
                        Text = AppResources.TituloTabEnqueteInteresse, 
                        TextColor = Color.White,
                        FontSize = 18,
                        FontFamily = Device.OnPlatform(
                            iOS: "Helvetica",
                            Android: "Roboto",
                            WinPhone: "Segoe"),
                        LineBreakMode = LineBreakMode.WordWrap,
                        FontAttributes = FontAttributes.Bold,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        XAlign = TextAlignment.Center,
                        YAlign = TextAlignment.Center
                    }
                }
            };
            buttonRight.GestureRecognizers.Add(irParaEnquetesDeSeuInteresse);
            #endregion

            var mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { buttonLeft, buttonRight },
                VerticalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.FromHex("#F7F7F7"),
                MinimumHeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenHeight,
                Padding = new Thickness(0, Device.OnPlatform(0, 10, 0), 0, 0)
            };

            this.Content = mainLayout;
        }
    }
}


