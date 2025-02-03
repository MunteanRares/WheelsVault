using System.Windows.Input;

namespace ItemsProject.Core.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public abstract void Execute(object? parameter);

        protected virtual void OnCanExecuteChanged(object? parameter)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
