using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross.Navigation;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class OpenConfirmationWindow : CommandBase
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly string _message;
        private readonly string _title;
        private readonly string _iconSource;

        public OpenConfirmationWindow(IMvxNavigationService navigationService, string message, string title, string iconSource)
        {
            _navigationService = navigationService;
            _message = message;
            _title = title;
            _iconSource = iconSource;
        }

        public override void Execute(object? parameter)
        {

            if ((FolderModel?)parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            else
            {
                FolderModel folder = (FolderModel)parameter;
                MessageBoxModel parameters = new MessageBoxModel()
                {
                    Message = _message,
                    Title = _title,
                    IconSource = _iconSource,
                    FolderToDelete = folder
                };

                _navigationService.Navigate<CustomMessageBoxViewModel, MessageBoxModel>(parameters);
            }
        }
    }
}
