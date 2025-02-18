using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class CancelItemEditingMessage : MvxMessage
    {
        public CancelItemEditingMessage(object sender) : base(sender) { }
    }
}
