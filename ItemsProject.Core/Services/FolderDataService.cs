using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Services
{
    public class FolderDataService : IFolderDataService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IDatabaseData _db;

        public FolderDataService(IMvxMessenger messenger, IDatabaseData db)
        {
            _messenger = messenger;
            _db = db;
        }

        public void AddFolder(string folderName)
        {
            var newFolder = _db.CreateNewFolder(folderName);
            var message = new AddedFolderMessage(Mvx.IoCProvider.Resolve<AddFolderViewModel>(), newFolder);
            _messenger.Publish(message);
        }
    }
}
