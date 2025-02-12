using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ItemsProject.Wpf.Helper_Functions
{
    public static class FindChildrenInTemplates
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObj) where T : DependencyObject
        {
            if (dependencyObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
