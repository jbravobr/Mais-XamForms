using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mais
{
    public interface IFacebook
    {
        Task<bool> DoLogin(string appKey);

        Task<IDictionary<string,object>> RecuperaDadosUsuario(string userToken);

        Task<List<IDictionary<string,object>>> GetAmigos(string userToken);

        Task<bool> PostToWall(string message, string userToken);
    }
}

