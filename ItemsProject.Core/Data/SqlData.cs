using ItemsProject.Core.Databases;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Models;
using WikiHotWheelsWebScraper.Models;
using WikiHotWheelsWebScraper.Services;

namespace ItemsProject.Core.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private readonly IScrapeHotWheelsWiki _scrapeService;
        private readonly string connectionStringName = "SqlServerDB";

        public SqlData(ISqlDataAccess db, IScrapeHotWheelsWiki scrapeService)
        {
            _db = db;
            _scrapeService = scrapeService;
        }

        public async Task DefaultHotwheelsDbPopulation()
        {
            int isDbPopulated = IsDbPopulated();
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


                        _db.SaveData("dbo.spHotwheelsCars_InsertCar",
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
                                     connectionStringName,
                                     true);
                    }
                }

                SetDbToPopulated();
            }
            else
            {
                List<int> availableYears = await _scrapeService.GetAllAvailableYears();
                List<HotWheelsModel> hotWheelsModels = await _scrapeService.DefaultDataBasePopulation(availableYears.Last());
                foreach (HotWheelsModel car in hotWheelsModels)
                {
                    _db.SaveData("dbo.spHotwheelsCars_UpdateCurrentYear",
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
                                 connectionStringName,
                                 true);
                }
            }
        }

        public int IsDbPopulated()
        {
            int output = _db.LoadData<int, dynamic>("dbo.spAppSettings_IsDbPopulated", new { }, connectionStringName, true).First();
            return output;
        }

        public void SetDbToPopulated()
        {
            _db.SaveData("dbo.spAppSettings_SetDbToPopulated", new { }, connectionStringName, true);
        }

        public List<ItemModel> GetAllItems()
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

        public ItemModel CreateCustomItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName)
        {
            _db.SaveData("dbo.spItems_CreateItem", new { FolderId, ModelName = ModelName.Capitalize(), ModelReleaseDate, CollectionName = CollectionName.ToUpper(), IsCustom = 1 }, connectionStringName, true);
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

        public void DeleteItem(int itemId, int folderId)
        {
            _db.SaveData("dbo.spItems_Remove", new { itemId, folderId }, connectionStringName, true);
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
            _db.SaveData("dbo.spItems_EditItem", new { itemId, newName, newReleaseDate, newCollectionName }, connectionStringName, true);
        }

        public List<int> GetAllFolderIdsForItem(int selectedItemID)
        {
            List<int> output = _db.LoadData<int, dynamic>("dbo.spFolders_GetAllFolderIdsForItem", new { selectedItemID }, connectionStringName, true);
            return output;
        }

        public void AddItemToFolder(int selectedItemId, int selectedFolderId)
        {
            _db.SaveData("dbo.spItems_AddToFolder", new { selectedItemId, selectedFolderId }, connectionStringName, true);
        }

        public void DeleteAllItemsFromFolder(int itemId)
        {
            _db.SaveData("dbo.spItems_RemoveAllFromFolder", new { itemId }, connectionStringName, true);
        }

        public List<HotWheelsModel> SearchHotWheels(string searchhwText)
        {
            List<HotWheelsModel> searchResult = _db.LoadData<HotWheelsModel, dynamic>("dbo.spHotwheelsCars_FindCarByText", new { searchhwText }, connectionStringName, true);
            return searchResult;
        }

        public ItemModel AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            _db.SaveData("dbo.Items_AddHotWheelsModel", new { folderId, modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom = 0 }, connectionStringName, true);
            ItemModel output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetUnique", new { modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom = 0 }, connectionStringName, true).First();
            //ItemModel output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetLast", new { }, connectionStringName, true).First();
            return output;
        }

        public List<ItemModel> GetAllNonCustom()
        {
            List<ItemModel> output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetAllNonCustom", new {}, connectionStringName, true);
            return output;
        }

        public int GetAllQuantities()
        {
            int output = _db.LoadData<int, dynamic>("dbo.spItems_GetAllQuantities", new { }, connectionStringName, true).FirstOrDefault();
            return output;
        }

        public ItemModel RemoveOneQuantity(ItemModel? itemModel)
        {
            _db.SaveData("dbo.spItems_RemoveOneQuantity", new { itemId = itemModel.Id }, connectionStringName, true);
            ItemModel output = _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetUnique",
                                             new { modelName = itemModel.ModelName,
                                                 seriesName = itemModel.SeriesName,
                                                 seriesNum = itemModel.SeriesNum,
                                                 yearProduced = itemModel.YearProduced,
                                                 yearProducedNum = itemModel.YearProducedNum,
                                                 toyNum = itemModel.ToyNum,
                                                 photoURL = itemModel.PhotoURL,
                                                 isCustom = 0 },
                                             connectionStringName,
                                             true).First();
            return output;
        }
    }
}
