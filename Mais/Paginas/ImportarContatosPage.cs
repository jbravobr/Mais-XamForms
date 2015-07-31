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

            await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync("Nesta tela você irá importar os seus amigos do Facebook que também usam o Mais e poderá criar enquetes entre vocês", "Informação", "OK", "Cancel");
        }

        public ImportarContatosPage()
        {
            this.BindingContext = model = new ImportarContatosViewModel();

            btnImportar = new Button
            {
                Text = AppResources.TextoBotaoImportarAmigos,
                Style = Estilos._estiloPadraoButtonFonteMenor,
            };
            btnImportar.IsEnabled = true;
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
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Carregando...");

            var service = App.Container.Resolve<ILogin>();

            var dbUsuario = new Repositorio<Usuario>();
            var _usuario = (await dbUsuario.RetornarTodos()).FirstOrDefault();

            if (_usuario != null)
            {
                var friends = await DependencyService.Get<IFacebook>().GetAmigos(_usuario.FacebookToken);
                var dbAmigos = new Repositorio<Amigo>();
                    
                var tels = friends.data.Select(x => x.id).ToList();
                var existemNoServer = await service.RetornarAmigos(tels);

                var amigos = new List<Amigo>();

                if (existemNoServer != null && existemNoServer.Any())
                {
                    foreach (var item in existemNoServer)
                    {
                        amigos.Add(new Amigo
                            {
                                Nome = item.Nome,
                                FacebookID = item.FacebookID,
                                UsuarioId = item.Id
                            });
                    }

                    dbAmigos.InserirTodos(amigos);
                }

                this.listViewContatos.ItemsSource = (await dbAmigos.RetornarTodos()).Distinct();

                var cellTemplate = new DataTemplate(typeof(TextCell));

                cellTemplate.SetBinding(TextCell.TextProperty, "Nome");
                this.listViewContatos.ItemTemplate = cellTemplate;

                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            else
                Acr.UserDialogs.UserDialogs.Instance.ShowError("Problemas com a autenticação");
        }
    }
}


