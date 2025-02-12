using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Services
{
    public class MessageBoxDataService : IMessageBoxDataService
    {
        private readonly IMvxMessenger _messenger;
        public MessageBoxDataService(IMvxMessenger messenger)
        {
            _messenger = messenger;
        }

        public void ConfirmAdd(bool result, FolderModel folderToDelete)
        {
            CanRemoveFolderMessage folderMessage = new CanRemoveFolderMessage(Mvx.IoCProvider.Resolve<CustomMessageBoxViewModel>(), result, folderToDelete);
            _messenger.Publish(folderMessage);
        }
    }
}
