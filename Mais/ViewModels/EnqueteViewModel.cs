using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Linq.Expressions;
using Xamarin;

namespace Mais
{
    public class EnqueteViewModel
    {
        public ObservableCollection<Enquete> Enquetes { get; set; }

        public ObservableCollection<Banner> Banners { get; protected set; }

        public ObservableCollection<Enquete> Mensagem { get; set; }

        public INavigation Navigation { get; set; }

        readonly ILogin service;

        public EnqueteViewModel(ILogin loginService)
        {
            this.service = loginService;
        }

        public void ConfigurarNavegacao(INavigation nav)
        {
            this.Navigation = nav;
        }

        private async Task InsertEnquetes()
        {
            var enquetes = EnqueteMock.MockEnquetes();

            var db = new Repositorio<Enquete>();
            await db.InserirTodos(enquetes);
        }

        //		public async Task<ObservableCollection<Enquete>> GetEnquetesPublicas()
        //		{
        //			return new ObservableCollection<Enquete>(EnqueteMock.MockEnquetes().Where(x => x.Tipo == EnumTipoEnquete.Publica));
        //		}
        //
        //		public async Task<ObservableCollection<Enquete>> GetEnquetesInteresse()
        //		{
        //			return new ObservableCollection<Enquete>(EnqueteMock.MockEnquetes().Where(x => x.Tipo == EnumTipoEnquete.Interesse));
        //		}


        public async Task<ObservableCollection<Enquete>> GetEnquetesPublicas()
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Buscando Enquetes...");
            var db = new Repositorio<Enquete>();
            var dbUsuario = new Repositorio<Usuario>();

            var ultimaEnquete = 0;
            ICollection<Enquete> listaEnquetes = null;

            var temRegistro = await db.ExisteEnquetePublica();
            if (!temRegistro)
            {
                listaEnquetes = await this.service.RetornarEnquetesPublicas(-1);
                await db.InserirTodos(listaEnquetes.ToList());

                foreach (var item in listaEnquetes)
                {
                    if (!String.IsNullOrEmpty(item.Imagem))
                        await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(item.Imagem, Constants.baseImageAddress);

                    if (!String.IsNullOrEmpty(item.UrlVideo))
                    {
                        var str = new Uri(item.UrlVideo).Segments;

                        var url = String.Format(Constants.uriYoutubeThumbnail, str[2]);
                        await DependencyService.Get<ISaveAndLoadFile>().BaixaThumbnailYoutubeSalvarEmDisco(url, String.Concat(str[2], ".jpg"));
                    }

                    foreach (var resposta in item.Pergunta.Respostas)
                    {
                        if (!String.IsNullOrEmpty(resposta.Imagem))
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(resposta.Imagem, Constants.baseImageAddress);
                    }
                }
            }
            else
            {
                ultimaEnquete = (await db.RetornarTodos()).OrderByDescending(e => e.Id).First(e => e.ServerEnqueteId != -1 && e.Tipo == EnumTipoEnquete.Publica).ServerEnqueteId;
                listaEnquetes = await this.service.RetornarEnquetesPublicas(ultimaEnquete);

                if (listaEnquetes != null && listaEnquetes.Any())
                {
                    await db.InserirTodos(listaEnquetes.ToList());

                    foreach (var item in listaEnquetes)
                    {
                        if (!String.IsNullOrEmpty(item.Imagem))
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(item.Imagem, Constants.baseImageAddress);

                        if (!String.IsNullOrEmpty(item.UrlVideo))
                        {
                            var str = new Uri(item.UrlVideo).Segments;

                            var url = String.Format(Constants.uriYoutubeThumbnail, str[2]);
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaThumbnailYoutubeSalvarEmDisco(url, String.Concat(str[2], ".jpg"));
                        }
                    }
                }

                var enquetesNoTelefone = (await db.RetornarTodos()).Where(e => e.Tipo == EnumTipoEnquete.Publica);

                foreach (var enquete in enquetesNoTelefone)
                {
                    if (!listaEnquetes.Contains(enquete))
                        listaEnquetes.Add(enquete);
                }
            }

            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            //return new ObservableCollection<Enquete>(listaEnquetes.Where(e => e.Tipo == EnumTipoEnquete.Publica));
            return await this.GetMensagens(listaEnquetes.Where(e => e.Tipo == EnumTipoEnquete.Publica));
        }

