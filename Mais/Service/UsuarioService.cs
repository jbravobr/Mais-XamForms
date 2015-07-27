using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;

namespace Mais
{
    public class UsuarioService : ILogin
    {
        HttpResponseMessage response;

        #region ILogin implementation

        public async Task<Usuario> CadastraNovoUsuario(Usuario NovoUsuario)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                var usuarioJSON = JsonConvert.SerializeObject(NovoUsuario);
                response = await client.PostAsJsonAsync(Constants.uriNovoCadastro, usuarioJSON);

                if (response.IsSuccessStatusCode)
                {
                    var id = await response.Content.ReadAsAsync<int>();
                    NovoUsuario.Id = id;
                    return await Task.FromResult(NovoUsuario);
                }
                else if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.BadRequest)
                    return null;

                throw new ArgumentException("Erro Geral");
            }
        }

        public async Task<bool> AtualizarUsuario(Usuario NovoUsuario)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                var usuarioJSON = JsonConvert.SerializeObject(NovoUsuario);
                response = await client.PostAsJsonAsync(Constants.uriAtualizaUsuario, usuarioJSON);

                if (response.IsSuccessStatusCode)
                    return await Task.FromResult(true);

                return await Task.FromResult(false);
            }
        }

        public async Task<bool> EsqueciMinhaSenha(string _email)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                var email = JsonConvert.SerializeObject(_email);
                response = await client.PostAsJsonAsync(Constants.uriEsqueciSenha, email);
                if (response.IsSuccessStatusCode)
                    return await Task.FromResult(true);

                return await Task.FromResult(false);
            }
        }

        public async Task<RetornoGravacaoEnquete> CadastrarNovaEnquete(Enquete enquete)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                var enqueteJSON = JsonConvert.SerializeObject(enquete);
                response = await client.PostAsJsonAsync(Constants.uriNovaEnquete, enqueteJSON);

                if (response.IsSuccessStatusCode)
                {
                    var srt = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<RetornoGravacaoEnquete>(srt);
                    return json;
                }
            }

            return null;
        }

        public async Task<ICollection<PerguntaResposta>> CadastrarRespostaEnquete(PerguntaResposta resposta)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                resposta.TextoResposta = string.Empty;
                var respostaJSON = JsonConvert.SerializeObject(resposta);
                response = await client.PostAsJsonAsync(Constants.uriRespondeEnquete, respostaJSON);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var enquetesJsonToDomain = JsonConvert.DeserializeObject(jsonString);
                    var perguntaRespostaFromJson = JsonConvert.DeserializeObject<List<PerguntaResposta>>(enquetesJsonToDomain.ToString());
                    return perguntaRespostaFromJson;
                }
            }

            throw new ArgumentException(response.StatusCode.ToString());
        }

        public async Task<List<Resposta>> CadastrarRespostasDaPergunta(List<Resposta> perguntas)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                var perguntasJSON = JsonConvert.SerializeObject(perguntas);
                response = await client.PostAsJsonAsync(Constants.uriSalvarResposta, perguntasJSON);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var RespostaFromJson = JsonConvert.DeserializeObject<List<Resposta>>(jsonString);
                    return RespostaFromJson;
                }
            }

            return null;
        }

        public async Task<ICollection<Enquete>> RetornarEnquetesPublicas(int intMaiorEnquete)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(string.Format("{0}{1}", Constants.uriGetEnquetePublica, intMaiorEnquete));

                if (response.IsSuccessStatusCode)
                {
                    var enqueteResponse = response.Content.ReadAsStringAsync().Result;
                    var enquetesJsonToDomain = JsonConvert.DeserializeObject(enqueteResponse);
                    var listaEnquetesPublicas = JsonConvert.DeserializeObject<List<Enquete>>(enquetesJsonToDomain.ToString());
                    return listaEnquetesPublicas;
                }
            }

            throw new ArgumentException(response.StatusCode.ToString());
        }

        public async Task<ICollection<Usuario>> RetornarAmigos(List<string> telefones)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                var json = JsonConvert.SerializeObject(telefones);
                response = await client.PostAsJsonAsync(Constants.uriGetAmigos, json);

                if (response.IsSuccessStatusCode)
                {
                    var amigosResponse = response.Content.ReadAsStringAsync().Result;
                    var amigosToDomain = JsonConvert.DeserializeObject(amigosResponse);
                    var listaDeAmigos = JsonConvert.DeserializeObject<List<Usuario>>(amigosToDomain.ToString());
                    return listaDeAmigos;
                }
            }

            throw new ArgumentException(response.StatusCode.ToString());
        }

        public async Task<ICollection<Enquete>> RetornarMensagens(int intMaiorEnquete, int EmpresaId)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(string.Format("{0}{1}/{2}", Constants.uriGetMensagens, intMaiorEnquete, EmpresaId));

                if (response.IsSuccessStatusCode)
                {
                    var enqueteResponse = response.Content.ReadAsStringAsync().Result;
                    var enquetesJsonToDomain = JsonConvert.DeserializeObject(enqueteResponse);

                    if (enquetesJsonToDomain.ToString() == "Não há mensagens")
                        return null;

                    var listaEnquetesPublicas = JsonConvert.DeserializeObject<List<Enquete>>(enquetesJsonToDomain.ToString());
                    return listaEnquetesPublicas;
                }
            }

            throw new ArgumentException(response.StatusCode.ToString());
        }

        public async Task<ICollection<Enquete>> RetornarEnquetesInteresse(int intMaiorEnquete, int usuarioId)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(string.Format("{0}{1}/{2}", Constants.uriGetEnqueteInteresse, intMaiorEnquete, usuarioId));

                if (response.IsSuccessStatusCode)
                {
                    var enqueteResponse = response.Content.ReadAsStringAsync().Result;
                    var enquetesJsonToDomain = JsonConvert.DeserializeObject(enqueteResponse);
                    var listaEnquetesInteresse = JsonConvert.DeserializeObject<List<Enquete>>(enquetesJsonToDomain.ToString());
                    return listaEnquetesInteresse;
                }
            }

            throw new ArgumentException(response.StatusCode.ToString());
        }

        public async Task<ICollection<Categoria>> RetornarCategorias(int categoriaId)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(string.Format("{0}{1}", Constants.uriGetCategorias, categoriaId));

                if (response.IsSuccessStatusCode)
                {
                    var categoriasResponse = response.Content.ReadAsStringAsync().Result;
                    var categoriasJsonToDomain = JsonConvert.DeserializeObject(categoriasResponse);
                    var listaCategorias = JsonConvert.DeserializeObject<List<Categoria>>(categoriasJsonToDomain.ToString());
                    return listaCategorias;
                }
            }

            throw new ArgumentException(response.StatusCode.ToString());
        }

        public async Task<bool> FazerLogin(string email, string senha)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(string.Format("{0}{1}/{2}", Constants.uriAutenticar, email, senha));

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuario = JsonConvert.DeserializeObject<Usuario>(json);

                    var dbCategoria = new Repositorio<Categoria>();
                    var cats = string.Empty;

                    foreach (var item in usuario.Categorias)
                    {
                        var cat = await dbCategoria.RetornarPorId(item.Id);
                        cat.UsuarioId = usuario.Id;

                        await dbCategoria.Atualizar(cat);

                        cats += item.Id.ToString() + ';';
                    }
                    cats = cats.TrimEnd(';');
                    usuario.CategoriaMobileSelection = cats;

                    var dbUsuario = new Repositorio<Usuario>();
                    dbUsuario.Inserir(usuario);

                    App.Current.Properties["UsuarioLogado"] = (await dbUsuario.RetornarTodos()).FirstOrDefault();

                    return await Task.FromResult(true);
                }
            }

            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Dados inválidos");
            return await Task.FromResult(false);
        }

        public async Task<ICollection<Banner>> RetornarBanners(int id, int empresaId, string categorias)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(string.Format("{0}{1}/{2}/{3}", Constants.uriGetBanners, id, empresaId, categorias));

                if (response.IsSuccessStatusCode)
                {
                    var bannerResponse = response.Content.ReadAsStringAsync().Result;
                    var bannersJsonToDomain = JsonConvert.DeserializeObject(bannerResponse);
                    var listaBanners = JsonConvert.DeserializeObject<List<Banner>>(bannersJsonToDomain.ToString());
                    return listaBanners;
                }
            }

            throw new ArgumentException(response.StatusCode.ToString());
        }


        public async Task<bool> GravaGeolocalizacao(Geolocalizacao geo)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                var geoJSON = JsonConvert.SerializeObject(geo);
                response = await client.PostAsJsonAsync(Constants.uriGravaGeolocalizacao, geoJSON);

                if (response.IsSuccessStatusCode)
                    return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        #endregion
    }
}

