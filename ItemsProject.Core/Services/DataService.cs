using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ItemsProject.Core.Data;
using ItemsProject.Core.Models;

namespace ItemsProject.Core.Services
{
    public class DataService : IDataService
    {
        private readonly IDatabaseData _db;
        public DataService(IDatabaseData db)
        {
            _db = db;
        }

        public List<ItemModel> LoadItemsForFolder(FolderModel selectedFolder)
        {
            List<ItemModel> allFolderItems = new List<ItemModel>();

            if (selectedFolder.Name == "All Cars")
            {
                allFolderItems = _db.GetAllItems();
            }
            else
            {
                allFolderItems = _db.GetItemsByFolderId(selectedFolder.Id);
            }

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
                List<ItemModel> filteredItems = allFolderItems.Where(i => i.ModelName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
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

        public ItemModel RemoveItemFromFolder(int itemId)
        {
            ItemModel itemToRemove = _db.GetItemById(itemId);
            _db.DeleteItemById(itemId);

            return itemToRemove;
        }

    }
}
