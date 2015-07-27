using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;
using Share.Forms.Plugin.Abstractions;

namespace Mais
{
    public class PerguntaViewModel
    {
        public ICommand btnGravar_Click { get; protected set; }

        public ICommand btnCompartilhar_Click { get; protected set; }

        public List<Resposta> Respostas { get; private set; }

        public Enquete Mensagem { get; set; }

        public Pergunta Pergunta { get; private set; }

        public Xamarin.Forms.INavigation Navigation { get; private set; }

        readonly ILogin service;

        public PerguntaViewModel(ILogin service)
        {
            this.btnGravar_Click = new Xamarin.Forms.Command(async () => await this.GravarResposta());
            this.btnCompartilhar_Click = new Xamarin.Forms.Command(() => this.Compartilhar());
            this.service = service;
        }

        public void AdicionarPergunta(Pergunta pergunta)
        {
            this.Pergunta = pergunta;
        }

        public void AdicionaMensagem(Enquete enquete)
        {
            this.Mensagem = enquete;
        }

        public void ConfigurarNavigation(Xamarin.Forms.INavigation nav)
        {
            this.Navigation = nav;
        }

        public async Task<Pergunta> GetPerguntaPorId(int id)
        {
            var db = new Repositorio<Pergunta>();

            return await db.RetornarPorId(id);
        }

        public async Task<Enquete> GetMensagem(int id)
        {
            var db = new Repositorio<Enquete>();

            return await db.RetornarPorId(id);
        }

        public void AdicionaRespostasRespondidas(List<Resposta> respostas, bool? temVoucher)
        {
            this.Respostas = respostas;

            foreach (var resposta in this.Respostas)
            {
                resposta.temVoucher = (bool)temVoucher;
            }
        }

        private async Task GravarResposta()
        {
            if (this.Respostas != null)
            {
                var db = new Repositorio<Resposta>();
                if (await db.InserirTodos(this.Respostas))
                {
                    var dbEnquete = new Repositorio<Enquete>();
                    var enquete = (await dbEnquete.RetornarTodos()).First(e => e.PerguntaId == this.Pergunta.Id);
                    enquete.EnqueteRespondida = true;

                    if (await dbEnquete.Atualizar(enquete))
                    {
                        foreach (var resposta in this.Respostas.Where(r=>r.Respondida))
                        {
                            var perguntaResposta = new PerguntaResposta();

                            var dbUsuario = new Repositorio<Usuario>();
                            var usuarioLogado = (await dbUsuario.RetornarTodos()).First();

                            perguntaResposta.RespostaId = resposta.RespostaServerId;
                            perguntaResposta.PerguntaId = enquete.PerguntaServerId == 0 ? enquete.Pergunta.PerguntaServerId : enquete.PerguntaServerId;
                            perguntaResposta.UsuarioId = usuarioLogado.Id;

                            var salvouNoServidor = await this.service.CadastrarRespostaEnquete(perguntaResposta);

                            if (salvouNoServidor == null || !salvouNoServidor.Any())
                            {
                                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Erro ao salvar resposta, tente novamente");
                                return;
                            }

                            var dbperguntaResposta = new Repositorio<PerguntaResposta>();
                            await dbperguntaResposta.InserirTodos(salvouNoServidor.ToList());

                            var dbResposta = new Repositorio<Resposta>();
                            foreach (var pr in salvouNoServidor)
                            {
                                Expression<Func<Resposta,bool>> porPerguntaServerId = (x) => x.PerguntaServerId == pr.PerguntaId;
                                var _resposta = await dbResposta.ProcurarPorFiltro(porPerguntaServerId);
                                _resposta.percentualResposta = pr.percentual;
									
                                await dbResposta.Atualizar(_resposta);
                            }
                        }

                        if (enquete.TemVoucher)
                        {
                            var pagina = Activator.CreateInstance(typeof(VotoSalvoComVoucherPage), new[]{ this.Pergunta }) as VotoSalvoComVoucherPage;
                            await this.Navigation.PushModalAsync(pagina);
                        }
                        else
                        {
                            var pagina = Activator.CreateInstance(typeof(VotoSalvoPage), new[]{ this.Pergunta }) as VotoSalvoPage;
                            await this.Navigation.PushModalAsync(pagina);
                        }
                    }
                }
            }
            else
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(AppResources.TituloErro, AppResources.MsgErroAoResponder, "OK");
        }

        private void Compartilhar()
        {
            if (this.Pergunta != null)
            {
                var title = this.Pergunta.TextoPergunta;
                var status = String.Format("Eu votei na enquete {0} com {1}%... minha resposta foi tal {2}"
				, this.Pergunta.TextoPergunta
				, this.Pergunta.Respostas.First(x => x.Respondida).percentualResposta.ToString()
				, this.Pergunta.Respostas.First(x => x.Respondida).TextoResposta);

                DependencyService.Get<IShare>().ShareLink(title, status, string.Empty);
            }
            else if (this.Mensagem != null)
            {
                var title = this.Mensagem.Titulo;
                var status = this.Mensagem.Titulo;

                DependencyService.Get<IShare>().ShareLink(title, status, string.Empty);
            }
        }

        public async Task<string> RetornaImagemPerguntaDaEnquete(int perguntaId)
        {
            var dbEnquete = new Repositorio<Enquete>();
            return (await dbEnquete.RetornarTodos()).First(e => e.PerguntaId == perguntaId).Imagem;
        }
    }
}

