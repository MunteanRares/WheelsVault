using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands;
using ItemsProject.Core.Messages;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        private readonly IMvxMessenger _messenger;
        public SettingsViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;
            ToggleSettingsCommand = new ToggleSettingsCommand(changeCurrentView);
        }

        private void changeCurrentView()
        {
            ChangeCurrentViewMessage message = new ChangeCurrentViewMessage(this, Mvx.IoCProvider.Resolve<HomePageViewModel>());
            _messenger.Publish(message);
        }

        public ICommand ToggleSettingsCommand { get; }
    }
}
