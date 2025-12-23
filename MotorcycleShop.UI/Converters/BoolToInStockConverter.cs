using System;
using System.Globalization;
using System.Windows.Data;

namespace MotorcycleShop.UI.Converters
{
    /// <summary>
    /// Конвертер для преобразования булевого значения наличия в строковое представление
    /// </summary>
    public class BoolToInStockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isInStock)
            {
                return isInStock ? "В наличии" : "Нет в наличии";
            }
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}