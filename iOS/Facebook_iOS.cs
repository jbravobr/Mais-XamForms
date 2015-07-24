using System;
using Xamarin.Forms;
using Mais;
using Mais.iOS;
using Xamarin.Auth;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

[assembly: ExportRenderer(typeof(CadastroComFacebook), typeof(Facebook_iOS))]
namespace Mais.iOS
{
	public class Facebook_iOS : Xamarin.Forms.Platform.iOS.PageRenderer
	{
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			var auth = new OAuth2Authenticator(
				           clientId: "1169967153020820", // your OAuth2 client id
				           scope: "publish_actions, user_friends", // the scopes for the particular API you're accessing, delimited by "+" symbols
				           authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"), // the auth URL for the service
				           redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html")); // the redirect URL for the service

			auth.Completed += async (sender, eventArgs) =>
			{
				if (eventArgs.IsAuthenticated)
				{
					//App.SuccessfulLoginAction.Invoke();
					// Use eventArgs.Account to do wonderful things
					App.FacebookAccessToken = eventArgs.Account.Properties["access_token"];

					var client = RetornaClientHttp();

					using (client)
					{
						var result = await client.GetAsync(String.Format("/me?fields=id&access_token={0}", App.FacebookAccessToken));

						if (result.IsSuccessStatusCode)
						{
							var json = await result.Content.ReadAsStringAsync();
							App.FacebookUserID = JsonConvert.DeserializeObject<JsonModel>(json).id;
							App.SuccessfulLoginAction.Invoke();
							DismissViewController(true, null);
							return;
						}
					}
				}
				else
				{
					// The user cancelled
				}
			};


			if (String.IsNullOrEmpty(App.FacebookAccessToken) && String.IsNullOrEmpty(App.FacebookUserID))
				PresentViewController(auth.GetUI(), true, null);
		}

		public static HttpClient RetornaClientHttp()
		{
			var client = new HttpClient();
			try
			{
				client.BaseAddress = new Uri("https://graph.facebook.com");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return client;
		}
	}

	public class JsonModel
	{
		[JsonProperty("id")]
		public string id { get; set; }
	}
}

