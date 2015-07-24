using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Mais
{
	[Table("AmigoEnvioEnquete")]
	public class AmigoEnvioEnquete
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }

		public string NroTelefoneUsuarioAmigo { get; set; }

		public int IdUsuarioAmigo { get; set; }

		public string NomeUsuarioAmigo { get; set; }

		[ForeignKey(typeof(Enquete))]
		public int EnqueteId { get; set; }
	}
}

