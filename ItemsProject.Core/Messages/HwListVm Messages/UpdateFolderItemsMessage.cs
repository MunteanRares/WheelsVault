using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class UpdateFolderItemsMessage : MvxMessage
    {
        public UpdateFolderItemsMessage(object sender, object? parameter, string methodOption) : base(sender)
        {
            Parameter = parameter;
            MethodOption = methodOption;
        }

        public object? Parameter { get; set; }
        public string MethodOption { get; set; }
    }
}
