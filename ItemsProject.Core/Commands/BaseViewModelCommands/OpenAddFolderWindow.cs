using ItemsProject.Core.Commands.General;
using ItemsProject.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;


namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class OpenAddFolderWindow : CommandBase
   {
        private readonly IMvxNavigationService _nav;
        private readonly Action _changeWindowState;
        public OpenAddFolderWindow(IMvxNavigationService nav, Action changeWindowState)
        {
            _nav = nav;
            _changeWindowState = changeWindowState;
        }

        public override void Execute(object? parameter)
        {
            _changeWindowState();
            _nav.Navigate<AddFolderViewModel>();
        }
    }
}
