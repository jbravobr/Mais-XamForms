using System;
using Xamarin.Forms;
using Mais.iOS;
using System.Threading.Tasks;
using Facebook;
using System.Collections.Generic;
using UIKit;

[assembly: Dependency(typeof(FacebookIntegration_iOS))]

namespace Mais.iOS
{
	public class FacebookIntegration_iOS : IFacebook
	{
		public async Task<bool> PostToWall(string message, string userToken)
		{
			var fb = new FacebookClient(userToken);
			var postou = false;
			await fb.PostTaskAsync("me/feed", new { message = message, link = "http://google.com.br"}).ContinueWith(async(t) =>
				{
					if (t.IsFaulted)
					{
						var _error = new UIAlertView("Erro", "Erro ao postar, tente novamente!", null, "Ok", null);
						_error.Show();
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

		public Task GetAmigos(string userToken)
		{
			throw new NotImplementedException();
		}
	}
}

