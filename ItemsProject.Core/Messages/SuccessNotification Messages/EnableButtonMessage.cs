using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages.SuccessNotification_Messages
{
    public class EnableButtonMessage : MvxMessage
    {
        public EnableButtonMessage(object sender) : base(sender) {}
    }
}
