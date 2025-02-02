using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross;
using MvvmCross.ViewModels;
using System.Windows.Input;
using ItemsProject.Core.Commands.CustomMessageBoxCommands;
using Xceed.Wpf.Toolkit;
using DevExpress.Data;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.ViewModels
{
    public class CustomMessageBoxViewModel : MvxViewModel<MessageBoxModel>
    {
        private readonly IMvxMessenger _messenger;
        private readonly IDataService _dataService;
        public CustomMessageBoxViewModel(IMvxMessenger messenger, IDataService dataService)
        {
            _messenger = messenger; 
            _dataService = dataService;

            ConfirmCommand = new Confirm(CloseConfirmWindow);
            CancelCommand = new Cancel(CloseCancelWindow);
        }

        public override void Prepare(MessageBoxModel parameter)
        {
            Message = parameter.Message;
            Title = parameter.Title;
            IconSource = parameter.IconSource;
            FolderToDelete = parameter.FolderToDelete;
        }

        // COMMANDS 
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        // FUNCTIONS
        public void CloseConfirmWindow(bool result)
        {
            var message = new CanRemoveFolderMessage(this, result, FolderToDelete);
            _messenger.Publish(message);
            _dataService.CloseWindow(this);
        }

        public void CloseCancelWindow(bool result)
        {
            _dataService.CloseWindow(this);
        }

        // PROPERTIES
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                SetProperty(ref _message, value); 
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        private string _iconSource;
        public string IconSource
        {
            get { return _iconSource; }
            set
            {
                SetProperty(ref _iconSource, value);
            }
        }

        private FolderModel _folderToDelete;
        public FolderModel FolderToDelete
        {
            get { return _folderToDelete; }
            set { SetProperty(ref _folderToDelete, value); }
        }

    }
}
