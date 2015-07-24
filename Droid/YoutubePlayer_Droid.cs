using System;
using Xamarin.Forms;
using Mais.Droid;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System.Net;
using System.IO;
using Android.Content;

[assembly: Dependency(typeof(YoutuvePlayer_Droid))]
namespace Mais.Droid
{
	public class YoutuvePlayer_Droid : IYoutubePlayer
	{
		#region IYoutubePlayer implementation

		public void PlayVideo(string url)
		{
			var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
			Forms.Context.StartActivity(Intent.CreateChooser(intent, "video"));
		}

		#endregion
		
	}
}

