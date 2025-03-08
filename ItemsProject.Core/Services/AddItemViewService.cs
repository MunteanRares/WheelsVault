using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Services
{
    public class AddItemViewService : IItemDataService
    {
        private readonly IDatabaseData _db;
        private readonly IMvxMessenger _messenger;
        public AddItemViewService(IDatabaseData db, IMvxMessenger messenger)
        {
            _db = db;
            _messenger = messenger;
        }

        public async Task AddItem(int folderId, string modelName, string modelReleaseDate, string collectionName)
        {
            ItemModel newItem = await _db.CreateCustomItem(folderId, modelName, modelReleaseDate, collectionName);
            var message = new AddedItemMessage(Mvx.IoCProvider.Resolve<AddItemViewModel>(), newItem);
            _messenger.Publish(message);
        }
    }
}
