using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages.Settings_Messages
{
    public class OpenFileDialogMessage : MvxMessage
    {
        public OpenFileDialogMessage(object sender) : base(sender)
        {
        }

        public OpenFileDialogMessage(object sender, string jsonContents) : base(sender)
        {
            JsonContents = jsonContents;
        }

        public string JsonContents { get; set; }
    }
}
