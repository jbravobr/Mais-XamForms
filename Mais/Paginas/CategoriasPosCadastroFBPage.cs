using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;
using Xamarin;
using Autofac;

namespace Mais
{
    public class CategoriasPosCadastroFBPage : ContentPage
    {
        CategoriaViewModel model;
        List<Categoria> categoriasSelecionada;
        Entry txtEmail;

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
            return false;
        }

        public CategoriasPosCadastroFBPage(List<Categoria> categoriasSelecionada = null)
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
                
                var screenWidth = Acr.DeviceInfo.DeviceInfo.Hardware.ScreenWidth;
                
                var imgCancel_Tapped = new TapGestureRecognizer();
                imgCancel_Tapped.Tapped += async (sender, e) =>
                {
                    if (this.Content.IsVisible == true)
                    {
                                       
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
                        try
                        {
                            var model = App.Container.Resolve<CadastroViewModel>();
                            var result = await model.FazCadastroCategoriasFB(this.model.Categorias.Where(b => b.Selecionada).ToList(), this.txtEmail.Text);

                            if (result)
                            {
                                if (Device.OS == TargetPlatform.Android)
                                {
                                    var dbSession = new Repositorio<ControleSession>();
                                    var isLogado = new ControleSession{ Logado = true };

                                    await dbSession.InserirAsync(isLogado);

                                    await this.Navigation.PushModalAsync(new TutorialPage_Android());
                                }
                                else
                                {
                                    var dbSession = new Repositorio<ControleSession>();
                                    var isLogado = new ControleSession{ Logado = true };

                                    await dbSession.InserirAsync(isLogado);
                                    await this.Navigation.PushModalAsync(new TutorialPage_iOS());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Insights.Report(ex);
                        }        
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

                txtEmail = new Entry { Placeholder = "Informe o seu e-mail" };
                var lblEmail = new Label { Text = "Seu e-mail é importante para participar de enquetes premiadas", FontSize = 9 };
                
                var mainLayout = new StackLayout
                {
                    Padding = 20,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = { headerWrap, txtEmail, lblEmail, categoriasListView }
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


