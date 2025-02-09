using DevExpress.XtraReports.Native;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class SaveEditFolder : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<string> _getEditingFolderName;
        private readonly Action _stopEditing;

        public SaveEditFolder(IDataService dataService, Func<string> getEditingFolderName, Action stopEditing)
        {
            _dataService = dataService;
            _getEditingFolderName = getEditingFolderName;
            _stopEditing = stopEditing;
        }

        public override void Execute(object? parameter)
        {
            FolderModel selectedFolder = (FolderModel)parameter;
            string newFolderName = _getEditingFolderName().Capitalize();

            _dataService.EditFolderName(newFolderName, selectedFolder.Id);
            _stopEditing();
        }
    }
}
