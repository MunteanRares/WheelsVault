using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsProject.Core.Commands.CustomMessageBoxCommands
{
    public class Cancel : CommandBase
    {
        private readonly Action<bool> _closeWindow;
        public Cancel(Action<bool> closeWindow)
        {
            _closeWindow = closeWindow;
        }

        public override void Execute(object? parameter)
        {
            _closeWindow(false);    
        }
    }
}
