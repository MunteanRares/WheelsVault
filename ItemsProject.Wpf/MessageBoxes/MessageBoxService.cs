using System.Windows;
using ItemsProject.Core.Messages;

namespace ItemsProject.Wpf.MessageBoxes
{
    public class MessageBoxService : IMessageBoxService
    {
        public bool ShowMessageBox(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
