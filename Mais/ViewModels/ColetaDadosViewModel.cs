using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mais
{
	public class ColetaDadosViewModel
	{
		readonly ILogin service;

		public ColetaDadosViewModel(ILogin service)
		{
			this.service = service;
		}

		public async Task<ICollection<Categoria>> RetornarCategoriasDoServidor(int categoriaId)
		{
			if (categoriaId > 0)
				return await this.service.RetornarCategorias(categoriaId);
			else
				return await this.service.RetornarCategorias(-1);
		}
	}
}

