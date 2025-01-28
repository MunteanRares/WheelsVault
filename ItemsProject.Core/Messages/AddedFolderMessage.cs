using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class AddedFolderMessage : MvxMessage
    {
        public AddedFolderMessage(object sender, FolderModel newFolder) : base(sender)
        {
            NewFolder = newFolder;
        }

        public FolderModel NewFolder { get; private set; }
    }
}
