using System;
using SQLite.Net.Attributes;

namespace Mais
{
	public class FacebookInfos
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string access_token { get; set; }

		public string user_id { get; set; }
	}
}

