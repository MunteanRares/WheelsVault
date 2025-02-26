using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public AddHotWheelsCommand(IDataService dataService, Action<ItemModel> updateFolders)
        {
            _dataService = dataService;
            _updateFolders = updateFolders;
        }

        public override void Execute(object? parameter)
        {
            HotWheelsModel hotwheels = (HotWheelsModel)parameter;
            ItemModel addedItem =  _dataService.AddHotWheelsModel(hotwheels.ModelName, hotwheels.SeriesName, hotwheels.SeriesNum, hotwheels.YearProduced, hotwheels.YearProducedNum, hotwheels.ToyNum, hotwheels.PhotoURL);
            _updateFolders(addedItem);
        }
    }
}
