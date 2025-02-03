using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class AddedItemMessage : MvxMessage
    {
        public AddedItemMessage(object sender, ItemModel newItem) : base(sender)
        {
            NewItem = newItem;
        }

        public ItemModel NewItem {  get; private set; }
    }
}
