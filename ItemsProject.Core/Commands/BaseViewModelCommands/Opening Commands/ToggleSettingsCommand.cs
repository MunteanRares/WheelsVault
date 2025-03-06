using ItemsProject.Core.Commands.General;


namespace ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands
{
    public class ToggleSettingsCommand : CommandBase
    {
        private readonly Action _changeCurrentView;

        public ToggleSettingsCommand(Action changeCurrentView)
        {
            _changeCurrentView = changeCurrentView;
        }

        public override void Execute(object? parameter)
        {
            _changeCurrentView();
        }
    }
}
