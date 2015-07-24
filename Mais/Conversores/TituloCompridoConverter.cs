using System;
using Xamarin.Forms;

namespace Mais
{
	public class TituloCompridoConverter : IValueConverter
	{
		#region IValueConverter implementation

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var titulo = value.ToString();

			return titulo.Length > 32 ? string.Format("{0}...", titulo.Substring(0, 31)) : value.ToString();

		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return new object();
		}

		#endregion
	}
}

