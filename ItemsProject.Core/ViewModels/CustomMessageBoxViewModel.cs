using MvvmCross.ViewModels;
using System.Windows.Input;
using ItemsProject.Core.Commands.CustomMessageBoxCommands;
using ItemsProject.Core.Models;
using ItemsProject.Core.Commands.General;
using MvvmCross.Navigation;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.ViewModels
{
    public class CustomMessageBoxViewModel : MvxViewModel<MessageBoxModel>
    {
        private readonly IMvxNavigationService _nav;
        private readonly IMessageBoxDataService _messageBoxDataService;
        public CustomMessageBoxViewModel(IMvxNavigationService nav, IMessageBoxDataService messageBoxDataService)
        {
            _messageBoxDataService = messageBoxDataService;
            _nav = nav;

            ConfirmCommand = new MessageBoxConfirm(CloseConfirmWindow);
            CancelCommand = new Cancel(CloseCancelWindow);
        }

        // COMMANDS 
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
   
        // FUNCTIONS
        public void CloseConfirmWindow(bool result)
        {
            _messageBoxDataService.ConfirmAdd(result, FolderToDelete);
            _nav.Close(this);
        }

        public void CloseCancelWindow()
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

        // 
        public override void Prepare(MessageBoxModel parameter)
        {
            Message = parameter.Message;
            Title = parameter.Title;
            IconSource = parameter.IconSource;
            FolderToDelete = parameter.FolderToDelete;
        }
    }
}
