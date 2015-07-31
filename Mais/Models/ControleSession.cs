using System;
using SQLite.Net.Attributes;

namespace Mais
{
    [Table("ControleSession")]
    public class ControleSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public bool Logado { get; set; }

        public bool ViuTutorial { get; set; }
    }
}

