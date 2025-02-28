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

        public void AddItemToFolder(int selectedItemId, int selectedFolderId)
        {
            string sqlStatement = "insert into FolderItems (folderId, itemId) values (@selectedFolderId, @selectedItemId)";
            _db.SaveData(sqlStatement, new { selectedItemId, selectedFolderId }, connectionStringName);
        }

        public FolderModel CreateNewFolder(string folderName)
        {
            string sqlStatement = "insert into Folders (name) " +
                                  "values (@folderName)";
            _db.SaveData(sqlStatement, new { folderName = folderName.Capitalize() }, connectionStringName);

            //sqlStatement = "select Folders.* from Folders order by Folders.Id desc limit 1";
            sqlStatement = "select * from Folders order by Id desc limit 1";    
            FolderModel output = _db.LoadData<FolderModel, dynamic>(sqlStatement, new { }, connectionStringName).First();
            return output;
        }

        public ItemModel CreateCustomItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName)
        {
            string sqlStatement = "select Id from Folders where IsDefault = 1";
            int defaultFolderId = _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName).First();

            sqlStatement = @"insert into Items (modelName, modelReleaseDate, collectionName)
                                    values (@ModelName, @ModelReleaseDate, @CollectionName)";
            _db.SaveData(sqlStatement, new { ModelName = ModelName.Capitalize(), ModelReleaseDate, CollectionName = CollectionName.ToUpper() }, connectionStringName);

            sqlStatement = "select * from Items order by Id desc limit 1;";
            int addedItemId = _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName).First();

            sqlStatement = "insert into FolderItems (folderId, itemId) values (@defaultFolderId, @addedItemId)";
            _db.SaveData(sqlStatement, new { defaultFolderId, addedItemId }, connectionStringName);

            if (defaultFolderId != FolderId)
            {
                sqlStatement = "insert into FolderItems (folderId, itemId) values (@FolderId, @addedItemId)";
                _db.SaveData(sqlStatement, new { FolderId, addedItemId }, connectionStringName);
            }

            sqlStatement = "select Items.* from Items " +
                           "join FolderItems on Items.Id = FolderItems.itemId " +
                           "order by Items.Id desc limit 1";
            ItemModel output = _db.LoadData<ItemModel, dynamic>(sqlStatement, new { }, connectionStringName).First();
            return output;
        }

        public void DeleteAllItemsFromFolder(int itemId)
        {
            string sqlStatement = "delete from Items where Items.Id = @itemId";
            _db.SaveData(sqlStatement, new { itemId }, connectionStringName);
        }

        public void DeleteItem(int itemId, int folderId)
        {
            string sqlStatement = "select Id from Folders where IsDefault = 1";
            int defaultFolderId = _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName).First();

            if (defaultFolderId == folderId)
            {
                sqlStatement = "delete from Items where Id = @itemId";
                _db.SaveData(sqlStatement, new { itemId }, connectionStringName);
            }
            else
            {
                sqlStatement = "delete from FolderItems where FolderItems.folderId = @folderId and FolderItems.itemId = @itemId";
                _db.SaveData(sqlStatement, new { itemId, folderId }, connectionStringName);
            }            
        }

        public void EditFolderName(string folderName, int folderId)
        {
            string sqlStatement = "update Folders set name = @folderName where Folders.Id = @folderId";
            _db.SaveData(sqlStatement, new { folderName, folderId }, connectionStringName);
        }

        public void EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName)
        {
            string sqlStatement = "update Items set modelName = @newName, modelReleaseDate = @newReleaseDate, collectionName = @newCollectionName where Items.Id = @itemId";
            _db.SaveData(sqlStatement, new { itemId, newName, newReleaseDate, newCollectionName }, connectionStringName);
        }

        public List<int> GetAllFolderIdsForItem(int selectedItemId)
        {
            string sqlStatement = "select folderId from FolderItems where FolderItems.itemId = @selectedItemId";
            List<int> output = _db.LoadData<int, dynamic>(sqlStatement, new { selectedItemId }, connectionStringName);

            return output;
        }

        public List<FolderModel> GetAllFolders()
        {
            string sqlStatement = @"select *
                                   from Folders";
            List<FolderModel> output = _db.LoadData<FolderModel, dynamic>(sqlStatement, new { }, connectionStringName);

            return output;
        }

        public List<ItemModel> GetAllItems()
        {
            string sqlStatement = @"select *
                                   from Items";
            List<ItemModel> output = _db.LoadData<ItemModel, dynamic>(sqlStatement, new { }, connectionStringName);

            return output;
        }

        public FolderModel GetFolderById(int folderId)
        {
            string sqlStatement = "select * from Folders where Id = @folderId";
            FolderModel output = _db.LoadData<FolderModel, dynamic>(sqlStatement, new { folderId }, connectionStringName).First();

            return output;
        }

        public ItemModel GetItemById(int itemId)
        {
            string sqlStatement = @"select Items.*
                                   from Items
                                   join FolderItems on Items.Id = FolderItems.ItemId
                                   where Items.Id = @itemId";
            ItemModel output = _db.LoadData<ItemModel, dynamic>(sqlStatement, new { itemId }, connectionStringName).First();

            return output;
        }

        public List<ItemModel> GetItemsByFolderId(int folderId)
        {
            string sqlStatement = @"select Items.*
                                   from Items
                                   join FolderItems on Items.Id = FolderItems.itemId
                                   where FolderItems.folderId = @folderId";
            List<ItemModel> output = _db.LoadData<ItemModel, dynamic>(sqlStatement, new { folderId }, connectionStringName);

            return output;
        }

        public void RemoveFolderById(int folderId)
        {
            string sqlStatement = "select * from Folders where Folders.Id = @folderId";
            _db.SaveData(sqlStatement, new { folderId }, connectionStringName);
        }

        public List<HotWheelsModel> SearchHotWheels(string searchhwText)
        {
            throw new NotImplementedException();
        }

        Task IDatabaseData.DefaultHotwheelsDbPopulation()
        {
            throw new NotImplementedException();
        }

        public ItemModel AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            throw new NotImplementedException();
        }
    }
}
