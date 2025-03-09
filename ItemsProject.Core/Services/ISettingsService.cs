using ItemsProject.Core.Models;

namespace ItemsProject.Core.Services
{
    public interface ISettingsService
    {
        Task LoadItems(string jsonContents);
        void SaveItems(List<ItemModel> userItems);
    }
}