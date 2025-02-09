using DevExpress.XtraReports.Native;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class EditSave : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<string> _getEditingFolderName;
        private readonly Action _stopEditing;

        public EditSave(IDataService dataService, Func<string> getEditingFolderName, Action stopEditing)
        {
            _dataService = dataService;
            _getEditingFolderName = getEditingFolderName;
            _stopEditing = stopEditing;
        }

        public override void Execute(object? parameter)
        {
            FolderModel selectedFolder = (FolderModel)parameter;
            string newFolderName = _getEditingFolderName();

            _dataService.EditFolderName(newFolderName, selectedFolder.Id);
            _stopEditing();
        }
    }
}
