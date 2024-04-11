using System;
using System.Globalization;
using System.Windows.Media;
using MVVM;

namespace Main
{
    public class OpacityConverter : SimpleConverter<bool>
    {
        protected override object OnConvert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value)
            {
                return (double)1;

            }
            else
            {
                return (double)0.5;
            }
        }
    }

    public class ForegroundConverter : SimpleConverter<bool>
    {
        protected override object OnConvert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Black;
            }

        }
    }
}
