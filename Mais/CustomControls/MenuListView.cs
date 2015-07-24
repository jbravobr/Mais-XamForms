using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Mais
{
	public class MenuListView : ListView
	{
		public MenuListView()
		{
			List<MenuItem> data = new MenuListData();

			ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			SeparatorVisibility = SeparatorVisibility.Default;
			ItemTemplate = new DataTemplate(typeof(MenuViewCell));
		}
	}
}

