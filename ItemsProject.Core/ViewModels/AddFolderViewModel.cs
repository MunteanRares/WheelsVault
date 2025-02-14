using System.Data;
using System.Windows.Input;
using ItemsProject.Core.Commands.AddFolderViewModelCommands;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Data;
using ItemsProject.Core.Services;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public class AddFolderViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _nav;
        private readonly IFolderDataService _folderDataService;
        public AddFolderViewModel(IMvxNavigationService nav, IFolderDataService folderDataService)
        {
            _nav = nav;
            _folderDataService = folderDataService;
            CancelAddFolderCommand = new Cancel(CancelAddFolder);
            AddFolderCommand = new AddFolderCommand(AddFolder);
        }

        // COMMANDS
        public ICommand CancelAddFolderCommand { get; set; }
        public ICommand AddFolderCommand { get; set; }


        // FUNCTIONS    
        public void CancelAddFolder()
        {
            _nav.Close(this);
        }

        public void AddFolder()
        {
            _folderDataService.AddFolder(FolderName);
            _nav.Close(this);
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
