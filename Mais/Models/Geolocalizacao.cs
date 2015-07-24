using System;
using SQLite.Net.Attributes;
using Newtonsoft.Json;

namespace Mais
{
	[Table("Geolocalizacao")]
	public class Geolocalizacao
	{
		[AutoIncrement,PrimaryKey]
		[JsonIgnore]
		public int Id { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int UsuarioId { get; set; }
	}
}

