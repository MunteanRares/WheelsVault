using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Services
{
    public class AddFolderViewService : IFolderDataService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IDatabaseData _db;

        public AddFolderViewService(IMvxMessenger messenger, IDatabaseData db)
        {
            _messenger = messenger;
            _db = db;
        }

        public async Task AddFolder(string folderName)
        {
            var newFolder = await _db.CreateNewFolder(folderName);
            var message = new AddedFolderMessage(Mvx.IoCProvider.Resolve<AddFolderViewModel>(), newFolder);
            _messenger.Publish(message);
        }
    }
}
