using System;
using System.Linq;

using Xamarin.Forms;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Autofac;
using Geolocator.Plugin;
using Xamarin;

//using Geofence.Plugin.Abstractions;
//using Geofence.Plugin;

namespace Mais
{
    public class ColetaDadosPage : ContentPage
    {
        ColetaDadosViewModel model;

        protected async override void OnAppearing()
        {
            try
            {
                var progress = UserDialogs.Instance.Progress(AppResources.MsgLoading);
                progress.Show();
                
                #region -- FAKE DATABASE --
                //          var enquetes = EnqueteMock.MockEnquetes();
                //          var dbEnquete = new Repositorio<Enquete>();
                //          if (!(await dbEnquete.RetornarTodos()).Any())
                //              await dbEnquete.InserirTodos(enquetes);
                //
                //          var usuario = UsuarioMock.MockUsuario();
                //          var dbUsuario = new Repositorio<Usuario>();
                //          if (!(await dbUsuario.RetornarTodos()).Any())
                //              await dbUsuario.Inserir(usuario);
                //
                //          var amigos = AmigoMock.RetornaListaMockAmigo();
                //          var dbAmigo = new Repositorio<Amigo>();
                //          if (!(await dbAmigo.RetornarTodos()).Any())
                //              await dbAmigo.InserirTodos(amigos);
                #endregion
                
                #region -- Atualiza Categorias
                this.BindingContext = model = App.Container.Resolve<ColetaDadosViewModel>();
                
                var dbCategoria = new Repositorio<Categoria>();
                var ultimaCategoria = -1;
                
                if (await dbCategoria.ExisteCategoria())
                    ultimaCategoria = (await dbCategoria.RetornarTodos()).OrderByDescending(c => c.Id).First().Id;
                
                var categorias = await this.model.RetornarCategoriasDoServidor(ultimaCategoria);

                dbCategoria.InserirTodos(categorias.ToList());
                
                #endregion
                
                //Simula o carregamento de dados.
                Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                    {
                        if (progress.IsShowing && progress.PercentComplete < 100)
                            progress.PercentComplete = progress.PercentComplete + 10;
                        else if (progress.IsShowing && progress.PercentComplete >= 100)
                        {
                            progress.Hide();
                            IsLogado();
                        }
                
                        return true;
                    });
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        private async Task IsLogado()
        {
            try
            {
                var dbSession = new Repositorio<ControleSession>();
                var isLogado = await dbSession.RetornarTodos();
                
                if (isLogado != null && isLogado.Any(c => c.Logado))
                {
                    App.Current.Properties["isLogado"] = true;

                    await this.Navigation.PushModalAsync(new MainPage());
                }
                else
                {
                    App.Current.Properties["isLogado"] = false;
                    await this.Navigation.PushModalAsync(new RootPage(), false);
                }
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        public ColetaDadosPage()
        {
            var imgLogo = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("logo.png"))
            };

            Double Latitude;
            Double Longitude;
            Geofence.Plugin.Abstractions.GeofenceCircularRegion region;

            Latitude = -23.0063245;
            Longitude = -43.4303358;

            region = new Geofence.Plugin.Abstractions.GeofenceCircularRegion("Endereço da Natália", Latitude, Longitude, 100)
            {
                NotifyOnStay = true,
                NotifyOnEntry = true,
                NotifyOnExit = true,
                StayedInThresholdDuration = TimeSpan.FromMinutes(5),
                Persistent = true,
                ShowNotification = true,    
                NotificationEntryMessage = "Entrou na área marcada - PushWoosh",
                NotificationExitMessage = "Saiu da área marcada - PushWoosh",
                NotificationStayMessage = "Continua dentro da área marcada - PushWoosh"
            };

            Geofence.Plugin.CrossGeofence.Current.StartMonitoring(region);

            this.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = 30,
                Children = { imgLogo }
            };
        }
    }
}


