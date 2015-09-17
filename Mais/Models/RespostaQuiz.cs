using System;
using SQLite.Net.Attributes;

namespace Mais
{
    [Table("RespostaQuiz")]
    public class RespostaQuiz
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public int RespostaId { get; set; }

        public int PerguntaId { get; set; }

        public int EnqueteId { get; set; }
    }
}

