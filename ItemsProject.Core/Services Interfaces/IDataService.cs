﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using WikiHotWheelsWebScraper.Models;


namespace ItemsProject.Core.Services
{
    public interface IDataService
    {
        List<ItemModel> FilterItems(string searchText, List<ItemModel> allFolderItems);
        List<ItemModel> LoadItemsForFolder(FolderModel selectedFolder);
        ObservableCollection<ItemModel> UpdateFolderItems(List<ItemModel> iterateList, ObservableCollection<ItemModel> folderItems);
        ItemModel RemoveItemFromFolder(int itemId, int folderId);
        FolderModel RemoveFolder(int folderId);
        ObservableCollection<FolderModel> UpdateFolders(List<FolderModel> updatedFolders, ObservableCollection<FolderModel> folders);
        void ExecuteDeleteFolderCommand(CanRemoveFolderMessage message, ICommand command);        
        void EditFolderName(string folderName, int folderId);
        void EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName);
        ObservableCollection<ItemModel> SortItems(string selectedSortOption, List<ItemModel> allFolderItems, ObservableCollection<ItemModel> folderItems);
        void PostCancelEditMessage();
        List<int> GetFolderIdsForItem(int selectedItemId);
        void AddItemToFolder(int selectedItemId, int selectedFolderId);
        ItemModel DeleteAllItemsFromFolder(int id);
        ObservableCollection<HotWheelsModel> SearchHotWheels(string searchhwText);
        ItemModel AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL);
        int GetAllHotWheelsCount();
        List<FolderModel> GetAllFolders();
        int GetAllCarsCount();
        ItemModel RemoveOneQuantity(ItemModel? itemModel);
    }
}