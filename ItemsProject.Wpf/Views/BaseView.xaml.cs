using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Xceed.Wpf.Toolkit;

namespace ItemsProject.Wpf.Views
{
    public partial class BaseView : MvxWpfView
    {
        public BaseView()
        {
            InitializeComponent();
        }

        private void EditFolderNameTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox editTextBox = (TextBox)sender;
            Keyboard.Focus(editTextBox);
            editTextBox.CaretIndex = editTextBox.Text.Length;
        }
    }
}
