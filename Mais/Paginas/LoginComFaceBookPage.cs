using System;

using Xamarin.Forms;

namespace Mais
{
	public class LoginComFaceBookPage : ContentPage
	{
		public LoginComFaceBookPage()
		{
			Content = new StackLayout
			{ 
				Children =
				{
					new Label { Text = "Iniciando autenticação..." }
				}
			};
		}
	}
}


