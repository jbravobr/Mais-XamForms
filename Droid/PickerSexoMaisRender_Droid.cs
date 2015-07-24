using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Mais;
using Mais.Droid;
using Android.Widget;

[assembly: ExportRenderer(typeof(PickerSexoMais), typeof(PickerSexoMaisRender_Droid))]
namespace Mais.Droid
{
	public class PickerSexoMaisRender_Droid : PickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
				return;

			//Control.Hint = "Sexo";
			SetNativeControl(Control);
		}
	}
}

