using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Collections.Generic;

namespace Mais
{
    public class LoginViewModel
    {
        public string senha { get; set; }

        public string login { get; set; }

        public ICommand btnEntrar_Click { get; protected set; }

        public INavigation Navigation { get; protected set; }

        readonly ILogin service;

        public LoginViewModel(ILogin service)
        {
            this.btnEntrar_Click = new Command(async () => await this.ValidaLogin());
            this.service = service;
        }

        public void ConfiguraNavegacao(INavigation navigation)
        {
            this.Navigation = navigation;
        }

        public async Task ValidaLogin()
        {
            var autenticado = await this.service.FazerLogin(this.login, this.senha);

            if (autenticado)
            {
                var dbUsuario = new Repositorio<Usuario>();
                var _usuarioLogado = App.Current.Properties.ContainsKey("UsuarioLogado") ? App.Current.Properties["UsuarioLogado"] as Usuario : null;

                var temUsuario = (await dbUsuario.RetornarTodos()).FirstOrDefault();
                if (_usuarioLogado != null)
                    await dbUsuario.InserirAsync(_usuarioLogado);

                temUsuario = (await dbUsuario.RetornarTodos()).FirstOrDefault();
                if (temUsuario != null)
                    await dbUsuario.InserirAsync(_usuarioLogado);

                var dbSession = new Repositorio<ControleSession>();
                await dbSession.Inserir(new ControleSession { Logado = true });

                if (temUsuario != null)
                {
                    App.Current.Properties["UsuarioLogado"] = _usuarioLogado;

                    await this.Navigation.PushModalAsync(new MainPage());
                }
                else
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro na gravação do usuário");
            }
            else
                UserDialogs.Instance.Alert("Dados incorretos!", "Erro", "OK");
        }

        public async Task<bool> EsqueciMinhaSenha(string email)
        {
            return await this.service.EsqueciMinhaSenha(email);
        }
    }
}

