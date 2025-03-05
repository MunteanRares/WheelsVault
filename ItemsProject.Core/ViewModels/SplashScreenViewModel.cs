using ItemsProject.Core.Data;
using System.ComponentModel;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Navigation;
using MvvmCross;
using MvvmCross.ViewModels;
using Nito.AsyncEx;

namespace ItemsProject.Core.ViewModels
{
    public class SplashScreenViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _nav;

        public SplashScreenViewModel(IMvxNavigationService nav)
        {
            _nav = nav;
        }

        void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            AsyncContext.Run(() => Task.Delay(2000));
            AsyncContext.Run(Mvx.IoCProvider.Resolve<IDatabaseData>().DefaultHotwheelsDbPopulation);
        }

        void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _nav.Close(this);
            AsyncContext.Run(() => _nav.Navigate<BaseViewModel>());
        }

        public override Task Initialize()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += WorkerDoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync();
            return base.Initialize();
        }
    }
}
