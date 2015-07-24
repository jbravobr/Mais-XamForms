using System;

using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;
using PropertyChanged;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Mais
{
    [ImplementPropertyChanged]
    [Table("Resposta")]
    public class Resposta
    {
        [JsonIgnore]
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100),NotNull]
        public string TextoResposta { get; set; }

        [JsonIgnore]
        public DateTime? DataResposta { get; set; }

        public string Imagem { get; set; }

        [JsonIgnore]
        public ImageSource ImgRespostaSource
        {
            get
            {
                ImageSource img = null;

                if (!String.IsNullOrEmpty(this.Imagem))
                    img = ImageSource.FromFile(DependencyService.Get<ISaveAndLoadFile>().GetImage(this.Imagem));

                return img;
            }
        }

        [JsonIgnore]
        public bool TemImagemResposta
        {
            get
            {
                return String.IsNullOrEmpty(this.Imagem);
            }
        }

        [JsonIgnore]
        public bool Respondida { get; set; }

        [JsonIgnore]
        [ForeignKey(typeof(Pergunta))]
        public int PerguntaId { get; set; }

        public int PerguntaServerId { get; set; }

        public int RespostaServerId { get; set; }

        [JsonIgnore]
        [Ignore]
        public ImageSource ImgSource
        {
            get
            {
                ImageSource img = null;

                if (!this.Respondida)
                    img = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("circulo_resposta.png"));
                else
                    img = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemCaminho("circulo_reposta_selecionada.png"));

                return img;
            }
        }

        public double percentualResposta { get; set; }

        public bool temVoucher { get; set; }

        [JsonIgnore]
        public double percent
        {
            get
            {
                return this.percentualResposta / 100;
            }
        }
    }
}

