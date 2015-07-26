using System;

using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mais
{
    [Table("Usuario")]
    public class Usuario
    {
        [PrimaryKey]
        public int Id { get; set; }

        public EnumSexo Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]
        public List<Categoria> Categorias { get; set; }

        public string CategoriaMobileSelection { get; set; }

        public string Email { get; set; }

        public string Nome { get; set; }

        public string Senha { get; set; }

        public string DDD { get; set; }

        public string Telefone { get; set; }

        public string Municipio { get; set; }
    }
}

