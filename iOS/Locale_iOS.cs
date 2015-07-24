using System;
using System.Threading;

using Xamarin.Forms;
using Foundation;

[assembly:Dependency(typeof(Mais.iOS.Locale_iOS))]

namespace Mais.iOS
{
	public class Locale_iOS : Mais.ILocale
	{
		public void SetLocale()
		{
			var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
			var netLocale = iosLocaleAuto.Replace("_", "-");
			var ci = new System.Globalization.CultureInfo(netLocale);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
		}

		/// <remarks>
		/// Not sure if we can cache this info rather than querying every time
		/// </remarks>
		public string GetCurrent()
		{
			var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
			var iosLanguageAuto = NSLocale.AutoUpdatingCurrentLocale.LanguageCode;
			var netLocale = iosLocaleAuto.Replace("_", "-");
			var netLanguage = iosLanguageAuto.Replace("_", "-");

			#region Debugging Info
			// prefer *Auto updating properties
			//          var iosLocale = NSLocale.CurrentLocale.LocaleIdentifier;
			//          var iosLanguage = NSLocale.CurrentLocale.LanguageCode;
			//          var netLocale = iosLocale.Replace ("_", "-");
			//          var netLanguage = iosLanguage.Replace ("_", "-");

			Console.WriteLine("nslocaleid:" + iosLocaleAuto);
			Console.WriteLine("nslanguage:" + iosLanguageAuto);
			Console.WriteLine("ios:" + iosLanguageAuto + " " + iosLocaleAuto);
			Console.WriteLine("net:" + netLanguage + " " + netLocale);

			var ci = new System.Globalization.CultureInfo(netLocale);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			Console.WriteLine("thread:  " + Thread.CurrentThread.CurrentCulture);
			Console.WriteLine("threadui:" + Thread.CurrentThread.CurrentUICulture);
			#endregion

			if (NSLocale.PreferredLanguages.Length > 0)
			{
				var pref = NSLocale.PreferredLanguages[0];
				netLanguage = pref.Replace("_", "-");
				Console.WriteLine("preferred:" + netLanguage);
			}
			else
			{
				netLanguage = "en"; // default, shouldn't really happen :)
			}
			return netLanguage;
		}
	}
}

