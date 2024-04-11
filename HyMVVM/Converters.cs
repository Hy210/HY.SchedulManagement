using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace HyMVVM
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
}
