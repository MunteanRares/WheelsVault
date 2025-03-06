using System.Collections.ObjectModel;
using System.Runtime.InteropServices.Marshalling;
using System.Windows.Input;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using Nito.AsyncEx;
using WikiHotWheelsWebScraper.Models;


namespace ItemsProject.Core.Services
{
    public class DataService : IDataService
    {
        private readonly IDatabaseData _db;
        private readonly IMvxMessenger _messenger;
        private readonly SynchronizationContext _uiContext;
        public DataService(IDatabaseData db, IMvxNavigationService navigationService, IMvxMessenger messenger)
        {
            _db = db;
            _messenger = messenger;
            _uiContext = SynchronizationContext.Current;
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
                        (i.SeriesName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (i.YearProduced.Contains(searchText, StringComparison.OrdinalIgnoreCase))
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

        public ItemModel RemoveItemFromFolder(int itemId, int folderId)
        {
            ItemModel itemToRemove = _db.GetItemById(itemId);
            _db.DeleteItem(itemId, folderId);

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

        public void PostCancelEditMessage()
        {
            CancelItemEditingMessage message = new CancelItemEditingMessage(Mvx.IoCProvider.Resolve<BaseViewModel>());
            _messenger.Publish(message);
        }

        public List<int> GetFolderIdsForItem(int selectedItemID)
        {
            List<int> output = _db.GetAllFolderIdsForItem(selectedItemID);
            return output;        
        }

        public void AddItemToFolder(int selectedItemId, int selectedFolderId)
        {
            _db.AddItemToFolder(selectedItemId, selectedFolderId);
        }

        public ItemModel DeleteAllItemsFromFolder(int itemId)
        {
            ItemModel itemToRemove = _db.GetItemById(itemId);
            _db.DeleteAllItemsFromFolder(itemId);
            return itemToRemove;
        }

        public ObservableCollection<HotWheelsModel> SearchHotWheels(string searchhwText)
        {
            ObservableCollection<HotWheelsModel> searchResult = new ObservableCollection<HotWheelsModel>(_db.SearchHotWheels(searchhwText));
            return searchResult;
        }

        public ItemModel AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            ItemModel output =  _db.AddHotWheelsModel(folderId, modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL);
            return output;   
        }

        public int GetAllHotWheelsCount()
        {
            int output = _db.GetAllNonCustom().Count;
            return output;
        }

        public List<FolderModel> GetAllFolders()
        {
            List<FolderModel> output = _db.GetAllFolders();
            return output;
        }

        public int GetAllCarsCount()
        {
            int output = _db.GetAllQuantities();
            return output;
        }

        public ItemModel RemoveOneQuantity(ItemModel? itemModel)
        {
            ItemModel output = _db.RemoveOneQuantity(itemModel);
            return output;
        }
    }
}
