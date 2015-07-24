using System;
using Mais;
using Mais.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryDDDMais), typeof(EntryDDDMaisRenderer_Droid))]
namespace Mais.Droid
{
	public class EntryDDDMaisRenderer_Droid : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
				return;

			Control.Hint = "DDD";
			SetNativeControl(Control);
		}
	}
}

