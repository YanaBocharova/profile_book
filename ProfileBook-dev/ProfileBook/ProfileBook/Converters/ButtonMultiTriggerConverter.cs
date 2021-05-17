using System;
using System.Globalization;
using Xamarin.Forms;

namespace ProfileBook.Converters
{
    public class ButtonMultiTriggerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value > 0;


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}