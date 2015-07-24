using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Mais
{
	public class LoginService : ILogin
	{
		HttpResponseMessage response;

		#region ILogin implementation

		public async Task<bool> FazelLogin(string email, string senha)
		{
			using (var client = CallAPI.RetornaClientHttp())
			{
				response = await client.GetAsync(string.Format("{0}{1}/{2}", Constants.uriAutenticar, email, senha));

				if (response.IsSuccessStatusCode)
					return true;
			}

			return false;
		}

		public bool EsqueciMinhaSenha(string usuario)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