        public async Task<ObservableCollection<Enquete>> GetEnquetesDeSeuInteresse()
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Buscando Enquetes...");
            var db = new Repositorio<Enquete>();
            var dbUsuario = new Repositorio<Usuario>();
            var usuario = (await dbUsuario.RetornarTodos()).First();

            var ultimaEnquete = 0;
            ICollection<Enquete> listaEnquetes = null;

            var temRegistro = await db.ExisteEnqueteInteresse();
            if (!temRegistro)
            {
                listaEnquetes = await this.service.RetornarEnquetesInteresse(-1, usuario.Id);
                await db.InserirTodos(listaEnquetes.ToList());

                foreach (var item in listaEnquetes)
                {
                    if (!String.IsNullOrEmpty(item.Imagem))
                        await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(item.Imagem, Constants.baseImageAddress);

                    if (!String.IsNullOrEmpty(item.UrlVideo))
                    {
                        var str = new Uri(item.UrlVideo).Segments;

                        var url = String.Format(Constants.uriYoutubeThumbnail, str[2]);
                        await DependencyService.Get<ISaveAndLoadFile>().BaixaThumbnailYoutubeSalvarEmDisco(url, String.Concat(str[2], ".jpg"));
                    }
                }
            }
            else
            {
                Expression<Func<Enquete,bool>> filtro = (f) => f.Tipo == EnumTipoEnquete.Interesse;
                ultimaEnquete = (await db.ProcurarPorColecao(filtro)).OrderByDescending(e => e.ServerEnqueteId).First().ServerEnqueteId;
                listaEnquetes = await this.service.RetornarEnquetesInteresse(ultimaEnquete, usuario.Id);

                if (listaEnquetes != null && listaEnquetes.Any())
                {
                    await db.InserirTodos(listaEnquetes.ToList());

                    foreach (var item in listaEnquetes)
                    {
                        if (!String.IsNullOrEmpty(item.Imagem))
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(item.Imagem, Constants.baseImageAddress);

                        if (!String.IsNullOrEmpty(item.UrlVideo))
                        {
                            var str = new Uri(item.UrlVideo).Segments;

                            var url = String.Format(Constants.uriYoutubeThumbnail, str[2]);
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaThumbnailYoutubeSalvarEmDisco(url, String.Concat(str[2], ".jpg"));
                        }
                    }
                }

                var enquetesNoTelefone = (await db.RetornarTodos()).Where(e => e.Tipo == EnumTipoEnquete.Interesse);

                foreach (var enquete in enquetesNoTelefone)
                {
                    if (!listaEnquetes.Contains(enquete))
                        listaEnquetes.Add(enquete);
                }
            }

            ICollection<Banner> banners = null;
            List<Banner> ultimoBanner;
            var dbBanner = new Repositorio<Banner>();

            var categorias = String.Empty;

            if (App.Current.Properties.ContainsKey("UsuarioLogado"))
            {
                var usuarioLogado = App.Current.Properties["UsuarioLogado"] as Usuario;

                categorias = usuarioLogado.Categorias.Select(x => x.EnqueteId).Aggregate((x, y) => x + ';' + y).ToString();
            }

            var temBannerGravado = await db.ExisteBanner();
            if (!temBannerGravado)
            {
                banners = await this.service.RetornarBanners(-1, 1, categorias);

                foreach (var banner in banners.ToList())
                {
                    await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(banner.FileName, Constants.baseImageAddress);
                }

                await dbBanner.InserirTodos(banners.ToList());
            }
            else
            {
                ultimoBanner = await dbBanner.RetornarTodos();
                banners = ultimoBanner != null ? 
					await this.service.RetornarBanners(ultimoBanner.OrderByDescending(e => e.Id).First().Id, 1, categorias) : 
					await this.service.RetornarBanners(-1, 1, categorias);

                foreach (var banner in banners.ToList())
                {
                    await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(banner.FileName, Constants.baseImageAddress);
                }

                await dbBanner.InserirTodos(banners.ToList());
            }

