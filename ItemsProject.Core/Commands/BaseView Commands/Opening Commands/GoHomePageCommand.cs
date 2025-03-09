using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Commands.General;

namespace ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands
{
    public class GoHomePageCommand : CommandBase
    {
        private readonly Action _goHomeView;
        public GoHomePageCommand(Action goHomeView)
        {
            _goHomeView = goHomeView;
        }

        public override void Execute(object? parameter)
        {
            _goHomeView();
        }
    }
}
