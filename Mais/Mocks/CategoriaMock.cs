using System;
using System.Collections.Generic;

namespace Mais
{
	public static class CategoriaMock
	{
		public static List<Categoria> RetornaListaMockCategoria()
		{
			return new List<Categoria>
			{
				new Categoria { Id = 1, Nome = "Saúde", Selecionada = false },
				new Categoria { Id = 2, Nome = "Esportes", Selecionada = false },
				new Categoria { Id = 3, Nome = "Cinema", Selecionada = false },
				new Categoria { Id = 4, Nome = "Teatro", Selecionada = false },
				new Categoria { Id = 5, Nome = "Política", Selecionada = false },
				new Categoria { Id = 6, Nome = "Celebridade", Selecionada = false },
				new Categoria { Id = 7, Nome = "Carros", Selecionada = false },
				new Categoria { Id = 8, Nome = "Artes", Selecionada = false },
				new Categoria { Id = 9, Nome = "Teatro", Selecionada = false },
				new Categoria { Id = 10, Nome = "Ciência", Selecionada = false }
			};
		}
	}
}

