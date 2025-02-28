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
    public partial class SplashScreenView : MvxWpfView
    {
        public SplashScreenView()
        {
            InitializeComponent();
            Window window = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowState = WindowState.Normal;
            window.MinHeight = 400;
            window.MinWidth = 600;
            window.Height = 400;
            window.Width = 600;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += WorkerDoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync();
        }

        void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            AsyncContext.Run(() => Task.Delay(2000));
            AsyncContext.Run(Mvx.IoCProvider.Resolve<IDatabaseData>().DefaultHotwheelsDbPopulation);
        }

        void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<BaseViewModel>();
        }
    }
}
