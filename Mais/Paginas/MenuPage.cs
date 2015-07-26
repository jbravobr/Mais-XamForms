using System;
using Xamarin.Forms;

namespace Mais
{
    public class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public MenuPage()
        {
            //if (Device.OS == TargetPlatform.iOS)
            Icon = "settings.png";
			
            Title = "menu";
            BackgroundColor = Colors._defaultColorFromHex;

            Menu = new MenuListView();

            var menuLabel = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    TextColor = Color.White,
                    Text = "MENU", 
                }
            };

            var layout = new StackLayout
            { 
                Spacing = 10, 
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);

            Content = layout;
        }
    }
}
