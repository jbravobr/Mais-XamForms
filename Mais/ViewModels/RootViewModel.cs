using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Mais
{
	public class RootViewModel
	{
		public ICommand btnEntrar_Click { get; protected set; }

		public ICommand btnCadastrar_Click { get; protected set; }

		public INavigation Navigation { get; protected set; }

		public RootViewModel()
		{

			this.btnEntrar_Click = new Command(async () => await this.IrParaPaginaDeLogin());
			this.btnCadastrar_Click = new Command(async () => await this.IrParaPaginaCadastro());
		}

		public void ConfiguraNavigation(INavigation navigation)
		{
			this.Navigation = navigation;
		}

		private async Task IrParaPaginaDeLogin()
		{
			var paginaLogin = Activator.CreateInstance<LoginPage>();
			await this.Navigation.PushModalAsync(paginaLogin);
		}

		private async Task IrParaPaginaCadastro()
		{
			var paginaCadastro = Activator.CreateInstance<CadastroPage>();
			await this.Navigation.PushModalAsync(paginaCadastro);
		}
	}
}

