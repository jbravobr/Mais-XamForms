using System;

using Android.App;
using Android.Support.V4.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Org.Json;
using ArelloMobile.Push;
using ArelloMobile.Push.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Acr.UserDialogs;
using Xamarin;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Mais.Droid
{
    [Activity(Label = "Mais", 
        LaunchMode = LaunchMode.SingleTask,
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation),
        IntentFilter(new string[]{ "com.aplicativo.mais.MESSAGE" }, Categories = new string[]{ "android.intent.category.DEFAULT" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        LocalMessageBroadcastReceiver mMessageReceiver;
        LocalRegisterBroadcastReceiver mRegisterReceiver;

        bool mBroadcastPush = true;

        public static HttpClient RetornaClientHttp()
        {
            var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri(Mais.Constants.baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return client;
        }

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
            //Geofence.Plugin.CrossGeofence.Initialize<CrossGeofenceListener>();

            mMessageReceiver = new LocalMessageBroadcastReceiver();
            mMessageReceiver.activity = this;

            mRegisterReceiver = new LocalRegisterBroadcastReceiver();
            mRegisterReceiver.activity = this;
            registerReceivers();

            ArelloMobile.Push.PushManager manager = ArelloMobile.Push.PushManager.GetInstance(this);
            manager.OnStartup(this);

            //Register for push!
            manager.RegisterForPushNotifications();

            checkMessage(Intent);

            LoadApplication(new App());
        }

        protected override void OnNewIntent(Intent intent)
        {
            checkMessage(intent);
        }

        public void checkMessage(Intent intent)
        {
            if (null != intent)
            {
                if (intent.HasExtra(PushManager.PushReceiveEvent))
                {
                    doOnMessageReceive(intent.Extras.GetString(PushManager.PushReceiveEvent));
                }
                else if (intent.HasExtra(PushManager.RegisterEvent))
                {
                    doOnRegistered(intent.Extras.GetString(PushManager.RegisterEvent));
                }
                else if (intent.HasExtra(PushManager.UnregisterEvent))
                {
                    doOnUnregisteredError(intent.Extras.GetString(PushManager.UnregisterEvent));
                }
                else if (intent.HasExtra(PushManager.RegisterErrorEvent))
                {
                    doOnRegisteredError(intent.Extras.GetString(PushManager.RegisterErrorEvent));
                }
                else if (intent.HasExtra(PushManager.UnregisterErrorEvent))
                {
                    doOnUnregistered(intent.Extras.GetString(PushManager.UnregisterErrorEvent));
                }

                resetIntentValues();
            }
        }

        public void doOnRegistered(String registrationId)
        {
            // code to run if device has succesfully registered
            var pushWooshID = registrationId;
            App.PushWooshToken = pushWooshID;
        }

        public void doOnRegisteredError(String errorId)
        {
            // code to run if device failed to register
        }

        public void doOnUnregistered(String registrationId)
        {
            // code to run if device has succesfully unregistered
        }

        public void doOnUnregisteredError(String errorId)
        {
            // code to run if device failed to unregister properly
        }

        public void doOnMessageReceive(String message)
        {
            // code to run when device receives notification
        }

        /**
* Will check main Activity intent and if it contains any Pushwoosh data,
* will clear it
*/
        private void resetIntentValues()
        {
            Intent mainAppIntent = Intent;

            if (mainAppIntent.HasExtra(PushManager.PushReceiveEvent))
            {
                mainAppIntent.RemoveExtra(PushManager.PushReceiveEvent);
            }
            else if (mainAppIntent.HasExtra(PushManager.RegisterEvent))
            {
                mainAppIntent.RemoveExtra(PushManager.RegisterEvent);
            }
            else if (mainAppIntent.HasExtra(PushManager.UnregisterEvent))
            {
                mainAppIntent.RemoveExtra(PushManager.UnregisterEvent);
            }
            else if (mainAppIntent.HasExtra(PushManager.RegisterErrorEvent))
            {
                mainAppIntent.RemoveExtra(PushManager.RegisterErrorEvent);
            }
            else if (mainAppIntent.HasExtra(PushManager.UnregisterErrorEvent))
            {
                mainAppIntent.RemoveExtra(PushManager.UnregisterErrorEvent);
            }

            Intent = mainAppIntent;
        }

        protected override void OnResume()
        {
            base.OnResume();

            registerReceivers();
        }

        protected override void OnPause()
        {
            base.OnPause();

            unregisterReceivers();
        }

        public void registerReceivers()
        {
            IntentFilter intentFilter = new IntentFilter(PackageName + ".action.PUSH_MESSAGE_RECEIVE");

            if (mBroadcastPush)
            {
                RegisterReceiver(mMessageReceiver, intentFilter);
            }

            RegisterReceiver(mRegisterReceiver, new IntentFilter(PackageName + "." + PushManager.RegisterBroadCastAction));
        }

        public void unregisterReceivers()
        {
            UnregisterReceiver(mMessageReceiver);
            UnregisterReceiver(mRegisterReceiver);
        }
    }

    public class LocalMessageBroadcastReceiver : BasePushMessageReceiver
    {
        public MainActivity activity { get; set; }

        protected override void OnMessageReceive(Intent intent)
        {
            activity.doOnMessageReceive(intent.GetStringExtra(BasePushMessageReceiver.JsonDataKey));
        }
    }

    public class LocalRegisterBroadcastReceiver : RegisterBroadcastReceiver
    {
        public MainActivity activity { get; set; }

        protected override void OnRegisterActionReceive(Context p0, Intent intent)
        {
            activity.checkMessage(intent);
        }
    }
}

