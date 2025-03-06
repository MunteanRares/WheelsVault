
using ItemsProject.Core.Models;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Messages
{
    public class ChangeCurrentViewMessage : MvxMessage
    {
        public ChangeCurrentViewMessage(object sender, object? viewModel) : base(sender)
        {
            ViewModel = viewModel;
        }

        public object? ViewModel { get; set; }
    }
}
