using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Xamarin;

namespace Mais
{
    public class BuscarPage : ContentPage
    {
        ICollection<Categoria> categoriasSelecionadas;
        Picker pickerTipoEnquete;
        Entry txtTitulo;

        public BuscarPage()
        {
            #region -- Controles --

            try
            {
                var btnCategorias = new Button
                { 
                    Text = AppResources.TextoPlaceHolderCategoriasCadastro, 
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Style = Estilos._estiloPadraoButtonFonteMenor,
                    WidthRequest = 290
                };
                btnCategorias.Clicked += async (sender, e) =>
                {
                    if (this.categoriasSelecionadas != null && this.categoriasSelecionadas.Any())
                        await this.Navigation.PushModalAsync(new CategoriasPage(this.categoriasSelecionadas.ToList()));
                    else
                        await this.Navigation.PushModalAsync(new CategoriasPage(null));
                };
                
                pickerTipoEnquete = new Picker();
                pickerTipoEnquete.Items.Add("Selecione uma opção");
                pickerTipoEnquete.Items.Add("Enquetes abertas");
                pickerTipoEnquete.Items.Add("Enquetes encerradas");
                pickerTipoEnquete.Items.Add("Enquetes em que votei");
                pickerTipoEnquete.Items.Add("Enquetes em que não votei");
                pickerTipoEnquete.VerticalOptions = LayoutOptions.FillAndExpand;
                pickerTipoEnquete.Title = "Tipo de Enquete";
                pickerTipoEnquete.WidthRequest = 290;
                
                txtTitulo = new Entry();
                txtTitulo.Placeholder = "Buscar por Título";
                txtTitulo.WidthRequest = 290;
                
                var btnBuscar = new Button();
                btnBuscar.Style = Estilos._estiloPadraoButtonFonteMenor;
                btnBuscar.Text = "Buscar";
                btnBuscar.HorizontalOptions = LayoutOptions.Fill;
                btnBuscar.Clicked += async (sender, e) =>
                {
                    try
                    {
                        if (Device.OS == TargetPlatform.iOS)
                        {
                            await TrataBusca();
                        }
                        else
                        {
                            await TrataBusca();//await TrataBuscaAndroid();
                        }
                    }
                    catch (Exception ex)
                    {
                        Insights.Report(ex);
                    }
                };
                
                var btnLimpar = new Button();
                btnLimpar.Style = Estilos._estiloPadraoButtonFonteMenor;
                btnLimpar.Text = "Limpar";
                btnLimpar.HorizontalOptions = LayoutOptions.Fill;
                btnLimpar.Clicked += (sender, e) =>
                {
                    txtTitulo.Text = String.Empty;
                    txtTitulo.Placeholder = "Buscar por Título";
                
                    pickerTipoEnquete.SelectedIndex = 0;
                    this.categoriasSelecionadas = null;
                };
                
                var gridButtons = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
                    }
                };
                gridButtons.Children.Add(btnLimpar, 0, 0);
                gridButtons.Children.Add(btnBuscar, 1, 0);
                #endregion -- Controles--
                
                MessagingCenter.Subscribe<CategoriasPage,ICollection<Categoria>>(this, "gravarCategorias", (sender, e) =>
                    {
                        AdicionaCategoriasAoFiltro(e);
                    });
                
                var abslayout = new AbsoluteLayout { WidthRequest = 200 };
                
