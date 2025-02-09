using System.Collections.ObjectModel;
using System.Windows.Input;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;


namespace ItemsProject.Core.Services
{
    public interface IDataService
    {
        List<ItemModel> FilterItems(string searchText, List<ItemModel> allFolderItems);
        List<ItemModel> LoadItemsForFolder(FolderModel selectedFolder);
        ObservableCollection<ItemModel> UpdateFolderItems(List<ItemModel> iterateList, ObservableCollection<ItemModel> folderItems);
        ItemModel RemoveItemFromFolder(int itemId, int folderId, string modelName, string modelReleaseDate, string collectionName);
        FolderModel RemoveFolder(int folderId);
        void AddItem(int folderId, string modelName, string modelReleaseDate, string collectionName);
        ObservableCollection<FolderModel> UpdateFolders(List<FolderModel> updatedFolders, ObservableCollection<FolderModel> folders);
        void ExecuteDeleteFolderCommand(CanRemoveFolderMessage message, ICommand command);
        
        void EditFolderName(string folderName, int folderId);
        void EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName);
    }
}