using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;


namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class DeleteItemFromFolder : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<List<ItemModel>> _updateFolderItems;
        private readonly Func<List<ItemModel>> _getFolderItems;
        private readonly Func<FolderModel> _getSelectedFolder;

        public DeleteItemFromFolder(IDataService dataService,
                                    Action<List<ItemModel>> updateFolderItems,
                                    Func<List<ItemModel>> getFolderItems,
                                    Func<FolderModel> getSelectedFolder)
        {
            _dataService = dataService;
            _getFolderItems = getFolderItems;
            _updateFolderItems = updateFolderItems;
            _getSelectedFolder = getSelectedFolder;
        }

        public override void Execute(object? parameter)
        {
            ItemModel valuePassedInFromButton = parameter as ItemModel;
            FolderModel selectedFolder = _getSelectedFolder();
            List<ItemModel> allFolderItems = _getFolderItems();
            ItemModel itemToRemoveCopy = _dataService.RemoveItemFromFolder(valuePassedInFromButton.Id,
                                                                           selectedFolder.Id);

            ItemModel itemToRemove = allFolderItems.Where(i => i.Id == itemToRemoveCopy.Id).FirstOrDefault();

            allFolderItems.Remove(itemToRemove);

            _updateFolderItems(allFolderItems);

        }
    }
}
