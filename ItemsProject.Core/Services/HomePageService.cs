using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Data;
using ItemsProject.Core.Helper_Methods.List_Manipulation;
using ItemsProject.Core.Models;

namespace ItemsProject.Core.Services
{
    public class HomePageService : IHomePageService
    {
        private readonly IDatabaseData _db;

        public HomePageService(IDatabaseData db)
        {
            _db = db;
        }

        public async Task<List<ItemModel>> GetLatestCars()
        {
            List<ItemModel> output = await _db.GetLatestCars();
            ListManipulation.Shuffle(output);
            return output;
        }

        public async Task<bool> IsCurrentItemInCollection(ItemModel currentItem)
        {
            FolderModel defaultFolder = await _db.GetDefaultFolder();
            int defaultFolderId = defaultFolder.Id;
            List<ItemModel> defFolderItems = await _db.GetItemsByFolderId(defaultFolderId);

            return defFolderItems.Any(item => item.ModelName == currentItem.ModelName);
        }
    }
}
