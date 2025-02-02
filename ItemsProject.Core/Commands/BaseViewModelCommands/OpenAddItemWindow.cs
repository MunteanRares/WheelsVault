using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class OpenAddItemWindow : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<FolderModel> _getSelectedFolder;
        private readonly Action _clearSearchText;
        public OpenAddItemWindow(IDataService dataService, Func<FolderModel> getSelectedFolder, Action clearSearchText)
        {
            _dataService = dataService;
            _getSelectedFolder = getSelectedFolder;
            _clearSearchText = clearSearchText;
        }

        public override void Execute(object? parameter)
        {
            _clearSearchText();
            FolderModel folderToAddTo = _getSelectedFolder();
            _dataService.NavigateAddItemViewModel(folderToAddTo);
        }
    }
}
