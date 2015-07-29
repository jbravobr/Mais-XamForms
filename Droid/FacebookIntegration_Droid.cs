using System;

using Xamarin.Forms;
using Mais.Droid;
using Facebook;
using Android.Widget;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

[assembly: Dependency(typeof(FacebookIntegration_Droid))]
namespace Mais.Droid
{
    public class FacebookIntegration_Droid : IFacebook
    {
        public async Task<bool> PostToWall(string message, string userToken)
        {
            var fb = new FacebookClient(userToken);
            var postou = false;
            await fb.PostTaskAsync("me/feed", new { message = message, link = "http://google.com.br"}).ContinueWith(async(t) =>
                {
                    if (t.IsFaulted)
                    {
                        Toast.MakeText(Forms.Context, "Erro ao postar", ToastLength.Short).Show();
                    }
                    else
                        postou = true;
                });

            return await Task.FromResult(postou);
        }

        public Task<bool> DoLogin(string appKey)
        {
            throw new NotImplementedException();
        }

        public async Task<IDictionary<string,object>> RecuperaDadosUsuario(string userToken)
        {
            // This uses Facebook Graph API
            // See https://developers.facebook.com/docs/reference/api/ for more information.
            var fb = new FacebookClient(userToken);
            object result = null;
            await fb.GetTaskAsync("me").ContinueWith(async (t) =>
                {
                    if (!t.IsFaulted)
                    {
                        var i = (IDictionary<string, object>)(await t);
                        result = i;
                        return i;
                    }
                    return null;
                });

            return (IDictionary<string, object>)result;
        }

        public async Task<RootObject> GetAmigos(string userToken)
        {
            var fb = new FacebookClient(userToken);
            object result = null;
            await fb.GetTaskAsync("me/friends?limit=9000").ContinueWith(async (t) =>
                {
                    if (!t.IsFaulted)
                    {
                        var jsonString = (await t).ToString();
                        var i = JsonConvert.DeserializeObject <RootObject>(jsonString);
                        result = i;
                        return i;
                    }
                    return null;
                });

            return (RootObject)result;
        }
    }
}

