using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xamarin;

namespace Mais
{
    public class CadastroViewModel
    {
        public ICollection<Categoria> Categorias { get; protected set; }

        public ICommand btnCadastrar_Click { get; protected set; }

        public ICommand btnVoltar_Click { get; protected set; }

        public Action Logar { get; set; }

        public INavigation Navigation { get; protected set; }

        public Usuario Usuario { get; set; }

        readonly ILogin service;

        public void ConfiguraNavigation(INavigation navigation)
        {
            this.Navigation = navigation;
        }

        public async Task<bool> AtualizarCadastro(Usuario user, INavigation nav)
        {
            var categorias = string.Empty;
            foreach (var item in this.Categorias)
            {
                categorias += item.Id.ToString() + ';';
            }

            categorias.TrimEnd(';');

            user.CategoriaMobileSelection = categorias;

            var dbFacebook = new Repositorio<FacebookInfos>();
            var _token = (await dbFacebook.RetornarTodos()).FirstOrDefault();

            if (_token != null)
            {
                user.FacebookID = _token.user_id;
                user.FacebookToken = _token.access_token;
            }

            user.EmpresaApp = 1;

            var atualizou = await service.AtualizarUsuario(user);

            if (atualizou)
            {
                Acr.UserDialogs
					.UserDialogs
					.Instance
                    .ShowSuccess("Cadastro Atualizado com sucesso!");
                return await Task.FromResult(true);
            }
            else
            {
                await Acr.UserDialogs
					.UserDialogs
					.Instance
					.AlertAsync("Erro na atualização, tente novamente", AppResources.TituloErro, "OK");
                return await Task.FromResult(false);
            }
        }

        private async Task EfetuarCadastro()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Enviando...");
                
                if (this.Usuario.Sexo != null &&
                    this.Usuario.DataNascimento != null &&
                    (this.Usuario.Categorias != null && this.Usuario.Categorias.Any()) &&
                    this.Usuario.DDD != null &&
                    this.Usuario.Telefone != null &&
                    this.Usuario.Nome != null &&
                    this.Usuario.Email != null &&
                    this.Usuario.Municipio != null)
                {
                    var db = new Repositorio<Usuario>();
                
                    if (this.Usuario.Categorias == null || !this.Usuario.Categorias.Any())
                    {
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Selecione ao menos uma categoria, clique no botão 'Selecionar Categoria'");
                        return;
                    }
                
                    var categorias = string.Empty;
                    foreach (var categoria in this.Usuario.Categorias)
                    {
                        categorias += categoria.Id.ToString() + ';';
                    }
                    categorias = categorias.TrimEnd(';');
                
                    this.Usuario.CategoriaMobileSelection = categorias;
                
                    var dbFacebook = new Repositorio<FacebookInfos>();
                    var _token = (await dbFacebook.RetornarTodos()).FirstOrDefault();
                
                    if (_token != null)
                    {
                        this.Usuario.FacebookID = _token.user_id;
                        this.Usuario.FacebookToken = _token.access_token;
                    }
                
                    this.Usuario.EmpresaApp = 1;
                
                    var cadastrou = await this.service.CadastraNovoUsuario(this.Usuario);
                
                    try
                    {
                        if (cadastrou != null)
                        {
                            this.Logar = new Action(async () =>
                                {
                                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                                    var autenticado = await this.service.FazerLogin(this.Usuario.Email, this.Usuario.Senha);
                
                                    if (autenticado)
                                    {
                                        var dbUsuario = new Repositorio<Usuario>();
                
                                        var temUsuario = (await dbUsuario.RetornarTodos()).FirstOrDefault();
                                        if (temUsuario != null)
                                            await dbUsuario.Inserir(cadastrou);
                
                                        var dbSession = new Repositorio<ControleSession>();
                                        await dbSession.Inserir(new ControleSession { Logado = true });
                
                                        if (temUsuario != null)
                                            await this.Navigation.PushModalAsync(new MainPage());
                                        else
                                            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro na gravação do usuário");
                                    }
                                    else
                                        Acr.UserDialogs.UserDialogs.Instance.Alert("Dados incorretos!", "Erro", "OK");
                                });
                
                            await db.Inserir(cadastrou);
                            foreach (var categoria in cadastrou.Categorias)
                            {
                                var dbUsuarioCategoria = new Repositorio<UsuarioCategoria>();
                                dbUsuarioCategoria.Inserir(new UsuarioCategoria{ CategoriaId = categoria.Id });
                            }
                
                            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                
                            Task.Run(() => Acr.UserDialogs
                .UserDialogs
                .Instance
                                .ShowSuccess(AppResources.MensagemSucessoCadastroNovoUsuario, 2));
                
                            this.Logar.Invoke();
                        }
                        else
                        {
                            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                
                            await Acr.UserDialogs
                .UserDialogs
                .Instance
                .AlertAsync(AppResources.MsgErroCadastroUsuario, AppResources.TituloErro, "OK");
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                
                        await Acr.UserDialogs
                .UserDialogs
                .Instance
                .AlertAsync(AppResources.MsgErroCadastroUsuarioCamposEmBranco, AppResources.TituloErro, "OK");
                    }
                }
                else
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                
                    if (this.Usuario.Categorias == null || !this.Usuario.Categorias.Any())
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Selecione ao menos uma categoria, clique no botão 'Categorias de Interesse'");
                        return;
                    }
                    else
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Informe todas as informações solicitadas.");
                }
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        public async Task<bool> FazerCadastro(string Nome)
        {
            var dbFacebook = new Repositorio<FacebookInfos>();
            var _token = (await dbFacebook.RetornarTodos()).FirstOrDefault();

            if (_token != null)
            {
                this.Usuario.FacebookID = _token.user_id;
                this.Usuario.FacebookToken = _token.access_token;
            }

            this.Usuario.EmpresaApp = 1;
            this.Usuario.Nome = Nome;

            var cadastrou = await this.service.CadastraNovoUsuario(this.Usuario, true);

            if (cadastrou != null)
            {
                var dbUsuario = new Repositorio<Usuario>();
                await dbUsuario.Inserir(cadastrou);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        private async Task Voltar()
        {
            await this.Navigation.PopModalAsync();
        }

        public CadastroViewModel(ILogin service)
        {
            this.btnCadastrar_Click = new Command(async (obj) => await this.EfetuarCadastro());
            this.btnVoltar_Click = new Command(async () => await this.Voltar());
            this.Usuario = new Usuario();
            this.service = service;
        }

        public void AdicionaCategoriasSelecionadas(ICollection<Categoria> categorias)
        {
            try
            {
                this.Categorias = categorias;
                this.Usuario.Categorias = categorias.ToList();
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        public async Task<bool> FazCadastroCategoriasFB(List<Categoria> categorias, string email)
        {
            var dbUsuario = new Repositorio<Usuario>();
            var query = await dbUsuario.RetornarTodos();
            var _usuario = query.First();

            var cats = string.Empty;
            foreach (var categoria in categorias)
            {
                cats += categoria.Id.ToString() + ';';
            }
            cats = cats.TrimEnd(';');

            _usuario.CategoriaMobileSelection = cats;

            if (!String.IsNullOrEmpty(email))
                _usuario.Email = email;

            var result = await this.service.AtualizarCategoriasFB(_usuario);

            if (result != null)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<Usuario> RetornarUsuario()
        {
            var dbUsuario = new Repositorio<Usuario>();
            var temRegistro = await dbUsuario.ExisteRegistro();
            return temRegistro ? (await dbUsuario.RetornarTodos()).FirstOrDefault() : null;
        }
    }
}

