using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Mais;
using Mais.iOS;
using Foundation;
using UIKit;

[assembly: ExportRenderer(typeof(DatePickerMais), typeof(DatePicker_iOS))]
namespace Mais.iOS
{
	public class DatePicker_iOS : DatePickerRenderer
	{
		// Override the OnElementChanged method so we can tweak this renderer post-initial setup
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
				return;

			//Control.AttributedPlaceholder = new NSAttributedString("Nascimento");
			Control.Text = "Nascimento";
			Control.TextColor = UIColor.LightGray;

			Control.AllTouchEvents += (sender, events) =>
			{
				if (((UITextField)sender).Text == "Nascimento")
				{
					((UITextField)sender).Text = string.Empty;
				}
			};
			SetNativeControl(Control);
		}
	}
}

