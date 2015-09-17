using System;

using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mais
{
    [Table("Pergunta")]
    public class Pergunta
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        [MaxLength(240),NotNull]
        public string TextoPergunta { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
        public List<Resposta> Respostas { get; set; }

        public int PerguntaServerId { get; set; }

        [JsonIgnore,Ignore]
        public bool correta { get; set; }
    }
}

