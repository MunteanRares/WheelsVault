using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class OpenConfirmationWindow : CommandBase
    {
        private readonly Func<string, string> _getMessage;
        private readonly string _title;
        private readonly string _iconSource;
        private readonly IMvxNavigationService _nav;
        private readonly Action _changeWindowState;

        public OpenConfirmationWindow(IMvxNavigationService nav, Func<string, string> getMessage, string title, string iconSource, Action changeWindowState)
        {
            _getMessage = getMessage;
            _title = title;
            _iconSource = iconSource;
            _nav = nav;
            _changeWindowState = changeWindowState;
        }

        public override void Execute(object? parameter)
        {
            _changeWindowState();

            if ((FolderModel?)parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            else
            {
                FolderModel folder = (FolderModel)parameter;
                MessageBoxModel parameters = new MessageBoxModel()
                {
                    Message = _getMessage(folder.Name),
                    Title = _title,
                    IconSource = _iconSource,
                    FolderToDelete = folder
                };

                _nav.Navigate<CustomMessageBoxViewModel, MessageBoxModel>(parameters);
            }
        }
    }
}
