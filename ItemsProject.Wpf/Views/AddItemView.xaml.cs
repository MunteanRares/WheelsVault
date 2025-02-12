using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ItemsProject.Core.Messages;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Wpf.Views
{
    public partial class AddItemView : MvxWindow
    {
        private readonly IMvxMessenger _messenger;
        public AddItemView()
        {
            InitializeComponent();
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Topmost = true;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);   
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            ChangeWindowStateMessage message = new ChangeWindowStateMessage(this, true);
            _messenger.Publish(message);
        }
    }
}
