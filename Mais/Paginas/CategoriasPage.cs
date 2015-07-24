﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;

namespace Mais
{
	public class CategoriasPage : ContentPage
	{
		CategoriaViewModel model;
		List<Categoria> categoriasSelecionada;

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			this.BindingContext = model = new CategoriaViewModel();

			if (categoriasSelecionada != null && categoriasSelecionada.Any())
			{
				model.Categorias = new System.Collections.ObjectModel.ObservableCollection<Categoria>(await model.GetCategorias());

				foreach (var categoria in model.Categorias)
				{
					if (categoriasSelecionada.Any(c => c.Id == categoria.Id))
						categoria.Selecionada = true;
				}
			}
		}

		protected override bool OnBackButtonPressed()
		{
			var _clickBackButton = new Action(async () =>
				{
					//await this.LayoutTo(new Rectangle(0, 0, screenWidth * -1, screenHeight * -1), 750, Easing.CubicOut);
					MessagingCenter.Send<CategoriasPage,ICollection<Categoria>>(this, "gravarCategorias", this.model.Categorias.Where(b => b.Selecionada).ToList());
					//await this.Navigation.PopModalAsync();
				});

			if (this.Content.IsVisible == true)
			{
				_clickBackButton.Invoke();
			}

			return base.OnBackButtonPressed();
		}

		public CategoriasPage(List<Categoria> categoriasSelecionada = null)
		{	
			this.categoriasSelecionada = categoriasSelecionada;

			var categoriasListView = new ListView();
			categoriasListView.SetBinding(ListView.ItemsSourceProperty, "Categorias");
			categoriasListView.ItemTemplate = new DataTemplate(typeof(CategoriasViewCell));
			categoriasListView.ItemTapped += (sender, e) =>
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
					MessagingCenter.Send<CategoriasPage,ICollection<Categoria>>(this, "gravarCategorias", this.model.Categorias.Where(b => b.Selecionada).ToList());
					await this.Navigation.PopModalAsync();
				}
			};

			var imgCancel = new Image
			{
				Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("cancel.png")),
				HorizontalOptions = LayoutOptions.StartAndExpand
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
				BackgroundColor = Colors._defaultColorFromHex,
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
				Children = { headerWrap, categoriasListView }
			};

			Content = new ScrollView { Content = mainLayout };
		}

		private void TrataClique(object e)
		{
			var itemSelecionado = (Categoria)e;
			var item = model.Categorias.FirstOrDefault(c => c.Id == itemSelecionado.Id);

			if (item != null && !item.Selecionada)
				model.Categorias.FirstOrDefault(c => c.Id == itemSelecionado.Id).Selecionada = true;
			else
				model.Categorias.FirstOrDefault(c => c.Id == itemSelecionado.Id).Selecionada &= item == null || !item.Selecionada;
		}
	}
}


