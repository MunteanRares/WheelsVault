using ItemsProject.Core.Models;
using WikiHotWheelsWebScraper.Models;

namespace ItemsProject.Core.Data
{
    public interface IDatabaseData
    {
        Task<List<ItemModel>> GetAllItems();
        public Task<List<FolderModel>> GetAllFolders();
        public Task<List<ItemModel>> GetItemsByFolderId(int folderId);
        //public Task<ItemModel> CreateCustomItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName);
        public Task<FolderModel> CreateNewFolder(string folderName);
        public Task<ItemModel> GetItemById(int itemId);
        public Task DeleteItem(int itemId, int folderId);
        void RemoveFolderById(int folderId);
        Task<FolderModel> GetFolderById(int folderId);
        Task EditFolderName(string folderName, int folderId);
        public Task EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName);
        Task<List<int>> GetAllFolderIdsForItem(int selectedItemID);
        Task AddItemToFolder(int selectedItemId, int selectedFolderId);
        Task DeleteAllItemsFromFolder(int itemId);
        Task DefaultHotwheelsDbPopulation();
        Task<List<HotWheelsModel>> SearchHotWheels(string searchhwText);
        Task<ItemModel> AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL);
        Task<List<ItemModel>> GetAllNonCustom();
        Task<int> GetAllQuantities();
        Task<ItemModel> RemoveOneQuantity(ItemModel? itemModel);
        Task<FolderModel> GetDefaultFolder();
        Task<List<ItemModel>> GetLatestCars();
    }
}