using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Mais
{
    public class MainPage : MasterDetailPage
    {
        public MainPage(List<Enquete> enquetesFiltradas = null)
        {
            var menuPage = new MenuPage();

            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = menuPage;

            Detail = new NavigationPage(new MenuPrincipalPage());

//			if (enquetesFiltradas != null && enquetesFiltradas.Any())
//				Detail = new NavigationPage(new EnquetePage(0, enquetesFiltradas));
//			else
//				Detail = new NavigationPage(new EnquetePage());
        }

        public MainPage(Page _page)
        {
            var menuPage = new MenuPage();

            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = menuPage;
            Detail = new NavigationPage(_page);
        }

        void NavigateTo(MenuItem menu)
        {
            var displayPage = (Page)Activator.CreateInstance(menu.TipoPagina);

            if (displayPage.GetType() == typeof(EnquetePublica))
                Detail = new NavigationPage(new EnquetePage(1));
            else if (displayPage.GetType() == typeof(EnqueteInteresse))
                Detail = new NavigationPage(new EnquetePage(2));
            else
                Detail = new NavigationPage(displayPage);

            IsPresented = false;
        }
    }
}


