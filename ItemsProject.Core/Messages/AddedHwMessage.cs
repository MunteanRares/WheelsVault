using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Browsing.Design;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class AddedHwMessage : MvxMessage
    {
        public AddedHwMessage(object sender, ItemModel newItem) : base(sender)
        {
            NewItem = newItem;
        }

        public ItemModel NewItem { get; set; }
    }
}
