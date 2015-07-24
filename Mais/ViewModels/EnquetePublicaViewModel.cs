using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace Mais
{
    public class EnquetePublicaViewModel
    {
        private INavigation Navigation { get; set; }

        public EnquetePublicaViewModel(INavigation nav)
        {
            this.Navigation = nav;
        }

        public async Task CarregarPergunta(int enqueteId)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Carregando");
                });
			
            var dbEnquete = new Repositorio<Enquete>();

            var _enquete = (await dbEnquete.RetornarTodos()).First(p => p.Id == enqueteId);

            if (_enquete.EnqueteRespondida)
            {
                Device.BeginInvokeOnMainThread(() =>
                    {
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    });
                await this.Navigation.PushAsync(new PerguntaRespondidaPage((int)_enquete.PerguntaId, _enquete.Imagem, _enquete.UrlVideo, _enquete.TemVoucher));
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                    {
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    });
                await this.Navigation.PushAsync(new PerguntaPage((int)_enquete.PerguntaId, _enquete.Imagem, _enquete.UrlVideo, _enquete.TemVoucher));
            }
			
        }

        public async Task CarregarMensagem(int enqueteId)
        {
            var dbEnquete = new Repositorio<Enquete>();

            Expression<Func<Enquete,bool>> porId = (x) => x.Id == enqueteId && x.Tipo == EnumTipoEnquete.Mensagem;

            var mensagem = (await dbEnquete.ProcurarPorFiltro(porId));

            var dbCategoria = new Repositorio<Categoria>();
            var _categoria = await dbCategoria.RetornarPorId(mensagem.CategoriaId);

            mensagem.Categoria = _categoria;
            mensagem.CategoriaId = _categoria.Id;
            mensagem.EnqueteRespondida = true;
            
            await dbEnquete.Atualizar(mensagem);

            await this.Navigation.PushAsync(new ExibirMensagemPage(enqueteId, mensagem.Imagem, mensagem.UrlVideo, mensagem.TemVoucher));
        }
    }
}

