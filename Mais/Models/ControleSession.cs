using System;
using SQLite.Net.Attributes;

namespace Mais
{
	[Table("ControleSession")]
	public class ControleSession
	{
		public int Id { get; set; }

		public bool Logado { get; set; }
	}
}

