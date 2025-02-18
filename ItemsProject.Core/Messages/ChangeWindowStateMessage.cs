using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class ChangeWindowStateMessage : MvxMessage
    {
        public ChangeWindowStateMessage(object sender, bool changeWindowState) : base(sender)
        {
            ChangeWindowState = changeWindowState;
        }

        public bool ChangeWindowState { get; }
    }
}
