using System;
using Xamarin.Forms;

namespace Mais
{
    public class SexoValueConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return new object();
            else
                return (EnumSexo)Enum.Parse(typeof(EnumSexo), value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object();
        }

        #endregion
    }
}

