using System;

namespace Mais
{
	public static class UsuarioMock
	{
		public static Usuario MockUsuario()
		{
			return new Usuario
			{
				DataNascimento = new DateTime(1983, 7, 21),
				DDD = "21",
				Email = "jbravo.br@gmail.com",
				Municipio = "Rio de Janeiro",
				Nome = "Rodrigo Amaro",
				Senha = "123",
				Sexo = (int)EnumSexo.Masculino,
				Telefone = "97551-9377" 
			};
		}
	}
}

