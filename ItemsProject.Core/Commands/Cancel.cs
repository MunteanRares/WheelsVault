using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsProject.Core.Commands
{
    public class Cancel : CommandBase
    {
        private readonly Action<bool> _closeWindowBool;
        private readonly Action _closeWindow;
        public Cancel(Action<bool> closeWindowBool)
        {
            _closeWindowBool = closeWindowBool;
        }

        public Cancel(Action closeWindow)
        {
            _closeWindow = closeWindow;
        }

        public override void Execute(object? parameter)
        {
            if (_closeWindowBool != null)
            {
                _closeWindowBool(false);
            }
            else if (_closeWindow != null)
            {
                _closeWindow();
            }

        }
    }
}
