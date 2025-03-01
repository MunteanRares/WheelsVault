using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Utils;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using WikiHotWheelsWebScraper.Models;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands
{
    public class AddHotWheelsCommand : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<ItemModel> _updateFolders;
        private readonly Func<FolderModel> _getSelectedFolder;
        public AddHotWheelsCommand(IDataService dataService, Action<ItemModel> updateFolders, Func<FolderModel> getSelectedFolder)
        {
            _dataService = dataService;
            _updateFolders = updateFolders;
            _getSelectedFolder = getSelectedFolder;
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
                _updateFolders(addedItem);
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
                _updateFolders(addedItem);
            }
        }
    }
}
