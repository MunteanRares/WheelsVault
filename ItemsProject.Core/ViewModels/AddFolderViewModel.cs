using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public class AddFolderViewModel : MvxViewModel
    {
        private readonly IDatabaseData _db;
        private readonly IMvxNavigationService _navigation;
        private readonly IMvxMessenger _messenger;
        public AddFolderViewModel(IDatabaseData db, IMvxNavigationService navigation, IMvxMessenger messenger)
        {
            _db = db;
            _navigation = navigation;
            _messenger = messenger;
            cancelAddFolderCommand = new MvxCommand(CancelAddFolder);
            addFolderCommand = new MvxCommand(AddFolder);
        }

        // COMMANDS
        public IMvxCommand cancelAddFolderCommand { get; set; }
        public IMvxCommand addFolderCommand { get; set; }


        // FUNCTIONS    
        public void CancelAddFolder()
        {
            _navigation.Close(this);
        }

        public void AddFolder()
        {
            var newFolder = _db.CreateNewFolder(FolderName);
            var message = new AddedFolderMessage(
                this,
                newFolder
            );
            _messenger.Publish(message);
            _navigation.Close(this);
        }

        // VALIDATIONS
        public bool CanAddFolder => !string.IsNullOrWhiteSpace(FolderName);

        // PROPERTIES
        private string _folderName;
        public string FolderName
        {
            get { return _folderName; }
            set 
            { 
                SetProperty(ref _folderName, value);
                RaisePropertyChanged(() => CanAddFolder);
            }
        }

    }
}
