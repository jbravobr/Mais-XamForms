using System;

using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using Xamarin.Forms;
using PropertyChanged;
using Newtonsoft.Json;

namespace Mais
{
    [ImplementPropertyChanged]
    [Table("Enquete")]
    public class Enquete
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public int ServerEnqueteId { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public bool propria { get; set; }

        [JsonIgnore]
        public string UrlImagem { get; set; }

        [JsonIgnore]
        public string VideoThumbnail { get; set; }

        public string UrlVideo { get; set; }

        public string colegas { get; set; }

        [JsonIgnore]
        [ForeignKey(typeof(Pergunta))]
        public int? PerguntaId { get; set; }

        [JsonIgnore]
        public int PerguntaServerId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public Pergunta Pergunta { get; set; }

        [ForeignKey(typeof(Categoria))]
        public int CategoriaId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public Categoria Categoria { get; set; }

        [JsonIgnore]
        public bool EnqueteRespondida { get; set; }

        [JsonIgnore]
        [Ignore]
        public ImageSource ImageSource
        {
            get
            {
                ImageSource imgSrc = null;

                if (this.EnqueteRespondida)
                    imgSrc = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_com_resposta.png"));
                    //imgSrc = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("enquete_respondida.png"));
                else
                    imgSrc = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_com_seta_sem_resposta.png"));
                //imgSrc = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("circuloEnquete.png"));

                return imgSrc;
            }
        }

        public EnumTipoEnquete Tipo { get; set; }

        public EnumStatusEnquete Status { get; set; }

        public int UsuarioId { get; set; }

        public int? EmpresaId { get; set; }

        public bool TemVoucher{ get; set; }

        public int QtdePush { get; set; }
    }

    public enum EnumStatusEnquete
    {
        Ativa = 0,
        Inativa = 1,
        Publicada = 2,
        Despublicada = 3
    }
}

