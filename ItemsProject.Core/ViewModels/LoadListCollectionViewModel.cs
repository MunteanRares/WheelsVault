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
using ItemsProject.Core.Messages.HwListVm_Messages;

namespace ItemsProject.Core.ViewModels
{
    public class LoadListCollectionViewModel : MvxViewModel<LoadListCollectionPrepareModel>
    {
        private readonly IMvxMessenger _messenger;
        private HwListCollectionViewModel _hwListVm = Mvx.IoCProvider.Resolve<HwListCollectionViewModel>();
        public LoadListCollectionViewModel(IMvxMessenger messenger)
        {
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
        ////////////////////////// OVERRIDE FUNCTIONS
        /// </summary>
        /// 
        private async Task RunWorkAsync()
        {
            await Task.Run(async () =>
            {
                await Task.Delay(500);
                LoadListCollectionPrepareModel param = new LoadListCollectionPrepareModel(SelectedFolder, Folders);
                _hwListVm.Prepare(param);
            });

            ChangeCurrentViewMessage message = new ChangeCurrentViewMessage(this, _hwListVm);
            WorkCompletedMessage messageCompleted = new WorkCompletedMessage(this);
            _messenger.Publish(message);
            _messenger.Publish(messageCompleted);
        }

        public async override Task Initialize()
        {
            if(_isWorkRunning) return;
            _isWorkRunning = true;

            await RunWorkAsync();

            await base.Initialize();
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
