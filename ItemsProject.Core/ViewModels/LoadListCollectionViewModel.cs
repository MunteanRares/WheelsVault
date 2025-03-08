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
using MvvmCross.Base;

namespace ItemsProject.Core.ViewModels
{
    public class LoadListCollectionViewModel : MvxViewModel<LoadListCollectionPrepareModel>
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxMainThreadAsyncDispatcher _thread;
        private readonly HwListCollectionViewModel _hwListVm = Mvx.IoCProvider.Resolve<HwListCollectionViewModel>();
        public LoadListCollectionViewModel(IMvxMessenger messenger, IMvxMainThreadAsyncDispatcher thread)
        {
            _messenger = messenger;
            _thread = thread;
        }

        /// <summary>
        /// ////////// BOOLEANS
        /// </summary>
        private bool _isWorkRunning = false;

        /// <summary>
        ////////////////////////// OVERRIDE FUNCTIONS
        /// </summary>
        /// 
        private void RunWorkAsync()
        {
            HwListCollectionViewModel hwlistvm = Mvx.IoCProvider.Resolve<HwListCollectionViewModel>();
            LoadListCollectionPrepareModel param = new LoadListCollectionPrepareModel(SelectedFolder, Folders);
            _hwListVm.Prepare(param);

            SendMessages(_hwListVm);
        }

        public void SendMessages(HwListCollectionViewModel hwlistvm)
        {
            ChangeCurrentViewMessage message = new ChangeCurrentViewMessage(this, hwlistvm);
            WorkCompletedMessage messageCompleted = new WorkCompletedMessage(this);
            _messenger.Publish(messageCompleted);
            _messenger.Publish(message);
        }

        public override Task Initialize()
        {
            if(_isWorkRunning) return Task.CompletedTask;
            _isWorkRunning = true;

            RunWorkAsync();

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
