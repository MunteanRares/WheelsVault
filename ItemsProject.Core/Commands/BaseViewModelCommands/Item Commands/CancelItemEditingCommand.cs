using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Plugin.Messenger.ThreadRunners;

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