                AbsoluteLayout.SetLayoutFlags(btnCategorias, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(btnCategorias, new Rectangle(0.45, 0.15, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                
                AbsoluteLayout.SetLayoutFlags(pickerTipoEnquete, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(pickerTipoEnquete, new Rectangle(0.45, 0.25, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                
                AbsoluteLayout.SetLayoutFlags(txtTitulo, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(txtTitulo, new Rectangle(0.45, 0.35, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                
                AbsoluteLayout.SetLayoutFlags(gridButtons, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(gridButtons, new Rectangle(0.45, 0.45, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                
                abslayout.Children.Add(btnCategorias);
                abslayout.Children.Add(pickerTipoEnquete);
                abslayout.Children.Add(txtTitulo);
                abslayout.Children.Add(gridButtons);
                abslayout.VerticalOptions = LayoutOptions.FillAndExpand;
                
                this.Content = abslayout;
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        private void AdicionaCategoriasAoFiltro(ICollection<Categoria> categorias)
        {
            this.categoriasSelecionadas = categorias;
        }

        private async Task TrataBusca()
        {
            try
            {
                var dbEnquete = new Repositorio<Enquete>();
                List<Enquete> query = (await dbEnquete.RetornarTodos());
                var controle = false;

                var _toList = query;

                if (categoriasSelecionadas != null && categoriasSelecionadas.Any())
                {
                    var l = new List<Enquete>();
                    foreach (var cat in categoriasSelecionadas)
                    {
                        if (_toList.Any(x => x.CategoriaId == cat.Id))
                        {
                            l.AddRange(query.Where(x => x.CategoriaId == cat.Id));
                            controle = true;
                        }
                    }

                    if (l.Any())
                        _toList = l;
                }

                if (categoriasSelecionadas != null && categoriasSelecionadas.Any() && !controle)
                {
                    _toList = new List<Enquete>{ new Enquete{ Titulo = "Nenhum" } };
                    await this.Navigation.PushModalAsync(new MainPage());
                    return;
                }
                
                if (pickerTipoEnquete.SelectedIndex != -1)
                {
                    switch (pickerTipoEnquete.SelectedIndex)
                    {
                        case 1:
                            _toList = _toList.Where(x => x.Status == EnumStatusEnquete.Publicada).ToList();
                            controle = true;
                            break;
                        case 2:
                            _toList = _toList.Where(x => x.Status == EnumStatusEnquete.Inativa).ToList();
                            controle = true;
                            break;
                        case 3:
                            _toList = (_toList.Where(x => x.Pergunta != null && x.Pergunta.Respostas != null &&
                                x.Pergunta.Respostas.Any() && x.Pergunta.Respostas.Any(c => c.Respondida))).ToList();
                            controle = true;
                            break;
                        case 4:
                            _toList = (_toList.Where(x => x.Pergunta != null && x.Pergunta.Respostas != null &&
                                x.Pergunta.Respostas.Any() && x.Pergunta.Respostas.All(c => !c.Respondida))).ToList();
                            controle = true;
                            break;
                    }
                }

                if (pickerTipoEnquete.SelectedIndex != -1 && !controle)
                {
                    _toList = new List<Enquete>{ new Enquete{ Titulo = "Nenhum" } };
                    await this.Navigation.PushModalAsync(new MainPage());
                    return;
                }

                if (!String.IsNullOrEmpty(txtTitulo.Text))
                {
                    _toList = (_toList
                        .Where(x => (x.Pergunta != null && x.Pergunta.TextoPergunta.ToLower().Contains(txtTitulo.Text.ToLower())) ||
                        (x.Pergunta == null && x.Titulo.ToLower().Contains(txtTitulo.Text.ToLower()))))
                        .ToList();
                    controle = true;
                }

                if (!String.IsNullOrEmpty(txtTitulo.Text) && !controle)
                {
                    _toList = new List<Enquete>{ new Enquete{ Titulo = "Nenhum" } };
                    await this.Navigation.PushModalAsync(new MainPage(_toList));
                    return;
                }

                if (_toList == null || !_toList.Any())
                {
                    _toList.Add(new Enquete{ Titulo = "Nenhum" });    
                }
                else
                    _toList.Distinct().ToList();
                
                await this.Navigation.PushModalAsync(new MainPage(_toList));
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        private async Task TrataBuscaAndroid()
        {
            var dbEnquete = new Repositorio<Enquete>();
            IEnumerable<Enquete> query = await dbEnquete.RetornarTodos();
            var controle = false;


            var _toList = query.ToList();
            if (categoriasSelecionadas != null && categoriasSelecionadas.Any())
            {
                foreach (var cat in categoriasSelecionadas)
                {
                    if (_toList.Any(x => x.CategoriaId == cat.Id))
                    {
                        _toList.AddRange(query.Where(x => x.CategoriaId == cat.Id));
                        controle = true;
                    }
                }
            }

            if (categoriasSelecionadas != null && categoriasSelecionadas.Any() && !controle)
            {
                _toList = new List<Enquete>{ new Enquete{ Titulo = "Nenhum" } };
                await this.Navigation.PushModalAsync(new MainPage(_toList));
                return;
            }

            if (pickerTipoEnquete.SelectedIndex != 0)
            {
                switch (pickerTipoEnquete.SelectedIndex)
                {
                    case 1:
                        _toList = _toList.Where(x => x.Status == EnumStatusEnquete.Publicada).ToList();
                        controle = true;
                        break;  
                    case 2:
                        _toList = (_toList.Where(x => x.Status == EnumStatusEnquete.Inativa)).ToList();
                        controle = true;
                        break;
                    case 3:
                        _toList = (_toList.Where(x => x.Pergunta != null && x.Pergunta.Respostas != null &&
                            x.Pergunta.Respostas.Any() && x.Pergunta.Respostas.Any(c => c.Respondida))).ToList();
                        controle = true;
                        break;
                    case 4:
                        _toList = (_toList.Where(x => x.Pergunta != null && x.Pergunta.Respostas != null &&
                            x.Pergunta.Respostas.Any() && x.Pergunta.Respostas.All(c => !c.Respondida))).ToList();
                        controle = true;
                        break;
                    default:
                        break;
                }
            }

            if (pickerTipoEnquete.SelectedIndex != 0 && !controle)
            {
                _toList = new List<Enquete>{ new Enquete{ Titulo = "Nenhum" } };
                await this.Navigation.PushModalAsync(new MainPage(_toList));
                return;
            }

            if (!String.IsNullOrEmpty(txtTitulo.Text))
            {
                _toList = (_toList
                    .Where(x => (x.Pergunta != null && x.Pergunta.TextoPergunta.ToLower().Contains(txtTitulo.Text.ToLower())) ||
                    (x.Pergunta == null && x.Titulo.ToLower().Contains(txtTitulo.Text.ToLower()))))
                    .ToList();
                controle = true;
            }

            if (!String.IsNullOrEmpty(txtTitulo.Text) && !controle)
            {
                _toList = new List<Enquete>{ new Enquete{ Titulo = "Nenhum" } };
                await this.Navigation.PushModalAsync(new MainPage(_toList));
                return;
            }

            if (_toList == null || !_toList.Any())
            {
                _toList.Add(new Enquete{ Titulo = "Nenhum" });    
            }
            else
                _toList.Distinct().ToList();


            await this.Navigation.PushModalAsync(new MainPage(_toList));
        }
    }
}


