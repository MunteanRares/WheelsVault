using DevExpress.Data.Filtering.Helpers;
using ItemsProject.Core.Databases;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Models;
using WikiHotWheelsWebScraper.Models;
using WikiHotWheelsWebScraper.Services;

namespace ItemsProject.Core.Data
{
    internal class SqliteData : IDatabaseData
    {
        private readonly ISqliteDataAccess _db;
        private readonly IScrapeHotWheelsWiki _scrapeService;
        private readonly string connectionStringName = "SqliteDb";
        public SqliteData(ISqliteDataAccess db, IScrapeHotWheelsWiki scrapeService)
        {
            _db = db;
            _scrapeService = scrapeService;
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

        //public async Task<ItemModel> CreateCustomItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName)
        //{
        //    string sqlStatement = "select Id from Folders where IsDefault = 1";
        //    List<int> defaultFolderIdList = await _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName);
        //    int defaultFolderId = defaultFolderIdList.First();

        //    sqlStatement = @"insert into Items (modelName, modelReleaseDate, collectionName)
        //                            values (@ModelName, @ModelReleaseDate, @CollectionName)";
        //    await _db.SaveData(sqlStatement, new { ModelName = ModelName.Capitalize(), ModelReleaseDate, CollectionName = CollectionName.ToUpper() }, connectionStringName);

        //    sqlStatement = "select * from Items order by Id desc limit 1;";
        //    List<int> addedItemIdList = await _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName);
        //    int addedItemId = addedItemIdList.First();

        //    sqlStatement = "insert into FolderItems (folderId, itemId) values (@defaultFolderId, @addedItemId)";
        //    await _db.SaveData(sqlStatement, new { defaultFolderId, addedItemId }, connectionStringName);

        //    if (defaultFolderId != FolderId)
        //    {
        //        sqlStatement = "insert into FolderItems (folderId, itemId) values (@FolderId, @addedItemId)";
        //        await _db.SaveData(sqlStatement, new { FolderId, addedItemId }, connectionStringName);
        //    }

        //    sqlStatement = "select Items.* from Items " +
        //                   "join FolderItems on Items.Id = FolderItems.itemId " +
        //                   "order by Items.Id desc limit 1";
        //    List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { }, connectionStringName);
        //    return output.First();
        //}

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
            string sqlStatement = """
                    SELECT *
                    FROM HotwheelsCars
                    WHERE modelName LIKE '%' || @searchhwText || '%'
                    OR seriesName LIKE '%' || @searchhwText || '%'
                    OR yearProduced LIKE '%' || @searchhwText || '%'
                    OR toyNum LIKE '%' || @searchhwText || '%'
                    OR yearProducedNum LIKE '%' || @searchhwText || '%'
                """;
            List<HotWheelsModel> searchResult = await _db.LoadData<HotWheelsModel, dynamic>(sqlStatement, new { searchhwText }, connectionStringName);
            return searchResult;
        }

