using System;
using System.Windows.Data;

namespace PortfolioManager3.Converters
{
    public class ProfitAndReturnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            double Profit = System.Convert.ToDouble(value);
            return Profit >= 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
