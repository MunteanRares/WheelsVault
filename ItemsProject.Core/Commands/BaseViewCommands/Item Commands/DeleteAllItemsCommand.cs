using System.Collections.ObjectModel;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Messages.HwListVm_Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.Item_Commands
{
    public class DeleteAllItemsCommand : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<List<ItemModel>> _executeUpdateFolder;
        private readonly Func<ObservableCollection<ItemModel>> _getAllItemsInFolder;
        private readonly IMvxMessenger _messenger;

        public DeleteAllItemsCommand(IDataService dataService, Action<List<ItemModel>> executeUpdateFolder, Func<ObservableCollection<ItemModel>> getAllItemsInFolder, IMvxMessenger messenger)
        {
            _dataService = dataService;
            _executeUpdateFolder = executeUpdateFolder;
            _getAllItemsInFolder = getAllItemsInFolder;
            _messenger = messenger;
        }

        public async override void Execute(object? parameter)
        {
            ItemModel item = (ItemModel)parameter!;
            List<ItemModel> allFolderItems = new List<ItemModel>(_getAllItemsInFolder());
            ItemModel itemToRemoveCopy = await _dataService.DeleteAllItemsFromFolder(item.Id);

            ClosePopupBeforeDeletingItemMessage message = new ClosePopupBeforeDeletingItemMessage(this);
            _messenger.Publish(message);

            ItemModel itemToRemove = allFolderItems.Where(i => i.Id == itemToRemoveCopy.Id).FirstOrDefault();

            allFolderItems .Remove(itemToRemove);
            _executeUpdateFolder(allFolderItems);
        }
    }
}