        public async Task DefaultHotwheelsDbPopulation()
        {
            int isDbPopulated = await IsDbPopulated();
            if (isDbPopulated == 0)
            {
                List<int> availableYears = await _scrapeService.GetAllAvailableYears();
                for (int year = availableYears.Last(); year >= 1980; year--)
                {
                    List<HotWheelsModel> hotWheelsModels = await _scrapeService.DefaultDataBasePopulation(year);
                    foreach (HotWheelsModel car in hotWheelsModels)
                    {
                        if (string.IsNullOrWhiteSpace(car.SeriesNum))
                        {
                            car.SeriesNum = "-";
                        }
                        else if (string.IsNullOrWhiteSpace(car.YearProducedNum))
                        {
                            car.YearProducedNum = "-";
                        }
                        else if (string.IsNullOrWhiteSpace(car.ToyNum))
                        {
                            car.ToyNum = "-";
                        }

                        string sqlStatement = """
                                insert into HotwheelsCars (modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoUrl)
                                values (@modelName, @seriesName, @seriesNum, @yearProduced, @yearProducedNum, @toyNum, @photoUrl)
                            """;

                        await _db.SaveData(sqlStatement,
                                     new
                                     {
                                         modelName = car.ModelName,
                                         seriesName = car.SeriesName,
                                         seriesNum = car.SeriesNum,
                                         photoUrl = car.PhotoURL,
                                         yearProduced = car.YearProduced,
                                         yearProducedNum = car.YearProducedNum,
                                         toyNum = car.ToyNum
                                     },
                                     connectionStringName);
                    }
                }

                await SetDbToPopulated();
            }
            else
            {
                List<int> availableYears = await _scrapeService.GetAllAvailableYears();
                int currentYear = DateTime.Now.Year;
                for (int year = currentYear; year <= availableYears.Last(); year++)
                {
                    List<HotWheelsModel> hotWheelsModels = await _scrapeService.DefaultDataBasePopulation(year);
                    foreach (HotWheelsModel car in hotWheelsModels)
                    {
                        string sqlStatement = """
                                select * from HotWheelsCars
                                where modelName = @modelName and seriesName = @seriesName and seriesNum = @seriesNum and yearProduced = @yearProduced and yearProducedNum = @yearProducedNum and toyNum = @toyNum
                            """;

                        List<FolderModel> itemExistsList = await _db.LoadData<FolderModel, dynamic>(sqlStatement, new { }, connectionStringName);
                        FolderModel? itemExists = itemExistsList.FirstOrDefault();

                        if (itemExists == null)
                        {
                            sqlStatement = """
                                        insert into HotWheelsCars (modelName, seriesName, photoUrl, seriesNum, yearProduced, yearProducedNum, toyNum)
                                        values (@modelName, @seriesName, @photoUrl, @seriesNum, @yearProduced, @yearProducedNum, @toyNum)
                                """;

                            await _db.SaveData(sqlStatement,
                                           new
                                           {
                                               modelName = car.ModelName,
                                               seriesName = car.SeriesName,
                                               seriesNum = car.SeriesNum,
                                               photoUrl = car.PhotoURL,
                                               yearProduced = car.YearProduced,
                                               yearProducedNum = car.YearProducedNum,
                                               toyNum = car.ToyNum
                                           },   
                                            connectionStringName);
                        }
                    }
                }
            }
        }

        public async Task<int> IsDbPopulated()
        {
            string sqlStatement = """
                    SELECT isDbPopulated
                    FROM AppSettings
                    LIMIT 1
                """;

            List<int> output = await _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName);
            return output.First();
        }

        public async Task SetDbToPopulated()
        {
            string sqlStatement = """
                    UPDATE AppSettings
                    SET isDbPopulated = 1
                    LIMIT 1
                """;

            await _db.SaveData(sqlStatement, new { }, connectionStringName);
        }

