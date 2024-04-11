using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace HY.SchedulManagement
{
    public abstract class SimpleConverter<T> : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is T tv)
            {
                return OnConvert(tv, targetType, parameter, culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return OnConvertBack(value, targetType, parameter, culture);
        }

        protected abstract object OnConvert(T value, Type targetType, object parameter, CultureInfo culture);

        protected abstract T OnConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }

    public class BoolToForeGroundConverter : SimpleConverter<bool>
    {
        protected override object OnConvert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Gray);
            //return value ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Red);
        }
        protected override bool OnConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as SolidColorBrush;
            return color.Color == Colors.White;
        }
    }

    public class BoolToBackGroundConverter : SimpleConverter<bool>
    {
        protected override object OnConvert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? new SolidColorBrush(Colors.YellowGreen) : new SolidColorBrush(Colors.Transparent);
        }

        protected override bool OnConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;
            return brush.Color == Colors.SkyBlue;
        }
    }


}
