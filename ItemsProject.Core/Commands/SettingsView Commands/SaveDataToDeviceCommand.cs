using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.SettingsView_Commands
{
    public class SaveDataToDeviceCommand : CommandBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;
        private readonly Action _disableButton;

        public SaveDataToDeviceCommand(ISettingsService settingsService,
                                       IDataService dataService,
                                       Action disableButton)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _disableButton = disableButton;
        }

        public async override void Execute(object? parameter)
        {
            _disableButton();

            FolderModel defaultFolder = await _dataService.GetDefaultFolder();
            List<ItemModel> userItems = await _dataService.LoadItemsForFolder(defaultFolder);

            _settingsService.SaveItems(userItems);
        }
    }
}
