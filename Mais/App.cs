using System;
using System.Globalization;

using Xamarin.Forms;
using Autofac;
using Geolocator.Plugin;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Xamarin;
using System.Linq;

namespace Mais
{
    public class App : Application
    {
        public static IContainer Container { get; set; }

        public static string FacebookAccessToken { get; set; }

        public static string FacebookUserID { get; set; }

        public static string PushWooshToken { get; set; }

        public static string UrlVideo { get; set; }

        static NavigationPage _NavPage;

        public static Page GetMainPage()
        {
            var profilePage = new CadastroComFacebookPage();

            _NavPage = new NavigationPage(profilePage);

            return profilePage;
        }

        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() =>
                    {
                        App.GetMainPage();
                        _NavPage.Navigation.PushModalAsync(new CadastroComFacebookPage());
                    });
            }
        }

        public App()
        {
            // Definindo linguagem do app.
            DependencyService.Get<ILocale>().SetLocale();

            // Definindo arquivo de Resource a ser utilizado.
            var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
            AppResources.Culture = new CultureInfo(netLanguage);

            // Inicializando Container para Injeção de Dependência.
            App.Container = AutofacConfiguration.Init();

            // Página inicial do App.
            MainPage = new ColetaDadosPage();
        }

        protected override void OnStart()
        {
            try
            {
                Action<Task> capturaPosicaoPeriodica = null;
                capturaPosicaoPeriodica = _task =>
                {
                    var locator = CrossGeolocator.Current;
                    var dbUsuario = new Repositorio<Usuario>();
                    var usuarioLogado = (dbUsuario.RetornarTodos()).Result.FirstOrDefault();
                
                    if (usuarioLogado != null)
                    {
                        if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                        {
                            locator.DesiredAccuracy = 50;
                            var pos = locator.GetPositionAsync(10000);
                            pos.Wait();
                
                            var geo = new Geolocalizacao
                            {
                                Latitude = pos.Result.Latitude,
                                Longitude = pos.Result.Longitude,
                                UsuarioId = usuarioLogado.Id,
                            };
                            var service = App.Container.Resolve<GeolocalizacaoViewModel>();
                            service.GravaGeolocalizacao(geo).Wait();
                
                            Task.Delay(10000, CancellationToken.None).ContinueWith(capturaPosicaoPeriodica, CancellationToken.None);
                            //Debug.WriteLine(String.Format("Posição é: LAT: {0} e LONG {1} - Data: {2}", geo.Latitude, geo.Longitude, DateTime.Now.ToString()));
                        }
                    }
                };
                
                Task.Delay(10000, CancellationToken.None).ContinueWith(capturaPosicaoPeriodica, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }

        }

        protected override void OnSleep()
        {
            try
            {
                Action<Task> capturaPosicaoPeriodica = null;
                capturaPosicaoPeriodica = _task =>
                {
                    var locator = CrossGeolocator.Current;
                    var dbUsuario = new Repositorio<Usuario>();
                    var usuarioLogado = (dbUsuario.RetornarTodos()).Result.FirstOrDefault();

                    if (usuarioLogado != null)
                    {
                        if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                        {
                            locator.DesiredAccuracy = 50;
                            var pos = locator.GetPositionAsync(10000);
                            pos.Wait();

                            var geo = new Geolocalizacao
                            {
                                Latitude = pos.Result.Latitude,
                                Longitude = pos.Result.Longitude,
                                UsuarioId = usuarioLogado.Id,
                            };
                            var service = App.Container.Resolve<GeolocalizacaoViewModel>();
                            service.GravaGeolocalizacao(geo).Wait();

                            Task.Delay(10000, CancellationToken.None).ContinueWith(capturaPosicaoPeriodica, CancellationToken.None);
                            //Debug.WriteLine(String.Format("Posição é: LAT: {0} e LONG {1} - Data: {2}", geo.Latitude, geo.Longitude, DateTime.Now.ToString()));
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        protected override void OnResume()
        {
            try
            {
                Action<Task> capturaPosicaoPeriodica = null;
                capturaPosicaoPeriodica = _task =>
                {
                    var locator = CrossGeolocator.Current;
                    var dbUsuario = new Repositorio<Usuario>();
                    var usuarioLogado = (dbUsuario.RetornarTodos()).Result.FirstOrDefault();

                    if (usuarioLogado != null)
                    {
                        if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                        {
                            locator.DesiredAccuracy = 50;
                            var pos = locator.GetPositionAsync(10000);
                            pos.Wait();

                            var geo = new Geolocalizacao
                            {
                                Latitude = pos.Result.Latitude,
                                Longitude = pos.Result.Longitude,
                                UsuarioId = usuarioLogado.Id,
                            };
                            var service = App.Container.Resolve<GeolocalizacaoViewModel>();
                            service.GravaGeolocalizacao(geo).Wait();

                            Task.Delay(10000, CancellationToken.None).ContinueWith(capturaPosicaoPeriodica, CancellationToken.None);
                            //Debug.WriteLine(String.Format("Posição é: LAT: {0} e LONG {1} - Data: {2}", geo.Latitude, geo.Longitude, DateTime.Now.ToString()));
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }
    }
}

