using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Data.Helpers;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;


namespace ItemsProject.Core.Services
{
    public class DataService : IDataService
    {
        private readonly IDatabaseData _db;
        private readonly IMvxMessenger _messenger;
        public DataService(IDatabaseData db, IMvxNavigationService navigationService, IMvxMessenger messenger)
        {
            _db = db;
            _messenger = messenger;
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
                List<ItemModel> filteredItems = allFolderItems.Where
                    (i =>
                        (i.ModelName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (i.CollectionName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (i.ModelReleaseDate.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
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

        public void AddItem(int folderId, string modelName, string modelReleaseDate, string collectionName)
        {
            ItemModel newItem = _db.CreateNewItem(folderId, modelName, modelReleaseDate, collectionName);
            var message = new AddedItemMessage(Mvx.IoCProvider.Resolve<AddItemViewModel>(), newItem);
            _messenger.Publish(message);
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

        public void EditFolderName(string folderName, int folderId)
        {
            _db.EditFolderName(folderName, folderId);
        }

        public void EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName)
        {
            _db.EditItem(itemId, newName, newReleaseDate, newCollectionName);
        }

        public ObservableCollection<ItemModel> SortItems(string sortOption, List<ItemModel> allFolderItems, ObservableCollection<ItemModel> folderItems)
        {
            try
            {
                if (sortOption == "Date Added")
                {
                    folderItems = UpdateFolderItems(allFolderItems, folderItems);
                }
                else if (sortOption == "A-Z")
                {
                    allFolderItems = allFolderItems.OrderBy(item => item.ModelName).ToList();
                    folderItems = UpdateFolderItems(allFolderItems, folderItems);
                }
                else if (sortOption == "Z-A")
                {
                    allFolderItems = allFolderItems.OrderByDescending(item => item.ModelName).ToList();
                    folderItems = UpdateFolderItems(allFolderItems, folderItems);
                }
            }
            catch (NullReferenceException ex)
            {
                throw;
            }

            return folderItems;
        }
    }
}
