


using ItemsProject.Core.Commands.General;

namespace ItemsProject.Core.Commands.AddItemViewModelCommands
{
    public class AddItemConfirm : CommandBase
    {
        private readonly Action _execute;
        public AddItemConfirm(Action execute)
        {
            _execute = execute;
        }

        public override void Execute(object? parameter)
        {
            _execute();
        }
    }
}
