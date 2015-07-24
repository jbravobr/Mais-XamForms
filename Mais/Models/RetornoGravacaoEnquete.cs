using System;
using SQLite.Net.Attributes;

namespace Mais
{
	[Table("RetornoGravacaoEnquete")]
	public class RetornoGravacaoEnquete
	{
		[AutoIncrement,PrimaryKey]
		public int Id { get; set; }

		public int EnqueteId { get; set; }

		public int PerguntaId { get; set; }
	}
}

