using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ItemsProject.Wpf.Converters
{
    public class MultiBooleanVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is bool isEditing &&
                values[1] is bool isCustom)
            {
                // Combine the two boolean values and return the appropriate Visibility
                if (isEditing || isCustom)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { false, false }; // We don't need to support ConvertBack for this case.
        }
    }
}