            var b = await dbBanner.RetornarTodos();
            this.Banners = new ObservableCollection<Banner>(b.ToList());

            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            return new ObservableCollection<Enquete>(listaEnquetes.Where(e => e.Tipo == EnumTipoEnquete.Interesse));
        }

        public async Task<ObservableCollection<Enquete>> GetMensagens(IEnumerable<Enquete> enquetes)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Buscando Mensagens...");
            var db = new Repositorio<Enquete>();

            var ultimaEnquete = 0;
            ICollection<Enquete> listaEnquetes = null;

            var temRegistro = await db.ExisteMensagem();
            if (!temRegistro)
            {
                listaEnquetes = await this.service.RetornarMensagens(-1, 1);

                if (listaEnquetes != null && listaEnquetes.Any())
                {
                    await db.InserirTodos(listaEnquetes.ToList());

                    foreach (var item in listaEnquetes)
                    {
                        if (!String.IsNullOrEmpty(item.Imagem))
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(item.Imagem, Constants.baseImageAddress);

                        if (!String.IsNullOrEmpty(item.UrlVideo))
                        {
                            var str = new Uri(item.UrlVideo).Segments;

                            var url = String.Format(Constants.uriYoutubeThumbnail, str[2]);
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaThumbnailYoutubeSalvarEmDisco(url, String.Concat(str[2], ".jpg"));
                        }
                    }
                }
            }
            else
            {
                ultimaEnquete = (await db.RetornarTodos()).OrderByDescending(e => e.Id).First(e => e.ServerEnqueteId != -1 && e.Tipo == EnumTipoEnquete.Mensagem).ServerEnqueteId;
                listaEnquetes = await this.service.RetornarMensagens(ultimaEnquete, 1);

                if (listaEnquetes != null && listaEnquetes.Any())
                {
                    await db.InserirTodos(listaEnquetes.ToList());

                    foreach (var item in listaEnquetes)
                    {
                        if (!String.IsNullOrEmpty(item.Imagem))
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaImagemSalvarEmDisco(item.Imagem, Constants.baseImageAddress);

                        if (!String.IsNullOrEmpty(item.UrlVideo))
                        {
                            var str = new Uri(item.UrlVideo).Segments;

                            var url = String.Format(Constants.uriYoutubeThumbnail, str[2]);
                            await DependencyService.Get<ISaveAndLoadFile>().BaixaThumbnailYoutubeSalvarEmDisco(url, String.Concat(str[2], ".jpg"));
                        }
                    }
                }

                var enquetesNoTelefone = (await db.RetornarTodos()).Where(e => e.Tipo == EnumTipoEnquete.Mensagem);

                if (listaEnquetes == null)
                    listaEnquetes = enquetesNoTelefone.ToList();
                else
                {
                    foreach (var enquete in enquetesNoTelefone)
                    {
                        if (!listaEnquetes.Contains(enquete))
                            listaEnquetes.Add(enquete);
                    }
                }
            }

            Acr.UserDialogs.UserDialogs.Instance.HideLoading();

            IEnumerable<Enquete> x = null;

            if (listaEnquetes != null && listaEnquetes.Any())
                x = enquetes.Union(listaEnquetes.Where(e => e.Tipo == EnumTipoEnquete.Mensagem));
            else
                x = enquetes;

            return new ObservableCollection<Enquete>(x);
        }

        public async Task CarregarRespostas(int enqueteId)
        {
            try
            {
                var dbEnquete = new Repositorio<Enquete>();
                var enquete = (await dbEnquete.RetornarTodos()).First(x => x.Id == enqueteId);
                
                if (enquete.EnqueteRespondida)
                    await this.Navigation.PushAsync(new PerguntaRespondidaPage(enquete.Pergunta.Id, enquete.Imagem, enquete.UrlVideo, enquete.TemVoucher));
                else
                    await this.Navigation.PushAsync(new PerguntaPage(enquete.Pergunta.Id, enquete.Imagem, enquete.UrlVideo, enquete.TemVoucher));
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        public async Task CarregarMensagem(int enqueteId)
        {
            var dbEnquete = new Repositorio<Enquete>();

            var imagemNome = (await dbEnquete.RetornarTodos()).First(p => p.Tipo == EnumTipoEnquete.Mensagem && p.Id == enqueteId);
            await this.Navigation.PushAsync(new ExibirMensagemPage(enqueteId, imagemNome.Imagem, imagemNome.UrlVideo, imagemNome.TemVoucher));
        }
    }
}

