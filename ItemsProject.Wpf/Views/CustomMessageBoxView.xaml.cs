using System.ComponentModel;
using ItemsProject.Core.Messages;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Wpf
{
    public partial class CustomMessageBoxView : MvxWindow
    {
        private readonly IMvxMessenger _messenger;
        public CustomMessageBoxView()
        {
            InitializeComponent();
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Topmost = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            ChangeWindowStateMessage message = new ChangeWindowStateMessage(this, true);
            _messenger.Publish(message);
        }
    }
}
