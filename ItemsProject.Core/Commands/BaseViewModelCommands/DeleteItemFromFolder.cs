﻿using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;


namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class DeleteItemFromFolder : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<List<ItemModel>> _updateFolderItems;
        private readonly Func<List<ItemModel>> _getFolderItems;

        public DeleteItemFromFolder(IDataService dataService,
                                    Action<List<ItemModel>> updateFolderItems,
                                    Func<List<ItemModel>> getFolderItems)
        {
            _dataService = dataService;
            _getFolderItems = getFolderItems;
            _updateFolderItems = updateFolderItems;
        }

        public override void Execute(object? parameter)
        {
            ItemModel valuePassedInFromButton = parameter as ItemModel;
            var allFolderItems = _getFolderItems();
            ItemModel itemToRemoveCopy = _dataService.RemoveItemFromFolder(valuePassedInFromButton.Id,
                                                                           valuePassedInFromButton.FolderId,
                                                                           valuePassedInFromButton.ModelName,
                                                                           valuePassedInFromButton.ModelReleaseDate,
                                                                           valuePassedInFromButton.CollectionName);

            ItemModel itemToRemove = allFolderItems.Where(i => i.Id == itemToRemoveCopy.Id).FirstOrDefault();

            allFolderItems.Remove(itemToRemove);

            _updateFolderItems(allFolderItems);

        }
    }
}
