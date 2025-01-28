﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
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

        public List<ItemModel> GetAllItems()
        {
            List<ItemModel> output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetAll", new { }, connectionStringName, true);
            return output;
        }

        public List<FolderModel> GetAllFolderItems()
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

    }
}
