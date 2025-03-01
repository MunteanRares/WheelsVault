
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands
{
    public class RemoveOneQuantityCommand : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<ItemModel> _updateFolderAction;
        public RemoveOneQuantityCommand(IDataService dataService, Action<ItemModel> updateFolderAction)
        {
            _dataService = dataService;
            _updateFolderAction = updateFolderAction;
        }

        public override void Execute(object? parameter)
        {
            ItemModel itemModel = parameter as ItemModel;
            ItemModel updatedItem = _dataService.RemoveOneQuantity(itemModel);
            _updateFolderAction(updatedItem);
        }
    }
}
