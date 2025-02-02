using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.Services
{
    public class DataService : IDataService
    {
        private readonly IDatabaseData _db;
        private readonly IMvxNavigationService _navigationService;
        public DataService(IDatabaseData db, IMvxNavigationService navigationService)
        {
            _db = db;
            _navigationService = navigationService;
        }

        public List<ItemModel> LoadItemsForFolder(FolderModel selectedFolder)
        {
            List<ItemModel> allFolderItems = new List<ItemModel>();

            if (selectedFolder == null)
            {
                return allFolderItems;
            }

            allFolderItems = _db.GetItemsByFolderId(selectedFolder.Id);

            return allFolderItems;
        }

        public List<ItemModel> FilterItems(string searchText, List<ItemModel> allFolderItems)
        {
            List<ItemModel> searchResult = new List<ItemModel>();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchResult = allFolderItems;
            }
            else
            {
                List<ItemModel> filteredItems = allFolderItems.Where(i => i.ModelName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                searchResult = filteredItems;
            }

            return searchResult;
        }

        public ObservableCollection<ItemModel> UpdateFolderItems(List<ItemModel> iterateList, ObservableCollection<ItemModel> folderItems)
        {
            folderItems.Clear();
            foreach (ItemModel item in iterateList)
            {
                folderItems.Add(item);
            }

            return folderItems;
        }

        public ObservableCollection<FolderModel> UpdateFolders(List<FolderModel> updatedFolders, ObservableCollection<FolderModel> folders)
        {
            folders.Clear();
            foreach (FolderModel folder in updatedFolders)
            {
                folders.Add(folder);
            }

            return folders;
        }

        public ItemModel RemoveItemFromFolder(int itemId, int folderId, string modelName, string modelReleaseDate, string collectionName)
        {
            ItemModel itemToRemove = _db.GetItemById(itemId);
            _db.DeleteItem(itemId, folderId, modelName, modelReleaseDate, collectionName);

            return itemToRemove;
        }

        public FolderModel RemoveFolder(int folderId)
        {
            FolderModel folderToRemove = _db.GetFolderById(folderId);
            _db.RemoveFolderById(folderId);

            return folderToRemove;
        }

        public void ExecuteDeleteFolderCommand(CanRemoveFolderMessage message, ICommand deleteCommand)
        {
            if (message.CanRemoveFolder == true)
            {
                deleteCommand.Execute(message.FolderToDelete);
            }
        }

        public void NavigateAddFolderViewModel()
        {
            _navigationService.Navigate<AddFolderViewModel>();
        }

        public void NavigateAddItemViewModel(FolderModel folderToAddTo)
        {
            _navigationService.Navigate<AddItemViewModel, FolderModel>(folderToAddTo);
        }

        public void CloseWindow(IMvxViewModel viewModel)
        {
            _navigationService.Close(viewModel);
        }

        public void NavigatToCustomMessageBoxViewModel(MessageBoxModel parameters)
        {
            _navigationService.Navigate<CustomMessageBoxViewModel, MessageBoxModel>(parameters);
        }
    }
}
