using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xamarin;
using PropertyChanged;

namespace Mais
{
    [ImplementPropertyChanged]
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

        public async Task<bool> AtualizarCadastro(Usuario user, INavigation nav, DateTime? dataNascimento, int? sexo)
        {
            var categorias = string.Empty;

            if (Device.OS == TargetPlatform.iOS)
            {
                if (App.Current.Properties.ContainsKey("Categorias_iOS"))
                {
                    var c = (ICollection<Categoria>)App.Current.Properties["Categorias_iOS"];

                    if (c != null && c.Any())
                    {
                        foreach (var item in c)
                        {
                            categorias += item.Id.ToString() + ';';
                        }
                    }
                }
            }
            else
            {
                foreach (var item in this.Categorias)
                {
                    categorias += item.Id.ToString() + ';';
                }
            }

            categorias = categorias.TrimEnd(new char[]{ ';' });

            user.CategoriaMobileSelection = categorias;

            var dbFacebook = new Repositorio<FacebookInfos>();
            var _token = (await dbFacebook.RetornarTodos()).FirstOrDefault();

            if (_token != null)
            {
                user.FacebookID = _token.user_id;
                user.FacebookToken = _token.access_token;
            }

            user.EmpresaApp = 1;

            if (dataNascimento.HasValue)
                user.DataNascimento = dataNascimento;

            if (sexo != null && sexo > 0)
            {
                if ((int)sexo == 1)
                    user.Sexo = EnumSexo.Masculino;
                else
                    user.Sexo = EnumSexo.Feminino;
            }

            var atualizou = await service.AtualizarUsuario(user);

            if (atualizou)
            {

                var dbUsuario = new Repositorio<Usuario>();
                await dbUsuario.Atualizar(user);

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

        /*
                    var _sexo = sexoPicker.SelectedIndex;
                    var _nascimento = nascimentoPicker.Date;
                    var _email = entEmail.Text;
                    var _nome = entNome.Text;
                    var _ddd = entDDD.Text;
                    var _tel = entTelefone.Text;
                    var _municipio = entMunicipio.Text;
        */

        public async Task EfetuarCadastro(int? sexo, DateTime? nascimento, string email, string nome, string ddd, string tel, string municipio, string senha)
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Enviando...");

                if (sexo != null &&
                    nascimento != null &&
                    (this.Usuario.Categorias != null && this.Usuario.Categorias.Any()) &&
                    !String.IsNullOrEmpty(email) &&
                    !String.IsNullOrEmpty(nome) &&
                    !String.IsNullOrEmpty(ddd) &&
                    !String.IsNullOrEmpty(tel) &&
                    !String.IsNullOrEmpty(municipio) &&
                    !String.IsNullOrEmpty(senha))
                {
                    var db = new Repositorio<Usuario>();
                    Usuario usuario = new Usuario();

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

                    usuario.CategoriaMobileSelection = categorias;

                    var dbFacebook = new Repositorio<FacebookInfos>();
                    var _token = (await dbFacebook.RetornarTodos()).FirstOrDefault();

                    if (_token != null)
                    {
                        usuario.FacebookID = _token.user_id;
                        usuario.FacebookToken = _token.access_token;
                    }

                    usuario.EmpresaApp = 1;
                    usuario.Nome = nome;
                    usuario.DataNascimento = nascimento;
                    usuario.DDD = ddd;
                    usuario.Telefone = tel;
                    usuario.Email = email;
                    usuario.Municipio = municipio;
                    usuario.Senha = senha;
                    //usuario.Categorias = this.Usuario.Categorias;

                    if (sexo == 1)
                        usuario.Sexo = EnumSexo.Masculino;
                    else
                        usuario.Sexo = EnumSexo.Feminino;

                    var cadastrou = await this.service.CadastraNovoUsuario(usuario);

                    try
                    {
                        if (cadastrou != null)
                        {
                            this.Logar = new Action(async () =>
                                {
                                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                                    var autenticado = await this.service.FazerLogin(cadastrou.Email, cadastrou.Senha);

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
                            foreach (var categoria in cadastrou.CategoriaMobileSelection.Split(';'))
                            {
                                var dbUsuarioCategoria = new Repositorio<UsuarioCategoria>();

                                await dbUsuarioCategoria.Inserir(new UsuarioCategoria{ CategoriaId = Convert.ToInt32(categoria) });
                            }

                            Acr.UserDialogs.UserDialogs.Instance.HideLoading();

//                            Acr.UserDialogs
//                                .UserDialogs
//                                .Instance
//                                .ShowSuccess(AppResources.MensagemSucessoCadastroNovoUsuario, 2);

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

                if (_token != null)
                {
                    cadastrou.FacebookID = _token.user_id;
                    cadastrou.FacebookToken = _token.access_token;
                }

                var dbUsuario = new Repositorio<Usuario>();
                await dbUsuario.Inserir(cadastrou);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task Voltar()
        {
            await this.Navigation.PopModalAsync();
        }

        public CadastroViewModel(ILogin service)
        {
            //this.btnCadastrar_Click = new Command(async (obj) => await this.EfetuarCadastro());
            this.btnVoltar_Click = new Command(async () => await this.Voltar());
            this.Usuario = new Usuario();
            this.service = service;
        }

        public void AdicionaCategoriasSelecionadas(ICollection<Categoria> categorias)
        {
            try
            {
                App.Current.Properties["Categorias_iOS"] = categorias;
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
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<Usuario> RetornarUsuario()
        {
            try
            {
                var dbUsuario = new Repositorio<Usuario>();
                var temRegistro = await dbUsuario.ExisteRegistro();

                return temRegistro ? (await dbUsuario.RetornarTodos()).FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
                return null;
            }
        }
    }
}

