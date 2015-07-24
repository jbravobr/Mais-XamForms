using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Acr.UserDialogs;
using XLabs.Forms;
using Xamarin;

namespace Mais.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate//global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UserDialogs.Init();
            global::Xamarin.Forms.Forms.Init();
            Insights.Initialize("0d729b1f8027a9219421908d521e3af664ae52fc");

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

