using System;
using System.Linq;

using Xamarin.Forms;
using System.Collections.Generic;
using Contacts.Plugin;
using Contacts.Plugin.Abstractions;
using System.Threading.Tasks;
using Autofac;
using System.Collections.ObjectModel;

namespace Mais
{
    public class ImportarContatosPage : ContentPage
    {
        readonly ImportarContatosViewModel model;
        Button btnImportar;
        List<Contact> contatos;
        ListView listViewContatos;
        List<Amigo> amigosImportados;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var confirmConfigs = new Acr.UserDialogs.ConfirmConfig();
            confirmConfigs.OkText = "Entendi";
            confirmConfigs.Message = "Para importar amigos, você precisa ter informado no cadastro o seu número de celular corretamente.";

            var result = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(confirmConfigs);

            if (result)
            {
                if (await CrossContacts.Current.RequestPermission())
                {
                    contatos = null;
                    CrossContacts.Current.PreferContactAggregation = false;

                    if (CrossContacts.Current.Contacts == null)
                        return;

                    contatos = CrossContacts.Current.Contacts
                        .Where(c => !String.IsNullOrEmpty(c.FirstName) && c.Phones.Count > 0
                        && c.Phones.All(p => p.Type == PhoneType.Mobile))
				.ToList()
				.OrderBy(c => c.FirstName)
				.ToList();

                    if (contatos != null && contatos.Any())
                        this.btnImportar.IsEnabled = true;
                    else if (contatos == null)
                        Acr.UserDialogs.UserDialogs.Instance.ShowError("Erro ao ler contatos", 1);
                }
                else
                    Acr.UserDialogs.UserDialogs.Instance.ShowError("Erro ao interface de contatos", 1);
            }

            var dbAmigos = new Repositorio<Amigo>();
            amigosImportados = await dbAmigos.RetornarTodos();

            this.listViewContatos.ItemsSource = amigosImportados;
            var cellTemplate = new DataTemplate(typeof(TextCell));

            cellTemplate.SetBinding(TextCell.TextProperty, "Nome");
            this.listViewContatos.ItemTemplate = cellTemplate;
        }

        public ImportarContatosPage()
        {
            this.BindingContext = model = new ImportarContatosViewModel();

            btnImportar = new Button
            {
                Text = AppResources.TextoBotaoImportarAmigos,
                Style = Estilos._estiloPadraoButtonFonteMenor,
            };
            btnImportar.IsEnabled = false;
            btnImportar.Clicked += async (sender, e) => await TrataClique();

            listViewContatos = new ListView();
            listViewContatos.HorizontalOptions = LayoutOptions.CenterAndExpand;
            listViewContatos.VerticalOptions = LayoutOptions.StartAndExpand;
            listViewContatos.SeparatorVisibility = SeparatorVisibility.None;

            var mainLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 10,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 5, 5),
                Children = { btnImportar, listViewContatos }
            };

            this.Content = mainLayout;
        }

        public async Task TrataClique()
        {
            var service = App.Container.Resolve<ILogin>();
            var lista = await model.AdicionarAmigo(this.contatos);
            var dbAmigos = new Repositorio<Amigo>();
                       
            var tels = lista.Select(x => x.NroTelefone).ToList();
            var existemNoServer = await service.RetornarAmigos(tels);

            var amigos = new List<Amigo>();

            if (existemNoServer != null && existemNoServer.Any())
            {
                foreach (var item in existemNoServer)
                {
                    amigos.Add(new Amigo
                        {
                            Nome = item.Nome,
                            NroTelefone = String.Concat(item.DDD, item.Telefone),
                            UsuarioId = item.Id
                        });
                }

                dbAmigos.InserirTodos(amigos);
            }

            if ((await dbAmigos.RetornarTodos()).Any())
                this.listViewContatos.ItemsSource = (await dbAmigos.RetornarTodos()).Distinct();
            else
            {
                this.listViewContatos.ItemsSource = new ObservableCollection<Amigo>(CastContactsParaAmigo(this.contatos).Distinct().ToList());
            }

            var cellTemplate = new DataTemplate(typeof(TextCell));

            cellTemplate.SetBinding(TextCell.TextProperty, "Nome");
            cellTemplate.SetBinding(TextCell.DetailProperty, "NroTelefone");
            this.listViewContatos.ItemTemplate = cellTemplate;
        }

        private IEnumerable<Amigo> CastContactsParaAmigo(ICollection<Contact> contacts)
        {
            foreach (var item in contacts)
            {
                yield return new Amigo{ Nome = item.FirstName, NroTelefone = item.Phones.FirstOrDefault().Number };
            }
        }
    }
}


