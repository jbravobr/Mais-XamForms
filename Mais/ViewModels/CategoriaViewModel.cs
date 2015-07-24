using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using PropertyChanged;
using System.Threading.Tasks;

namespace Mais
{
	[ImplementPropertyChanged]
	public class CategoriaViewModel
	{
		public ObservableCollection<Categoria> Categorias { get; set; }

		public CategoriaViewModel()
		{
			MontaCategoriasFake();
		}

		public async Task MontaCategoriasFake()
		{
			var dbCat = new Repositorio<Categoria>();

			var lista = await dbCat.RetornarTodos();

			this.Categorias = new ObservableCollection<Categoria>(lista.OrderBy(x => x.Nome));
		}

		public async Task<List<Categoria>> GetCategorias()
		{
			var dbCat = new Repositorio<Categoria>();
			var lista = await dbCat.RetornarTodos();
			return lista.OrderBy(x => x.Nome).ToList();
		}
	}
}

