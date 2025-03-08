using ItemsProject.Core.Databases;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Models;
using WikiHotWheelsWebScraper.Models;

namespace ItemsProject.Core.Data
{
    internal class SqliteData : IDatabaseData
    {
        private readonly ISqliteDataAccess _db;
        private readonly string connectionStringName = "SqliteDb";
        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
            SqliteDatabaseInitializer.SetDefaultFolder(_db, connectionStringName);
        }

        public async Task AddItemToFolder(int selectedItemId, int selectedFolderId)
        {
            string sqlStatement = "insert into FolderItems (folderId, itemId) values (@selectedFolderId, @selectedItemId)";
            await _db.SaveData(sqlStatement, new { selectedItemId, selectedFolderId }, connectionStringName);
        }

        public async Task<FolderModel> CreateNewFolder(string folderName)
        {
            string sqlStatement = "insert into Folders (name) " +
                                  "values (@folderName)";
            await _db.SaveData(sqlStatement, new { folderName = folderName.Capitalize() }, connectionStringName);

            sqlStatement = "select * from Folders order by Id desc limit 1";    
            List<FolderModel>  output = await _db.LoadData<FolderModel, dynamic>(sqlStatement, new { }, connectionStringName);
            return output.First();
        }

        public async Task<ItemModel> CreateCustomItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName)
        {
            string sqlStatement = "select Id from Folders where IsDefault = 1";
            List<int> defaultFolderIdList = await _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName);
            int defaultFolderId = defaultFolderIdList.First();

            sqlStatement = @"insert into Items (modelName, modelReleaseDate, collectionName)
                                    values (@ModelName, @ModelReleaseDate, @CollectionName)";
            await _db.SaveData(sqlStatement, new { ModelName = ModelName.Capitalize(), ModelReleaseDate, CollectionName = CollectionName.ToUpper() }, connectionStringName);

            sqlStatement = "select * from Items order by Id desc limit 1;";
            List<int> addedItemIdList = await _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName);
            int addedItemId = addedItemIdList.First();

            sqlStatement = "insert into FolderItems (folderId, itemId) values (@defaultFolderId, @addedItemId)";
            await _db.SaveData(sqlStatement, new { defaultFolderId, addedItemId }, connectionStringName);

            if (defaultFolderId != FolderId)
            {
                sqlStatement = "insert into FolderItems (folderId, itemId) values (@FolderId, @addedItemId)";
                await _db.SaveData(sqlStatement, new { FolderId, addedItemId }, connectionStringName);
            }

            sqlStatement = "select Items.* from Items " +
                           "join FolderItems on Items.Id = FolderItems.itemId " +
                           "order by Items.Id desc limit 1";
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { }, connectionStringName);
            return output.First();
        }

        public async Task DeleteAllItemsFromFolder(int itemId)
        {
            string sqlStatement = "delete from Items where Items.Id = @itemId";
            await _db.SaveData(sqlStatement, new { itemId }, connectionStringName);
        }

        public async Task DeleteItem(int itemId, int folderId)
        {
            string sqlStatement = "select Id from Folders where IsDefault = 1";
            List<int> defaultFolderIdList = await _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName);
            int defaultFolderId = defaultFolderIdList.First();

            if (defaultFolderId == folderId)
            {
                sqlStatement = "delete from Items where Id = @itemId";
                await _db.SaveData(sqlStatement, new { itemId }, connectionStringName);
            }
            else
            {
                sqlStatement = "delete from FolderItems where FolderItems.folderId = @folderId and FolderItems.itemId = @itemId";
                await _db.SaveData(sqlStatement, new { itemId, folderId }, connectionStringName);
            }            
        }

        public async Task EditFolderName(string folderName, int folderId)
        {
            string sqlStatement = "update Folders set name = @folderName where Folders.Id = @folderId";
            await _db.SaveData(sqlStatement, new { folderName, folderId }, connectionStringName);
        }

        public async Task EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName)
        {
            string sqlStatement = "update Items set modelName = @newName, modelReleaseDate = @newReleaseDate, collectionName = @newCollectionName where Items.Id = @itemId";
            await _db.SaveData(sqlStatement, new { itemId, newName, newReleaseDate, newCollectionName }, connectionStringName);
        }

        public async Task<List<int>> GetAllFolderIdsForItem(int selectedItemId)
        {
            string sqlStatement = "select folderId from FolderItems where FolderItems.itemId = @selectedItemId";
            List<int> output = await _db.LoadData<int, dynamic>(sqlStatement, new { selectedItemId }, connectionStringName);

            return output;
        }

        public async Task<List<FolderModel>> GetAllFolders()
        {
            string sqlStatement = @"select *
                                   from Folders";
            List<FolderModel> output = await _db.LoadData<FolderModel, dynamic>(sqlStatement, new { }, connectionStringName);

            return output;
        }

        public async Task<List<ItemModel>> GetAllItems()
        {
            string sqlStatement = @"select *
                                   from Items";
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { }, connectionStringName);

            return output;
        }

        public async Task<FolderModel> GetFolderById(int folderId)
        {
            string sqlStatement = "select * from Folders where Id = @folderId";
            List<FolderModel> output = await _db.LoadData<FolderModel, dynamic>(sqlStatement, new { folderId }, connectionStringName);

            return output.First();
        }

        public async Task<ItemModel> GetItemById(int itemId)
        {
            string sqlStatement = @"select Items.*
                                   from Items
                                   join FolderItems on Items.Id = FolderItems.ItemId
                                   where Items.Id = @itemId";
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { itemId }, connectionStringName);

            return output.First();
        }

        public async Task<List<ItemModel>> GetItemsByFolderId(int folderId)
        {
            string sqlStatement = @"select Items.*
                                   from Items
                                   join FolderItems on Items.Id = FolderItems.itemId
                                   where FolderItems.folderId = @folderId";
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { folderId }, connectionStringName);

            return output;
        }

        public async Task RemoveFolderById(int folderId)
        {
            string sqlStatement = "select * from Folders where Folders.Id = @folderId";
            await _db.SaveData(sqlStatement, new { folderId }, connectionStringName);
        }

        public async Task<List<HotWheelsModel>> SearchHotWheels(string searchhwText)
        {
            throw new NotImplementedException();
        }

        Task IDatabaseData.DefaultHotwheelsDbPopulation()
        {
            throw new NotImplementedException();
        }

        public async Task<ItemModel> AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemModel>> GetAllNonCustom()
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetAllQuantities()
        {
            throw new NotImplementedException();
        }

        public async Task<ItemModel> RemoveOneQuantity(ItemModel? itemModel)
        {
            throw new NotImplementedException();
        }

        public Task<FolderModel> GetDefaultFolder()
        {
            throw new NotImplementedException();
        }
    }
}
