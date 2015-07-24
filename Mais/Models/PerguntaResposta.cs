using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Mais
{
	[Table("PerguntaResposta")]
	public class PerguntaResposta
	{
		[JsonIgnore]
		[PrimaryKey,AutoIncrement]
		public int Id{ get; set; }

		public int PerguntaId { get; set; }

		public int RespostaId { get; set; }

		public string TextoResposta { get; set; }

		public int UsuarioId { get; set; }

		public double percentual { get; set; }

	}
}

