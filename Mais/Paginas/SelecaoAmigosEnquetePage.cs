using System;
using System.Linq;

using Xamarin.Forms;
using System.Collections.Generic;

namespace Mais
{
    public class SelecaoAmigosEnquetePage : ContentPage
    {
        SelecaoAmigosViewModel model;
        ListView amigosListView;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            model = new SelecaoAmigosViewModel();
            await model.RetornarAmigos();

            this.BindingContext = model;
            amigosListView.SetBinding(ListView.ItemsSourceProperty, "Amigos");
        }

        public SelecaoAmigosEnquetePage()
        {
            amigosListView = new ListView();
            amigosListView.ItemTemplate = new DataTemplate(typeof(AmigosViewCell));
            amigosListView.ItemTapped += (sender, e) =>
            {
                this.TrataClique(e.Item);
                ((ListView)sender).SelectedItem = null; 
            };

            var screenWidth = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth;

            var imgCancel = new Button
            {
                Style = Estilos._estiloPadraoButtonFonteMenor,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Salvar"
            };
            imgCancel.Clicked += async (sender, e) =>
            {
                if (this.Content.IsVisible == true)
                {
                    MessagingCenter.Send<SelecaoAmigosEnquetePage,ICollection<Amigo>>(this, "gravarAmigos", this.model.Amigos.Where(b => b.Selecionado).ToList());
                    await this.Navigation.PopModalAsync();
                }
            };

            var btnSelTodos = new Button
            {
                Style = Estilos._estiloPadraoButtonFonteMenor,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Text = "Sel. Todos" 
            };
            btnSelTodos.Clicked += (sender, e) =>
            {
                foreach (var amigo in this.model.Amigos)
                {
                    amigo.Selecionado = true;
                }
            };

            var wrapperImage = new StackLayout
            {
                Padding = new Thickness(10, 5, 10, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { imgCancel, btnSelTodos }
            };

            var headerWrap = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = screenWidth - 1,
                HeightRequest = 60,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { wrapperImage }
            };

            var mainLayout = new StackLayout
            {
                Padding = 20,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { headerWrap, amigosListView }
            };

            Content = new ScrollView { Content = mainLayout };
        }

        private void TrataClique(object e)
        {
            var itemSelecionado = (Amigo)e;
            var item = model.Amigos.FirstOrDefault(c => c.Id == itemSelecionado.Id);

            if (item != null && !item.Selecionado)
                model.Amigos.FirstOrDefault(c => c.Id == itemSelecionado.Id).Selecionado = true;
            else
                model.Amigos.FirstOrDefault(c => c.Id == itemSelecionado.Id).Selecionado &= item == null || !item.Selecionado;
        }
    }
}


