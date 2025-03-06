using System.Collections.ObjectModel;
using System.Xml.Xsl;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Messages.HwListVm_Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class DeleteItemFromFolder : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Action<List<ItemModel>> _updateFolderItems;
        private readonly Func<ObservableCollection<ItemModel>> _getFolderItems;
        private readonly Func<FolderModel> _getSelectedFolder;
        private readonly IMvxMessenger _messenger;

        public DeleteItemFromFolder(IDataService dataService,
                                    Action<List<ItemModel>> updateFolderItems,
                                    Func<ObservableCollection<ItemModel>> getFolderItems,
                                    Func<FolderModel> getSelectedFolder,
                                    IMvxMessenger messenger)
        {
            _dataService = dataService;
            _getFolderItems = getFolderItems;
            _updateFolderItems = updateFolderItems;
            _getSelectedFolder = getSelectedFolder;
            _messenger = messenger;
        }

        public override void Execute(object? parameter)
        {
            ItemModel valuePassedInFromButton = parameter as ItemModel;
            FolderModel selectedFolder = _getSelectedFolder();
            List<ItemModel> allFolderItems = new List<ItemModel>(_getFolderItems());

            ClosePopupBeforeDeletingItemMessage message = new ClosePopupBeforeDeletingItemMessage(this);
            _messenger.Publish(message);    

            ItemModel itemToRemoveCopy = _dataService.RemoveItemFromFolder(valuePassedInFromButton.Id,
                                                                           selectedFolder.Id);
            ItemModel itemToRemove = allFolderItems.Where(i => i.Id == itemToRemoveCopy.Id).FirstOrDefault();

            allFolderItems.Remove(itemToRemove);

            _updateFolderItems(allFolderItems);

        }
    }
}
