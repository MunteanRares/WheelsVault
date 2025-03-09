using System.Collections.ObjectModel;
using System.Runtime.InteropServices.Marshalling;
using System.Windows.Input;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Base;
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
        private readonly IMvxMainThreadAsyncDispatcher _thread;
        public DataService(IDatabaseData db, IMvxNavigationService navigationService, IMvxMessenger messenger, IMvxMainThreadAsyncDispatcher thread)
        {
            _db = db;
            _messenger = messenger;
            _thread = thread;
        }

        public async Task<List<ItemModel>> LoadItemsForFolder(FolderModel selectedFolder)
        {
            List<ItemModel> allFolderItems = new List<ItemModel>();

            if (selectedFolder == null)
            {
                return allFolderItems;
            }
            allFolderItems = await _db.GetItemsByFolderId(selectedFolder.Id);

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
            List<ItemModel> temp = new List<ItemModel>(folderItems);
            temp.Clear();
            foreach (ItemModel item in iterateList)
            {
                temp.Add(item);
            }

            ObservableCollection<ItemModel> output = new ObservableCollection<ItemModel>(temp);

            return output;
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

        public async Task<ItemModel> RemoveItemFromFolder(int itemId, int folderId)
        {
            ItemModel itemToRemove = await _db.GetItemById(itemId);
            await _db.DeleteItem(itemId, folderId);

            return itemToRemove;
        }

        public async Task<FolderModel> RemoveFolder(int folderId)
        {
            FolderModel folderToRemove = await _db.GetFolderById(folderId);
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

        public async Task EditFolderName(string folderName, int folderId)
        {
            await _db.EditFolderName(folderName, folderId);
        }

        public async Task EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName)
        {
            await _db.EditItem(itemId, newName, newReleaseDate, newCollectionName);
        }

        public ObservableCollection<ItemModel> SortItems(string sortOption, List<ItemModel> allFolderItems, ObservableCollection<ItemModel> folderItems)
        {
            try
            {
                if (sortOption == "Date Added")
                {
                    folderItems =  UpdateFolderItems(allFolderItems, folderItems);
                }
                else if (sortOption == "A-Z")
                {
                    allFolderItems = allFolderItems.OrderBy(item => item.ModelName).ToList();
                    folderItems =  UpdateFolderItems(allFolderItems, folderItems);
                }
                else if (sortOption == "Z-A")
                {
                    allFolderItems = allFolderItems.OrderByDescending(item => item.ModelName).ToList();
                    folderItems =  UpdateFolderItems(allFolderItems, folderItems);
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

        public async Task<List<int>> GetFolderIdsForItem(int selectedItemID)
        {
            List<int> output = await _db.GetAllFolderIdsForItem(selectedItemID);
            return output;        
        }

        public async Task AddItemToFolder(int selectedItemId, int selectedFolderId)
        {
            await _db.AddItemToFolder(selectedItemId, selectedFolderId);
        }

        public async Task<ItemModel> DeleteAllItemsFromFolder(int itemId)
        {
            ItemModel itemToRemove = await _db.GetItemById(itemId);
            await _db.DeleteAllItemsFromFolder(itemId);
            return itemToRemove;
        }

        public async Task<ObservableCollection<HotWheelsModel>> SearchHotWheels(string searchhwText)
        {
            ObservableCollection<HotWheelsModel> searchResult = new ObservableCollection<HotWheelsModel>(await _db.SearchHotWheels(searchhwText));
            return searchResult;
        }

        public async Task<ItemModel> AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            ItemModel output = await _db.AddHotWheelsModel(folderId, modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL);
            return output;   
        }

        public async Task<int> GetAllHotWheelsCount()
        {
            List<ItemModel> output = await _db.GetAllNonCustom();
            return output.Count();
        }

        public async Task<List<FolderModel>> GetAllFolders()
        {
            List<FolderModel> output = await _db.GetAllFolders();
            return output;
        }

        public async Task<int> GetAllCarsCount()
        {
            int output = await _db.GetAllQuantities();
            return output;
        }

        public async Task<ItemModel> RemoveOneQuantity(ItemModel? itemModel)
        {
            ItemModel output = await _db.RemoveOneQuantity(itemModel);
            return output;
        }

        public async Task<FolderModel?> GetDefaultFolder()
        {
            FolderModel output = await _db.GetDefaultFolder();
            return output;
        }
    }
}
