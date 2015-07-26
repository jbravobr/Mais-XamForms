using System;
using SQLite.Net.Attributes;

namespace Mais
{
    [Table("UsuarioCategoria")]
    public class UsuarioCategoria
    {
        [AutoIncrement,PrimaryKey]
        public int Id{ get; set; }

        public int CategoriaId { get; set; }
    }
}

