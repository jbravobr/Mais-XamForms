using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;
using Xamarin;

namespace Mais
{
    public class CategoriasPage : ContentPage
    {
        CategoriaViewModel model;
        List<Categoria> categoriasSelecionada;

        protected async override void OnAppearing()
        {
            try
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
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            try
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
                
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }

            return base.OnBackButtonPressed();

        }

        public CategoriasPage(List<Categoria> categoriasSelecionada = null)
        {	
            try
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
                
                var imgCancel = new Button
                {
                    //Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("cancel.png")),
                    Style = Estilos._estiloPadraoButtonFonteMenor,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Text = "Salvar"
                };
                imgCancel.Clicked += async (object sender, EventArgs e) =>
                {
                    if (this.Content.IsVisible == true)
                    {
                        MessagingCenter.Send<CategoriasPage,ICollection<Categoria>>(this, "gravarCategorias", this.model.Categorias.Where(b => b.Selecionada).ToList());
                        await this.Navigation.PopModalAsync();
                    }
                };
                
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
                    Children = { headerWrap, categoriasListView }
                };
                
                Content = new ScrollView { Content = mainLayout };
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        private void TrataClique(object e)
        {
            try
            {
                var itemSelecionado = (Categoria)e;
                var item = model.Categorias.FirstOrDefault(c => c.Id == itemSelecionado.Id);
                
                if (item != null && !item.Selecionada)
                    model.Categorias.FirstOrDefault(c => c.Id == itemSelecionado.Id).Selecionada = true;
                else
                    model.Categorias.FirstOrDefault(c => c.Id == itemSelecionado.Id).Selecionada &= item == null || !item.Selecionada;
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }
    }
}


