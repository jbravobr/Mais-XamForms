using System;

using Autofac;

namespace Mais
{
	public static class AutofacConfiguration
	{
		public static IContainer Init()
		{
			var builder = new ContainerBuilder();

			// Registrando Serviços e dependências.
			builder.RegisterInstance(new UsuarioService()).As<ILogin>();

			// Registrando ViewModels.
			builder.RegisterType<RootViewModel>();
			builder.RegisterType<CadastroViewModel>();
			builder.RegisterType<LoginViewModel>();
			builder.RegisterType<NovaEnqueteViewModel>();
			builder.RegisterType<EnqueteViewModel>();
			builder.RegisterType<ColetaDadosViewModel>();
			builder.RegisterType<CadastroViewModel>();
			builder.RegisterType<PerguntaViewModel>();
			builder.RegisterType<RespostaViewModel>();
			builder.RegisterType<GeolocalizacaoViewModel>();

			return builder.Build();
		}
	}
}
