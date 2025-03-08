using System.Collections.ObjectModel;
using System.Windows.Input;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using WikiHotWheelsWebScraper.Models;


namespace ItemsProject.Core.Services
{
    public interface IDataService
    {
        List<ItemModel> FilterItems(string searchText, List<ItemModel> allFolderItems);
        Task<List<ItemModel>> LoadItemsForFolder(FolderModel selectedFolder);
        ObservableCollection<ItemModel> UpdateFolderItems(List<ItemModel> iterateList, ObservableCollection<ItemModel> folderItems);
        Task<ItemModel> RemoveItemFromFolder(int itemId, int folderId);
        Task<FolderModel> RemoveFolder(int folderId);
        ObservableCollection<FolderModel> UpdateFolders(List<FolderModel> updatedFolders, ObservableCollection<FolderModel> folders);
        void ExecuteDeleteFolderCommand(CanRemoveFolderMessage message, ICommand command);        
        Task EditFolderName(string folderName, int folderId);
        Task EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName);
        ObservableCollection<ItemModel> SortItems(string selectedSortOption, List<ItemModel> allFolderItems, ObservableCollection<ItemModel> folderItems);
        void PostCancelEditMessage();
        Task<List<int>> GetFolderIdsForItem(int selectedItemId);
        Task AddItemToFolder(int selectedItemId, int selectedFolderId);
        Task<ItemModel> DeleteAllItemsFromFolder(int id);
        Task<ObservableCollection<HotWheelsModel>> SearchHotWheels(string searchhwText);
        Task<ItemModel> AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL);
        Task<int> GetAllHotWheelsCount();
        Task<List<FolderModel>> GetAllFolders();
        Task<int> GetAllCarsCount();
        Task<ItemModel> RemoveOneQuantity(ItemModel? itemModel);
        Task<FolderModel?> GetDefaultFolder();
    }
}