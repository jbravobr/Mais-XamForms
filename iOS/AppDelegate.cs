using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Acr.UserDialogs;
using XLabs.Forms;
using Xamarin;
using Pushwoosh;

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

            PushNotificationManager pushmanager = PushNotificationManager.PushManager;
            pushmanager.Delegate = this;

            if (options != null)
            {
                if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                { 
                    pushmanager.HandlePushReceived(options);
                }
            }

            pushmanager.RegisterForPushNotifications();

            try
            {
                var token = PushNotificationManager.PushManager.GetPushToken;
                
                if (!String.IsNullOrEmpty(token))
                    App.PushWooshToken = token;
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            PushNotificationManager.PushManager.HandlePushRegistration(deviceToken);
            try
            {
                var token = PushNotificationManager.PushManager.GetPushToken;

                if (!String.IsNullOrEmpty(token))
                    App.PushWooshToken = token;
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }

        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            PushNotificationManager.PushManager.HandlePushRegistrationFailure(error);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            PushNotificationManager.PushManager.HandlePushReceived(userInfo);
        }
    }
}

