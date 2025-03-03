﻿using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands
{
    public class OpenPopupCommand : CommandBase
    {
        private readonly Action<ItemModel> _setSelectedItemFolderIds;   
        private readonly Action<ItemModel> _setCheckedToFolders;

        public OpenPopupCommand(Action<ItemModel> setSelectedItemFolderIds, Action<ItemModel> setCheckedToFolders)
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
