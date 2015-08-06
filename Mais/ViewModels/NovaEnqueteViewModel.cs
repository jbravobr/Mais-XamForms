using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace Mais
{
    public class NovaEnqueteViewModel
    {
        public Pergunta Pergunta { get; set; }

        public Enquete Enquete { get; set; }

        private List<Resposta> Respostas { get; set; }

        private INavigation Navigation { get; set; }

        readonly ILogin service;

        public NovaEnqueteViewModel(ILogin _service)
        {
            this.Enquete = new Enquete();
            this.Pergunta = new Pergunta();
            this.Respostas = new List<Resposta>();
            this.service = _service;
        }

        public void ConfiguraNavegacao(INavigation nav)
        {
            this.Navigation = nav;
        }

        public async Task AdicionarAmigoAoEnvio(ICollection<Amigo> amigos)
        {
            var colegas = string.Empty;
            foreach (var item in amigos)
            {
                colegas += item.UsuarioId.ToString() + ';';
            }
            this.Enquete.colegas = colegas.TrimEnd(';');
        }

        public void AdicionaRespostaParaColecao(List<Resposta> respostas)
        {
            this.Respostas = respostas;
        }

        public async Task SalvaEnquete()
        {
            var confirmConfig = new Acr.UserDialogs.ConfirmConfig();
            confirmConfig.CancelText = "Cancelar";
            confirmConfig.OkText = "Finalizar";
            confirmConfig.Message = "Tem certeza de que deseja criar esta enquete?";

            if (await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(confirmConfig))
            {
                try
                {
                    this.Pergunta.Respostas = new List<Resposta>();
                    this.Pergunta.Respostas = this.Respostas;

                    this.Enquete.Pergunta = new Pergunta();
                    this.Enquete.Pergunta = this.Pergunta;
                    this.Enquete.Tipo = EnumTipoEnquete.Interesse;
                    this.Enquete.Titulo = this.Pergunta.TextoPergunta;
                    this.Enquete.Status = EnumStatusEnquete.Publicada;

                    var dbUsuario = new Repositorio<Usuario>();
                    var usuarioLogado = (await dbUsuario.RetornarTodos()).First();

                    this.Enquete.UsuarioId = usuarioLogado.Id;
                    this.Enquete.ServerEnqueteId = -1;
                    this.Enquete.propria = true;
                    this.Enquete.AtivaNoFront = true;

                    List<Resposta> gravou = new List<Resposta>();
                    var db = new Repositorio<Enquete>();

                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Enviando...");
                    if (await db.Inserir(this.Enquete))
                    {
                        var result = await this.service.CadastrarNovaEnquete(this.Enquete);

                        if (result != null)
                        {
                            try
                            {
                                // Atualizando dados locais com o ID da pergunta no servidor
                                var dbEnquete = new Repositorio<Enquete>();
                                this.Enquete.PerguntaServerId = result.PerguntaId;
                                this.Enquete.Pergunta.PerguntaServerId = result.PerguntaId;
                                this.Enquete.ServerEnqueteId = result.EnqueteId;
							
                                foreach (var item in Respostas)
                                {
                                    item.PerguntaServerId = result.PerguntaId;
                                }
							
                                //await dbEnquete.Atualizar(Enquete);
                                // Fim da Atualização

                                gravou = await this.service.CadastrarRespostasDaPergunta(this.Pergunta.Respostas);
                                var dbResposta = new Repositorio<Resposta>();

                                if (gravou != null && gravou.Any())
                                {
                                    foreach (var resp in gravou)
                                    {
                                        var _r = this.Enquete.Pergunta.Respostas.First(x => x.PerguntaServerId == resp.PerguntaServerId
                                                     && x.TextoResposta == resp.TextoResposta);
                                        _r.RespostaServerId = resp.RespostaServerId;
                                        _r.PerguntaServerId = resp.PerguntaServerId;

                                        await dbResposta.Atualizar(_r);
                                        await dbEnquete.Atualizar(Enquete);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                        if (gravou != null && gravou.Any())
                        {
                            await this.Navigation.PushModalAsync(new MainPage());
                        }
                        else
                            await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(String.Empty, AppResources.TextoMsgCadastroErroEnquete, "OK");
                    }
                    else
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(String.Empty, AppResources.TextoMsgCadastroErroEnquete, "OK");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

