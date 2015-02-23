using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace SignalMan.App.Converters
{
    public class InverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value != null && value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
