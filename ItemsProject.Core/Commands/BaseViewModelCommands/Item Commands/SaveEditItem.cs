using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class SaveEditItem : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<string> _newName;
        private readonly Func<string> _newReleaseDate;
        private readonly Func<string> _newCollectionName;
        private readonly Action _saveEditItem;

        public SaveEditItem(IDataService dataService, Func<string> newName, Func<string> newReleaseDate, Func<string> newCollectionName, Action saveEditItem)
        {
            _dataService = dataService;
            _newCollectionName = newCollectionName;
            _newName = newName;
            _newReleaseDate = newReleaseDate;
            _saveEditItem = saveEditItem;
        }

        public override void Execute(object? parameter)
        {
            _dataService.EditItem(((ItemModel)parameter).Id, _newName().Capitalize(), _newReleaseDate(), _newCollectionName().ToUpper());
            _saveEditItem();
        }
    }
}
