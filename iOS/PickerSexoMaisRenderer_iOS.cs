using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Mais;
using Mais.iOS;
using Foundation;
using UIKit;

[assembly: ExportRenderer(typeof(PickerSexoMais), typeof(PickerSexoMaisRenderer_iOS))]
namespace Mais.iOS
{
	public class PickerSexoMaisRenderer_iOS : PickerRenderer
	{
		// Override the OnElementChanged method so we can tweak this renderer post-initial setup
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
				return;
			
			Control.AttributedPlaceholder = new NSAttributedString("Sexo");
			SetNativeControl(Control);
		}
	}
}

