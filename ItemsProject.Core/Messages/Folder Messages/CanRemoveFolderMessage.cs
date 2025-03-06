using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class CanRemoveFolderMessage : MvxMessage
    {
        public CanRemoveFolderMessage(object sender, bool canRemoveFolder, FolderModel folderToDelete) : base(sender)
        {
            CanRemoveFolder = canRemoveFolder;
            FolderToDelete = folderToDelete;
        }

        public bool CanRemoveFolder { get; set; }
        public FolderModel FolderToDelete { get; set; }
    }
}
