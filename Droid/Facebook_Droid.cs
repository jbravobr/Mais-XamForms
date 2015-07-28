using System;
using Xamarin.Auth;
using Mais.Droid;
using Mais;
using Android.App;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(CadastroComFacebookPage), typeof(FacebookLoginRenderer_Droid))]
namespace Mais.Droid
{
    public class FacebookLoginRenderer_Droid : Xamarin.Forms.Platform.Android.PageRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // this is a ViewGroup - so should be able to load an AXML file and FindView<>
            var activity = this.Context as Activity;

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
                            MessagingCenter.Send<CadastroComFacebookPage>((CadastroComFacebookPage)e.NewElement, "CadastrouComFacebook");
                            return;
                        }
                    }
                }
                else
                {
                    // The user cancelled
                }
            };

            activity.StartActivity(auth.GetUI(activity));
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

