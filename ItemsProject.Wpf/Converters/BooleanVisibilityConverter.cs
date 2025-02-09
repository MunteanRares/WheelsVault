using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace ItemsProject.Wpf.Converters
{
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (bool)value;
            if (isVisible)
            { 
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            if (visibility == Visibility.Hidden)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
