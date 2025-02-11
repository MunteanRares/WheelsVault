using ItemsProject.Core.Databases;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Models;

namespace ItemsProject.Core.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private readonly string connectionStringName = "SqlServerDB";
        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<ItemModel> GetAllUniqueItems()
        {
            List<ItemModel> output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetAll", new { }, connectionStringName, true);
            return output;
        }

        public List<FolderModel> GetAllFolders()
        {
            List<FolderModel> output = _db.LoadData<FolderModel, dynamic>("dbo.spFolders_GetAll", new { }, connectionStringName, true);
            return output;
        }

        public List<ItemModel> GetItemsByFolderId(int folderId)
        {
            List<ItemModel> output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetByFolderId", new { folderId }, connectionStringName, true);
            return output;
        }

        public ItemModel CreateNewItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName)
        {
            _db.SaveData("dbo.spItems_CreateItem", new { FolderId, ModelName = ModelName.Capitalize(), ModelReleaseDate, CollectionName = CollectionName.ToUpper() }, connectionStringName, true);
            ItemModel output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetLast", new { }, connectionStringName, true).First();                        
            return output;
        }

        public FolderModel CreateNewFolder(string folderName)
        {
            _db.SaveData("spFolders_CreateFolder", new { folderName = folderName.Capitalize() }, connectionStringName, true);
            FolderModel output = _db.LoadData<FolderModel, dynamic>("dbo.spFolders_GetLast", new { }, connectionStringName, true).First();
            return output;
        }

        public ItemModel GetItemById(int itemId)
        {
            ItemModel output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetById", new { itemId }, connectionStringName, true).First();
            return output;
        }

        public void DeleteItem(int itemId, int folderId, string modelName, string modelReleaseDate, string collectionName)
        {
            _db.SaveData("dbo.spItems_Remove", new { itemId }, connectionStringName, true);
        }

        public FolderModel GetFolderById(int folderId)
        {
            FolderModel output = _db.LoadData<FolderModel, dynamic>("dbo.spFolders_GetById", new { folderId }, connectionStringName, true).First();
            return output;
        }

        public void RemoveFolderById(int folderId)
        {
            _db.SaveData("dbo.spFolders_Remove", new { folderId }, connectionStringName, true);
        }

        public void EditFolderName(string folderName, int folderId)
        {
            _db.SaveData("dbo.spFolders_EditName", new { folderName, folderId }, connectionStringName, true);
        }

        public void EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName)
        {
            _db.SaveData("dbo.spItems_EditItem", new { itemId, newName, newReleaseDate, newCollectionName}, connectionStringName, true);
        }
    }
}
