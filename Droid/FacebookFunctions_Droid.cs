using System;
using Xamarin.Forms;
using Mais.Droid;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

[assembly: Dependency(typeof(FacebookFunctions_Droid))]
namespace Mais.Droid
{
	public class FacebookFunctions_Droid : IFacebook
	{
		const string baseUrl = "https://graph.facebook.com/v2.4/";

		#region IFacebook implementation

		public System.Threading.Tasks.Task<bool> DoLogin(string appKey)
		{
			throw new NotImplementedException();
		}

		public async System.Threading.Tasks.Task GetAmigos()
		{
			using (var client = RetornaClientHttp(baseUrl))
			{
				var result = await client.GetAsync(String.Format("me/friendlists?access_token={0}", App.FacebookAccessToken));

				if (result.IsSuccessStatusCode)
				{
					var json = await result.Content.ReadAsStringAsync();
					var dadosUsuario = JsonConvert.DeserializeObject<string>(json);
					return;
				}
			}
		}

		public async System.Threading.Tasks.Task<string> RecuperaDadosUsuario()
		{
			using (var client = RetornaClientHttp(baseUrl))
			{
				var result = await client.GetAsync(String.Format("{0}?access_token={1}", App.FacebookUserID, App.FacebookAccessToken));

				if (result.IsSuccessStatusCode)
				{
					var json = await result.Content.ReadAsStringAsync();
					var dadosUsuario = JsonConvert.DeserializeObject<string>(json);
					return dadosUsuario;
				}

				return await Task.FromResult("");
			}
		}

		public static HttpClient RetornaClientHttp(string url)
		{
			var client = new HttpClient();
			try
			{
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return client;
		}

		#endregion
	}

	public class FacebookUser
	{
		public string name { get; set; }

		public string id { get; set; }
	}
}

