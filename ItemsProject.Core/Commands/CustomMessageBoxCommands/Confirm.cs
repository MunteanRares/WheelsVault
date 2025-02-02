using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Utils.Filtering.Internal;
using ItemsProject.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;

namespace ItemsProject.Core.Commands.CustomMessageBoxCommands
{
    public class Confirm : CommandBase
    {
        private readonly Action<bool> _closeWindow;
        public Confirm(Action<bool> closeWindow)
        {
            _closeWindow = closeWindow;
        }

        public override void Execute(object? parameter)
        {
            _closeWindow(true);
        }
    }
}
