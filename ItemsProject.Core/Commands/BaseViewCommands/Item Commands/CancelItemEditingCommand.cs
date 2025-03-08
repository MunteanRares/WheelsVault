using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class CancelItemEditingCommand : CommandBase
    {
        private readonly IDataService _dataService;

        public CancelItemEditingCommand(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override void Execute(object? parameter)
        {
            _dataService.PostCancelEditMessage();
        }
    }
}
