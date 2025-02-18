using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.Item_Commands
{
    public class DeleteAllItemsCommand : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<List<ItemModel>> _executeUpdateFolder;
        private readonly Func<List<ItemModel>> _getAllItemsInFolder;

        public DeleteAllItemsCommand(IDataService dataService, Action<List<ItemModel>> executeUpdateFolder, Func<List<ItemModel>> getAllItemsInFolder)
        {
            _dataService = dataService;
            _executeUpdateFolder = executeUpdateFolder;
            _getAllItemsInFolder = getAllItemsInFolder;
        }

        public override void Execute(object? parameter)
        {
            ItemModel item = (ItemModel)parameter!;
            List<ItemModel> allFolderItems = _getAllItemsInFolder();
            ItemModel itemToRemoveCopy = _dataService.DeleteAllItemsFromFolder(item.Id);
            ItemModel itemToRemove = allFolderItems.Where(i => i.Id == itemToRemoveCopy.Id).FirstOrDefault();

            allFolderItems.Remove(itemToRemove);
            _executeUpdateFolder(allFolderItems);
        }
    }
}
