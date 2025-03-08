using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Messages.HomePage_Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Commands.HomePageCommands
{
    public class AddCurrentItemToCollectionCommand : CommandBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly Func<ItemModel> _getCurrentItem;
        private readonly IDataService _dataService;
        private readonly Func<ItemModel, Task> _updateCurrentItemCollectionAsync;

        public AddCurrentItemToCollectionCommand(IMvxMessenger messenger, Func<ItemModel> getCurrentItem, IDataService dataService, Func<ItemModel, Task> updateCurrentItemCollectionAsync)
        {
            _messenger = messenger; 
            _getCurrentItem = getCurrentItem;
            _dataService = dataService;
            _updateCurrentItemCollectionAsync  = updateCurrentItemCollectionAsync;
        }

        public async override void Execute(object? parameter)
        {
            ItemModel currentItem =  _getCurrentItem();
            FolderModel defaultFolder = await _dataService.GetDefaultFolder();
            int defaultFolderId = defaultFolder.Id;

            await _dataService.AddHotWheelsModel(defaultFolderId,
                                                    currentItem.ModelName,
                                                    currentItem.SeriesName,
                                                    currentItem.SeriesNum,
                                                    currentItem.YearProduced,
                                                    currentItem.YearProducedNum,
                                                    currentItem.ToyNum,
                                                    currentItem.PhotoURL);

            await _updateCurrentItemCollectionAsync(currentItem);

            AddCurrentItemMessage message = new AddCurrentItemMessage(this, currentItem);
            _messenger.Publish(message);
        }
    }
}
