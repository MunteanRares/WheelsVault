using MvvmCross.ViewModels;
using System.Windows.Input;
using ItemsProject.Core.Commands.CustomMessageBoxCommands;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Commands.General;
using MvvmCross.Navigation;

namespace ItemsProject.Core.ViewModels
{
    public class CustomMessageBoxViewModel : MvxViewModel<MessageBoxModel>
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxNavigationService _nav;
        public CustomMessageBoxViewModel(IMvxNavigationService nav, IMvxMessenger messenger)
        {
            _messenger = messenger; 
            _nav = nav;

            ConfirmCommand = new MessageBoxConfirm(CloseConfirmWindow);
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
            CanRemoveFolderMessage folderMessage = new CanRemoveFolderMessage(this, result, FolderToDelete);
            _messenger.Publish(folderMessage);
            _nav.Close(this);
        }

        public void CloseCancelWindow(bool result)
        {
            _nav.Close(this);
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
            get 
            {
                return  _iconSource;
            }

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
