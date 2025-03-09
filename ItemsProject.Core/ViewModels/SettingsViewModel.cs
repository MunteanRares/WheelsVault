using Newtonsoft.Json;
using System.Windows.Input;
using ItemsProject.Core.Commands.SettingsView_Commands;
using ItemsProject.Core.Messages.Settings_Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using ItemsProject.Core.Messages.SuccessNotification_Messages;

namespace ItemsProject.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        private readonly IMvxMessenger _messenger;
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;
        private readonly List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

        public SettingsViewModel(IMvxMessenger messenger,
                                 ISettingsService settingsService,
                                 IDataService dataService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _messenger = messenger;

            _tokens.Add(_messenger.Subscribe<OpenFileDialogMessage>(OnOpenFileDialogMessage));
            _tokens.Add(_messenger.Subscribe<EnableButtonMessage>(OnEnableButtonMessage));

            SaveDataToDeviceCommand = new SaveDataToDeviceCommand(_settingsService, _dataService, ToggleButton);
            ImportSaveData = new ImportSaveDataCommand(_messenger);
        }

        private void ToggleButton()
        {
            if (IsButtonEnabled)
            {
                IsButtonEnabled = false;
            }
            else
            {
                IsButtonEnabled = true;
            }
        }
        private void OnEnableButtonMessage(EnableButtonMessage message)
        {
            ToggleButton();
        }

        private void OnOpenFileDialogMessage(OpenFileDialogMessage message)
        {
            if (message.Sender is not ImportSaveDataCommand)
            {
                string jsonContents = message.JsonContents;
                _settingsService.LoadItems(jsonContents);
            }
        }

        // COMMANDS
        public ICommand SaveDataToDeviceCommand { get; }
        public ICommand ImportSaveData { get; }

        /// PROPERTIES
        private bool _isButtonEnabled = true;
        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set
            { 
                SetProperty(ref _isButtonEnabled, value);   
            }
        }
    }
}
