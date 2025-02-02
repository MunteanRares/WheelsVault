using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DirectX.Common.Direct2D;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class OpenAddFolderWindow : CommandBase
   {
        private readonly IMvxNavigationService _nav;
        public OpenAddFolderWindow(IMvxNavigationService nav)
        {
            _nav = nav;
        }

        public override void Execute(object? parameter)
        {
            _nav.Navigate<AddFolderViewModel>();
        }
    }
}
