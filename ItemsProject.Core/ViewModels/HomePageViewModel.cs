using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemsProject.Core.Commands.HomePageCommands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public class HomePageViewModel : MvxViewModel
    {
        public HomePageViewModel(IMvxMessenger messenger)
        {
            // COMMANDS 
            SelectDefaultFolderCommand = new SelectDefaultFolderCommand(messenger);
        }

        public ICommand SelectDefaultFolderCommand { get; }
    }
}
