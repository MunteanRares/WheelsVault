using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Navigation;
using MvvmCross;
using MvvmCross.ViewModels;
using Nito.AsyncEx;
using MvvmCross.Plugin.Messenger;
using ItemsProject.Core.Messages;
using System.Collections.ObjectModel;

namespace ItemsProject.Core.ViewModels
{
    public class LoadListCollectionViewModel : MvxViewModel<LoadListCollectionPrepareModel>
    {
        private readonly IDataService _dataService;
        private readonly IMvxNavigationService _nav;
        private readonly IMvxMessenger _messenger;
        private HwListCollectionViewModel _hwListVm = Mvx.IoCProvider.Resolve<HwListCollectionViewModel>();
        public LoadListCollectionViewModel(IDataService dataService, IMvxNavigationService nav, IMvxMessenger messenger)
        {
            _dataService = dataService;
            _nav = nav;
            _messenger = messenger;
        }

        /// <summary>
        /// ////////// BOOLEANS
        /// </summary>
        private bool _isWorkRunning = false;


        /// <summary>
        //////////////////// GENERAL FUNCTIONS
        /// </summary>
        public void LoadItemsForFolder()
        {
            LoadListCollectionPrepareModel param = new LoadListCollectionPrepareModel(SelectedFolder, Folders);
            _hwListVm.Prepare(param);
        }

        /// <summary>
        ////////////////// WORKER FUNCTIONS
        /// </summary>
        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            AsyncContext.Run(async () => await Task.Delay(500));
            AsyncContext.Run(LoadItemsForFolder);
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            LoadListCollectionMessage message = new LoadListCollectionMessage(this, _hwListVm);
            _messenger.Publish(message);
            _isWorkRunning = false;
        }

        /// <summary>
        ////////////////////////// OVERRIDE FUNCTIONS
        /// </summary>
        public override Task Initialize()
        {
            if(_isWorkRunning) return Task.CompletedTask;

            _isWorkRunning = true;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
            return base.Initialize();
        }

        public override void Prepare(LoadListCollectionPrepareModel parameter)
        {
            SelectedFolder = parameter.SelectedFolder;
            Folders = parameter.Folders;
        }

        /// <summary>
        ///////////////// PROPERTIES
        /// </summary>
        private FolderModel _selectedFolder;
        public FolderModel SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                SetProperty(ref _selectedFolder, value);
            }
        }

        private ObservableCollection<FolderModel> _folders;
        public ObservableCollection<FolderModel> Folders
        {
            get { return _folders; }
            set
            {
                SetProperty(ref _folders, value);
            }
        }
    }
}
