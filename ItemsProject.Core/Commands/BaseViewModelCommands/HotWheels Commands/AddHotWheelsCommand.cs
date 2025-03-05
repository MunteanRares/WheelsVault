using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Utils;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Plugin.Messenger;
using WikiHotWheelsWebScraper.Models;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands
{
    public class AddHotWheelsCommand : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<FolderModel> _getSelectedFolder;
        private readonly IMvxMessenger _messenger;
        public AddHotWheelsCommand(IDataService dataService, Func<FolderModel> getSelectedFolder, IMvxMessenger messenger)
        {
            _dataService = dataService;
            _getSelectedFolder = getSelectedFolder;
            _messenger = messenger;
        }

        public override void Execute(object? parameter)
        {
            HotWheelsModel? hotwheels = parameter as HotWheelsModel;
            int folderId = _getSelectedFolder().Id;

            if (hotwheels == null)
            {
                ItemModel itemModel = parameter as ItemModel;
                ItemModel addedItem = _dataService.AddHotWheelsModel(folderId,
                                                                     itemModel.ModelName,
                                                                     itemModel.SeriesName,
                                                                     itemModel.SeriesNum,
                                                                     itemModel.YearProduced,
                                                                     itemModel.YearProducedNum,
                                                                     itemModel.ToyNum,
                                                                     itemModel.PhotoURL);
                AddedHwMessage message = new AddedHwMessage(this, addedItem);
                _messenger.Publish(message);
            }
            else
            {
                ItemModel addedItem = _dataService.AddHotWheelsModel(folderId,
                                                                     hotwheels.ModelName,
                                                                     hotwheels.SeriesName,
                                                                     hotwheels.SeriesNum,
                                                                     hotwheels.YearProduced,
                                                                     hotwheels.YearProducedNum,
                                                                     hotwheels.ToyNum,
                                                                     hotwheels.PhotoURL);
                AddedHwMessage message = new AddedHwMessage(this, addedItem);
                _messenger.Publish(message);
            }
        }
    }
}
