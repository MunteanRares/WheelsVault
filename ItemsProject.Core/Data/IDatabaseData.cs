using ItemsProject.Core.Models;

namespace ItemsProject.Core.Data
{
    public interface IDatabaseData
    {
        List<ItemModel> GetAllItems();
        public List<FolderModel> GetAllFolders();
        public List<ItemModel> GetItemsByFolderId(int folderId);
        public ItemModel CreateNewItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName);
        public FolderModel CreateNewFolder(string folderName);
        public ItemModel GetItemById(int itemId);
        public void DeleteItem(int itemId, int folderId);
        void RemoveFolderById(int folderId);
        FolderModel GetFolderById(int folderId);
        void EditFolderName(string folderName, int folderId);
        public void EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName);
        List<int> GetAllFolderIdsForItem(int selectedItemID);
        void AddItemToFolder(int selectedItemId, int selectedFolderId);
        void DeleteAllItemsFromFolder(int itemId);
    }
}