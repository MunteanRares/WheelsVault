using ItemsProject.Core.Models;

namespace ItemsProject.Core.Services
{
    public interface IMessageBoxDataService
    {
        void ConfirmAdd(bool result, FolderModel folderToDelete);
    }
}