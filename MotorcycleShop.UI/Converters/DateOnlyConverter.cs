using System;
using System.Globalization;
using System.Windows.Data;

namespace MotorcycleShop.UI.Converters
{
    /// <summary>
    /// Конвертер для преобразования между DateOnly и DateTime
    /// </summary>
    public class DateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
            {
                return dateOnly.ToDateTime(TimeOnly.MinValue);
            }
            else if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);
            }
            else if (value is DateOnly dateOnly)
            {
                return dateOnly.ToDateTime(TimeOnly.MinValue);
            }
            return value;
        }
    }
}