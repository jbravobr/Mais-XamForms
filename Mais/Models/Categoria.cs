using System;

using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using PropertyChanged;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mais
{
    [ImplementPropertyChanged]
    [Table("Categoria")]
    public class Categoria
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public string Nome { get; set; }

        [Ignore]
        public bool Selecionada { get; set; }

        public string Imagem { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int UsuarioId{ get; set; }

        [ForeignKey(typeof(Enquete))]
        [JsonIgnore]
        public int EnqueteId{ get; set; }
    }
}

