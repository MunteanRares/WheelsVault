
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands
{
    public class RemoveOneQuantityCommand : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<ItemModel, Task> _updateFolderAction;
        public RemoveOneQuantityCommand(IDataService dataService, Func<ItemModel, Task> updateFolderAction)
        {
            _dataService = dataService;
            _updateFolderAction = updateFolderAction;
        }

        public async override void Execute(object? parameter)
        {
            ItemModel itemModel = parameter as ItemModel;
            ItemModel updatedItem = await _dataService.RemoveOneQuantity(itemModel);
            await _updateFolderAction(updatedItem);
        }
    }
}
