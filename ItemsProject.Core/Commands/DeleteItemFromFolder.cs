using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Commands
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
            var allFolderItems = _getFolderItems();
            ItemModel itemToRemoveCopy = _dataService.RemoveItemFromFolder(((ItemModel)parameter).Id);
            ItemModel itemToRemove = allFolderItems.Where(i => i.Id == itemToRemoveCopy.Id).FirstOrDefault();

            allFolderItems.Remove(itemToRemove);

            _updateFolderItems(allFolderItems);

        }
    }
}
