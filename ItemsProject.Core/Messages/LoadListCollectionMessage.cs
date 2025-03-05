
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class LoadListCollectionMessage : MvxMessage
    {
        public LoadListCollectionMessage(object sender, HwListCollectionViewModel hwListVm) : base(sender)
        {
            HwListVm = hwListVm;
        }

        public HwListCollectionViewModel HwListVm { get; set; }
    }
}
