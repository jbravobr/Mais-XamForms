﻿using System;
using System.Linq;

using Xamarin.Forms;
using XLabs.Forms.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autofac;

namespace Mais
{
    public class CadastroEdicaoPage : ContentPage
    {
        CadastroViewModel model;
        PickerSexoMais sexoPicker;
        Button btnCategorias;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            model.ConfiguraNavigation(this.Navigation);

            if (Device.OS == TargetPlatform.Android)
            {
                sexoPicker.Items.Add("Selecione uma opção");
                sexoPicker.Items.Add(AppResources.TextoSexoMasculino);
                sexoPicker.Items.Add(AppResources.TextoSexoFeminino);
            }

            model = App.Container.Resolve<CadastroViewModel>();
            this.BindingContext = model.Usuario = await model.RetornarUsuario();

            btnCategorias.Clicked += async (sender, e) =>
            {
                if (model != null && model.Usuario != null && model.Usuario.Categorias != null && model.Usuario.Categorias.Any())
                    await this.Navigation.PushModalAsync(new CategoriasPage(model.Usuario.Categorias));
                else
                    await this.Navigation.PushModalAsync(new CategoriasPage(null));
            };
            
            if (Device.OS == TargetPlatform.iOS && model.Usuario != null)
            {
                sexoPicker.Items.Add("Selecione uma opção");
                sexoPicker.Items.Add(AppResources.TextoSexoMasculino);
                sexoPicker.Items.Add(AppResources.TextoSexoFeminino);
            }
            else if (Device.OS == TargetPlatform.iOS && (model == null || model.Usuario == null))
            {
                sexoPicker.Focused += (sender, e) =>
                {
                    ((PickerSexoMais)sender).Items.Add("Selecione uma opção");
                    ((PickerSexoMais)sender).Items.Add(AppResources.TextoSexoMasculino);
                    ((PickerSexoMais)sender).Items.Add(AppResources.TextoSexoFeminino);
                };
            }

            if (model.Usuario.Sexo == EnumSexo.Masculino)
                sexoPicker.SelectedIndex = 1;
            else
                sexoPicker.SelectedIndex = 2;
        }

        public CadastroEdicaoPage()
        {
            this.BindingContext = model = App.Container.Resolve<CadastroViewModel>();
            model.ConfiguraNavigation(this.Navigation);
            MessagingCenter.Subscribe<CategoriasPage,ICollection<Categoria>>(this, "gravarCategorias", (sender, arg) =>
                {
                    this.model.AdicionaCategoriasSelecionadas(arg);

                    if (arg.Count > 1)
                        this.btnCategorias.Text = String.Format("{0} Categoria selecionada", arg.Count);
                    else
                        this.btnCategorias.Text = String.Format("{0} Categorias selecionadas", arg.Count);
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
            
            sexoPicker.SetBinding<CadastroViewModel>(PickerSexoMais.SelectedIndexProperty, x => x.Usuario.Sexo, BindingMode.Default,
                new SexoValueConverter(), null);

            var nascimentoPicker = new DatePickerMais();
            nascimentoPicker.SetBinding(DatePickerMais.DateProperty, "DataNascimento");

            var entEmail = new Entry
            { 
                Placeholder = AppResources.TextoPlaceHolderEmailCadastro,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            entEmail.SetBinding(Entry.TextProperty, "Email");

            var entNome = new Entry
            { 
                Placeholder = AppResources.TextoPlaceHolderNomeCadastro,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            entNome.SetBinding(Entry.TextProperty, "Nome");

            var entDDD = new Entry
            { 
                Placeholder = AppResources.TextoPlaceHolderDDDCadastro,
            };
            entDDD.SetBinding(Entry.TextProperty, "DDD");

            var entTelefone = new Entry
            { 
                Placeholder = AppResources.TextoPlaceHolderTelefoneCadastro
            };
            entTelefone.SetBinding(Entry.TextProperty, "Telefone");

            var entMunicipio = new Entry
            { 
                Placeholder = AppResources.TextoPlaceHolderMunicipioCadastro,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            entMunicipio.SetBinding(Entry.TextProperty, "Municipio");

            btnCategorias = new Button
            { 
                Text = AppResources.TextoPlaceHolderCategoriasCadastro, 
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Style = Estilos._estiloPadraoButtonFonteMenor
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
                Text = "Atualizar",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start
            };
            btnCriar.Clicked += async (sender, e) =>
            {
                var result = await model.AtualizarCadastro(model.Usuario, this.Navigation);

                if (result)
                    this.Navigation.PushModalAsync(new MainPage());
            };

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
            //gridWrapButtons.Children.Add(btnVoltar, 0, 0);
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
    }
}

