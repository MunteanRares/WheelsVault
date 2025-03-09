using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands
{
    public class OpenPopupCommand : CommandBase
    {
        private readonly Func<ItemModel, Task> _setSelectedItemFolderIds;   
        private readonly Action<ItemModel> _setCheckedToFolders;

        public OpenPopupCommand(Func<ItemModel, Task> setSelectedItemFolderIds, Action<ItemModel> setCheckedToFolders)
        {
            _setSelectedItemFolderIds = setSelectedItemFolderIds;
            _setCheckedToFolders = setCheckedToFolders;
        }

        public override void Execute(object? parameter)
        {
            ItemModel passedItemModel = (ItemModel)parameter;
            _setSelectedItemFolderIds(passedItemModel);
            _setCheckedToFolders(passedItemModel);
            if (parameter != null)
            {
                passedItemModel.IsPopupOpened = true;
            }
        }
    }
}
