using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages.HwListVm_Messages
{
    public class ClosePopupBeforeDeletingItemMessage : MvxMessage
    {
        public ClosePopupBeforeDeletingItemMessage(object sender) : base(sender) { }
    }
}
