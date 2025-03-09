using Newtonsoft.Json;
using ItemsProject.Core.Models;
using ItemsProject.Core.Data;
using MvvmCross.Navigation;
using ItemsProject.Core.ViewModels;
using Xceed.Wpf.Toolkit;
using System.Diagnostics;

namespace ItemsProject.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IDatabaseData _db;
        private readonly IMvxNavigationService _nav;

        public SettingsService(IDatabaseData db, IMvxNavigationService nav)
        {
            _db = db;
            _nav = nav;
        }

        public void SaveItems(List<ItemModel> userItems)
        {
            string jsonOutput = JsonConvert.SerializeObject(userItems, Formatting.Indented);
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WheelsVaultUserData.json");

            File.WriteAllText(filePath, jsonOutput);

            _nav.Navigate<SuccessNotificationViewModel, SuccessNotificationMessageModel>(new SuccessNotificationMessageModel { SuccessMessage = "Data saved successfully", Source = this });
        }

        public async Task LoadItems(string jsonContents)
        {
            try
            {
                List<ItemModel> importedCars = JsonConvert.DeserializeObject<List<ItemModel>>(jsonContents);

                FolderModel defaultFolderModel = await _db.GetDefaultFolder();
                foreach (ItemModel car in importedCars)
                {
                    await _db.AddHotWheelsModel(defaultFolderModel.Id, car.ModelName, car.SeriesName, car.SeriesNum, car.YearProduced, car.YearProducedNum, car.ToyNum, car.PhotoURL);
                }
            } catch(Exception e)
            {
                _nav.Navigate<ErrorMessageBoxViewModel, string>("The save file contains invalid data.");
            }
        }
    }
}
