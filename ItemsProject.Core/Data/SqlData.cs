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


                        await _db.SaveData("dbo.spHotwheelsCars_InsertCar",
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
                        await _db.SaveData("dbo.spHotwheelsCars_UpdateCurrentYear",
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
        }

        public async Task<int> IsDbPopulated()
        {
            List<int> output = await _db.LoadData<int, dynamic>("dbo.spAppSettings_IsDbPopulated", new { }, connectionStringName, true);
            return output.First();
        }

        public async Task SetDbToPopulated()
        {
            await _db.SaveData("dbo.spAppSettings_SetDbToPopulated", new { }, connectionStringName, true);
        }

        public async Task<List<ItemModel>> GetAllItems()
        {
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetAll", new { }, connectionStringName, true);
            return output;
        }

        public async Task<List<FolderModel>> GetAllFolders()
        {
            List<FolderModel> output = await _db.LoadData<FolderModel, dynamic>("dbo.spFolders_GetAll", new { }, connectionStringName, true);
            return output;
        }

        public async Task<List<ItemModel>> GetItemsByFolderId(int folderId)
        {
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetByFolderId", new { folderId }, connectionStringName, true);
            return output;
        }

        public async Task<ItemModel> CreateCustomItem(int FolderId, string ModelName, string ModelReleaseDate, string CollectionName)
        {
            await _db.SaveData("dbo.spItems_CreateItem", new { FolderId, ModelName = ModelName.Capitalize(), ModelReleaseDate, CollectionName = CollectionName.ToUpper(), IsCustom = 1 }, connectionStringName, true);
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetLast", new { }, connectionStringName, true);
            return output.First();
        }

        public async Task<FolderModel> CreateNewFolder(string folderName)
        {
            await _db.SaveData("spFolders_CreateFolder", new { folderName = folderName.Capitalize() }, connectionStringName, true);
            List<FolderModel> output = await _db.LoadData<FolderModel, dynamic>("dbo.spFolders_GetLast", new { }, connectionStringName, true);
            return output.First();
        }

        public async Task<ItemModel> GetItemById(int itemId)
        {
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetById", new { itemId }, connectionStringName, true);
            return output.First();
        }

        public async Task DeleteItem(int itemId, int folderId)
        {
            await _db.SaveData("dbo.spItems_Remove", new { itemId, folderId }, connectionStringName, true);
        }

        public async Task<FolderModel> GetFolderById(int folderId)
        {
            List<FolderModel> output = await _db.LoadData<FolderModel, dynamic>("dbo.spFolders_GetById", new { folderId }, connectionStringName, true);
            return output.First();
        }

        public async Task RemoveFolderById(int folderId)
        {
            await _db.SaveData("dbo.spFolders_Remove", new { folderId }, connectionStringName, true);
        }

        public async Task EditFolderName(string folderName, int folderId)
        {
            await _db.SaveData("dbo.spFolders_EditName", new { folderName, folderId }, connectionStringName, true);
        }

        public async Task EditItem(int itemId, string newName, string newReleaseDate, string newCollectionName)
        {
            await _db.SaveData("dbo.spItems_EditItem", new { itemId, newName, newReleaseDate, newCollectionName }, connectionStringName, true);
        }

        public async Task<List<int>> GetAllFolderIdsForItem(int selectedItemID)
        {
            List<int> output = await _db.LoadData<int, dynamic>("dbo.spFolders_GetAllFolderIdsForItem", new { selectedItemID }, connectionStringName, true);
            return output;
        }

        public async Task AddItemToFolder(int selectedItemId, int selectedFolderId)
        {
            await _db.SaveData("dbo.spItems_AddToFolder", new { selectedItemId, selectedFolderId }, connectionStringName, true);
        }

        public async Task DeleteAllItemsFromFolder(int itemId)
        {
            await _db.SaveData("dbo.spItems_RemoveAllFromFolder", new { itemId }, connectionStringName, true);
        }

        public async Task<List<HotWheelsModel>> SearchHotWheels(string searchhwText)
        {
            List<HotWheelsModel> searchResult = await _db.LoadData<HotWheelsModel, dynamic>("dbo.spHotwheelsCars_FindCarByText", new { searchhwText }, connectionStringName, true);
            return searchResult;
        }

        public async Task<ItemModel> AddHotWheelsModel(int folderId, string modelName, string seriesName, string seriesNum, string yearProduced, string yearProducedNum, string toyNum, string photoURL)
        {
            await _db.SaveData("dbo.Items_AddHotWheelsModel", new { folderId, modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom = 0 }, connectionStringName, true);
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetUnique", new { modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom = 0 }, connectionStringName, true);
            return output.First();
        }

        public async Task<List<ItemModel>> GetAllNonCustom()
        {
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetAllNonCustom", new {}, connectionStringName, true);
            return output;
        }

        public async Task<int> GetAllQuantities()
        {
            List<int> output = await _db.LoadData<int, dynamic>("dbo.spItems_GetAllQuantities", new { }, connectionStringName, true);
            return output.FirstOrDefault();
        }

        public async Task<ItemModel> RemoveOneQuantity(ItemModel? itemModel)
        {
            await _db.SaveData("dbo.spItems_RemoveOneQuantity", new { itemId = itemModel.Id }, connectionStringName, true);
            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetUnique",
                                             new { modelName = itemModel.ModelName,
                                                 seriesName = itemModel.SeriesName,
                                                 seriesNum = itemModel.SeriesNum,
                                                 yearProduced = itemModel.YearProduced,
                                                 yearProducedNum = itemModel.YearProducedNum,
                                                 toyNum = itemModel.ToyNum,
                                                 photoURL = itemModel.PhotoURL,
                                                 isCustom = 0 },
                                             connectionStringName,
                                             true);
            return output.First();
        }

        public async Task<FolderModel> GetDefaultFolder()
        {
            List<FolderModel> output = await _db.LoadData<FolderModel, dynamic>("dbo.spFolders_GetDefault", new {}, connectionStringName, true);
            return output.First();
        }

        public async Task<List<ItemModel>> GetLatestCars()
        {
            int currentYear = DateTime.Now.Year;
            List<int> allYears = await _scrapeService.GetAllAvailableYears();
            int lastYearHw = allYears.Last();

            List<ItemModel> output = await _db.LoadData<ItemModel, dynamic>("dbo.spItems_GetLatest", new { currentYear, lastYearHw }, connectionStringName, true);
            output = output.Where(item => item.PhotoURL != null && !item.PhotoURL.Contains("Image_Not_Available")).ToList();
            return output;
        }
    }
}
