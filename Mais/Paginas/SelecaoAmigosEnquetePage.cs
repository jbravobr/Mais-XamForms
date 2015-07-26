﻿using System;
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

            var screenWidth = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth;

            var imgCancel_Tapped = new TapGestureRecognizer();
            imgCancel_Tapped.Tapped += async (sender, e) =>
            {
                if (this.Content.IsVisible == true)
                {
                    //await this.LayoutTo(new Rectangle(0, 0, screenWidth * -1, screenHeight * -1), 750, Easing.CubicOut);
                    MessagingCenter.Send<SelecaoAmigosEnquetePage,ICollection<Amigo>>(this, "gravarAmigos", this.model.Amigos.Where(b => b.Selecionado).ToList());
                    await this.Navigation.PopModalAsync();
                }
            };

            var imgCancel = new Button
            {
                //Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("cancel.png")),
                Style = Estilos._estiloPadraoButtonFonteMenor,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Salvar"
            };
            imgCancel.GestureRecognizers.Add(imgCancel_Tapped);

            var wrapperImage = new StackLayout
            {
                Padding = new Thickness(10, 5, 10, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { imgCancel }
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

