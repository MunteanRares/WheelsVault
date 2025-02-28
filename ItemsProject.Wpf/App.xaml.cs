using System.Configuration;
using System.Data;
using System.Windows;
using ItemsProject.Core.Data;
using ItemsProject.WPF;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace ItemsProject.Wpf
{
    public partial class App : MvxApplication
    {
        protected override async void RegisterSetup()
        {
            this.RegisterSetupType<Setup>();
        }
    }

}
