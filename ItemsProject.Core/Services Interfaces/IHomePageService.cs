using ItemsProject.Core.Models;

namespace ItemsProject.Core.Services
{
    public interface IHomePageService
    {
        Task<List<ItemModel>> GetLatestCars();
        Task<bool> IsCurrentItemInCollection(ItemModel currentItem);
    }
}