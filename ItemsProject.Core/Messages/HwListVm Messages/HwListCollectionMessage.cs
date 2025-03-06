using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class HwListCollectionMessage : MvxMessage
    {
        public HwListCollectionMessage(object sender, List<ItemModel> folderItems) : base(sender)
        {
            FolderItems = folderItems;
        }

        public List<ItemModel> FolderItems { get; set; }
    }
}
