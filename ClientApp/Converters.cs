using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ClientApp
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal inValue = (decimal)value;

            return (int)Math.Truncate(inValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int retVal = (int)value;
            return (decimal)retVal;
        }
    }

    public class YearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal retVal;
            decimal inValue = (decimal)value;

            retVal = inValue - Math.Truncate(inValue);
            return (int)(retVal * 10000);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int retVal = (int)value;
            return (decimal)retVal / 10000;
        }
    }
}
