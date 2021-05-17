using System;
using System.Globalization;
using Xamarin.Forms;

namespace ProfileBook.Converters
{
    public class ItemTappedEventArgsToTapItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemTappedEventArgs eventArgs = value as ItemTappedEventArgs;
            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}