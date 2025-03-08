namespace ItemsProject.Core.Services
{
    public interface IItemDataService
    {
        Task AddItem(int folderId, string modelName, string modelReleaseDate, string collectionName);
    }
}