using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Mais
{
    public static class CallAPI
    {
        // Retorna uma instância da chamada para a API
        public static HttpClient RetornaClientHttp()
        {
            var client = new HttpClient();
            try
            {
                //if (!TestaConexao())
                //new NetworkErrorAlert();

                //client.BaseAddress = new Uri(Constants.baseAddress);
                client.BaseAddress = new Uri(Constants.baseAddressTest); // URL de Teste
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(3);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return client;
        }

        // Testa conexão com a internet
        internal static bool TestaConexao()
        {
            return true;
            //return DependencyService.Get<INetworkStatus>().VerificaStatusConexao();
        }
    }
}

