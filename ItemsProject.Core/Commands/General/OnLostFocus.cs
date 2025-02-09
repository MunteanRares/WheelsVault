using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;

namespace ItemsProject.Core.Commands.General
{
    public class OnLostFocus : CommandBase
    {
        private readonly Action<string> _onLostFocusAction;

        public OnLostFocus(Action<string> onLostFocusAction)
        {
            _onLostFocusAction = onLostFocusAction;
        }

        public override void Execute(object? parameter)
        {
            _onLostFocusAction((string)parameter);
        }
    }
}
