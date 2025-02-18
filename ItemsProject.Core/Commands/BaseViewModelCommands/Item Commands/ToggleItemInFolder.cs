using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.Item_Commands
{
    public class ToggleItemInFolder : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<List<ItemModel>> _updateFolderItems;
        private readonly Func<List<ItemModel>> _getAllFolderItems;
        public ToggleItemInFolder(IDataService dataService, Action<List<ItemModel>> updateFolderItems, Func<List<ItemModel>> getAllFolderItems)
        {
            _dataService = dataService;
            _updateFolderItems = updateFolderItems;
            _getAllFolderItems = getAllFolderItems;
        }

        public override void Execute(object? parameter)
        {
            var values = (object[])parameter;
            FolderModel passedFolder = (FolderModel)values[0];
            ItemModel passedSelectedItem = (ItemModel)values[1];
            FolderModel passedSelectedFolder = (FolderModel)values[2];

            if (passedSelectedItem.FolderIds.Contains(passedFolder.Id))
            {
                _dataService.RemoveItemFromFolder(passedSelectedItem.Id, passedFolder.Id);
                if (passedFolder.Id == passedSelectedFolder.Id)
                {
                    List<ItemModel> itemModels = _getAllFolderItems();
                    ItemModel itemToRemove = itemModels.Where(i => i.Id == passedSelectedItem.Id).FirstOrDefault();
                    itemModels.Remove(itemToRemove);
                    _updateFolderItems(itemModels);
                }
                
                passedFolder.IsChecked = false;
                passedSelectedItem.FolderIds.Remove(passedFolder.Id);
            }

            else
            {
                _dataService.AddItemToFolder(passedSelectedItem.Id, passedFolder.Id);
                passedFolder.IsChecked = true;
                passedSelectedItem.FolderIds.Add(passedFolder.Id);
            }
        }
    }
}
