using System;
using System.Linq;

using Xamarin.Forms;
using Autofac;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mais
{
    public class EnquetePage : ContentPage
    {
        EnqueteViewModel model;
        readonly StackLayout mainLayout;
        bool OnApperaringLoaded = false;
        int tipo;
        List<Enquete> enquetesFiltradas;
        StackLayout stackBanner;

        protected async override void OnAppearing()
        {
            try
            {
                if (this.OnApperaringLoaded == true)
                    return;
				
                base.OnAppearing();
                this.OnApperaringLoaded = true;
				
                if (!Application.Current.Properties.ContainsKey("Pagina"))
                    Application.Current.Properties["Pagina"] = 1;
				
                model = App.Container.Resolve<EnqueteViewModel>();
                model.ConfigurarNavegacao(this.Navigation);
				
                if (this.enquetesFiltradas != null && this.enquetesFiltradas.Any())
                {
                    if (this.enquetesFiltradas.Any(c => c.Titulo == "Nenhum"))
                    {
                        model.Enquetes = null;
                    }
                    else
                        model.Enquetes = new System.Collections.ObjectModel.ObservableCollection<Enquete>(this.enquetesFiltradas);
                }
                else
                    model.Enquetes = await model.GetEnquetesPublicas();
				
                this.BindingContext = model.Enquetes;
                var enquetesLayout = new StackLayout();

                if (this.model.Enquetes == null)
                {
                    enquetesLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenHeight * 3,
                        Orientation = StackOrientation.Vertical,
                        Padding = new Thickness(5, 50, 5, 0),
                        Children = { new Label{ Text = "Nenhum resultado encontrado !", FontSize = 28, FontAttributes = FontAttributes.Bold, YAlign = TextAlignment.Center } }
                    };
                }
                else
                {
                    enquetesLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenHeight * 3,
                        Orientation = StackOrientation.Vertical,
                        Padding = new Thickness(5, 3, 5, 0)
                    };
				
                    var cat = string.Empty;
                    var enquetesAgrupadas = from e in this.model.Enquetes
                                                           group e by e.Categoria.Nome into g
                                                           select new
					{
						CategoriaId = g.Key,
						Enquetes = g.ToList()
					};
				
                    foreach (var enquete in enquetesAgrupadas)
                    {
                        foreach (var item in enquete.Enquetes)
                        {
                            if (String.IsNullOrEmpty(cat) || cat != item.Categoria.Nome)
                            {
                                cat = item.Categoria.Nome;
                                var lblCategoriaNome = new Label
                                {
                                    Text = item.Categoria.Nome,
                                    BackgroundColor = Color.FromHex("#DDDDDD"),
                                    YAlign = TextAlignment.Start,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    FontSize = 14
                                };
                                enquetesLayout.Children.Add(lblCategoriaNome);
                            }
				
                            if (item.Tipo == EnumTipoEnquete.Publica)
                            {
                                var frame = new EnquetePublicaView(item);
                                enquetesLayout.Children.Add(frame);
                            }
                            else if (item.Tipo == EnumTipoEnquete.Mensagem)
                            {
                                var frame = new EnqueteMensagemView(item);
                                enquetesLayout.Children.Add(frame);	
                            }
                        }
                    }
                }
                var scrollEnquetes = new ScrollView { Content = enquetesLayout, Orientation = ScrollOrientation.Vertical };
                				
                var tabbedMenu = new TabbedMenuView(this.Navigation);
                mainLayout.Children.Add(scrollEnquetes);
                mainLayout.Children.Add(tabbedMenu);
				
                if (tipo == 1)
                    await CarregaEnquetesPublicas();
				
                if (tipo == 2)
                    await CarregaEnquetesInteresse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EnquetePage(int tipo = 0, List<Enquete> enquetesFiltradas = null)
        {
            try
            {
                this.tipo = tipo;
				
                if (enquetesFiltradas != null && enquetesFiltradas.Any())
                    this.enquetesFiltradas = enquetesFiltradas;
				
                this.Title = "MAIS APP";
                this.BackgroundColor = Color.FromHex("#F7F7F7");
				
                this.ToolbarItems.Add(new ToolbarItem
                    {
                        Icon = "atualizar.png",
                        Command = new Command(async (obj) =>
                            {
                                if (Application.Current.Properties.ContainsKey("Pagina"))
                                {
                                    var pagina = Convert.ToInt32(Application.Current.Properties["Pagina"]);
				
                                    switch (pagina)
                                    {
                                        case 1:
                                            await CarregaEnquetesPublicas();
                                            break;
                                        case 2:
                                            await CarregaEnquetesInteresse();
                                            break;
                                    }
                                }
                                else
                                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Nada para atualizar!");
                            }) 
                    });
				
                MessagingCenter.Subscribe<TabbedMenuView>(this, "CarregaEnquetesPublicas", async (obj) =>
                    {
                        await CarregaEnquetesPublicas();
                    });
				
                MessagingCenter.Subscribe<TabbedMenuView>(this, "CarregaEnquetesDeSeuInteresse", async (obj) =>
                    {
                        await CarregaEnquetesInteresse();
                    });
				
                if (Device.OS == TargetPlatform.iOS)
                {
                    MessagingCenter.Subscribe<EnquetePublicaView,int>(this, "CarregarRespostas", async (sender, perguntaId) => await this.model.CarregarRespostas(perguntaId));
                    MessagingCenter.Subscribe<EnqueteMensagemView,int>(this, "CarregarMensagem", async (sender, enqueteId) => await this.model.CarregarMensagem(enqueteId));
                }
				
                mainLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };
				
                mainLayout.ChildAdded += (sender, e) =>
                {
                    if (e.Element.GetType() == typeof(ScrollView))
                    {
                        var view = (ScrollView)e.Element;
                        view.FadeTo(1, 750, Easing.Linear);
                    }
                };
				
                this.Content = mainLayout;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task CarregaEnquetesPublicas()
        {
            try
            {
                Application.Current.Properties["Pagina"] = 1;
                mainLayout.Children.RemoveAt(0);
                mainLayout.Children[0].IsVisible = false;
				
                var ldg = Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.MsgLoading);
                ldg.Show();
				
                model.Enquetes = await model.GetEnquetesPublicas();
                this.BindingContext = model.Enquetes;
				
                var enquetesLayout = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenHeight * 3,
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(5, 3, 5, 0)
                };
				
                var cat = string.Empty;
                var enquetesAgrupadas = from e in this.model.Enquetes.Distinct()
                                                    group e by e.Categoria.Nome into g
                                                    select new
					{
						CategoriaId = g.Key,
						Enquetes = g.ToList()
					};
				
                foreach (var enquete in enquetesAgrupadas)
                {
                    foreach (var item in enquete.Enquetes)
                    {
                        if (String.IsNullOrEmpty(cat) || cat != item.Categoria.Nome)
                        {
                            cat = item.Categoria.Nome;
                            var lblCategoriaNome = new Label
                            {
                                Text = item.Categoria.Nome,
                                BackgroundColor = Color.FromHex("#DDDDDD"),
                                YAlign = TextAlignment.Start,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                FontSize = 14
                            };
                            enquetesLayout.Children.Add(lblCategoriaNome);
                        }
				
                        if (item.Tipo == EnumTipoEnquete.Publica)
                        {
                            var frame = new EnquetePublicaView(item);
                            enquetesLayout.Children.Add(frame);
                        }
                        else if (item.Tipo == EnumTipoEnquete.Mensagem)
                        {
                            var frame = new EnqueteMensagemView(item);
                            enquetesLayout.Children.Add(frame);	
                        }
                    }
                }
				
                var scrollEnquetes = new ScrollView { Content = enquetesLayout, Orientation = ScrollOrientation.Vertical };
				
                mainLayout.Children.Insert(0, scrollEnquetes);
				
                foreach (var child in mainLayout.Children)
                {
                    child.IsVisible = true;
                }
				
                ldg.Hide();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task CarregaEnquetesInteresse()
        {
            Application.Current.Properties["Pagina"] = 2;
            mainLayout.Children.RemoveAt(0);
            mainLayout.Children[0].IsVisible = false;

            var ldg = Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.MsgLoading);
            ldg.Show();

            model.Enquetes = await model.GetEnquetesDeSeuInteresse();
            this.BindingContext = model.Enquetes;

            var enquetesLayout = new StackLayout
            {   
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenHeight * 3,
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(5, 3, 5, 0)
            };

            #region -- Inserção dos Banners --
            var banners = model.Banners;
            if (banners != null && banners.Any())
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    stackBanner = new StackLayout
                    { 
                        Children = { new EnqueteInteresseView(model.Banners[0]) },
                        Padding = new Thickness(0, 0, 0, 10)
                    };
                }
                else
                {
                    stackBanner = new StackLayout
                    { 
                        Children = { new EnqueteInteresseView(model.Banners[0]) },
                        Padding = new Thickness(0, 0, 0, 10),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    };
                }

                var oldIndex = 1;

                if (banners != null && banners.Any())
                    enquetesLayout.Children.Add(stackBanner);

                if (banners.Count > 1)
                {
                    Device.StartTimer(TimeSpan.FromSeconds(4), () =>
                        {
                            if (oldIndex < banners.Count)
                            {
                                if (model.Banners.Skip(oldIndex).Take(1).Any())
                                {
                                    var obj = new EnqueteInteresseView(model.Banners.Skip(oldIndex).Take(1).First());

                                    if (Device.OS == TargetPlatform.iOS)
                                        stackBanner.Children[0] = obj;
                                    else
                                    {
                                        stackBanner.Children.Clear();
                                        stackBanner.Children.Add(obj);
                                    }

                                    oldIndex++;

                                    if (oldIndex >= banners.Count)
                                        oldIndex = 0;
                                }
                            }

                            return true;
                        });
                }
            }
            #endregion -- Inserção dos banners ---

            var cat = string.Empty;

            foreach (var enquete in this.model.Enquetes)
            {
                if (enquete.Categoria == null && enquete.CategoriaId <= 0 && enquete.Tipo != EnumTipoEnquete.Mensagem)
                    enquete.Categoria = new Categoria{ Nome = "Suas Enquetes" };
            }

            var enquetesAgrupadas = from e in this.model.Enquetes
                                             group e by e.Categoria.Nome into g
                                             select new
				{
					CategoriaId = g.Key,
					Enquetes = g.ToList()
				};

            foreach (var enquete in enquetesAgrupadas)
            {
                foreach (var item in enquete.Enquetes)
                {
                    if (String.IsNullOrEmpty(cat) || cat != item.Categoria.Nome)
                    {
                        cat = item.Categoria.Nome;
                        var lblCategoriaNome = new Label
                        {
                            Text = item.Categoria.Nome,
                            BackgroundColor = Color.FromHex("#DDDDDD"),
                            AnchorY = 10,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            FontSize = 14,
                            FontAttributes = FontAttributes.Bold
                        };
                        enquetesLayout.Children.Add(lblCategoriaNome);
                    }

                    if (item.Tipo == EnumTipoEnquete.Interesse)
                    {
                        var frame = new EnquetePublicaView(item);
                        enquetesLayout.Children.Add(frame);
                    }
                    else if (item.Tipo == EnumTipoEnquete.Mensagem)
                    {
                        var frame = new EnqueteMensagemView(item);
                        enquetesLayout.Children.Add(frame);	
                    }
                }
            }

            /*foreach (var enquete in this.model.Enquetes)
			{
				if (enquete.Tipo == EnumTipoEnquete.Interesse)
				{
					var frame = new EnquetePublicaView(enquete);
					enquetesLayout.Children.Add(frame);
				}
				else if (enquete.Tipo == EnumTipoEnquete.Mensagem)
				{
					var frame = new EnqueteMensagemView(enquete);
					enquetesLayout.Children.Add(frame);	
				}
			}*/

            var scrollEnquetes = new ScrollView { Content = enquetesLayout, Orientation = ScrollOrientation.Vertical };

            mainLayout.Children.Insert(0, scrollEnquetes);

            foreach (var child in mainLayout.Children)
            {
                child.IsVisible = true;
            }

            ldg.Hide();
        }
    }
}


