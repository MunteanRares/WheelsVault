using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands
{
    public class OpenPopupCommand : CommandBase
    {
        private readonly IDataService _dataService;

        public OpenPopupCommand(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override void Execute(object? parameter)
        {
            ItemModel passedItemModel = (ItemModel)parameter;
            passedItemModel.IsPopupOpened = true;
        }
    }
}
