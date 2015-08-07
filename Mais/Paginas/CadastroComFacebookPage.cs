using System;
using System.Linq;

using Xamarin.Forms;
using XLabs.Forms.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autofac;
using Xamarin;
using System.Threading.Tasks;

namespace Mais
{
    public class CadastroComFacebookPage : ContentPage
    {
        readonly CadastroViewModel model;
        PickerSexoMais sexoPicker;
        string nome;
        Entry entNome;
        Button btnCategorias;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    sexoPicker.Items.Add("Selecione uma opção");
                    sexoPicker.Items.Add(AppResources.TextoSexoMasculino);
                    sexoPicker.Items.Add(AppResources.TextoSexoFeminino);
                }
                
                if (Device.OS == TargetPlatform.iOS)
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Atualizando informações ...");

                    if (!String.IsNullOrEmpty(App.FacebookAccessToken) && !String.IsNullOrEmpty(App.FacebookUserID))
                    {
                        var db = new Repositorio<FacebookInfos>();
                        await db.Inserir(new FacebookInfos { access_token = App.FacebookAccessToken, user_id = App.FacebookUserID });
                        var dados = await DependencyService.Get<IFacebook>().RecuperaDadosUsuario(App.FacebookAccessToken);
                
                        if (dados != null && dados.Any())
                        {
                            var usuarioNome = dados.First(k => k.Key == "name").Value.ToString();
//                            Device.BeginInvokeOnMainThread(() =>
//                                {
//                                    this.entNome.Text = usuarioNome;
//                                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
//                                });

                            var result = await this.FazCadastro(usuarioNome);

                            if (result)
                                await this.Navigation.PushModalAsync(new CategoriasPosCadastroFBPage());
                            else
                            {
                                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                                Acr.UserDialogs.UserDialogs.Instance.ShowError("Erro ao cadastrar", 2);
                            }
                        }
                    }       
                }
                
                MessagingCenter.Subscribe<CadastroComFacebookPage>(this, "CadastrouComFacebook", async (obj) =>
                    {
                        Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Atualizando informações ...");
                
                        if (!String.IsNullOrEmpty(App.FacebookAccessToken) && !String.IsNullOrEmpty(App.FacebookUserID))
                        {
                            var db = new Repositorio<FacebookInfos>();
                            await db.Inserir(new FacebookInfos { access_token = App.FacebookAccessToken, user_id = App.FacebookUserID });
                            var dados = await DependencyService.Get<IFacebook>().RecuperaDadosUsuario(App.FacebookAccessToken);

                            if (dados != null && dados.Any())
                            {
                                var usuarioNome = dados.First(k => k.Key == "name").Value.ToString();
                                //                            Device.BeginInvokeOnMainThread(() =>
                                //                                {
                                //                                    this.entNome.Text = usuarioNome;
                                //                                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                                //                                });

                                var result = await this.FazCadastro(usuarioNome);

                                if (result)
                                {
                                    var dbUsuario = new Repositorio<Usuario>();
                                    var _usuario = (await db.RetornarTodos()).FirstOrDefault();

                                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                                    if (_usuario != null)
                                        await this.Navigation.PushModalAsync(new MenuPrincipalPage());
                                    else
                                        await this.Navigation.PushModalAsync(new CategoriasPosCadastroFBPage());
                                }
                                else
                                {
                                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                                    Acr.UserDialogs.UserDialogs.Instance.ShowError("Erro ao cadastrar", 2);
                                }
                            }
                        }       
                    });
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }			
        }

        private async Task<bool> FazCadastro(string nome)
        {
            var srv = App.Container.Resolve<CadastroViewModel>();
            var result = await srv.FazerCadastro(nome);

            return result;
        }

        public CadastroComFacebookPage()
        {

           
            try
            {
                this.BindingContext = model = App.Container.Resolve<CadastroViewModel>();
                model.ConfiguraNavigation(this.Navigation);
                MessagingCenter.Subscribe<CategoriasPage,ICollection<Categoria>>(this, "gravarCategorias", (sender, arg) =>
                    {
                        this.model.AdicionaCategoriasSelecionadas(arg);
                        var _categorias = arg.Select(x => x.Nome).Aggregate((a, b) => a + ',' + b).TrimEnd(',');
                
                        if (_categorias.Length > 1)
                            this.btnCategorias.Text = String.Format("{0} Categorias selecionadas!", arg.Count);
                        else
                            this.btnCategorias.Text = String.Format("{0} Categoria selecionada!", arg.Count);
                    });
                
                var imgLogo = new Image
                {
                    Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("logo_mini.png")),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                
                sexoPicker = new PickerSexoMais();
                
                if (Device.OS == TargetPlatform.Android)
                    sexoPicker.Title = "Sexo";
                else if (Device.OS == TargetPlatform.iOS)
                {
                    sexoPicker.Focused += (sender, e) =>
                    {
                        ((PickerSexoMais)sender).Items.Add("Selecione uma opção");
                        ((PickerSexoMais)sender).Items.Add(AppResources.TextoSexoMasculino);
                        ((PickerSexoMais)sender).Items.Add(AppResources.TextoSexoFeminino);
                    };
                }
                sexoPicker.SetBinding<CadastroViewModel>(PickerSexoMais.SelectedIndexProperty, x => x.Usuario.Sexo, BindingMode.Default,
                    new SexoValueConverter(), null);
                
                var nascimentoPicker = new DatePickerMais();
                nascimentoPicker.SetBinding(DatePickerMais.DateProperty, "Usuario.DataNascimento");
                
                var entEmail = new Entry
                { 
                    Placeholder = AppResources.TextoPlaceHolderEmailCadastro,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                entEmail.SetBinding<CadastroViewModel>(Entry.TextProperty, x => x.Usuario.Email);
                
                entNome = new Entry
                { 
                    Placeholder = AppResources.TextoPlaceHolderNomeCadastro,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                entNome.SetBinding<CadastroViewModel>(Entry.TextProperty, x => x.Usuario.Nome);
                
                var entSenha = new Entry
                { 
                    Placeholder = AppResources.TextoPlaceHolderSenhaCadastro,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IsPassword = true
                };
                entSenha.SetBinding<CadastroViewModel>(Entry.TextProperty, x => x.Usuario.Senha);
                
                var entDDD = new Entry
                { 
                    Placeholder = AppResources.TextoPlaceHolderDDDCadastro,
                };
                entDDD.SetBinding<CadastroViewModel>(Entry.TextProperty, x => x.Usuario.DDD);
                
                var entTelefone = new Entry
                { 
                    Placeholder = AppResources.TextoPlaceHolderTelefoneCadastro
                };
                entTelefone.SetBinding<CadastroViewModel>(Entry.TextProperty, x => x.Usuario.Telefone);
                
                var entMunicipio = new Entry
                { 
                    Placeholder = AppResources.TextoPlaceHolderMunicipioCadastro,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                entMunicipio.SetBinding<CadastroViewModel>(Entry.TextProperty, x => x.Usuario.Municipio);
                
                btnCategorias = new Button
                { 
                    Text = AppResources.TextoPlaceHolderCategoriasCadastro, 
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Style = Estilos._estiloPadraoButtonFonteMenor
                };
                btnCategorias.Clicked += async (sender, e) =>
                {
                    if (model != null && model.Usuario != null && (model.Usuario.Categorias != null && model.Usuario.Categorias.Any() || !String.IsNullOrEmpty(model.Usuario.CategoriaMobileSelection)))
                    {
                        var dbCategorias = new Repositorio<Categoria>();
                        var categorias = new List<Categoria>();
                
                        foreach (var item in model.Usuario.CategoriaMobileSelection.Split(';'))
                        {
                            var catId = Convert.ToInt32(item);
                            var c = await dbCategorias.RetornarPorId(catId);
                            categorias.Add(c);
                        }
                
                        await this.Navigation.PushModalAsync(new CategoriasPage(categorias));
                    }
                    else
                        await this.Navigation.PushModalAsync(new CategoriasPage(null));
                };
                
                var gridWrapSexoNascimento = new Grid();
                gridWrapSexoNascimento.Children.Add(sexoPicker, 0, 0);
                gridWrapSexoNascimento.Children.Add(nascimentoPicker, 1, 0);
                
                var gridWrapDDDTelefone = new Grid();
                gridWrapDDDTelefone.Children.Add(entDDD, 0, 0);
                gridWrapDDDTelefone.Children.Add(entTelefone, 1, 0);
                
                var btnCriar = new Button
                {
                    Style = Estilos._estiloPadraoButton,
                    Text = AppResources.TextoBotaoCriarConta,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Start
                };
                btnCriar.SetBinding<CadastroViewModel>(Button.CommandProperty, x => x.btnCadastrar_Click);
                
                var btnVoltar = new Button
                {
                    Style = Estilos._estiloPadraoButton,
                    Text = AppResources.TextoBotaoVoltarTelaLogin,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start
                };
                btnVoltar.SetBinding<CadastroViewModel>(Button.CommandProperty, x => x.btnVoltar_Click);
                
                var gridWrapButtons = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };
                gridWrapButtons.Children.Add(btnVoltar, 0, 0);
                gridWrapButtons.Children.Add(btnCriar, 1, 0);
                
                var mainLayout = 
                    new StackLayout
                    {
                        Children =
                        { 
                            imgLogo,
                            gridWrapSexoNascimento,
                            btnCategorias,
                            entEmail,
                            entNome,
                            entSenha,
                            gridWrapDDDTelefone,
                            entMunicipio,
                            gridWrapButtons
                        },
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Spacing = 5,
                        Padding = new Thickness(5, 50, 5, 50)
                    };
                
                this.Content = new ScrollView { Content = mainLayout };
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }
    }
}


