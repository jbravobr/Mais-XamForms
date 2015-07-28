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
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Carregando...");

            var service = App.Container.Resolve<ILogin>();

            if (App.Current.Properties.ContainsKey("UsuarioLogado"))
            {
                var _user = App.Current.Properties["UsuarioLogado"] as Usuario;

                var friends = await DependencyService.Get<IFacebook>().GetAmigos(_user.FacebookID);
                var dbAmigos = new Repositorio<Amigo>();
                    
                var tels = friends.SelectMany(x => x.Keys).ToList();
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


