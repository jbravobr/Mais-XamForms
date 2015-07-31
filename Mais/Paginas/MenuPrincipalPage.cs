using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Mais
{
    public class MenuPrincipalPage : ContentPage
    {
        ListView menus;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var control = new Repositorio<ControleSession>();
            var isLogado = (await control.RetornarTodos()).First();

            if (!isLogado.ViuTutorial)
            {
                if (Device.OS == TargetPlatform.Android)
                    await this.Navigation.PushModalAsync(new TutorialPage_Android());
                else
                    await this.Navigation.PushModalAsync(new TutorialPage_iOS());
            }
        }

        public MenuPrincipalPage()
        {
            List<MenuItem> data = new MenuListData();

            menus = new ListView();
            menus.ItemsSource = data;
            menus.VerticalOptions = LayoutOptions.FillAndExpand;
            menus.BackgroundColor = Color.Transparent;
            menus.SeparatorVisibility = SeparatorVisibility.None;
            menus.ItemTemplate = new DataTemplate(typeof(MenuViewCell));
            menus.ItemTapped += async (sender, e) => NavigateTo(e.Item as MenuItem);

            var mainLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10, 90, 10, 0),
                Children = { menus }
            };
            
            this.BackgroundColor = Colors._defaultColorFromHex;

            this.Content = mainLayout;
        }

        void NavigateTo(MenuItem menu)
        {
            var displayPage = (Page)Activator.CreateInstance(menu.TipoPagina);

            if (displayPage.GetType() == typeof(EnquetePublica))
                this.Navigation.PushModalAsync(new MainPage(new EnquetePage(1)));
            else if (displayPage.GetType() == typeof(EnqueteInteresse))
                this.Navigation.PushModalAsync(new MainPage(new EnquetePage(2)));
            else
                this.Navigation.PushModalAsync(new MainPage(displayPage));
        }
    }
}


