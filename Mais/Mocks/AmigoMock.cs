using System;
using System.Collections.Generic;

namespace Mais
{
	public static class AmigoMock
	{
		public static List<Amigo> RetornaListaMockAmigo()
		{
			return new List<Amigo>
			{
				new Amigo { Id = 1, Nome = "Messi", Selecionado = false },
				new Amigo { Id = 2, Nome = "Ronaldo", Selecionado = false },
				new Amigo { Id = 3, Nome = "Batman", Selecionado = false },
				new Amigo { Id = 4, Nome = "Flash", Selecionado = false },
				new Amigo { Id = 5, Nome = "Superman", Selecionado = false },
				new Amigo { Id = 6, Nome = "Dilma", Selecionado = false },
				new Amigo { Id = 7, Nome = "Obama", Selecionado = false },
				new Amigo { Id = 8, Nome = "Brad Pitt", Selecionado = false },
				new Amigo { Id = 9, Nome = "Cid Moreira", Selecionado = false },
				new Amigo { Id = 10, Nome = "Mônica", Selecionado = false }
			};
		}
	}
}

