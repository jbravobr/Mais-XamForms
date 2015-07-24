using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Mais
{
	[Table("SubCategoria")]
	public class SubCategoria
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }

		[MaxLength(40)]
		public string Descricao { get; set; }

		[ForeignKey(typeof(Categoria))]
		public int CategoriaId { get; set; }
	}
}

