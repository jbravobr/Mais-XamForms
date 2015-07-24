using System;
using Mais;
using Mais.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePickerMais), typeof(DatePickerMaisRenderer_Droid))]
namespace Mais.Droid
{
	public class DatePickerMaisRenderer_Droid : DatePickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
				return;

			Control.Text = "Nascimento";
			Control.SetTextColor(Android.Graphics.Color.DimGray);

			Control.TextChanged += (sender, events) =>
			{
				if (((Android.Widget.EditText)sender).Text != "Nascimento")
					((Android.Widget.EditText)sender).SetTextColor(Android.Graphics.Color.Black);
			};
			SetNativeControl(Control);
		}
	}
}

