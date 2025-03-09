

using ItemsProject.Core.Commands.General;

namespace ItemsProject.Core.Commands.AddFolderViewModelCommands
{
    public class AddFolderCommand : CommandBase
    {
        private readonly Action _execute;
        public AddFolderCommand(Action execute)
        {
            _execute = execute;
        }

        public override void Execute(object? parameter)
        {
            _execute();
        }
    }
}
