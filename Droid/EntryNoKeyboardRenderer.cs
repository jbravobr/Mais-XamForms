using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Mais.Droid;
using Android.Views.InputMethods;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(EntryNoKeyboardRenderer))]
namespace Mais.Droid
{
	public class EntryNoKeyboardRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
				return;

			SetNativeControl(Control);
		}
	}
}

