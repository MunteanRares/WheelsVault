using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Messages.HomePage_Messages;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Commands.HomePageCommands
{
    public class SelectDefaultFolderCommand : CommandBase
    {
        private readonly IMvxMessenger _messenger;
        public SelectDefaultFolderCommand(IMvxMessenger messenger)
        {
            _messenger = messenger;
        }

        public override void Execute(object? parameter)
        {
            SelectDefaultFolderMessage message = new SelectDefaultFolderMessage(this);
            _messenger.Publish(message);
        }
    }
}
