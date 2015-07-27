using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Acr.UserDialogs;
using Xamarin;

namespace Mais.Droid
{
    [Activity(Label = "Mais.Droid", 
        LaunchMode = LaunchMode.SingleTask,
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public override void OnBackPressed()
        {
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            base.OnBackPressed();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(() => (Activity)Forms.Context);
            Insights.Initialize("0d729b1f8027a9219421908d521e3af664ae52fc", Forms.Context);
            Geofence.Plugin.CrossGeofence.Initialize<CrossGeofenceListener>();

            LoadApplication(new App());
        }
    }
}

