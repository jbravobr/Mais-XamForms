using System;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Mais
{
	public class CriaNovaEnqueteView : ContentView
	{
		public Entry Resposta;
		public StackLayout RespostasStack;
		public NovaEnqueteViewModel model;

		public CriaNovaEnqueteView()
		{
			//this.BindingContext = model = new NovaEnqueteViewModel();

			var Titulo = new Entry
			{
				Placeholder = AppResources.TextoPlaceholderTituloNaEnquete,
				WidthRequest = 100
			};
			Titulo.SetBinding<NovaEnqueteViewModel>(Entry.TextProperty, n => n.Pergunta.TextoPergunta);
			
			RespostasStack = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(5, 5, 5, 5)
			};

			for (int i = 0; i < 2; i++)
			{
				Resposta = new Entry
				{
					Placeholder = AppResources.TextoPlaceholderTituloNaRespostaEnquete,
					WidthRequest = 100
				};
				RespostasStack.Children.Add(Resposta);
			}
				
			var mainlayout = new StackLayout
			{
				Padding = 5,
				Children =
				{
					Titulo,
					RespostasStack
				}
			};

			this.Content = mainlayout;
		}
	}
}


