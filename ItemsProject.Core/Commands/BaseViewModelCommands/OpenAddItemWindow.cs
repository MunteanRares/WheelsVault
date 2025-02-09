using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross.Navigation;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class OpenAddItemWindow : CommandBase
    {
        private readonly Func<FolderModel> _getSelectedFolder;
        private readonly Action _clearSearchText;
        private readonly IMvxNavigationService _nav;
        public OpenAddItemWindow(IMvxNavigationService nav, Func<FolderModel> getSelectedFolder, Action clearSearchText)
        {
            _getSelectedFolder = getSelectedFolder;
            _clearSearchText = clearSearchText;
            _nav = nav;
        }

        public override void Execute(object? parameter)
        {
            _clearSearchText();
            FolderModel folderToAddTo = _getSelectedFolder();
            _nav.Navigate<AddItemViewModel, FolderModel>(folderToAddTo);
        }
    }
}
