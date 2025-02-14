using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class CancelItemEditingMessage : MvxMessage
    {
        public CancelItemEditingMessage(object sender) : base(sender) { }
    }
}
