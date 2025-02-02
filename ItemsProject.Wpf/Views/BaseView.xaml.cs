using ItemsProject.Core.Messages;
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
    }
}
