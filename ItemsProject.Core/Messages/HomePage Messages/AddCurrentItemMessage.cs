using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages.HomePage_Messages
{
    public class AddCurrentItemMessage : MvxMessage
    {
        public AddCurrentItemMessage(object sender, ItemModel newItem) : base(sender)
        {
            NewItem = newItem;
        }

        public ItemModel NewItem { get; set; }
    }
}
