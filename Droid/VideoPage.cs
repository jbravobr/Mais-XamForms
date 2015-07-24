using System;

using Xamarin.Forms;
using Mais;
using Mais.Droid;

using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Webkit;

[assembly: ExportRenderer(typeof(Mais.VideoPage), typeof(Mais.Droid.VideoPage))]
namespace Mais.Droid
{
    public class VideoPage : PageRenderer
    {
        Android.Views.View view;

        public VideoPage()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            try
            {
                if (e.OldElement != null || Element == null)
                    return;
				
                var activity = this.Context as Activity;

                activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
              				
                var _videoView = activity.LayoutInflater.Inflate(Resource.Layout.VideoPlayerLayout, this, false);
                view = _videoView;

                var _webViewPlayer = _videoView.FindViewById<Android.Webkit.WebView>(Resource.Id.VideoPlayer);


                //var uri = Android.Net.Uri.Parse("https://m.youtube.com/watch?v=AJMiYMlH4NY");
                _webViewPlayer.Settings.JavaScriptEnabled = true;
                //_webViewPlayer.Settings.SetPluginState(WebSettings.PluginState.On);
                _webViewPlayer.Settings.MediaPlaybackRequiresUserGesture = false;
                //_webViewPlayer.LoadUrl("http://www.youtube.com/embed/AJMiYMlH4NY");
                _webViewPlayer.LoadData(String.Format("<html><body><iframe allowfullscreen=\"allowfullscreen\" width=\"100%\" height=\"100%\" src=\"{0}?autoplay=1&fullscreen=1&modestbranding=1&showinfo=0&fs=0\" frameborder=\"0\" allowfullscreen></iframe></body></html>", App.UrlVideo), "text/html", "UTF-8");
                _webViewPlayer.SetWebChromeClient(new WebChromeClient());


                AddView(view);
					
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }
    }
}

