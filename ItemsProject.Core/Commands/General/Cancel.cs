namespace ItemsProject.Core.Commands.General
{
    public class Cancel : CommandBase
    {
        private readonly Action _closeWindow;

        public Cancel(Action closeWindow)
        {
            _closeWindow = closeWindow;
        }

        public override void Execute(object? parameter)
        {
            if (_closeWindow != null)
            {
                _closeWindow();
            }
        }
    }
}
