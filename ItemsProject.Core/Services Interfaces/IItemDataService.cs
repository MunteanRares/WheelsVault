namespace ItemsProject.Core.Services
{
    public interface IItemDataService
    {
        void AddItem(int folderId, string modelName, string modelReleaseDate, string collectionName);
    }
}