using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands
{
    public class AddHotWheelsCommand : CommandBase
    {
        private readonly IDataService _dataService;
        public AddHotWheelsCommand(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override void Execute(object? parameter)
        {
            
        }
    }
}
