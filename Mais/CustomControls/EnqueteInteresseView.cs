using System;

using Xamarin.Forms;

namespace Mais
{
	public class EnqueteInteresseView : ContentView
	{
		public EnqueteInteresseView(Banner banner)
		{
			this.BindingContext = banner;

			var imgBanner_Click = new TapGestureRecognizer();
			imgBanner_Click.Tapped += (sender, e) => Device.OpenUri(new Uri(banner.Url));

			var imgBanner = new Image
			{
				Source = ImageSource.FromFile(DependencyService.Get<ISaveAndLoadFile>().GetImage(banner.FileName)),
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			imgBanner.GestureRecognizers.Add(imgBanner_Click);

			var contanierLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Orientation = StackOrientation.Horizontal,
				Children = { imgBanner },
				HeightRequest = 180,
				WidthRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenWidth - 10
			};

			Content = contanierLayout;
		}
	}
}


