using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mais
{
    public interface ILogin
    {
        Task<bool> FazerLogin(string email, string senha);

        Task<RetornoGravacaoEnquete> CadastrarNovaEnquete(Enquete e);

        Task<List<Resposta>> CadastrarRespostasDaPergunta(List<Resposta> perguntas);

        Task<ICollection<Enquete>> RetornarEnquetesPublicas(int idMaiorEnquetePublica);

        Task<ICollection<Enquete>> RetornarEnquetesInteresse(int idMaiorEnqueteInteresse, int empresaId);

        Task<ICollection<PerguntaResposta>> CadastrarRespostaEnquete(PerguntaResposta r);

        Task<Usuario> CadastraNovoUsuario(Usuario u);

        Task<ICollection<Categoria>> RetornarCategorias(int categoriaId);

        Task<ICollection<Banner>> RetornarBanners(int id, int empresaId, string categorias);

        Task<bool> GravaGeolocalizacao(Geolocalizacao geo);

        Task<bool> AtualizarUsuario(Usuario u);

        Task<ICollection<Enquete>> RetornarMensagens(int idMaiorMensagem, int EmpresaId);

        Task<bool> EsqueciMinhaSenha(string email);

        Task<ICollection<Usuario>> RetornarAmigos(List<string> telefones);

        Task<bool> GravaChavePushWoosh(string token, int usuarioId);

        Task<bool> AtualizaFacebookToken(string token, int usuarioId);
    }
}

