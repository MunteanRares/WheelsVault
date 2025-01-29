using System.Collections.ObjectModel;
using ItemsProject.Core.Models;

namespace ItemsProject.Core.Services
{
    public interface IDataService
    {
        List<ItemModel> FilterItems(string searchText, List<ItemModel> allFolderItems);
        List<ItemModel> LoadItemsForFolder(FolderModel selectedFolder);
        public ObservableCollection<ItemModel> UpdateFolderItems(List<ItemModel> iterateList, ObservableCollection<ItemModel> folderItems);
        public ItemModel RemoveItemFromFolder(int itemId);
    }
}