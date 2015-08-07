using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(Mais.CustomEntry), typeof(Mais.Droid.CustomEntry_Droid))]
namespace Mais.Droid
{
    public class CustomEntry_Droid : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
                Control.SetBackgroundDrawable(null);

                SetNativeControl(Control);
            }
        }
    }
}

