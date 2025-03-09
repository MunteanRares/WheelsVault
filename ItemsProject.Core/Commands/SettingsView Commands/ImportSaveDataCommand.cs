using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Messages.Settings_Messages;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.Commands.SettingsView_Commands
{
    public class ImportSaveDataCommand : CommandBase
    {
        private readonly IMvxMessenger _messenger;

        public ImportSaveDataCommand(IMvxMessenger messenger)
        {
            _messenger = messenger;
        }

        public override void Execute(object? parameter)
        {
            OpenFileDialogMessage message = new OpenFileDialogMessage(this);
            _messenger.Publish(message);
        }
    }
}
