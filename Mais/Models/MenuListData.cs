using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Mais
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
                { 
                    Titulo = "Conteúdo Público", 
                    Icone = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("enquete.png")), 
                    TipoPagina = typeof(EnquetePublica),
                    //Enviar = new Action(() => MessagingCenter.Send<MenuListData>(this, "CarregaEnquetesPublicasViaMenu"))
                    Color = Colors._defaultColorFromHex
                });

            this.Add(new MenuItem()
                { 
                    Titulo = "Conteúdo Pessoal", 
                    Icone = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("enquete.png")), 
                    TipoPagina = typeof(EnqueteInteresse),
                    //Enviar = new Action(() => MessagingCenter.Send<MenuListData>(this, "CarregaEnquetesDeSeuInteresseViaMenu"))
                    Color = Colors._defaultColorFromHexLighter
                });
			
            this.Add(new MenuItem()
                { 
                    Titulo = "Criar enquete", 
                    Icone = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("novaEnquete.png")), 
                    TipoPagina = typeof(NovaEnquetePage),
                    Color = Colors._defaultColorFromHex
                });

            this.Add(new MenuItem()
                { 
                    Titulo = "Importar amigos", 
                    Icone = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("importarContatos.png")), 
                    TipoPagina = typeof(ImportarContatosPage),
                    Color = Colors._defaultColorFromHexLighter
                });

            this.Add(new MenuItem()
                { 
                    Titulo = "Buscar conteúdo", 
                    Icone = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("buscar.png")), 
                    TipoPagina = typeof(BuscarPage),
                    Color = Colors._defaultColorFromHex
                });
			
            this.Add(new MenuItem()
                { 
                    Titulo = "Editar cadastro", 
                    Icone = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("settings.png")), 
                    TipoPagina = typeof(CadastroEdicaoPage),
                    Color = Colors._defaultColorFromHexLighter
                });
        }
    }
}