        public async Task<ItemModel> AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            FolderModel defaultFolder = await GetDefaultFolder();
            int defaultFolderId = defaultFolder.Id;
            string sqlStatement = """
                    SELECT 1 
                    FROM Items
                    WHERE modelName = @modelName
                    AND seriesName = @seriesName
                    AND seriesNum = @seriesNum
                    AND yearProduced = @yearProduced
                    AND toyNum = @toyNum
                    AND photoUrl = @photoURL
                    AND isCustom = 0
                """;
            List<ItemModel> itemExistsList = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL}, connectionStringName);
            ItemModel itemExists = itemExistsList.FirstOrDefault();

            if(itemExists == null)
            {
                sqlStatement = """
                        insert into Items (modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom, quantity)
                        values(@modelName, @seriesName, @seriesNum, @yearProduced, @yearProducedNum, @toyNum, @photoURL, @isCustom, 1)
                    """;

                await _db.SaveData(sqlStatement, new { modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom = 0, quantity = 1 }, connectionStringName);
                sqlStatement = """
                        select * from Items order by Id desc limit 1
                    """;
                List<ItemModel> lastAddedItemList = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { }, connectionStringName);
                ItemModel lastAddedItem = lastAddedItemList.First();

                sqlStatement = """
                        insert into FolderItems (folderId, itemId)
                        values (@defaultFolderId, @addedItemId)
                    """;
                await _db.SaveData(sqlStatement, new {defaultFolderId, addedItemId = lastAddedItem.Id }, connectionStringName);

                if (defaultFolderId != folderId)
                {
                    sqlStatement = """
                            insert into FolderItems (folderId, itemId)
                            values (@folderId, @addedItemID)
                        """;
                    await _db.SaveData(sqlStatement, new { folderId, addedItemId = lastAddedItem }, connectionStringName);
                }
            }
            else
            {
                sqlStatement = """
                        update Items
                        set quantity = quantity + 1
                        where modelName = @modelName
                        and seriesName = @seriesName
                        and seriesNum = @seriesNum
                        and yearProduced = @yearProduced
                        and toyNum = @toyNum
                        and photoUrl = @photoURL
                        and isCustom = 0
                    """;
                await _db.SaveData(sqlStatement, new { modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL }, connectionStringName);
            }

            ItemModel output = await GetUniqueItem(modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL);
            return output;
        }

        public async Task<ItemModel> GetUniqueItem(string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            string sqlStatement = """
                    select [Id], [modelName], [seriesName], [seriesNum], [yearProduced], [yearProducedNum], [toyNum], [photoUrl], [isCustom], [quantity]
                    from Items
                    where modelName = @modelName
                    and seriesName = @seriesName
                    and seriesNum = @seriesNum
                    and yearProduced = @yearProduced
                    and yearProducedNum = @yearProducedNum
                    and toyNum = @toyNum
                    and photoUrl = @photoURL
                    and isCustom = @isCustom
                """;

            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom = 0 }, connectionStringName);
            return output.First();
        }

        public async Task<List<ItemModel>> GetAllNonCustom()
        {
            string sqlStatement = """
                    select *
                    from Items
                    where Items.isCustom = 0    
                """;

            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { }, connectionStringName);
            return output;
        }

        public async Task<int> GetAllQuantities()
        {
            string sqlStatement = """
                    select coalesce(Sum(quantity),0) from Items
                """;

            List<int> output = await _db.LoadData<int, dynamic>(sqlStatement, new { }, connectionStringName);
            return output.FirstOrDefault();
        }

        public async Task<ItemModel> RemoveOneQuantity(ItemModel? itemModel)
        {
            string sqlStatement = """
                    update Items
                    set quantity = quantity - 1
                    where Id = @itemId
                """;

            await _db.SaveData(sqlStatement, new { itemId = itemModel.Id }, connectionStringName);
            ItemModel output =  await GetUniqueItem(itemModel.ModelName,
                                                    itemModel.SeriesName,
                                                    itemModel.SeriesNum,
                                                    itemModel.YearProduced,
                                                    itemModel.YearProducedNum,
                                                    itemModel.ToyNum,
                                                    itemModel.PhotoURL);
            return output;
        }

        public async Task<FolderModel> GetDefaultFolder()
        {
            string sqlStatement = """
                    select * from Folders
                    where Folders.isDefault = 1;
                """;

            List<FolderModel> output = await _db.LoadData<FolderModel, dynamic>(sqlStatement, new { }, connectionStringName);
            return output.First();
        }

        public async Task<List<ItemModel>> GetLatestCars()
        {
            int currentYear = DateTime.Now.Year;
            List<int> allYears = await _scrapeService.GetAllAvailableYears();
            int lastYearHw = allYears.Last();

            string sqlStatement = """
                    select * from HotwheelsCars
                    where HotwheelsCars.yearProduced in (@currentYear, @lastYearHw)
                """;

            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>(sqlStatement, new { currentYear, lastYearHw }, connectionStringName);
            output = output.Where(item => item.PhotoURL != null && !item.PhotoURL.Contains("Image_Not_Available")).ToList();
            return output;
        }
    }
}
