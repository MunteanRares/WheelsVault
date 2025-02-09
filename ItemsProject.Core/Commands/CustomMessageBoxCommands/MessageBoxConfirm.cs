


using ItemsProject.Core.Commands.General;

namespace ItemsProject.Core.Commands.CustomMessageBoxCommands
{
    public class MessageBoxConfirm : CommandBase
    {
        private readonly Action<bool> _closeWindow;
        public MessageBoxConfirm(Action<bool> closeWindow)
        {
            _closeWindow = closeWindow;
        }

        public override void Execute(object? parameter)
        {
            _closeWindow(true);
        }
    }
}
