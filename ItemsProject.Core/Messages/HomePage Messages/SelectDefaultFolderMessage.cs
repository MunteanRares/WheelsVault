using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages.HomePage_Messages
{
    public class SelectDefaultFolderMessage : MvxMessage
    {
        public SelectDefaultFolderMessage(object sender) : base(sender){}
    }
}
