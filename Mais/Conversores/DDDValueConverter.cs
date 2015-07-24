using System;
using Xamarin.Forms;

namespace Mais
{
	public class DDDValueConverter : IValueConverter
	{
		#region IValueConverter implementation

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value == null ? 0 : (int)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return 0;
		}

		#endregion
	}
}

