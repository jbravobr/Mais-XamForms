using System;
using SQLite.Net.Attributes;

namespace Mais
{
	public class Banner
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string FileName { get; set; }

		public string Url { get; set; }

		public DateTime DataValidade { get; set; }
	}
}

