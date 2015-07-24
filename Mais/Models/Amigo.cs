using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using PropertyChanged;

namespace Mais
{
    [ImplementPropertyChanged]
    [Table("Amigo")]
    public class Amigo
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public string Nome { get; set; }

        public int UsuarioId { get; set; }

        public string NroTelefone { get; set; }

        [Ignore]
        public bool Selecionado { get; set; }
    }
}

