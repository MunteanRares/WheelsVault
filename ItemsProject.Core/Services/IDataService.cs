using System.Collections.ObjectModel;
using System.Windows.Input;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.Services
{
    public interface IDataService
    {
        List<ItemModel> FilterItems(string searchText, List<ItemModel> allFolderItems);
        List<ItemModel> LoadItemsForFolder(FolderModel selectedFolder);
        public ObservableCollection<ItemModel> UpdateFolderItems(List<ItemModel> iterateList, ObservableCollection<ItemModel> folderItems);
        public ItemModel RemoveItemFromFolder(int itemId, int folderId, string modelName, string modelReleaseDate, string collectionName);
        public FolderModel RemoveFolder(int folderId);
        public ObservableCollection<FolderModel> UpdateFolders(List<FolderModel> updatedFolders, ObservableCollection<FolderModel> folders);
        public void ExecuteDeleteFolderCommand(CanRemoveFolderMessage message, ICommand command);
        public void NavigateAddFolderViewModel();
        public void NavigateAddItemViewModel(FolderModel folderToAddTo);
        public void CloseWindow(IMvxViewModel viewModel);
    }
}