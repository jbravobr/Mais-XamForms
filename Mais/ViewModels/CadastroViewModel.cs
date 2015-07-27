using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

                var cadastrou = await this.service.CadastraNovoUsuario(this.Usuario);

                try
                {
                    if (cadastrou != null)
                    {
                        this.Logar = new Action(async () =>
                            {
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

                        Task.Run(() => Acr.UserDialogs
						 .UserDialogs
						 .Instance
                            .ShowSuccess(AppResources.MensagemSucessoCadastroNovoUsuario));

                        this.Logar.Invoke();
                    }
                    else
                    {
                        await Acr.UserDialogs
						.UserDialogs
						.Instance
						.AlertAsync(AppResources.MsgErroCadastroUsuario, AppResources.TituloErro, "OK");
                    }
                }
                catch (NullReferenceException)
                {
                    await Acr.UserDialogs
					.UserDialogs
					.Instance
					.AlertAsync(AppResources.MsgErroCadastroUsuarioCamposEmBranco, AppResources.TituloErro, "OK");
                }
            }
            else
            {
                if (this.Usuario.Categorias == null || !this.Usuario.Categorias.Any())
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Selecione ao menos uma categoria, clique no botão 'Categorias de Interesse'");
                    return;
                }
                else
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Informe todas as informações solicitadas.");
            }
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
            this.Categorias = categorias;
            this.Usuario.Categorias = categorias.ToList();
        }

        public async Task<Usuario> RetornarUsuario()
        {
            var dbUsuario = new Repositorio<Usuario>();
            var temRegistro = await dbUsuario.ExisteRegistro();
            return temRegistro ? (await dbUsuario.RetornarTodos()).FirstOrDefault() : null;
        }
    }
}

