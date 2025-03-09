using System.IO;
using DevExpress.Utils;
using DevExpress.Utils.CommonDialogs.Internal;
using ItemsProject.Core.Commands.SettingsView_Commands;
using ItemsProject.Core.Messages.Settings_Messages;
using Microsoft.Win32;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;


namespace ItemsProject.Wpf.Views
{
    public partial class SettingsView : MvxWpfView
    {
        private readonly IMvxMessenger _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
        private readonly List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

        public SettingsView()
        {
            InitializeComponent();
            _tokens.Add(_messenger.Subscribe<OpenFileDialogMessage>(OnOpenFileDialogMessage));
        }

        private void OnOpenFileDialogMessage(OpenFileDialogMessage message)
        {
            if(message.Sender is ImportSaveDataCommand)
            {
                string jsonContent = string.Empty;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Json Files | *.json";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Title = "Choose Save File";

                if (openFileDialog.ShowDialog() == true)
                {
                    var fileStream = openFileDialog.OpenFile();
                    using (StreamReader sr = new StreamReader(fileStream))
                    {
                        jsonContent = sr.ReadToEnd();
                        OpenFileDialogMessage jsonContentMessage = new OpenFileDialogMessage(this, jsonContent);
                        _messenger.Publish(jsonContentMessage);
                    }
                }
            }
        }

        private void UnsubscribeMessages()
        {
            foreach(var token in _tokens)
            {
                token.Dispose();
            }
            _tokens.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            UnsubscribeMessages();
            base.Dispose(disposing);
        }
    }
}
