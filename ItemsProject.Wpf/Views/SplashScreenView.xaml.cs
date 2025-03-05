using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using DevExpress.Entity.Model.Metadata;
using ItemsProject.Core.Data;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Wpf.Views;
using Nito.AsyncEx;

namespace ItemsProject.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SplashScreenView.xaml
    /// </summary>
    public partial class SplashScreenView : MvxWindow
    {
        public SplashScreenView()
        {
            Window window = Application.Current.MainWindow;
            window.ShowInTaskbar = false;
            window.Hide();
            InitializeComponent();
        }
    }
}
