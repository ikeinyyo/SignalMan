using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SignalMan.App.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility ValueForTrue { get; set; }
        public Visibility ValueForFalse { get; set; }

        public BoolToVisibilityConverter()
        {
            ValueForTrue = Visibility.Visible;
            ValueForFalse = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = ValueForFalse;

            if(value != null &&  value is bool && (bool)value)
            {
                visibility = ValueForTrue;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
