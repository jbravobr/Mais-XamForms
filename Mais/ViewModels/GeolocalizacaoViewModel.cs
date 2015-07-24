using System;
using System.Threading.Tasks;

namespace Mais
{
	public class GeolocalizacaoViewModel
	{
		readonly ILogin service;

		public GeolocalizacaoViewModel(ILogin srv)
		{
			this.service = srv;
		}

		public async Task GravaGeolocalizacao(Geolocalizacao geo)
		{
			await this.service.GravaGeolocalizacao(geo);
		}
	}
}

