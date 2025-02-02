using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraReports.Native;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.AddItemViewModelCommands
{
    public class AddItemConfirm : CommandBase
    {
        private readonly Action _execute;
        public AddItemConfirm(Action execute)
        {
            _execute = execute;
        }

        public override void Execute(object? parameter)
        {
            _execute();
        }
    }
}
