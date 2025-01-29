using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class RemovedItemFromFolderMessage : MvxMessage
    {
        public RemovedItemFromFolderMessage(object sender, List<ItemModel> updatedAllFolderItems) : base(sender)
        {
            UpdatedAllFolderItems = updatedAllFolderItems;
        }

        public List<ItemModel> UpdatedAllFolderItems { get; private set; }  
    }
}
